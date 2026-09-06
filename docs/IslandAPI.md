# WinIslands 上岛 API 接入教程

让任何第三方软件把信息推送到 WinIslands 灵动岛（类似 iOS 第三方 App 的“灵动岛”集成）。

## 一、启用

1. 运行 WinIslands（版本 ≥ 1.0.4-beta2）。
2. 打开设置 → 「上岛 API」：
   - 勾选「启用上岛 API」
   - 端口默认 `9840`（可改）
   - 可选设置 Token（防局域网误连；设置后所有请求需带 `X-WinIslands-Token` 头）
   - 「默认显示时长」是全局兜底值（秒），第三方可按条覆盖

## 二、接口

| 方法 | 路径 | 说明 |
|---|---|---|
| POST | `/v1/island/push` | 推送 / 更新一张卡片（v1 基础字段） |
| POST | `/v3/island/push` | 推送 / 更新（v1 超集，额外支持图片 / 动态进度 / 心跳） |
| PATCH | `/v3/island/push/{id}` | 部分更新：只覆盖请求体里出现的字段，其余保留（含过期时间 / 队列位置） |
| DELETE | `/v1/island/push/{id}` | 移除一张卡片 |
| GET | `/v1/island/active`（或 `/v3/island/active`） | 查询当前活跃卡片 |
| GET | `/v3/ws` | WebSocket 双向通道：客户端发 JSON 消息，服务端广播事件 |
| GET | `/v1/health` | 健康检查 |

基址：`http://127.0.0.1:9840`（端口按设置）。

### POST /v1/island/push 请求体（JSON）

| 字段 | 必填 | 类型 | 说明 |
|---|---|---|---|
| `title` | 是 | string | 标题（紧凑态 + 展开态显示） |
| `body` | 否 | string | 正文详情（展开态显示） |
| `icon` | 否 | string | 图标：Segoe MDL2 字形（如 `"\uE8D6"`）或 emoji/文本 |
| `progress` | 否 | number | 进度 0~1（展开态显示进度条） |
| `duration_seconds` | 否 | number | 显示时长（秒），**覆盖** WinIslands 全局默认 |
| `buttons` | 否 | array | 按钮列表 |
| `id` | 否 | string | 自定义 ID；同 ID 重复推送会**更新**原卡片（并保持队列位置不变） |
| `subtitle` | 否 | string | 副标题（标题下方小字，紧凑/展开均显示） |
| `type` | 否 | string | 内容类型：`info`（默认）/ `success` / `warning` / `error`，用于提示色 |
| `priority` | 否 | string | 优先级：`high` / `normal`（默认）/ `low`；多条并存时高优先级排前 |
| `accent` | 否 | string | 自定义强调色 `#RRGGBB` 或 `#AARRGGBB`，覆盖类型默认色 |
| `theme` | 否 | string | 卡片主题：`dark` / `light` / `auto`（默认 auto 跟随 WinIslands 明暗主题） |
| `click` | 否 | object | 整卡点击回跳（结构同 `buttons[]` 项）：点击卡片执行该动作 |
| `input` | 否 | object | 输入框（结构见下）：用户在上岛卡片填写文字后提交，动作默认 `notify` 回传推送方 |
| `expires_at` | 否 | string | 服务端计算（返回用）；请求时忽略 |
| `image` | 否 | string | 图片：data URI（`data:image/png;base64,...`）或 http(s) 链接（v3，展开态显示） |
| `progress_from` / `progress_to` | 否 | number | 动态进度段 0~1（v3，配合 `progress_duration_seconds` 自动推进；默认 from=0 / to=1） |
| `progress_duration_seconds` | 否 | number | 动态进度持续时间（秒）（v3）：设置后进度条从 `progress_from` 自动推进到 `progress_to`，推送方无需反复更新 |
| `heartbeat_seconds` | 否 | number | 心跳间隔（秒）（v3）：推送方需周期性以同 id 更新续期；超过 2 倍间隔未续期的推送自动移除 |

`buttons[]` 每项：

| 字段 | 说明 |
|---|---|
| `label` | 按钮文字 |
| `action` | `url`（默认，用系统默认方式打开 value）、`launch`（启动 value 指定的程序）或 `command`（在本地执行 value 命令行，`cmd /c`） |
| `value` | url 地址 / 程序路径（可带参数）/ 命令字符串 |
| ⚠️ `command` | 仅本机回环 API（127.0.0.1）且可配 Token；会执行任意命令，请只给可信推送方使用 |

`input` 每项：

| 字段 | 说明 |
|---|---|
| `placeholder` | 输入框占位提示（可选） |
| `value` | 输入框初始值（可选，预填文字） |
| `submit_label` | 提交按钮文字（可选，默认「提交」） |
| `action` | 提交后执行的动作：`notify`（默认，把用户输入作为 `value` 通过 WebSocket `push_button` 事件回传推送方）、`url`（用系统默认方式打开 value）、`launch`（启动 value） |

### 响应

成功返回 JSON：
```json
{ "id": "abc123", "position": 1, "expires_at": "2026-08-23T15:05:30Z" }
```

| 字段 | 说明 |
|---|---|
| `id` | 本次推送的 ID（未传时服务端生成） |
| `position` | 该卡片在显示队列中的位置（从 1 开始）；同 ID 重复更新时位置保持不变 |
| `expires_at` | 过期时间（UTC）。同 ID 更新保留原过期时间，不会续期 |

## 三、WebSocket 双向通道（v3）

`GET /v3/ws`（WebSocket，需带 `X-WinIslands-Token` 头（若设置了 Token））。

### 客户端 → 服务端消息（JSON 文本帧）

| `action` | 说明 |
|---|---|
| `push` | 推送 / 更新卡片；卡片字段放在 `push` 字段内（结构同 POST /v3/island/push） |
| `remove` | 移除卡片；带 `id` 字段 |
| `ping` | 心跳探测，服务端回 `ok` |

```json
{ "action": "push", "push": { "id": "download-1", "title": "正在下载", "progress_from": 0, "progress_to": 1, "progress_duration_seconds": 60 } }
{ "action": "update", "push": { "id": "download-1", "title": "下载完成 60%", "progress": 0.6 } }
{ "action": "remove", "id": "download-1" }
{ "action": "ping" }
```

### 服务端 → 客户端事件（广播给所有连接）

| 事件 | 说明 |
|---|---|
| `push_updated` | 推送被新增 / 更新（`type: "event"`, `event: "push_updated"`, `push: {...}`） |
| `push_removed` | 推送被移除 / 过期（`type: "event"`, `event: "push_removed"`, `id: "..."`） |
| `push_button` | 推送按钮被点击（`type: "event"`, `event: "push_button"`, `push_id: "..."`, `button: "按钮文字"`, `value: "按钮值或用户输入"`）；`notify` 动作按钮 / 输入框提交时触发，推送方据此自行处理回调 |

```json
{ "type": "event", "event": "push_updated", "push": { "id": "download-1", "title": "正在下载", "progress_from": 0, "progress_to": 1, "progress_duration_seconds": 60, "expires_at": "2026-08-25T10:00:00Z" } }
```

## 四、各语言示例

仓库内提供可直接运行的示例脚本（见 `docs/sdk-examples/`）：
`push.bat` / `pull.bat`（curl）、`push.ps1`（PowerShell）、`push.py` / `pull.py`（Python 标准库）。

```bash
# Windows
docs\sdk-examples\push.bat 9840
docs\sdk-examples\pull.bat 9840

# PowerShell
powershell -ExecutionPolicy Bypass -File docs\sdk-examples\push.ps1 -Port 9840

# Python
python docs/sdk-examples/push.py 9840
```


### PowerShell
```powershell
$body = @{
    title = "外卖送达"
    body  = "您的订单已到，请取餐"
    icon  = "\uE7F4"
    duration_seconds = 30
    buttons = @(@{ label = "查看"; action = "url"; value = "https://example.com" })
} | ConvertTo-Json -Depth 5

Invoke-RestMethod -Uri "http://127.0.0.1:9840/v1/island/push" -Method Post `
    -Body $body -ContentType "application/json"
```

### Python
```python
import requests

requests.post("http://127.0.0.1:9840/v1/island/push", json={
    "title": "外卖送达",
    "body": "您的订单已到，请取餐",
    "icon": "\ue7f4",
    "duration_seconds": 30,
    "buttons": [{"label": "查看", "action": "url", "value": "https://example.com"}],
})
```

### C# (.NET)
```csharp
using System.Net.Http;
using System.Text;
using System.Text.Json;

var payload = JsonSerializer.Serialize(new {
    title = "外卖送达",
    body = "您的订单已到，请取餐",
    icon = "\uE7F4",
    duration_seconds = 30,
    buttons = new[] { new { label = "查看", action = "url", value = "https://example.com" } }
});
using var client = new HttpClient();
var resp = await client.PostAsync(
    "http://127.0.0.1:9840/v1/island/push",
    new StringContent(payload, Encoding.UTF8, "application/json"));
```

### curl
```bash
curl -X POST http://127.0.0.1:9840/v1/island/push \
  -H "Content-Type: application/json" \
  -d '{"title":"外卖送达","body":"您的订单已到","icon":"\ue7f4","duration_seconds":30,"buttons":[{"label":"查看","action":"url","value":"https://example.com"}]}'
```

### Node.js
```javascript
fetch("http://127.0.0.1:9840/v1/island/push", {
  method: "POST",
  headers: { "Content-Type": "application/json" },
  body: JSON.stringify({
    title: "外卖送达",
    body: "您的订单已到",
    icon: "\ue7f4",
    duration_seconds: 30,
    buttons: [{ label: "查看", action: "url", value: "https://example.com" }],
  }),
});
```

## 五、移除 / 查询

```powershell
# 移除
Invoke-RestMethod -Uri "http://127.0.0.1:9840/v1/island/push/<id>" -Method Delete

# 查询当前活跃
Invoke-RestMethod -Uri "http://127.0.0.1:9840/v1/island/active" -Method Get
```

## 六、常见场景

1. **下载 / 任务进度**：推送带 `progress` 的卡片，同一 `id` 持续更新，完成后 `duration_seconds=5` 短暂停留或直接 DELETE。
2. **倒计时 / 专注提醒**：推送 `duration_seconds` 为剩余时长，结束自动消失。
3. **实时状态（如网速、电量、Git 分支）**：设置较长时长或高频率更新同一 `id`。
4. **操作入口**：用 `buttons` 提供「打开链接」「启动程序」等快捷动作。
5. **动态进度（v3）**：下载 / 安装 / 渲染等任务只需推一次，设置 `progress_from=0,progress_to=1,progress_duration_seconds=120`，进度条自动推进；中途可 PATCH 覆盖。
6. **图片卡片（v3）**：二维码、验证码图片、截图等用 `image` 传 data URI 或 http 链接，展开态显示在卡片右侧。
7. **长驻状态 + 心跳（v3）**：长时间显示的状态（如勿扰中、VPN、监控）设置 `heartbeat_seconds` 并周期以同 id 重新 push/更新续期；停止续期超过 2 倍间隔自动消失，避免残留。
8. **输入框（v4 新增）**：如消息回复、关键词搜索等，推送 `input` 字段即可在上岛卡片内显示输入框 + 提交按钮。

```python
import requests

requests.post("http://127.0.0.1:9840/v1/island/push", json={
    "id": "ask-1",
    "title": "小助手提问",
    "body": "请输入你要查询的内容",
    "duration_seconds": 60,
    "input": {
        "placeholder": "输入内容…",
        "submit_label": "发送",
        "action": "notify",
    },
})
```

推送方通过 WebSocket 订阅 `push_button` 事件即可收到用户输入：`{ "type": "event", "event": "push_button", "push_id": "ask-1", "button": "发送", "value": "用户输入的文字" }`。

## 七、注意事项

- 服务仅监听 `127.0.0.1`（本机回环），外部网络无法访问。
- 若设置了 Token，请求需带请求头 `X-WinIslands-Token: <你的Token>`。
- `title` 为空会返回 `400`。
- 同一条推送可反复 `POST`（相同 `id`）更新内容并续期；也可以 `DELETE` 立即移除。
- v3 支持 `PATCH /v3/island/push/{id}` 部分更新：只覆盖请求体里出现的字段（如只更新 `progress`），未出现的字段（含过期时间、队列位置、图片）保持不变。
- 心跳：设置了 `heartbeat_seconds` 的推送，`POST` / `PATCH` / WebSocket `push` 都会刷新 `LastSeen`；超过 2 倍间隔未续期会被自动移除。
- 灵动岛点击展开后显示完整卡片（正文/进度/按钮）；按钮点击后自动关闭该推送。

- **不影响灵动岛宽度**：上岛推送**不会**导致灵动岛宽度变化。灵动岛宽度始终由 WinIslands 的「紧凑长度」设置（自动 / 手动）决定；推送卡片会在固定宽度内自适应显示（标题自动截断、正文换行）。第三方软件无法通过推送改变灵动岛宽度。
