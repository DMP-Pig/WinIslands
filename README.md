<div align="center">

**🌐 选择语言 / Select Language**

[简体中文](#简体中文) · [繁體中文](#繁體中文) · [English](#english) · [Español](#español) · [Français](#français) · [العربية](#العربية) · [Русский](#русский) · [Português](#português)

</div>

> **说明 / Note**: 以简体中文为标准 · Simplified Chinese is the standard reference.

---

- **🎶 卡拉OK逐字平滑重做（1.2.4）**：逐字推进改为 smoothstep 缓动 + 字间交叉过渡，高亮像光带一样从左到右连续流动，起笔/收笔有加减速，不再一顿一顿。
- **🔋 低电量常驻指示（1.2.4）**：电量低于阈值且未接电源时，灵动岛右上角常驻显示电量胶囊（≤10% 红色 / 其余橙色），随电量实时刷新；接上电源或电量回升后自动消失，设置中可开关。
- **🎧 设备连接动画（1.2.4）**：蓝牙设备连接/断开时展示 iOS 风格动画卡片；连接时自动读取设备电量并显示「设备名 · 电量 xx%」，读不到电量时只显示设备名，不阻塞界面。
- **🎤 歌词样式可调（1.2.3）**：普通歌词/当前行字号、行间距、卡拉OK推进速度、高亮色与基础色均可自定义，设置页即时生效，独立歌词小窗同步跟随。
- **📋 通知历史（1.2.3）**：展开灵动岛后在底部查看历史通知，点击可重新弹出，可设置保留条数并一键清空。
- **🔔 推送与通知优化（1.2.2）**：未展开时上岛/消息提醒单行显示（图标 + 标题 + 单行摘要），超长自动省略不再撑宽灵动岛；长通知时自动调节岛宽，右侧组件与文字完整显示，不被边缘裁切。
## 简体中文

# WinIslands — Windows 灵动岛

> **把 iOS 的灵动岛带到 Windows 11 / 10 —— 一款现代化、多功能的 Windows 灵动岛组件。**
> 基于 **.NET 8 + WPF**，免费开源（MIT），**无广告 · 无遥测 · 不上报数据**。

🌐 官网：https://WinIslands.JudeKwong.com ｜ GitHub：https://github.com/DMP-Pig/WinIslands

---

## ✨ 功能亮点
- **🎶 卡拉OK逐字平滑重做（1.2.4）**：逐字推进改为 smoothstep 缓动 + 字间交叉过渡，高亮像光带一样从左到右连续流动，起笔/收笔有加减速，不再一顿一顿。
- **🔋 低电量常驻指示（1.2.4）**：电量低于阈值且未接电源时，灵动岛右上角常驻显示电量胶囊（≤10% 红色 / 其余橙色），随电量实时刷新；接上电源或电量回升后自动消失，设置中可开关。
- **🎧 设备连接动画（1.2.4）**：蓝牙设备连接/断开时展示 iOS 风格动画卡片；连接时自动读取设备电量并显示「设备名 · 电量 xx%」，读不到电量时只显示设备名，不阻塞界面。
- **🎤 歌词样式可调（1.2.3）**：普通歌词/当前行字号、行间距、卡拉OK推进速度、高亮色与基础色均可自定义，设置页即时生效，独立歌词小窗同步跟随。
- **📋 通知历史（1.2.3）**：展开灵动岛后在底部查看历史通知，点击可重新弹出，可设置保留条数并一键清空。

- **🌊 丝滑动效（1.2.1）**：展开/收起改为 iOS 阻尼弹簧 + 内容交错过渡；音量调节联动动画；紧凑/展开智能透明度分层；切歌封面交叉淡入；浅色/深色主题平滑切换；逐帧动画性能优化，CPU 占用更低。

- **▶ 媒体播放控制**：原生接入 Windows 全局媒体会话（SMTC），兼容网易云、QQ音乐、Spotify、Apple Music、Groove、电影和电视等；额外专门支持 Cider 本地 API；无法接入时窗口标题兜底。专辑封面、进度拖拽 seek、播放/暂停/切歌一应俱全；多播放器同时打开时可一键切换控制来源。
- **♪ 卡拉OK逐字歌词**：展开卡片同步滚动高亮、逐字点亮；本地 `.lrc` → AMLL TTML → 播放器歌词接口 → 可选在线歌词，四级来源；双语歌词、翻译开关、一键复制当前行；歌词时间可每首歌微调，独立歌词小窗可调透明度与锁定。
- **▦ 可定制组件系统**：时间、天气、日期（含农历/节气）、CPU/GPU/内存/磁盘、网络速度、电量、输入法、快捷开关（WiFi/蓝牙/夜间/静音）等 30+ 组件；每组件可自定义图标，勾选与拖拽排序，单行/多行模式随时切换。
- **🏝 上岛 API**：本地 HTTP / WebSocket 接口，让任何第三方软件把信息实时推送到灵动岛（类似 iOS 第三方 App 的灵动岛集成）。支持图片、动态进度、心跳续期、深浅主题、按钮动作、输入框；推送不影响灵动岛长宽，不遮挡其他组件。
- **✨ 提示动画**：蓝牙连接/断开、开始充电/充电完成、低电量、网络恢复、日历/RSS/邮件提醒等事件，以精美动画在灵动岛上展示。
- **✦ 外观与动效**：18 种主题皮肤、自定义强调色、液态玻璃毛玻璃、壁纸取色、跑马灯、4 种动效皮肤（iOS 弹簧等）、4 种音频波纹样式（随音乐节奏抖动）；展开/收起非线性缓动，60fps 丝滑；PerMonitorV2 高 DPI。
- **🖱 交互与智能**：解锁拖动 + 边缘吸附、全屏/锁屏自动隐藏、双击/中键快捷动作、快捷操作按钮、文件中转站（拖文件上岛、可再拖出到其他应用）、录屏智能勿扰、定时明暗主题切换。
- **⚡ 效率工具与自动化**：番茄钟、待办、剪贴板历史、快速启动器、日程提醒；会议静音助手、屏幕录制/截图提示、文件复制/下载进度上岛；全局快捷键与规则引擎。
- **🛡 隐私安全**：无遥测、无广告、无数据上报。除用户手动开启的在线歌词/天气外完全离线；所有配置与数据仅存于本机 `%APPDATA%\WinIslands`。
- **🔁 多歌词源一键切换**：在「自动 / 本地 LRC / AMLL TTML / Cider API / 在线歌词」之间一键循环切换，立即重新加载当前歌曲歌词
- **🛡️ 崩溃自动恢复**：异常退出后下次启动自动提示已恢复，不再无响应、黑屏、状态丢失
- **⏱️ 动画时长可调**：展开/收起动画时长滑杆（300–1400ms），自由调节快慢，适配个人喜好


---

## 📥 下载（最新稳定版 1.2.4）

| 平台 | 下载 | 说明 |
| --- | --- | --- |
| Windows x64 | [x64 便携版](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-x64.exe) | 主流 64 位电脑首选，单文件免安装，直接运行 |
| Windows ARM64 | [ARM64 便携版](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-arm64.exe) | Surface Pro X / 骁龙机型等 ARM 设备 |
| Windows 通用 | [通用安装包](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-Setup-1.2.4.exe) | Inno Setup 安装向导，x64 / ARM64 自动按架构安装 |

历史版本与完整更新日志见 [GitHub Releases](https://github.com/DMP-Pig/WinIslands/releases)。

---

## 📊 性能指标

| 指标 | 数值 |
| --- | --- |
| 常驻内存（Private） | ~72 MB |
| 冷启动 | < 1 s |
| 空闲 CPU | ≈ 0% |
| 动效帧率 | 60 fps |
| 多实例 | 单实例防重复运行 |
| 遥测 | 0 遥测 · 无上报 · 无广告 |

---

## 🔧 构建

### 环境要求
- Windows 10 1809+ / Windows 11
- .NET 8 SDK

### 构建与测试
```powershell
dotnet build WinIslands.slnx -c Release
dotnet test  WinIslands.slnx -c Release
```

### 发布
```powershell
# 自包含（含 .NET 8 运行时，免安装运行）
.uild\publish.ps1

# 框架依赖（体积小，需安装 .NET 8 Desktop Runtime）
.uild\publish.ps1 -FrameworkDependent
```
产物位于 `publish\win-x64\`。正式版按版本放到 `releases\<版本>\win-x64\` 并按版本号重命名。

### 安装包（可选）
安装 [Inno Setup 6](https://jrsoftware.org/isinfo.php) 后执行 `iscc.exe build\release-<版本>.iss`，生成 `releases\<版本>\WinIslands-Setup-<版本>.exe`（通用安装包，x64 / ARM64 自动按架构安装）。

---

## 🚀 使用说明

1. 启动 `WinIslands.exe`（可设置开机自启），托盘出现图标；关闭主窗口不会退出进程，仅托盘化。
2. 播放任意音乐：网易云、QQ音乐、Spotify、Apple Music 官方版等自动通过系统媒体会话显示；Cider 详见下文；其它播放器兜底窗口标题识别。
3. **点击**灵动岛展开完整卡片（悬停不展开）：进度拖拽 seek、播放控制、音量、同步歌词；再点一下收回。
4. 托盘菜单：显示/隐藏、独立歌词窗口、开机自启、勿扰模式、检查更新、查看日志、设置、退出。
5. 全局快捷键（均可自定义）：`Ctrl+Alt+P` 播放/暂停 · `Ctrl+Alt+←/→` 上一首/下一首 · `Ctrl+Alt+I` 显示/隐藏 · `Ctrl+Alt+Space` 展开/收起 · `Ctrl+Space` 快速启动器 · `Ctrl+Alt+V` 剪贴板历史面板。
6. 常用命令行参数：`--demo` 演示模式 · `--diagnose` 输出诊断报告 · `--settings` 启动时打开设置。

### Cider 集成
1. 在 Cider 设置中开启「允许外部控制」（Allow external control）。
2. WinIslands 设置 → 媒体 → 启用 Cider（端口默认自动检测 `10767` 并扫描本机，也可手动填写）。
3. 播放时灵动岛来源显示 `Cider`，可显示封面/进度/歌词并控制播放、seek、音量。

---

## ⚙️ 配置项说明

配置文件：`%APPDATA%\WinIslands\settings.json`（JSON；设置界面改动即时生效，可导出/导入）。

| 键 | 默认 | 说明 |
| --- | --- | --- |
| `Language` | `zh-CN` | 界面语言：`zh-CN` / `en-US` |
| `Position` | `Center` | 位置：`Center` 顶部居中 / `Right` 顶部右侧 |
| `Monitor` | `Primary` | 显示器：`Primary` 主屏 / `All` 所有屏 / `Index` 指定屏 |
| `MonitorIndex` | `0` | 指定屏幕编号 |
| `OffsetX` / `OffsetY` | `0` / `8` | 位置偏移（像素） |
| `Opacity` | `0.92` | 不透明度 0.3–1.0 |
| `Theme` | `Auto` | `Auto` / `Light` / `Dark` |
| `ThemePreset` | `Default` | 主题皮肤（18 种预设 + Custom） |
| `AccentColor` | `#6C5CE7` | 主题色 `#RRGGBB` |
| `ThemeScheduledEnabled` | `false` | 定时明暗切换（仅 `Theme=Auto` 生效） |
| `ThemeScheduleDarkStartHour` / `EndHour` | `19` / `7` | 深色时段起止小时 |
| `FontScale` / `CornerRadius` | `1.0` / `28` | 字号缩放 0.8–1.4 / 圆角 16–40 |
| `AnimationStyle` | `Spring` | 动效皮肤：`Spring` / `Soft` / `Elastic` / `Fade` |
| `CompactWidth` / `CompactHeight` | `360` / `72` | 紧凑长度 / 宽度 |
| `CompactWidthAuto` / `CompactHeightAuto` | `true` | 紧凑尺寸自动调整 |
| `ExpandedWidth` / `MaxExpandedHeight` | `400` / `384` | 展开长度 / 展开最大高度 |
| `HideWhenNoMedia` | `true` | 无媒体播放时隐藏灵动岛 |
| `IslandAlwaysVisible` | `false` | 灵动岛常驻（无媒体时也显示组件） |
| `ShowWhenPaused` | `true` | 暂停时仍显示 |
| `StartWithWindows` | `false` | 开机自启 |
| `IsLocked` | `true` | 上锁（解锁后鼠标可拖动） |
| `EdgeSnapEnabled` | `true` | 拖动松手自动吸附屏幕边缘/居中 |
| `FullScreenAutoHideEnabled` | `true` | 全屏时自动隐藏 |
| `LockScreenAutoHideEnabled` | `true` | 锁屏（Win+L / 远程桌面断开）时自动隐藏，解锁后恢复 |
| `SingleLineMode` | `true` | 单行模式：紧凑态所有组件一行显示 |
| `DoubleClickAction` | `PlayPause` | 双击灵动岛快捷动作 |
| `MiddleClickAction` | `PlayPause` | 中键单击灵动岛快捷动作 |
| `CiderEnabled` / `CiderPort` | `true` / `0` | Cider 本地 API（0=自动检测） |
| `OnlineLyricsEnabled` | `true` | 在线歌词开关（右键灵动岛可一键开关；非官方接口，注意版权） |
| `AmllTtmlEnabled` | `true` | AMLL 逐字歌词（api.amll.dev，非官方） |
| `KaraokeHighlight` | `true` | 逐字卡拉OK高亮 |
| `StandaloneLyricsWindow` | `false` | 独立歌词小窗 |
| `UseSystemVolume` | `true` | 非 Cider 来源用系统音量 |
| `LowBatteryThreshold` | `20` | 低电量提醒阈值（%），0 关闭 |
| `DoNotDisturbManual` | `false` | 手动勿扰 |
| `DoNotDisturbEnabled` | `false` | 定时勿扰（按时间段静默提示） |
| `DoNotDisturbStartHour/Minute` | `22` / `0` | 勿扰开始（分钟级） |
| `DoNotDisturbEndHour/Minute` | `8` / `0` | 勿扰结束（分钟级） |
| `IslandApiEnabled` / `IslandApiPort` | `true` / `9840` | 上岛 API 开关 / 端口 |
| `IslandApiToken` | `""` | 上岛 API 可选 Token |
| `IslandApiDefaultDuration` | `30` | 上岛默认显示时长（秒） |
| `WaveStyle` | `Bars` | 音频波纹样式：`Bars` / `Spectrum` / `Ring` / `Particles` |
| `WidgetOrder` | `Time,Weather,...` | 组件摆放顺序（逗号分隔键名） |
| `Components` | 对象 | 各组件「无歌曲 / 有歌曲」两列勾选 |
| `MediaApps` | `[]` | 媒体程序启用/禁用与优先级 |
| `Rules` | `[]` | 自动化规则（条件 + 动作） |
| `ReduceMotion` | `false` | 减少动态效果（无障碍/省电） |
| `GlobalHotkeysEnabled` | `true` | 全局快捷键开关 |
| `LowPowerMode` | `false` | 低功耗模式：空闲降帧、简化动画 |

---

## 🏝 上岛 API（第三方集成）

任何软件都可通过本地 HTTP / WebSocket 接口把信息推送到灵动岛，类似 iOS 第三方 App 的“上岛”。

| 方法 | 路径 | 说明 |
| --- | --- | --- |
| POST | `/v1/island/push` | 推送/更新一张卡片 |
| POST | `/v3/island/push` | v1 超集：图片 / 动态进度 / 心跳 |
| PATCH | `/v3/island/push/{id}` | 部分更新 |
| DELETE | `/v1/island/push/{id}` | 移除卡片 |
| GET | `/v1/island/active` | 查询当前活跃卡片 |
| GET | `/v3/ws` | WebSocket 双向通道 |
| GET | `/v1/health` | 健康检查 |

支持：标题/正文/图标/副标题、进度、按钮（打开链接 / 启动程序 / 执行命令 / notify 回调）、输入框、图片、动态进度、心跳续期、深浅主题、自定义强调色、优先级队列。**推送不会改变灵动岛宽度**。

完整文档：[docs/IslandAPI.md](docs/IslandAPI.md)

---

## 🛡 隐私与安全

- **无遥测、无广告、无上报**。除用户手动开启的“在线歌词”“天气”外，应用不进行任何网络请求。
- 唯一联网场景：Cider 封面下载、AMLL 逐字歌词（api.amll.dev）、用户开启后的在线歌词与天气（Open-Meteo）。
- 所有数据本地存储于 `%APPDATA%\WinIslands\`；日志仅记录本地运行信息。

---

## ⚠️ 已知限制

- 逐字卡拉OK依赖歌词来源与进度：有 AMLL TTML / LRC 逐字时间轴时按字高亮，否则降级为整句高亮。
- 播放器偶发回退进度（如 Cider/SMTC 瞬间上报 0）：已做位置守卫，瞬间回退会被忽略。
- SMTC 覆盖范围取决于播放器是否注册全局媒体会话；个别旧播放器仅能通过窗口标题兜底（无控制按钮）。
- Cider 1.x（旧 API）未适配，仅支持 2.x 及以上。
- 在线歌词 / AMLL / 天气均为非官方接口，可能随版本变动而失效。

---

## ❓ 常见问题

**Q: 灵动岛没有出现？**
确认正在播放；`HideWhenNoMedia` 默认开启，无媒体时隐藏属正常。运行 `--diagnose` 查看会话列表。

**Q: Cider 显示“未连接”？**
确认 Cider 设置中开启“允许外部控制”，检查端口（默认 10767），并在 WinIslands 设置中确认已启用 Cider。

**Q: 退出后托盘图标仍在？**
托盘菜单 → 退出；直接关闭灵动岛窗口仅隐藏（托盘常驻设计）。

---

## 📄 开源许可

- 应用本体：MIT（见 [LICENSE](LICENSE)）
- 第三方组件：见 [THIRD_PARTY.md](THIRD_PARTY.md)

---

- **🎶 卡拉OK逐字平滑重做（1.2.4）**：逐字推進改為 smoothstep 緩動 + 字間交叉過渡，高亮像光帶一樣從左到右連續流動，起筆/收筆有加減速，不再一頓一頓。
- **🔋 低電量常駐指示（1.2.4）**：電量低於閾值且未接電源時，動態島右上角常駐顯示電量膠囊（≤10% 紅色 / 其餘橙色），隨電量即時刷新；接上電源或電量回升後自動消失，設定中可開關。
- **🎧 裝置連線動畫（1.2.4）**：藍牙裝置連線/中斷時展示 iOS 風格動畫卡片；連線時自動讀取裝置電量並顯示「裝置名稱 · 電量 xx%」，讀不到電量時只顯示裝置名稱，不阻塞介面。
- **🎤 歌詞樣式可調（1.2.3）**：普通歌詞/目前行字號、行距、卡拉OK推進速度、高亮色與基礎色皆可自訂，設定頁即時生效，獨立歌詞小窗同步跟隨。
- **📋 通知歷史（1.2.3）**：展開動態島後在底部檢視歷史通知，點擊可重新彈出，可設定保留筆數並一鍵清空。
- **🔔 推播與通知最佳化（1.2.2）**：未展開時上島/訊息提醒單行顯示（圖示 + 標題 + 單行摘要），過長自動省略不再撐寬動態島；長通知時自動調整島寬，右側元件與文字完整顯示，不被邊緣裁切。
## 繁體中文

# WinIslands — Windows 動態島

> **把 iOS 的動態島帶到 Windows 11 / 10 —— 一款現代化、多功能的 Windows 動態島元件。**
> 基於 **.NET 8 + WPF**，免費開源（MIT），**無廣告 · 無遙測 · 不上報資料**。

🌐 官網：https://WinIslands.JudeKwong.com ｜ GitHub：https://github.com/DMP-Pig/WinIslands

---

## ✨ 功能亮點
- **🎶 卡拉OK逐字平滑重做（1.2.4）**：逐字推進改為 smoothstep 緩動 + 字間交叉過渡，高亮像光帶一樣從左到右連續流動，起筆/收筆有加減速，不再一頓一頓。
- **🔋 低電量常駐指示（1.2.4）**：電量低於閾值且未接電源時，動態島右上角常駐顯示電量膠囊（≤10% 紅色 / 其餘橙色），隨電量即時刷新；接上電源或電量回升後自動消失，設定中可開關。
- **🎧 裝置連線動畫（1.2.4）**：藍牙裝置連線/中斷時展示 iOS 風格動畫卡片；連線時自動讀取裝置電量並顯示「裝置名稱 · 電量 xx%」，讀不到電量時只顯示裝置名稱，不阻塞介面。
- **🎤 歌詞樣式可調（1.2.3）**：普通歌詞/目前行字號、行距、卡拉OK推進速度、高亮色與基礎色皆可自訂，設定頁即時生效，獨立歌詞小窗同步跟隨。
- **📋 通知歷史（1.2.3）**：展開動態島後在底部檢視歷史通知，點擊可重新彈出，可設定保留筆數並一鍵清空。

- **🌊 絲滑動效（1.2.1）**：展開/收起改為 iOS 阻尼彈簧 + 內容交錯過渡；音量調節連動動畫；緊濟/展開智慧透明度分層；切歌封面交叉淡入；淺色/深色主題平滑切換；逐帧動畫效能最佳化。

- **▶ 媒體播放控制**：原生接入 Windows 全域媒體工作階段（SMTC），相容網易雲、QQ音樂、Spotify、Apple Music、Groove、電影和電視等；額外專門支援 Cider 本機 API；無法接入時以視窗標題兜底。專輯封面、進度拖曳 seek、播放/暫停/切歌一應俱全；多播放器同時開啟時可一鍵切換控制來源。
- **♪ 卡拉OK逐字歌詞**：展開卡片同步捲動高亮、逐字點亮；本機 `.lrc` → AMLL TTML → 播放器歌詞介面 → 可選線上歌詞，四級來源；雙語歌詞、翻譯開關、一鍵複製目前列；歌詞時間可逐首微調，獨立歌詞小窗可調透明度與鎖定。
- **▦ 可自訂元件系統**：時間、天氣、日期（含農曆/節氣）、CPU/GPU/記憶體/磁碟、網路速度、電量、輸入法、快捷開關（WiFi/藍牙/夜間/靜音）等 30+ 元件；每個元件可自訂圖示，勾選與拖曳排序，單行/多行模式隨時切換。
- **🏝 上島 API**：本機 HTTP / WebSocket 介面，讓任何第三方軟體把資訊即時推送到動態島（類似 iOS 第三方 App 的動態島整合）。支援圖片、動態進度、心跳續期、深淺主題、按鈕動作、輸入框；推送不影響動態島長寬，不遮擋其他元件。
- **✨ 提示動畫**：藍牙連線/斷開、開始充電/充電完成、低電量、網路恢復、行事曆/RSS/郵件提醒等事件，以精美動畫在動態島上展示。
- **✦ 外觀與動效**：18 種主題皮膚、自訂強調色、液態玻璃毛玻璃、桌布取色、跑馬燈、4 種動效皮膚（iOS 彈簧等）、4 種音訊波紋樣式（隨音樂節奏抖動）；展開/收起非線性緩動，60fps 絲滑；PerMonitorV2 高 DPI。
- **🖱 互動與智慧**：解鎖拖曳 + 邊緣吸附、全螢幕/鎖定螢幕自動隱藏、雙擊/中鍵快捷動作、快捷操作按鈕、檔案中轉站（拖檔案上島、可再拖出到其他應用）、錄影智慧勿擾、定時明暗主題切換。
- **⚡ 效率工具與自動化**：蕃茄鐘、待辦、剪貼簿歷史、快速啟動器、日程提醒；會議靜音助手、螢幕錄製/截圖提示、檔案複製/下載進度上島；全域快速鍵與規則引擎。
- **🛡 隱私安全**：無遙測、無廣告、無資料上報。除使用者手動開啟的線上歌詞/天氣外完全離線；所有設定與資料僅存於本機 `%APPDATA%\WinIslands`。
- **🔁 多歌詞來源一鍵切換**：在「自動 / 本地 LRC / AMLL TTML / Cider API / 線上歌詞」之間一鍵循環切換，立即重新載入目前歌曲歌詞
- **🛡️ 當機自動恢復**：異常結束後下次啟動自動提示已恢復，不再無回應、黑屏、狀態遺失
- **⏱️ 動畫時長可調**：展開/收起動畫時長滑桿（300–1400ms），自由調整快慢，配合個人喜好


---

## 📥 下載（最新穩定版 1.2.4）

| 平台 | 下載 | 說明 |
| --- | --- | --- |
| Windows x64 | [x64 攜帶版](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-x64.exe) | 主流 64 位元電腦首選，單檔免安裝，直接執行 |
| Windows ARM64 | [ARM64 攜帶版](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-arm64.exe) | Surface Pro X / 驍龍機型等 ARM 裝置 |
| Windows 通用 | [通用安裝包](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-Setup-1.2.4.exe) | Inno Setup 安裝精靈，x64 / ARM64 自動依架構安裝 |

歷史版本與完整更新日誌見 [GitHub Releases](https://github.com/DMP-Pig/WinIslands/releases)。

---

## 📊 效能指標

| 指標 | 數值 |
| --- | --- |
| 常駐記憶體（Private） | ~72 MB |
| 冷啟動 | < 1 s |
| 閒置 CPU | ≈ 0% |
| 動效幀率 | 60 fps |
| 多執行緒 | 單執行個體防重複執行 |
| 遙測 | 0 遙測 · 無上報 · 無廣告 |

---

## 🔧 建置

### 環境需求
- Windows 10 1809+ / Windows 11
- .NET 8 SDK

### 建置與測試
```powershell
dotnet build WinIslands.slnx -c Release
dotnet test  WinIslands.slnx -c Release
```

### 發佈
```powershell
# 自包含（含 .NET 8 執行階段，免安裝執行）
.uild\publish.ps1

# 框架依賴（體積小，需安裝 .NET 8 Desktop Runtime）
.uild\publish.ps1 -FrameworkDependent
```
產物位於 `publish\win-x64\`。正式版依版本放到 `releases\<版本>\win-x64\` 並依版本號重新命名。

### 安裝包（可選）
安裝 [Inno Setup 6](https://jrsoftware.org/isinfo.php) 後執行 `iscc.exe build\release-<版本>.iss`，產生 `releases\<版本>\WinIslands-Setup-<版本>.exe`（通用安裝包，x64 / ARM64 自動依架構安裝）。

---

## 🚀 使用說明

1. 啟動 `WinIslands.exe`（可設定開機自啟），托盤出現圖示；關閉主視窗不會結束處理程序，僅托盤化。
2. 播放任何音樂：網易雲、QQ音樂、Spotify、Apple Music 官方版等自動透過系統媒體工作階段顯示；Cider 詳見下文；其他播放器以視窗標題識別兜底。
3. **點擊**動態島展開完整卡片（懸停不展開）：進度拖曳 seek、播放控制、音量、同步歌詞；再點一下收回。
4. 托盤選單：顯示/隱藏、獨立歌詞視窗、開機自啟、勿擾模式、檢查更新、檢視日誌、設定、退出。
5. 全域快速鍵（皆可自訂）：`Ctrl+Alt+P` 播放/暫停 · `Ctrl+Alt+←/→` 上一首/下一首 · `Ctrl+Alt+I` 顯示/隱藏 · `Ctrl+Alt+Space` 展開/收起 · `Ctrl+Space` 快速啟動器 · `Ctrl+Alt+V` 剪貼簿歷史面板。
6. 常用命令列參數：`--demo` 示範模式 · `--diagnose` 輸出診斷報告 · `--settings` 啟動時開啟設定。

### Cider 整合
1. 在 Cider 設定中開啟「允許外部控制」（Allow external control）。
2. WinIslands 設定 → 媒體 → 啟用 Cider（連接埠預設自動偵測 `10767` 並掃描本機，也可手動填寫）。
3. 播放時動態島來源顯示 `Cider`，可顯示封面/進度/歌詞並控制播放、seek、音量。

---

## ⚙️ 設定項說明

設定檔：`%APPDATA%\WinIslands\settings.json`（JSON；設定介面改動即時生效，可匯出/匯入）。

| 鍵 | 預設 | 說明 |
| --- | --- | --- |
| `Language` | `zh-CN` | 介面語言：`zh-CN` / `en-US` |
| `Position` | `Center` | 位置：`Center` 頂部置中 / `Right` 頂部右側 |
| `Monitor` | `Primary` | 顯示器：`Primary` 主螢幕 / `All` 所有螢幕 / `Index` 指定螢幕 |
| `MonitorIndex` | `0` | 指定螢幕編號 |
| `OffsetX` / `OffsetY` | `0` / `8` | 位置偏移（像素） |
| `Opacity` | `0.92` | 不透明度 0.3–1.0 |
| `Theme` | `Auto` | `Auto` / `Light` / `Dark` |
| `ThemePreset` | `Default` | 主題皮膚（18 種預設 + Custom） |
| `AccentColor` | `#6C5CE7` | 主題色 `#RRGGBB` |
| `ThemeScheduledEnabled` | `false` | 定時明暗切換（僅 `Theme=Auto` 生效） |
| `ThemeScheduleDarkStartHour` / `EndHour` | `19` / `7` | 深色時段起止小時 |
| `FontScale` / `CornerRadius` | `1.0` / `28` | 字號縮放 0.8–1.4 / 圓角 16–40 |
| `AnimationStyle` | `Spring` | 動效皮膚：`Spring` / `Soft` / `Elastic` / `Fade` |
| `CompactWidth` / `CompactHeight` | `360` / `72` | 緊湊長度 / 寬度 |
| `CompactWidthAuto` / `CompactHeightAuto` | `true` | 緊湊尺寸自動調整 |
| `ExpandedWidth` / `MaxExpandedHeight` | `400` / `384` | 展開長度 / 展開最大高度 |
| `HideWhenNoMedia` | `true` | 無媒體播放時隱藏動態島 |
| `IslandAlwaysVisible` | `false` | 動態島常駐（無媒體時也顯示元件） |
| `ShowWhenPaused` | `true` | 暫停時仍顯示 |
| `StartWithWindows` | `false` | 開機自啟 |
| `IsLocked` | `true` | 上鎖（解鎖後滑鼠可拖曳） |
| `EdgeSnapEnabled` | `true` | 拖曳鬆手自動吸附螢幕邊緣/置中 |
| `FullScreenAutoHideEnabled` | `true` | 全螢幕時自動隱藏 |
| `LockScreenAutoHideEnabled` | `true` | 鎖定螢幕（Win+L / 遠端桌面斷線）時自動隱藏，解鎖後恢復 |
| `SingleLineMode` | `true` | 單行模式：緊湊態所有元件一行顯示 |
| `DoubleClickAction` | `PlayPause` | 雙擊動態島快捷動作 |
| `MiddleClickAction` | `PlayPause` | 中鍵單擊動態島快捷動作 |
| `CiderEnabled` / `CiderPort` | `true` / `0` | Cider 本機 API（0=自動偵測） |
| `OnlineLyricsEnabled` | `true` | 線上歌詞開關（右鍵動態島可一鍵開關；非官方介面，注意版權） |
| `AmllTtmlEnabled` | `true` | AMLL 逐字歌詞（api.amll.dev，非官方） |
| `KaraokeHighlight` | `true` | 逐字卡拉OK高亮 |
| `StandaloneLyricsWindow` | `false` | 獨立歌詞小窗 |
| `UseSystemVolume` | `true` | 非 Cider 來源用系統音量 |
| `LowBatteryThreshold` | `20` | 低電量提醒閾值（%），0 關閉 |
| `DoNotDisturbManual` | `false` | 手動勿擾 |
| `DoNotDisturbEnabled` | `false` | 定時勿擾（依時段靜默提示） |
| `DoNotDisturbStartHour/Minute` | `22` / `0` | 勿擾開始（分鐘級） |
| `DoNotDisturbEndHour/Minute` | `8` / `0` | 勿擾結束（分鐘級） |
| `IslandApiEnabled` / `IslandApiPort` | `true` / `9840` | 上島 API 開關 / 連接埠 |
| `IslandApiToken` | `""` | 上島 API 選用 Token |
| `IslandApiDefaultDuration` | `30` | 上島預設顯示時長（秒） |
| `WaveStyle` | `Bars` | 音訊波紋樣式：`Bars` / `Spectrum` / `Ring` / `Particles` |
| `WidgetOrder` | `Time,Weather,...` | 元件擺放順序（逗號分隔鍵名） |
| `Components` | 物件 | 各元件「無歌曲 / 有歌曲」兩欄勾選 |
| `MediaApps` | `[]` | 媒體程式啟用/停用與優先順序 |
| `Rules` | `[]` | 自動化規則（條件 + 動作） |
| `ReduceMotion` | `false` | 減少動態效果（無障礙/省電） |
| `GlobalHotkeysEnabled` | `true` | 全域快速鍵開關 |
| `LowPowerMode` | `false` | 低功耗模式：閒置降幀、簡化動畫 |

---

## 🏝 上島 API（第三方整合）

任何軟體都可透過本機 HTTP / WebSocket 介面把資訊推送到動態島，類似 iOS 第三方 App 的「上島」。

| 方法 | 路徑 | 說明 |
| --- | --- | --- |
| POST | `/v1/island/push` | 推送/更新一張卡片 |
| POST | `/v3/island/push` | v1 超集：圖片 / 動態進度 / 心跳 |
| PATCH | `/v3/island/push/{id}` | 部分更新 |
| DELETE | `/v1/island/push/{id}` | 移除卡片 |
| GET | `/v1/island/active` | 查詢目前活躍卡片 |
| GET | `/v3/ws` | WebSocket 雙向通道 |
| GET | `/v1/health` | 健康檢查 |

支援：標題/正文/圖示/副標題、進度、按鈕（開啟連結 / 啟動程式 / 執行命令 / notify 回呼）、輸入框、圖片、動態進度、心跳續期、深淺主題、自訂強調色、優先順序佇列。**推送不會改變動態島寬度**。

完整文件：[docs/IslandAPI.md](docs/IslandAPI.md)

---

## 🛡 隱私與安全

- **無遙測、無廣告、無上報**。除使用者手動開啟的「線上歌詞」「天氣」外，應用不進行任何網路請求。
- 唯一聯網場景：Cider 封面下載、AMLL 逐字歌詞（api.amll.dev）、使用者開啟後的線上歌詞與天氣（Open-Meteo）。
- 所有資料本機儲存於 `%APPDATA%\WinIslands\`；日誌僅記錄本機執行資訊。

---

## ⚠️ 已知限制

- 逐字卡拉OK依賴歌詞來源與進度：有 AMLL TTML / LRC 逐字時間軸時按字高亮，否則降級為整句高亮。
- 播放器偶發回退進度（如 Cider/SMTC 瞬間上報 0）：已做位置守衛，瞬間回退會被忽略。
- SMTC 涵蓋範圍取決於播放器是否註冊全域媒體工作階段；個別舊播放器僅能以視窗標題兜底（無控制按鈕）。
- Cider 1.x（舊 API）未適配，僅支援 2.x 及以上。
- 線上歌詞 / AMLL / 天氣均為非官方介面，可能隨版本變動而失效。

---

## ❓ 常見問題

**Q: 動態島沒有出現？**
確認正在播放；`HideWhenNoMedia` 預設開啟，無媒體時隱藏屬正常。執行 `--diagnose` 檢視工作階段清單。

**Q: Cider 顯示「未連線」？**
確認 Cider 設定中開啟「允許外部控制」，檢查連接埠（預設 10767），並在 WinIslands 設定中確認已啟用 Cider。

**Q: 退出後托盤圖示仍在？**
托盤選單 → 退出；直接關閉動態島視窗僅隱藏（托盤常駐設計）。

---

## 📄 開源授權

- 應用本體：MIT（見 [LICENSE](LICENSE)）
- 第三方元件：見 [THIRD_PARTY.md](THIRD_PARTY.md)

---

- **🎶 Smoother karaoke highlighting (1.2.4)**: character-by-character progress now uses smoothstep easing with cross-fades between letters — the highlight flows continuously left to right like a light band, easing in and out instead of stepping abruptly.
- **🔋 Persistent low-battery indicator (1.2.4)**: when the battery drops below the threshold and is not charging, a small battery pill stays in the island’s top-right corner (red ≤10% / orange otherwise), refreshing in real time; it disappears automatically when plugged in or recovered, and can be toggled in settings.
- **🎧 Device connection animation (1.2.4)**: Bluetooth connect/disconnect now shows an iOS-style animated card; on connect, the device’s battery is read automatically and shown as “Device name · Battery xx%” (name only when battery can’t be read), without blocking the UI.
- **🎤 Adjustable lyric style (1.2.3)**: customize normal/current-line font size, line spacing, karaoke advance speed, and highlight/base colors; changes apply instantly and the standalone lyrics window follows.
- **📋 Notification history (1.2.3)**: review recent notifications at the bottom of the expanded island, click to replay, configure the max count, and clear with one click.
- **🔔 Push & notification polish (1.2.2)**: collapsed alerts show a single line (icon + title + one-line summary) and truncate when long, so they no longer widen the island; the island auto-widens for long notifications so right-side widgets and text stay fully visible.
## English

# WinIslands — Dynamic Island for Windows

> **Bring the iOS Dynamic Island to Windows 11 / 10 — a modern, multi-functional Dynamic Island widget for Windows.**
> Built with **.NET 8 + WPF**, free and open source (MIT), **no ads · no telemetry · no data collection**.

🌐 Website: https://WinIslands.JudeKwong.com ｜ GitHub: https://github.com/DMP-Pig/WinIslands

---

## ✨ Highlights
- **🎶 Smoother karaoke highlighting (1.2.4)**: character-by-character progress now uses smoothstep easing with cross-fades between letters — the highlight flows continuously left to right like a light band, easing in and out instead of stepping abruptly.
- **🔋 Persistent low-battery indicator (1.2.4)**: when the battery drops below the threshold and is not charging, a small battery pill stays in the island’s top-right corner (red ≤10% / orange otherwise), refreshing in real time; it disappears automatically when plugged in or recovered, and can be toggled in settings.
- **🎧 Device connection animation (1.2.4)**: Bluetooth connect/disconnect now shows an iOS-style animated card; on connect, the device’s battery is read automatically and shown as “Device name · Battery xx%” (name only when battery can’t be read), without blocking the UI.
- **🎤 Adjustable lyric style (1.2.3)**: customize normal/current-line font size, line spacing, karaoke advance speed, and highlight/base colors; changes apply instantly and the standalone lyrics window follows.
- **📋 Notification history (1.2.3)**: review recent notifications at the bottom of the expanded island, click to replay, configure the max count, and clear with one click.

- **🌊 Buttery-smooth motion (1.2.1)**: iOS-style spring damping + staggered content transitions for expand/collapse; linked volume-change animation; smart opacity layering between compact and expanded; cross-fade cover transitions; smooth light/dark theme interpolation; per-frame animation performance optimizations.

- **▶ Media playback control**: Native integration with Windows global media sessions (SMTC), compatible with NetEase Cloud Music, QQ Music, Spotify, Apple Music, Groove, Movies & TV and more; plus dedicated support for the Cider local API; falls back to window-title detection when unavailable. Album artwork, draggable seek bar, play/pause/next/previous — all included; when multiple players are open you can switch the control source with one click.
- **♪ Karaoke word-by-word lyrics**: The expanded card scrolls with synchronized highlighting and word-by-word illumination; four-tier source priority: local `.lrc` → AMLL TTML → player lyrics API → optional online lyrics. Bilingual lyrics, translation toggle, one-click copy of the current line; per-song lyric time fine-tuning, and a standalone lyrics window with adjustable opacity and locking.
- **▦ Customizable widget system**: 30+ widgets — time, weather, date (with lunar calendar/solar terms), CPU/GPU/RAM/disk, network speed, battery, input method, quick toggles (WiFi/Bluetooth/night mode/mute), etc. Each widget supports a custom icon, checkbox selection and drag-to-reorder, with one-line/multi-line modes.
- **🏝 Island API**: A local HTTP / WebSocket interface that lets any third-party software push information to the Dynamic Island in real time (like iOS third-party App Island integration). Supports images, animated progress, heartbeat renewal, light/dark themes, button actions and input fields; pushes never change the island's width or cover other widgets.
- **✨ Event animations**: Bluetooth connect/disconnect, charging start/done, low battery, network restored, calendar/RSS/email reminders and more are shown on the island with elegant animations.
- **✦ Appearance & motion**: 18 theme presets, custom accent color, liquid-glass acrylic, wallpaper color extraction, marquee, 4 motion skins (iOS spring etc.), 4 audio-wave styles (pulsing with the music); non-linear easing for expand/collapse at a smooth 60 fps; PerMonitorV2 high-DPI support.
- **🖱 Interaction & intelligence**: unlock-to-drag with edge snapping, auto-hide on fullscreen/lock screen, double-click and middle-click quick actions, quick-action buttons, file transfer station (drag files onto the island and out to other apps), smart DND while screen-recording, scheduled light/dark theme switching.
- **⚡ Productivity & automation**: Pomodoro timer, to-dos, clipboard history, quick launcher, schedule reminders; meeting mute assistant, screen-record/screenshot hints, file-copy/download progress on the island; global hotkeys and a rules engine.
- **🛡 Privacy & security**: no telemetry, no ads, no data uploads. Fully offline except for user-enabled online lyrics/weather; all configuration and data stays local in `%APPDATA%\WinIslands`.
- **🔁 One-tap lyric-source switching**: cycle instantly between Auto / Local LRC / AMLL TTML / Cider API / Online lyrics, and the current song's lyrics reload right away
- **🛡️ Automatic crash recovery**: after an abnormal exit, the next launch auto-notifies recovery — no more hangs, black screens, or lost state
- **⏱️ Adjustable animation duration**: a new expand/collapse animation-duration slider (300–1400ms) lets you fine-tune speed to your liking


---

## 📥 Download (latest stable 1.2.4)

| Platform | Download | Notes |
| --- | --- | --- |
| Windows x64 | [x64 portable](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-x64.exe) | For mainstream 64-bit PCs; single file, no install needed |
| Windows ARM64 | [ARM64 portable](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-arm64.exe) | For Surface Pro X / Snapdragon ARM devices |
| Windows Universal | [Universal installer](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-Setup-1.2.4.exe) | Inno Setup wizard; auto-installs x64 / ARM64 by architecture |

All historical versions and the full changelog: [GitHub Releases](https://github.com/DMP-Pig/WinIslands/releases).

---

## 📊 Performance

| Metric | Value |
| --- | --- |
| Resident memory (Private) | ~72 MB |
| Cold start | < 1 s |
| Idle CPU | ≈ 0% |
| Animation frame rate | 60 fps |
| Multiple instances | Single instance, prevents duplicates |
| Telemetry | 0 telemetry · no uploads · no ads |

---

## 🔧 Building

### Requirements
- Windows 10 1809+ / Windows 11
- .NET 8 SDK

### Build & test
```powershell
dotnet build WinIslands.slnx -c Release
dotnet test  WinIslands.slnx -c Release
```

### Publish
```powershell
# Self-contained (includes the .NET 8 runtime, no install needed)
.uild\publish.ps1

# Framework-dependent (small, requires .NET 8 Desktop Runtime)
.uild\publish.ps1 -FrameworkDependent
```
Output goes to `publish\win-x64\`. For stable releases, place the build under `releases\<version>\win-x64\` and rename it with the version number.

### Installer (optional)
Install [Inno Setup 6](https://jrsoftware.org/isinfo.php), then run `iscc.exe build\release-<version>.iss` to produce `releases\<version>\WinIslands-Setup-<version>.exe` (universal installer; auto-installs x64 / ARM64 by architecture).

---

## 🚀 Usage

1. Launch `WinIslands.exe` (startup with Windows is optional); a tray icon appears. Closing the main window does not quit the process — it just minimizes to the tray.
2. Play any music: NetEase Cloud Music, QQ Music, Spotify, Apple Music official app, etc. appear automatically through the system media session; Cider — see below; other players fall back to window-title detection.
3. **Click** the island to expand the full card (hovering does not expand): draggable seek, playback controls, volume, synced lyrics; click again to collapse.
4. Tray menu: show/hide, standalone lyrics window, start with Windows, Do Not Disturb, check for updates, view logs, settings, quit.
5. Global hotkeys (all customizable): `Ctrl+Alt+P` play/pause · `Ctrl+Alt+←/→` previous/next · `Ctrl+Alt+I` show/hide · `Ctrl+Alt+Space` expand/collapse · `Ctrl+Space` quick launcher · `Ctrl+Alt+V` clipboard history panel.
6. Useful command-line arguments: `--demo` demo mode · `--diagnose` writes a diagnostic report · `--settings` opens settings at startup.

### Cider integration
1. Enable "Allow external control" in Cider settings.
2. WinIslands Settings → Media → enable Cider (port auto-detected from `10767` with a local scan, or set manually).
3. While playing, the island source shows `Cider`; you can display artwork/progress/lyrics and control play, seek and volume.

---

## ⚙️ Configuration

Config file: `%APPDATA%\WinIslands\settings.json` (JSON; changes in the settings UI apply instantly, export/import supported).

| Key | Default | Description |
| --- | --- | --- |
| `Language` | `zh-CN` | UI language: `zh-CN` / `en-US` |
| `Position` | `Center` | Position: `Center` top-center / `Right` top-right |
| `Monitor` | `Primary` | Monitor: `Primary` / `All` / `Index` |
| `MonitorIndex` | `0` | Monitor number when `Monitor=Index` |
| `OffsetX` / `OffsetY` | `0` / `8` | Position offset (px) |
| `Opacity` | `0.92` | Opacity 0.3–1.0 |
| `Theme` | `Auto` | `Auto` / `Light` / `Dark` |
| `ThemePreset` | `Default` | Theme preset (18 presets + Custom) |
| `AccentColor` | `#6C5CE7` | Accent color `#RRGGBB` |
| `ThemeScheduledEnabled` | `false` | Scheduled light/dark switching (only when `Theme=Auto`) |
| `ThemeScheduleDarkStartHour` / `EndHour` | `19` / `7` | Dark-period start/end hours |
| `FontScale` / `CornerRadius` | `1.0` / `28` | Font scale 0.8–1.4 / corner radius 16–40 |
| `AnimationStyle` | `Spring` | Motion skin: `Spring` / `Soft` / `Elastic` / `Fade` |
| `CompactWidth` / `CompactHeight` | `360` / `72` | Compact width / height |
| `CompactWidthAuto` / `CompactHeightAuto` | `true` | Auto-fit compact size |
| `ExpandedWidth` / `MaxExpandedHeight` | `400` / `384` | Expanded width / max expanded height |
| `HideWhenNoMedia` | `true` | Hide the island when no media is playing |
| `IslandAlwaysVisible` | `false` | Keep the island always visible (widgets shown without media) |
| `ShowWhenPaused` | `true` | Keep showing while paused |
| `StartWithWindows` | `false` | Start with Windows |
| `IsLocked` | `true` | Locked (drag only after unlocking) |
| `EdgeSnapEnabled` | `true` | Snap to screen edges/center when released |
| `FullScreenAutoHideEnabled` | `true` | Auto-hide on fullscreen |
| `LockScreenAutoHideEnabled` | `true` | Auto-hide on lock screen (Win+L / RDP disconnect), restore on unlock |
| `SingleLineMode` | `true` | One-line mode: all widgets on one row in compact state |
| `DoubleClickAction` | `PlayPause` | Double-click quick action |
| `MiddleClickAction` | `PlayPause` | Middle-click quick action |
| `CiderEnabled` / `CiderPort` | `true` / `0` | Cider local API (0 = auto-detect) |
| `OnlineLyricsEnabled` | `true` | Online lyrics toggle (right-click the island; unofficial API, mind copyright) |
| `AmllTtmlEnabled` | `true` | AMLL word-by-word lyrics (api.amll.dev, unofficial) |
| `KaraokeHighlight` | `true` | Word-by-word karaoke highlighting |
| `StandaloneLyricsWindow` | `false` | Standalone lyrics window |
| `UseSystemVolume` | `true` | Use system volume for non-Cider sources |
| `LowBatteryThreshold` | `20` | Low-battery alert threshold (%), 0 = off |
| `DoNotDisturbManual` | `false` | Manual Do Not Disturb |
| `DoNotDisturbEnabled` | `false` | Scheduled Do Not Disturb |
| `DoNotDisturbStartHour/Minute` | `22` / `0` | DND start (minute precision) |
| `DoNotDisturbEndHour/Minute` | `8` / `0` | DND end (minute precision) |
| `IslandApiEnabled` / `IslandApiPort` | `true` / `9840` | Island API switch / port |
| `IslandApiToken` | `""` | Optional island API token |
| `IslandApiDefaultDuration` | `30` | Default island display duration (s) |
| `WaveStyle` | `Bars` | Audio wave style: `Bars` / `Spectrum` / `Ring` / `Particles` |
| `WidgetOrder` | `Time,Weather,...` | Widget order (comma-separated keys) |
| `Components` | object | Per-widget "no song / playing" two-column checkboxes |
| `MediaApps` | `[]` | Media app enable/disable and priority |
| `Rules` | `[]` | Automation rules (conditions + actions) |
| `ReduceMotion` | `false` | Reduce motion (accessibility/battery) |
| `GlobalHotkeysEnabled` | `true` | Global hotkeys switch |
| `LowPowerMode` | `false` | Low-power mode: lower frame rate when idle, simplified animations |

---

## 🏝 Island API (third-party integration)

Any software can push information to the Dynamic Island through the local HTTP / WebSocket interface, similar to iOS third-party App "Island" integration.

| Method | Path | Description |
| --- | --- | --- |
| POST | `/v1/island/push` | Push/update a card |
| POST | `/v3/island/push` | v1 superset: image / animated progress / heartbeat |
| PATCH | `/v3/island/push/{id}` | Partial update |
| DELETE | `/v1/island/push/{id}` | Remove a card |
| GET | `/v1/island/active` | Query the active card |
| GET | `/v3/ws` | WebSocket two-way channel |
| GET | `/v1/health` | Health check |

Supports: title/body/icon/subtitle, progress, buttons (open link / launch app / run command / notify callback), input fields, images, animated progress, heartbeat renewal, light/dark themes, custom accent color and a priority queue. **Pushes never change the island's width.**

Full documentation: [docs/IslandAPI.md](docs/IslandAPI.md)

---

## 🛡 Privacy & security

- **No telemetry, no ads, no uploads**. The app makes no network requests except user-enabled "online lyrics" and "weather".
- The only networked scenarios: Cider artwork download, AMLL word-by-word lyrics (api.amll.dev), user-enabled online lyrics and weather (Open-Meteo).
- All data is stored locally in `%APPDATA%\WinIslands\`; logs only record local runtime information.

---

## ⚠️ Known limitations

- Word-by-word karaoke depends on the lyrics source and progress; it highlights per character when AMLL TTML / LRC timing exists, otherwise falls back to whole-line highlighting.
- Players occasionally report regressing progress (e.g. Cider/SMTC momentarily reporting 0): a position guard ignores momentary regressions.
- SMTC coverage depends on whether the player registers a global media session; some old players can only be detected via window title (no control buttons).
- Cider 1.x (old API) is not supported; only 2.x and above.
- Online lyrics / AMLL / weather are unofficial interfaces and may break as versions change.

---

## ❓ FAQ

**Q: The island does not appear?**
Make sure media is playing; `HideWhenNoMedia` is enabled by default, so hiding without media is expected. Run `--diagnose` to see the session list.

**Q: Cider shows "Not connected"?**
Make sure "Allow external control" is enabled in Cider settings, check the port (default 10767), and confirm Cider is enabled in WinIslands settings.

**Q: The tray icon is still there after quitting?**
Use Tray menu → Quit; closing the island window only hides it (tray-resident by design).

---

## 📄 License

- Application: MIT (see [LICENSE](LICENSE))
- Third-party components: see [THIRD_PARTY.md](THIRD_PARTY.md)

---

- **🎶 Resaltado de karaoke más fluido (1.2.4)**: el avance letra a letra ahora usa suavizado smoothstep con transiciones cruzadas entre caracteres: el resaltado fluye de izquierda a derecha como una cinta de luz, con aceleración/deceleración, sin pasos bruscos.
- **🔋 Indicador de batería baja persistente (1.2.4)**: cuando la batería baja del umbral y no está cargando, una pequeña píldora de batería permanece en la esquina superior derecha de la isla (roja ≤10 % / naranja en otros casos), actualizándose en tiempo real; desaparece al conectar la carga o recuperarse, y se puede activar en los ajustes.
- **🎧 Animación de conexión de dispositivos (1.2.4)**: al conectar/desconectar un dispositivo Bluetooth se muestra una tarjeta animada estilo iOS; al conectar se lee automáticamente la batería y se muestra «Nombre del dispositivo · Batería xx %» (solo el nombre si no se puede leer), sin bloquear la interfaz.
- **🎤 Estilo de letras ajustable (1.2.3)**: personaliza el tamaño de fuente de las líneas normales/actual, el interlineado, la velocidad del karaoke y los colores de resaltado/base; los cambios se aplican al instante y la ventana de letras independiente los sigue.
- **📋 Historial de notificaciones (1.2.3)**: revisa las notificaciones recientes en la parte inferior de la isla expandida, haz clic para reproducirlas, configura el límite y límpialo con un clic.
- **🔔 Mejoras en notificaciones (1.2.2)**: las alertas plegadas muestran una sola línea (icono + título + resumen de una línea) y se truncan si son largas, sin ensanchar la isla; la isla se ensancha automáticamente para que los widgets de la derecha y el texto se vean completos.
## Español

# WinIslands — Dynamic Island para Windows

> **Lleva el Dynamic Island de iOS a Windows 11 / 10: un widget de Dynamic Island moderno y multifuncional para Windows.**
> Desarrollado con **.NET 8 + WPF**, gratuito y de código abierto (MIT), **sin anuncios · sin telemetría · sin recopilación de datos**.

🌐 Sitio web: https://WinIslands.JudeKwong.com ｜ GitHub: https://github.com/DMP-Pig/WinIslands

---

## ✨ Características destacadas
- **🎶 Resaltado de karaoke más fluido (1.2.4)**: el avance letra a letra ahora usa suavizado smoothstep con transiciones cruzadas entre caracteres: el resaltado fluye de izquierda a derecha como una cinta de luz, con aceleración/deceleración, sin pasos bruscos.
- **🔋 Indicador de batería baja persistente (1.2.4)**: cuando la batería baja del umbral y no está cargando, una pequeña píldora de batería permanece en la esquina superior derecha de la isla (roja ≤10 % / naranja en otros casos), actualizándose en tiempo real; desaparece al conectar la carga o recuperarse, y se puede activar en los ajustes.
- **🎧 Animación de conexión de dispositivos (1.2.4)**: al conectar/desconectar un dispositivo Bluetooth se muestra una tarjeta animada estilo iOS; al conectar se lee automáticamente la batería y se muestra «Nombre del dispositivo · Batería xx %» (solo el nombre si no se puede leer), sin bloquear la interfaz.
- **🎤 Estilo de letras ajustable (1.2.3)**: personaliza el tamaño de fuente de las líneas normales/actual, el interlineado, la velocidad del karaoke y los colores de resaltado/base; los cambios se aplican al instante y la ventana de letras independiente los sigue.
- **📋 Historial de notificaciones (1.2.3)**: revisa las notificaciones recientes en la parte inferior de la isla expandida, haz clic para reproducirlas, configura el límite y límpialo con un clic.

- **🌊 Movimiento sedoso (1.2.1)**: resorte amortiguado estilo iOS + transiciones escalonadas al expandir/contraer; animación vinculada al volumen; capas de opacidad inteligentes; transición de portada; cambio de tema suave; optimización de la animación.

- **▶ Control de reproducción multimedia**: integración nativa con las sesiones multimedia globales de Windows (SMTC), compatible con NetEase Cloud Music, QQ Music, Spotify, Apple Music, Groove, Películas y TV, etc.; además, soporte dedicado para la API local de Cider; si no está disponible, se usa la detección por título de ventana. Portada del álbum, barra de progreso arrastrable (seek), reproducir/pausar/anterior/siguiente, todo incluido; con varios reproductores abiertos puedes cambiar la fuente de control con un clic.
- **♪ Letras karaoke palabra por palabra**: la tarjeta expandida se desplaza con resaltado sincronizado e iluminación palabra por palabra; prioridad de fuentes de cuatro niveles: `.lrc` local → AMLL TTML → API de letras del reproductor → letras en línea opcionales. Letras bilingües, interruptor de traducción, copiar la línea actual con un clic; ajuste fino del tiempo por canción y ventana de letras independiente con opacidad y bloqueo ajustables.
- **▦ Sistema de widgets personalizable**: más de 30 widgets: hora, clima, fecha (con calendario lunar/términos solares), CPU/GPU/RAM/disco, velocidad de red, batería, método de entrada, accesos rápidos (WiFi/Bluetooth/modo nocturno/silencio), etc. Cada widget admite icono personalizado, selección con casillas y reordenación por arrastre, con modos de una línea/varias líneas.
- **🏝 API Island**: una interfaz HTTP/WebSocket local que permite a cualquier software de terceros enviar información al Dynamic Island en tiempo real (como la integración Island de apps de terceros en iOS). Admite imágenes, progreso animado, renovación por heartbeat, temas claro/oscuro, acciones de botones y campos de entrada; los envíos nunca cambian el ancho de la isla ni cubren otros widgets.
- **✨ Animaciones de eventos**: conexión/desconexión de Bluetooth, inicio/fin de carga, batería baja, red restaurada, recordatorios de calendario/RSS/correo, etc., se muestran en la isla con animaciones elegantes.
- **✦ Apariencia y movimiento**: 18 temas preestablecidos, color de acento personalizado, acrílico de vidrio líquido, extracción de color del fondo de pantalla, marquesina, 4 pieles de animación (muelle iOS, etc.), 4 estilos de onda de audio (que pulsan con la música); easing no lineal para expandir/contraer a 60 fps fluidos; soporte de alto DPI PerMonitorV2.
- **🖱 Interacción e inteligencia**: desbloquear para arrastrar con ajuste a bordes, ocultar automáticamente en pantalla completa/pantalla de bloqueo, acciones rápidas de doble clic y clic central, botones de acción rápida, estación de transferencia de archivos (arrastrar archivos a la isla y arrastrarlos fuera a otras apps), No molestar inteligente durante la grabación de pantalla, cambio programado de tema claro/oscuro.
- **⚡ Productividad y automatización**: temporizador Pomodoro, tareas pendientes, historial del portapapeles, lanzador rápido, recordatorios de agenda; asistente de silencio en reuniones, avisos de grabación/captura de pantalla, progreso de copia/descarga de archivos en la isla; atajos globales y motor de reglas.
- **🛡 Privacidad y seguridad**: sin telemetría, sin anuncios, sin cargas de datos. Totalmente sin conexión excepto por las letras en línea y el clima habilitados por el usuario; toda la configuración y los datos permanecen locales en `%APPDATA%\WinIslands`.
- **🔁 Cambio de fuente de letras con un clic**: alterna al instante entre Automática / LRC local / AMLL TTML / API de Cider / Letras en línea, y las letras de la canción actual se recargan de inmediato
- **🛡️ Recuperación automática tras fallos**: después de un cierre anómalo, el siguiente inicio avisa de la recuperación — sin bloqueos, pantallas negras ni estados perdidos
- **⏱️ Duración de animación ajustable**: nuevo control deslizante de duración de expandir/contraer (300–1400ms)


---

## 📥 Descargas (última estable 1.2.4)

| Plataforma | Descarga | Notas |
| --- | --- | --- |
| Windows x64 | [Portátil x64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-x64.exe) | Para PCs de 64 bits convencionales; archivo único, sin instalación |
| Windows ARM64 | [Portátil ARM64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-arm64.exe) | Para Surface Pro X / dispositivos ARM Snapdragon |
| Windows Universal | [Instalador universal](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-Setup-1.2.4.exe) | Asistente Inno Setup; instala x64 / ARM64 según la arquitectura |

Todas las versiones históricas y el registro de cambios completo: [GitHub Releases](https://github.com/DMP-Pig/WinIslands/releases).

---

## 📊 Rendimiento

| Métrica | Valor |
| --- | --- |
| Memoria residente (Private) | ~72 MB |
| Arranque en frío | < 1 s |
| CPU en reposo | ≈ 0% |
| Fotogramas por segundo | 60 fps |
| Instancias múltiples | Instancia única, evita duplicados |
| Telemetría | 0 telemetría · sin cargas · sin anuncios |

---

## 🔧 Compilación

### Requisitos
- Windows 10 1809+ / Windows 11
- SDK de .NET 8

### Compilar y probar
```powershell
dotnet build WinIslands.slnx -c Release
dotnet test  WinIslands.slnx -c Release
```

### Publicar
```powershell
# Autocontenido (incluye el runtime de .NET 8, sin instalación)
.uild\publish.ps1

# Dependiente del framework (pequeño, requiere .NET 8 Desktop Runtime)
.uild\publish.ps1 -FrameworkDependent
```
La salida va a `publish\win-x64\`. Para versiones estables, coloca la compilación en `releases\<versión>\win-x64\` y renómbrala con el número de versión.

### Instalador (opcional)
Instala [Inno Setup 6](https://jrsoftware.org/isinfo.php) y ejecuta `iscc.exe build\release-<versión>.iss` para generar `releases\<versión>\WinIslands-Setup-<versión>.exe` (instalador universal; instala x64 / ARM64 según la arquitectura).

---

## 🚀 Uso

1. Inicia `WinIslands.exe` (opcional: iniciar con Windows); aparece un icono en la bandeja. Cerrar la ventana principal no cierra el proceso: solo minimiza a la bandeja.
2. Reproduce cualquier música: NetEase Cloud Music, QQ Music, Spotify, la app oficial de Apple Music, etc., aparecen automáticamente mediante la sesión multimedia del sistema; Cider, ver más abajo; otros reproductores usan detección por título de ventana.
3. **Haz clic** en la isla para expandir la tarjeta completa (pasar el cursor no expande): seek arrastrable, controles de reproducción, volumen, letras sincronizadas; haz clic de nuevo para contraer.
4. Menú de la bandeja: mostrar/ocultar, ventana de letras independiente, iniciar con Windows, No molestar, buscar actualizaciones, ver registros, ajustes, salir.
5. Atajos globales (todos personalizables): `Ctrl+Alt+P` reproducir/pausar · `Ctrl+Alt+←/→` anterior/siguiente · `Ctrl+Alt+I` mostrar/ocultar · `Ctrl+Alt+Space` expandir/contraer · `Ctrl+Space` lanzador rápido · `Ctrl+Alt+V` panel del historial del portapapeles.
6. Argumentos de línea de comandos útiles: `--demo` modo demostración · `--diagnose` escribe un informe de diagnóstico · `--settings` abre ajustes al inicio.

### Integración con Cider
1. Activa "Allow external control" en los ajustes de Cider.
2. Ajustes de WinIslands → Media → activa Cider (puerto autodetectado desde `10767` con escaneo local, o configúralo manualmente).
3. Mientras se reproduce, la fuente de la isla muestra `Cider`; puedes ver portada/progreso/letras y controlar reproducción, seek y volumen.

---

## ⚙️ Configuración

Archivo de configuración: `%APPDATA%\WinIslands\settings.json` (JSON; los cambios en la interfaz de ajustes se aplican al instante, con exportación/importación).

| Clave | Predeterminado | Descripción |
| --- | --- | --- |
| `Language` | `zh-CN` | Idioma de la interfaz: `zh-CN` / `en-US` |
| `Position` | `Center` | Posición: `Center` arriba-centro / `Right` arriba-derecha |
| `Monitor` | `Primary` | Monitor: `Primary` / `All` / `Index` |
| `MonitorIndex` | `0` | Número de monitor cuando `Monitor=Index` |
| `OffsetX` / `OffsetY` | `0` / `8` | Desplazamiento de posición (px) |
| `Opacity` | `0.92` | Opacidad 0.3–1.0 |
| `Theme` | `Auto` | `Auto` / `Light` / `Dark` |
| `ThemePreset` | `Default` | Tema (18 preajustes + Custom) |
| `AccentColor` | `#6C5CE7` | Color de acento `#RRGGBB` |
| `ThemeScheduledEnabled` | `false` | Cambio programado claro/oscuro (solo con `Theme=Auto`) |
| `ThemeScheduleDarkStartHour` / `EndHour` | `19` / `7` | Horas de inicio/fin del período oscuro |
| `FontScale` / `CornerRadius` | `1.0` / `28` | Escala de fuente 0.8–1.4 / radio de esquina 16–40 |
| `AnimationStyle` | `Spring` | Piel de animación: `Spring` / `Soft` / `Elastic` / `Fade` |
| `CompactWidth` / `CompactHeight` | `360` / `72` | Ancho/alto compacto |
| `CompactWidthAuto` / `CompactHeightAuto` | `true` | Ajuste automático del tamaño compacto |
| `ExpandedWidth` / `MaxExpandedHeight` | `400` / `384` | Ancho expandido / alto máximo expandido |
| `HideWhenNoMedia` | `true` | Ocultar la isla cuando no hay medios |
| `IslandAlwaysVisible` | `false` | Mantener la isla siempre visible (widgets sin medios) |
| `ShowWhenPaused` | `true` | Seguir mostrando en pausa |
| `StartWithWindows` | `false` | Iniciar con Windows |
| `IsLocked` | `true` | Bloqueado (arrastrar solo tras desbloquear) |
| `EdgeSnapEnabled` | `true` | Ajustar a bordes/centro al soltar |
| `FullScreenAutoHideEnabled` | `true` | Ocultar automáticamente en pantalla completa |
| `LockScreenAutoHideEnabled` | `true` | Ocultar en pantalla de bloqueo (Win+L / desconexión RDP), restaurar al desbloquear |
| `SingleLineMode` | `true` | Modo de una línea: todos los widgets en una fila en estado compacto |
| `DoubleClickAction` | `PlayPause` | Acción rápida de doble clic |
| `MiddleClickAction` | `PlayPause` | Acción rápida de clic central |
| `CiderEnabled` / `CiderPort` | `true` / `0` | API local de Cider (0 = autodetección) |
| `OnlineLyricsEnabled` | `true` | Interruptor de letras en línea (clic derecho en la isla; API no oficial, respeta los derechos de autor) |
| `AmllTtmlEnabled` | `true` | Letras palabra por palabra AMLL (api.amll.dev, no oficial) |
| `KaraokeHighlight` | `true` | Resaltado karaoke palabra por palabra |
| `StandaloneLyricsWindow` | `false` | Ventana de letras independiente |
| `UseSystemVolume` | `true` | Usar volumen del sistema para fuentes que no son Cider |
| `LowBatteryThreshold` | `20` | Umbral de batería baja (%), 0 = desactivado |
| `DoNotDisturbManual` | `false` | No molestar manual |
| `DoNotDisturbEnabled` | `false` | No molestar programado |
| `DoNotDisturbStartHour/Minute` | `22` / `0` | Inicio de No molestar (precisión de minutos) |
| `DoNotDisturbEndHour/Minute` | `8` / `0` | Fin de No molestar (precisión de minutos) |
| `IslandApiEnabled` / `IslandApiPort` | `true` / `9840` | Interruptor/puerto de la API Island |
| `IslandApiToken` | `""` | Token opcional de la API Island |
| `IslandApiDefaultDuration` | `30` | Duración predeterminada de visualización (s) |
| `WaveStyle` | `Bars` | Estilo de onda de audio: `Bars` / `Spectrum` / `Ring` / `Particles` |
| `WidgetOrder` | `Time,Weather,...` | Orden de widgets (claves separadas por comas) |
| `Components` | objeto | Casillas de dos columnas por widget "sin canción / reproduciendo" |
| `MediaApps` | `[]` | Habilitar/deshabilitar y prioridad de apps multimedia |
| `Rules` | `[]` | Reglas de automatización (condiciones + acciones) |
| `ReduceMotion` | `false` | Reducir movimiento (accesibilidad/batería) |
| `GlobalHotkeysEnabled` | `true` | Interruptor de atajos globales |
| `LowPowerMode` | `false` | Modo de bajo consumo: menor tasa de fotogramas en reposo, animaciones simplificadas |

---

## 🏝 API Island (integración de terceros)

Cualquier software puede enviar información al Dynamic Island mediante la interfaz HTTP/WebSocket local, similar a la integración "Island" de apps de terceros en iOS.

| Método | Ruta | Descripción |
| --- | --- | --- |
| POST | `/v1/island/push` | Enviar/actualizar una tarjeta |
| POST | `/v3/island/push` | Superconjunto de v1: imagen / progreso animado / heartbeat |
| PATCH | `/v3/island/push/{id}` | Actualización parcial |
| DELETE | `/v1/island/push/{id}` | Eliminar una tarjeta |
| GET | `/v1/island/active` | Consultar la tarjeta activa |
| GET | `/v3/ws` | Canal bidireccional WebSocket |
| GET | `/v1/health` | Comprobación de salud |

Soporta: título/cuerpo/icono/subtítulo, progreso, botones (abrir enlace / iniciar app / ejecutar comando / devolución de llamada notify), campos de entrada, imágenes, progreso animado, renovación por heartbeat, temas claro/oscuro, color de acento personalizado y cola de prioridad. **Los envíos nunca cambian el ancho de la isla.**

Documentación completa: [docs/IslandAPI.md](docs/IslandAPI.md)

---

## 🛡 Privacidad y seguridad

- **Sin telemetría, sin anuncios, sin cargas**. La app no hace solicitudes de red salvo las "letras en línea" y el "clima" habilitados por el usuario.
- Las únicas situaciones con red: descarga de portadas de Cider, letras palabra por palabra AMLL (api.amll.dev), letras en línea y clima habilitados por el usuario (Open-Meteo).
- Todos los datos se almacenan localmente en `%APPDATA%\WinIslands\`; los registros solo guardan información local de ejecución.

---

## ⚠️ Limitaciones conocidas

- El karaoke palabra por palabra depende de la fuente de letras y el progreso; resalta por carácter cuando existe temporización AMLL TTML / LRC; si no, vuelve al resaltado por línea completa.
- A veces los reproductores informan progreso hacia atrás (p. ej. Cider/SMTC reportando 0 momentáneamente): un guardián de posición ignora las regresiones momentáneas.
- La cobertura SMTC depende de si el reproductor registra una sesión multimedia global; algunos reproductores antiguos solo se detectan por título de ventana (sin botones de control).
- Cider 1.x (API antigua) no es compatible; solo 2.x y superiores.
- Las letras en línea / AMLL / el clima son interfaces no oficiales y pueden fallar cuando cambian las versiones.

---

## ❓ Preguntas frecuentes

**P: ¿La isla no aparece?**
Asegúrate de que haya medios reproduciéndose; `HideWhenNoMedia` está activado por defecto, así que ocultarse sin medios es lo esperado. Ejecuta `--diagnose` para ver la lista de sesiones.

**P: ¿Cider muestra "No conectado"?**
Asegúrate de que "Allow external control" esté activado en los ajustes de Cider, comprueba el puerto (por defecto 10767) y confirma que Cider esté habilitado en los ajustes de WinIslands.

**P: ¿El icono de la bandeja sigue ahí después de salir?**
Usa Menú de la bandeja → Salir; cerrar la ventana de la isla solo la oculta (diseño residente en la bandeja).

---

## 📄 Licencia

- Aplicación: MIT (ver [LICENSE](LICENSE))
- Componentes de terceros: ver [THIRD_PARTY.md](THIRD_PARTY.md)

---

- **🎶 Karaoké fluide affiné (1.2.4)** : la progression mot à mot utilise désormais un lissage smoothstep avec transitions croisées entre les lettres — la surbrillance coule de gauche à droite comme un faisceau de lumière, avec accélération/décélération, sans à-coups.
- **🔋 Indicateur de batterie faible persistant (1.2.4)** : lorsque la batterie descend sous le seuil sans charge, une petite pilule de batterie reste affichée en haut à droite de l’île (rouge ≤10 % / orange sinon), mise à jour en temps réel ; elle disparaît automatiquement une fois branché ou rechargé, et peut être activée dans les réglages.
- **🎧 Animation de connexion d’appareils (1.2.4)** : la connexion/déconnexion Bluetooth affiche désormais une carte animée style iOS ; à la connexion, le niveau de batterie de l’appareil est lu automatiquement et affiché comme « Nom de l’appareil · Batterie xx % » (nom seul si la batterie n’est pas lisible), sans bloquer l’interface.
- **🎤 Style des paroles réglable (1.2.3)** : personnalisez la taille de police des lignes normales/actuelle, l’interligne, la vitesse du karaoké et les couleurs de surbrillance/base ; les changements s’appliquent instantanément et la fenêtre de paroles indépendante suit.
- **📋 Historique des notifications (1.2.3)** : consultez les notifications récentes en bas de l’île déployée, cliquez pour les rejouer, configurez le nombre maximum et effacez en un clic.
- **🔔 Notifications optimisées (1.2.2)** : les alertes repliées affichent une seule ligne (icône + titre + résumé sur une ligne) et sont tronquées si longues, sans élargir l'île ; l'île s'élargit automatiquement pour que les widgets à droite et le texte restent entièrement visibles.
## Français

# WinIslands — Dynamic Island pour Windows

> **Apportez le Dynamic Island d'iOS à Windows 11 / 10 — un widget Dynamic Island moderne et multifonctionnel pour Windows.**
> Basé sur **.NET 8 + WPF**, gratuit et open source (MIT), **sans publicité · sans télémétrie · sans collecte de données**.

🌐 Site web : https://WinIslands.JudeKwong.com ｜ GitHub : https://github.com/DMP-Pig/WinIslands

---

## ✨ Points forts
- **🎶 Karaoké fluide affiné (1.2.4)** : la progression mot à mot utilise désormais un lissage smoothstep avec transitions croisées entre les lettres — la surbrillance coule de gauche à droite comme un faisceau de lumière, avec accélération/décélération, sans à-coups.
- **🔋 Indicateur de batterie faible persistant (1.2.4)** : lorsque la batterie descend sous le seuil sans charge, une petite pilule de batterie reste affichée en haut à droite de l’île (rouge ≤10 % / orange sinon), mise à jour en temps réel ; elle disparaît automatiquement une fois branché ou rechargé, et peut être activée dans les réglages.
- **🎧 Animation de connexion d’appareils (1.2.4)** : la connexion/déconnexion Bluetooth affiche désormais une carte animée style iOS ; à la connexion, le niveau de batterie de l’appareil est lu automatiquement et affiché comme « Nom de l’appareil · Batterie xx % » (nom seul si la batterie n’est pas lisible), sans bloquer l’interface.
- **🎤 Style des paroles réglable (1.2.3)** : personnalisez la taille de police des lignes normales/actuelle, l’interligne, la vitesse du karaoké et les couleurs de surbrillance/base ; les changements s’appliquent instantanément et la fenêtre de paroles indépendante suit.
- **📋 Historique des notifications (1.2.3)** : consultez les notifications récentes en bas de l’île déployée, cliquez pour les rejouer, configurez le nombre maximum et effacez en un clic.

- **🌊 Mouvements fluides (1.2.1)** : ressort amorti façon iOS + transitions en cascade à l'ouverture/au repli ; animation liée au volume ; opacité intelligente en couches ; transition de couverture ; changement de thème en douceur ; optimisation des animations.

- **▶ Contrôle de lecture multimédia** : intégration native des sessions multimédia globales de Windows (SMTC), compatible avec NetEase Cloud Music, QQ Music, Spotify, Apple Music, Groove, Films et TV, etc. ; plus une prise en charge dédiée de l'API locale de Cider ; en dernier recours, détection par titre de fenêtre. Pochette d'album, barre de progression déplaçable (seek), lecture/pause/précédent/suivant, tout est inclus ; avec plusieurs lecteurs ouverts, basculez la source de contrôle en un clic.
- **♪ Paroles karaoké mot à mot** : la carte dépliée défile avec une mise en évidence synchronisée et un éclairage mot à mot ; priorité des sources à quatre niveaux : `.lrc` local → AMLL TTML → API de paroles du lecteur → paroles en ligne facultatives. Paroles bilingues, interrupteur de traduction, copie de la ligne courante en un clic ; réglage fin du minutage par chanson et fenêtre de paroles indépendante avec opacité et verrouillage réglables.
- **▦ Système de widgets personnalisable** : plus de 30 widgets — heure, météo, date (avec calendrier lunaire/termes solaires), CPU/GPU/RAM/disque, vitesse réseau, batterie, méthode de saisie, bascules rapides (WiFi/Bluetooth/mode nuit/silencieux), etc. Chaque widget prend en charge une icône personnalisée, une sélection par cases à cocher et un réordonnancement par glisser-déposer, avec modes une ligne / plusieurs lignes.
- **🏝 API Island** : une interface HTTP / WebSocket locale qui permet à tout logiciel tiers de pousser des informations vers le Dynamic Island en temps réel (comme l'intégration Island des apps tierces sur iOS). Prend en charge les images, la progression animée, le renouvellement par heartbeat, les thèmes clair/sombre, les actions de boutons et les champs de saisie ; les envois ne modifient jamais la largeur de l'île et ne couvrent pas les autres widgets.
- **✨ Animations d'événements** : connexion/déconnexion Bluetooth, début/fin de charge, batterie faible, réseau rétabli, rappels de calendrier/RSS/e-mail, etc., affichés sur l'île avec des animations élégantes.
- **✦ Apparence et animation** : 18 thèmes prédéfinis, couleur d'accent personnalisée, acrylique verre liquide, extraction de couleur du fond d'écran, défilement défilant, 4 peaux d'animation (ressort iOS, etc.), 4 styles d'onde audio (palpitant au rythme de la musique) ; easing non linéaire pour déplier/replier à 60 fps fluides ; prise en charge haute DPI PerMonitorV2.
- **🖱 Interaction et intelligence** : déverrouillage pour glisser avec magnétisme aux bords, masquage automatique en plein écran / sur l'écran de verrouillage, actions rapides double-clic et clic central, boutons d'action rapide, station de transfert de fichiers (glisser des fichiers vers l'île puis vers d'autres applications), Ne pas déranger intelligent pendant l'enregistrement d'écran, bascule programmée du thème clair/sombre.
- **⚡ Productivité et automatisation** : minuteur Pomodoro, tâches, historique du presse-papiers, lanceur rapide, rappels d'agenda ; assistant de mise en sourdine en réunion, alertes d'enregistrement d'écran / de capture, progression de copie/téléchargement de fichiers sur l'île ; raccourcis globaux et moteur de règles.
- **🛡 Confidentialité et sécurité** : aucune télémétrie, aucune publicité, aucune remontée de données. Entièrement hors ligne sauf paroles en ligne et météo activées manuellement ; toute la configuration et les données restent locales dans `%APPDATA%\WinIslands`.
- **🔁 Changement de source des paroles en un clic** : basculez instantanément entre Auto / LRC local / AMLL TTML / API Cider / Paroles en ligne, et les paroles de la chanson en cours sont rechargées aussitôt
- **🛡️ Récupération automatique après un plantage** : après une fermeture anormale, le prochain lancement notifie la récupération — plus de blocages, d'écran noir ni d'état perdu
- **⏱️ Durée d'animation réglable** : nouveau curseur de durée de déploiement/repli (300–1400ms)


---

## 📥 Téléchargement (dernière version stable 1.2.4)

| Plateforme | Téléchargement | Notes |
| --- | --- | --- |
| Windows x64 | [Portable x64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-x64.exe) | Le choix recommandé pour les PC 64 bits ; fichier unique, sans installation, exécution directe |
| Windows ARM64 | [Portable ARM64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-arm64.exe) | Pour Surface Pro X / appareils ARM Snapdragon |
| Windows universel | [Installeur universel](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-Setup-1.2.4.exe) | Assistant Inno Setup ; installe x64 / ARM64 selon l'architecture |

Toutes les versions historiques et le journal complet : [GitHub Releases](https://github.com/DMP-Pig/WinIslands/releases).

---

## 📊 Performances

| Indicateur | Valeur |
| --- | --- |
| Mémoire résidente (Private) | ~72 Mo |
| Démarrage à froid | < 1 s |
| CPU au repos | ≈ 0% |
| Taux d'animation | 60 fps |
| Instances multiples | Instance unique, anti-doublon |
| Télémétrie | 0 télémétrie · aucune remontée · aucune publicité |

---

## 🔧 Compilation

### Prérequis
- Windows 10 1809+ / Windows 11
- SDK .NET 8

### Compiler et tester
```powershell
dotnet build WinIslands.slnx -c Release
dotnet test  WinIslands.slnx -c Release
```

### Publier
```powershell
# Autonome (contient le runtime .NET 8, aucune installation requise)
.\build\publish.ps1

# Dépendant du framework (léger, nécessite .NET 8 Desktop Runtime)
.\build\publish.ps1 -FrameworkDependent
```
Les artefacts se trouvent dans `publish\win-x64\`. Les versions stables sont placées dans `releases\<version>\win-x64\` et renommées selon le numéro de version.

### Installeur (facultatif)
Installez [Inno Setup 6](https://jrsoftware.org/isinfo.php), puis exécutez `iscc.exe build\release-<version>.iss` pour générer `releases\<version>\WinIslands-Setup-<version>.exe` (installeur universel, installe x64 / ARM64 selon l'architecture).

---

## 🚀 Utilisation

1. Lancez `WinIslands.exe` (démarrage automatique configurable) ; l'icône apparaît dans la barre d'état ; fermer la fenêtre principale ne quitte pas le processus (réduit dans la barre d'état).
2. Lisez de la musique : NetEase Cloud Music, QQ Music, Spotify, Apple Music officiel, etc. s'affichent automatiquement via la session multimédia système ; pour Cider voir ci-dessous ; les autres lecteurs utilisent la détection par titre de fenêtre.
3. **Cliquez** sur l'île pour déplier la carte complète (le survol ne déplie pas) : seek par glissement de la progression, contrôle de lecture, volume, paroles synchronisées ; recliquez pour replier.
4. Menu de la barre d'état : afficher/masquer, fenêtre de paroles indépendante, démarrage automatique, Ne pas déranger, rechercher les mises à jour, voir les journaux, paramètres, quitter.
5. Raccourcis globaux (tous personnalisables) : `Ctrl+Alt+P` lecture/pause · `Ctrl+Alt+←/→` précédent/suivant · `Ctrl+Alt+I` afficher/masquer · `Ctrl+Alt+Espace` déplier/replier · `Ctrl+Espace` lanceur rapide · `Ctrl+Alt+V` panneau de l'historique du presse-papiers.
6. Arguments de ligne de commande courants : `--demo` mode démo · `--diagnose` rapport de diagnostic · `--settings` ouvre les paramètres au démarrage.

### Intégration Cider
1. Dans les paramètres de Cider, activez « Autoriser le contrôle externe » (Allow external control).
2. Paramètres WinIslands → Média → activer Cider (port détecté automatiquement `10767` et scan local, ou saisie manuelle).
3. Pendant la lecture, la source affichée sur l'île est `Cider` : pochette/progression/paroles, contrôle lecture, seek, volume.

---

## ⚙️ Options de configuration

Fichier de configuration : `%APPDATA%\WinIslands\settings.json` (JSON ; les modifications dans l'interface prennent effet immédiatement, export/import possibles).

| Clé | Défaut | Description |
| --- | --- | --- |
| `Language` | `zh-CN` | Langue de l'interface : `zh-CN` / `en-US` |
| `Position` | `Center` | Position : `Center` haut centré / `Right` haut à droite |
| `Monitor` | `Primary` | Moniteur : `Primary` écran principal / `All` tous / `Index` écran choisi |
| `MonitorIndex` | `0` | Numéro de l'écran choisi |
| `OffsetX` / `OffsetY` | `0` / `8` | Décalage de position (pixels) |
| `Opacity` | `0.92` | Opacité 0.3–1.0 |
| `Theme` | `Auto` | `Auto` / `Light` / `Dark` |
| `ThemePreset` | `Default` | Peau de thème (18 préréglages + Custom) |
| `AccentColor` | `#6C5CE7` | Couleur d'accent `#RRGGBB` |
| `ThemeScheduledEnabled` | `false` | Bascule programmée clair/sombre (uniquement avec `Theme=Auto`) |
| `ThemeScheduleDarkStartHour` / `EndHour` | `19` / `7` | Heures de début/fin de la période sombre |
| `FontScale` / `CornerRadius` | `1.0` / `28` | Échelle de police 0.8–1.4 / rayon d'angle 16–40 |
| `AnimationStyle` | `Spring` | Peau d'animation : `Spring` / `Soft` / `Elastic` / `Fade` |
| `CompactWidth` / `CompactHeight` | `360` / `72` | Longueur / hauteur compactes |
| `CompactWidthAuto` / `CompactHeightAuto` | `true` | Ajustement automatique de la taille compacte |
| `ExpandedWidth` / `MaxExpandedHeight` | `400` / `384` | Longueur dépliée / hauteur maximale dépliée |
| `HideWhenNoMedia` | `true` | Masquer l'île quand aucun média ne joue |
| `IslandAlwaysVisible` | `false` | Île toujours visible (affiche les widgets même sans média) |
| `ShowWhenPaused` | `true` | Toujours afficher en pause |
| `StartWithWindows` | `false` | Démarrage avec Windows |
| `IsLocked` | `true` | Verrouillé (une fois déverrouillé, la souris peut faire glisser) |
| `EdgeSnapEnabled` | `true` | Aimantation automatique au bord / centrage au relâchement |
| `FullScreenAutoHideEnabled` | `true` | Masquage automatique en plein écran |
| `LockScreenAutoHideEnabled` | `true` | Masquage automatique sur l'écran de verrouillage (Win+L / déconnexion bureau à distance), restauration après déverrouillage |
| `SingleLineMode` | `true` | Mode une ligne : tous les widgets sur une ligne à l'état compact |
| `DoubleClickAction` | `PlayPause` | Action rapide au double-clic sur l'île |
| `MiddleClickAction` | `PlayPause` | Action rapide au clic central sur l'île |
| `CiderEnabled` / `CiderPort` | `true` / `0` | API locale Cider (0 = détection automatique) |
| `OnlineLyricsEnabled` | `true` | Paroles en ligne (bascule par clic droit sur l'île ; interface non officielle, attention au droit d'auteur) |
| `AmllTtmlEnabled` | `true` | Paroles mot à mot AMLL (api.amll.dev, non officiel) |
| `KaraokeHighlight` | `true` | Mise en évidence karaoké mot à mot |
| `StandaloneLyricsWindow` | `false` | Fenêtre de paroles indépendante |
| `UseSystemVolume` | `true` | Volume système pour les sources non-Cider |
| `LowBatteryThreshold` | `20` | Seuil d'alerte batterie faible (%), 0 = désactivé |
| `DoNotDisturbManual` | `false` | Ne pas déranger manuel |
| `DoNotDisturbEnabled` | `false` | Ne pas déranger programmé (notifications silencieuses par plage horaire) |
| `DoNotDisturbStartHour/Minute` | `22` / `0` | Début du Ne pas déranger (à la minute) |
| `DoNotDisturbEndHour/Minute` | `8` / `0` | Fin du Ne pas déranger (à la minute) |
| `IslandApiEnabled` / `IslandApiPort` | `true` / `9840` | Interrupteur / port de l'API Island |
| `IslandApiToken` | `""` | Jeton facultatif de l'API Island |
| `IslandApiDefaultDuration` | `30` | Durée d'affichage par défaut (secondes) |
| `WaveStyle` | `Bars` | Style d'onde audio : `Bars` / `Spectrum` / `Ring` / `Particles` |
| `WidgetOrder` | `Time,Weather,...` | Ordre des widgets (clés séparées par des virgules) |
| `Components` | objet | Cases « sans musique / avec musique » pour chaque widget |
| `MediaApps` | `[]` | Activation/désactivation et priorité des lecteurs média |
| `Rules` | `[]` | Règles d'automatisation (condition + action) |
| `ReduceMotion` | `false` | Réduire les animations (accessibilité/économie d'énergie) |
| `GlobalHotkeysEnabled` | `true` | Interrupteur des raccourcis globaux |
| `LowPowerMode` | `false` | Mode basse consommation : réduction de la fréquence au repos, animations simplifiées |

---

## 🏝 API Island (intégration tierce)

Tout logiciel peut pousser des informations vers le Dynamic Island via l'interface HTTP / WebSocket locale, comme l'intégration « Island » des apps tierces sur iOS.

| Méthode | Chemin | Description |
| --- | --- | --- |
| POST | `/v1/island/push` | Pousser / mettre à jour une carte |
| POST | `/v3/island/push` | Sur-ensemble de v1 : image / progression animée / heartbeat |
| PATCH | `/v3/island/push/{id}` | Mise à jour partielle |
| DELETE | `/v1/island/push/{id}` | Retirer une carte |
| GET | `/v1/island/active` | Interroger la carte active |
| GET | `/v3/ws` | Canal WebSocket bidirectionnel |
| GET | `/v1/health` | Contrôle de santé |

Prend en charge : titre/corps/icône/sous-titre, progression, boutons (ouvrir un lien / lancer un programme / exécuter une commande / rappel notify), champs de saisie, images, progression animée, renouvellement par heartbeat, thèmes clair/sombre, couleur d'accent personnalisée, file de priorité. **L'envoi ne modifie pas la largeur de l'île**.

Documentation complète : [docs/IslandAPI.md](docs/IslandAPI.md)

---

## 🛡 Confidentialité et sécurité

- **Aucune télémétrie, aucune publicité, aucune remontée**. Sauf « paroles en ligne » et « météo » activées manuellement, l'application ne fait aucune requête réseau.
- Seules connexions réseau : téléchargement des pochettes Cider, paroles mot à mot AMLL (api.amll.dev), paroles en ligne et météo après activation (Open-Meteo).
- Toutes les données sont stockées localement dans `%APPDATA%\WinIslands\` ; les journaux n'enregistrent que des informations d'exécution locales.

---

## ⚠️ Limites connues

- Le karaoké mot à mot dépend de la source et de la progression des paroles : avec une timeline mot à mot AMLL TTML / LRC, mise en évidence par mot, sinon repli sur la mise en évidence de la phrase entière.
- Certains lecteurs signalent parfois une progression qui recule (ex. Cider/SMTC signalent brièvement 0) : une garde de position ignore les retours instantanés.
- La couverture SMTC dépend de l'enregistrement de la session multimédia globale par le lecteur ; certains anciens lecteurs ne sont détectés que par titre de fenêtre (sans boutons de contrôle).
- Cider 1.x (ancienne API) non adapté, uniquement 2.x et supérieur.
- Paroles en ligne / AMLL / météo sont des interfaces non officielles, susceptibles de cesser de fonctionner.

---

## ❓ Questions fréquentes

**Q : L'île n'apparaît pas ?**
Vérifiez que quelque chose est en lecture ; `HideWhenNoMedia` est activé par défaut, le masquage sans média est normal. Lancez `--diagnose` pour voir la liste des sessions.

**Q : Cider affiche « non connecté » ?**
Vérifiez que « Autoriser le contrôle externe » est activé dans les paramètres de Cider, contrôlez le port (défaut 10767) et confirmez que Cider est activé dans les paramètres de WinIslands.

**Q : L'icône de la barre d'état reste après la sortie ?**
Menu de la barre d'état → Quitter ; fermer la fenêtre de l'île ne fait que la masquer (l'île reste résidente par conception).

---

## 📄 Licence open source

- Application : MIT (voir [LICENSE](LICENSE))
- Composants tiers : voir [THIRD_PARTY.md](THIRD_PARTY.md)

---

- **🎶 تحسين انسياب كلمات الكاريوكي (1.2.4)**: أصبح تقدّم الحروف يستخدم تدرّجًا سلسًا (smoothstep) مع انتقالات متداخلة بين الأحرف — يشعّ التمييز من اليسار إلى اليمين كشريط ضوئي متواصل، مع تسارع/تباطؤ في البداية والنهاية، دون توقّفات مفاجئة.
- **🔋 مؤشر انخفاض البطارية الدائم (1.2.4)**: عندما تنخفض البطارية دون الحدّ المحدد دون شحن، تبقى حبّة بطارية صغيرة في الزاوية العلوية اليمنى من الجزيرة (حمراء ≤10% / برتقالية في غير ذلك)، وتتحدّث لحظيًا؛ تختفي تلقائيًا عند التوصيل بالشاحن أو ارتفاع الشحن، ويمكن إيقافها من الإعدادات.
- **🎧 حركة اتصال الأجهزة (1.2.4)**: عند اتصال/انقطاع جهاز بلوتوث تظهر بطاقة متحركة بأسلوب iOS؛ وعند الاتصال تُقرأ بطارية الجهاز تلقائيًا ويُعرض «اسم الجهاز · البطارية xx%»، وعند تعذّر القراءة يُعرض الاسم فقط، دون تعطيل الواجهة.
- **🎤 ضبط نمط الكلمات (1.2.3)**: خصّص حجم خط الكلمات العادية/الحالية وتباعد الأسطر وسرعة الكاريوكي وألوان التمييز/الأساس؛ تُطبَّق التغييرات فورًا وتتبعها نافذة الكلمات المستقلة.
- **📋 سجل الإشعارات (1.2.3)**: اعرض الإشعارات الأخيرة أسفل الجزيرة الموسّعة، وانقر لإعادة عرضها، وحدّد الحد الأقصى وامسح بنقرة واحدة.
- **🔔 تحسين الإشعارات والتنبيهات (1.2.2)**: تعرض التنبيهات المطوية سطرًا واحدًا (أيقونة + عنوان + ملخص سطر واحد) وتُقتطع عند الطول، فلا توسّع الجزيرة؛ وتتوسع الجزيرة تلقائيًا لعرض أدوات الجانب الأيمن والنص كاملًا.
## العربية

# WinIslands — ديناميك آيلاند لويندوز

> **انقل «الديناميك آيلاند» من iOS إلى ويندوز 11 / 10 — أداة ديناميك آيلاند عصرية ومتعددة الوظائف لويندوز.**
> مبني على **.NET 8 + WPF**، مجاني ومفتوح المصدر (MIT)، **بدون إعلانات · بدون تتبع عن بُعد · بدون جمع بيانات**.

🌐 الموقع: https://WinIslands.JudeKwong.com ｜ GitHub: https://github.com/DMP-Pig/WinIslands

---

## ✨ أبرز المزايا
- **🎶 تحسين انسياب كلمات الكاريوكي (1.2.4)**: أصبح تقدّم الحروف يستخدم تدرّجًا سلسًا (smoothstep) مع انتقالات متداخلة بين الأحرف — يشعّ التمييز من اليسار إلى اليمين كشريط ضوئي متواصل، مع تسارع/تباطؤ في البداية والنهاية، دون توقّفات مفاجئة.
- **🔋 مؤشر انخفاض البطارية الدائم (1.2.4)**: عندما تنخفض البطارية دون الحدّ المحدد دون شحن، تبقى حبّة بطارية صغيرة في الزاوية العلوية اليمنى من الجزيرة (حمراء ≤10% / برتقالية في غير ذلك)، وتتحدّث لحظيًا؛ تختفي تلقائيًا عند التوصيل بالشاحن أو ارتفاع الشحن، ويمكن إيقافها من الإعدادات.
- **🎧 حركة اتصال الأجهزة (1.2.4)**: عند اتصال/انقطاع جهاز بلوتوث تظهر بطاقة متحركة بأسلوب iOS؛ وعند الاتصال تُقرأ بطارية الجهاز تلقائيًا ويُعرض «اسم الجهاز · البطارية xx%»، وعند تعذّر القراءة يُعرض الاسم فقط، دون تعطيل الواجهة.
- **🎤 ضبط نمط الكلمات (1.2.3)**: خصّص حجم خط الكلمات العادية/الحالية وتباعد الأسطر وسرعة الكاريوكي وألوان التمييز/الأساس؛ تُطبَّق التغييرات فورًا وتتبعها نافذة الكلمات المستقلة.
- **📋 سجل الإشعارات (1.2.3)**: اعرض الإشعارات الأخيرة أسفل الجزيرة الموسّعة، وانقر لإعادة عرضها، وحدّد الحد الأقصى وامسح بنقرة واحدة.

- **🌊 حركة سلسة (1.2.1)**: زنبرك مخمّد بأسلوب iOS + انتقالات متدرّجة للطي/الفتح؛ حركة مرتبطة بمستوى الصوت؛ طبقات شفافية ذكية؛ انتقال الغلاف؛ تبديل ثيم سلس؛ تحسين أداء الحركة.

- **▶ التحكم في تشغيل الوسائط**: تكامل أصلي مع جلسات الوسائط العامة في ويندوز (SMTC)، متوافق مع NetEase Cloud Music وQQ Music وSpotify وApple Music وGroove والأفلام والتلفزيون وغيرها؛ بالإضافة إلى دعم مخصص لواجهة Cider المحلية؛ وعند عدم التوفّر، كشف عبر عنوان النافذة كحل احتياطي. غلاف الألبوم، وشريط تقدّم قابل للسحب (seek)، وتشغيل/إيقاف/التالي/السابق — كل ذلك مُتضمّن؛ وعند فتح أكثر من مشغّل يمكن تبديل مصدر التحكم بنقرة واحدة.
- **♪ كلمات كاريوكي كلمةً كلمة**: البطاقة الموسّعة تتمرّر مع إبراز متزامن وإضاءة كلمةً كلمة؛ أولوية المصادر بأربعة مستويات: `.lrc` المحلي ← AMLL TTML ← واجهة كلمات المشغّل ← الكلمات عبر الإنترنت اختياريًا. كلمات ثنائية اللغة، ومفتاح ترجمة، ونسخ السطر الحالي بنقرة واحدة؛ ضبط دقيق لتوقيت كل أغنية ونافذة كلمات مستقلة مع شفافية وقفل قابلين للضبط.
- **▦ نظام أدوات قابل للتخصيص**: أكثر من 30 أداة — الوقت والطقس والتاريخ (مع التقويم القمري/المواسم) وCPU/GPU/الذاكرة/القرص وسرعة الشبكة والبطارية وطريقة الإدخال والمفاتيح السريعة (WiFi/البلوتوث/الوضع الليلي/كتم الصوت) وغيرها. تدعم كل أداة أيقونة مخصصة واختيارًا بخانات وترتيبًا بالسحب، مع وضعَي سطر واحد / عدة أسطر.
- **🏝 واجهة Island**: واجهة HTTP / WebSocket محلية تتيح لأي برنامج خارجي دفع المعلومات إلى الديناميك آيلاند في الوقت الفعلي (مثل تكامل Island لتطبيقات iOS الخارجية). تدعم الصور والتقدّم المتحرك وتجديد heartbeat والسمات الفاتحة/الداكنة وأفعال الأزرار وحقول الإدخال؛ لا تغيّر عمليات الدفع عرض الجزيرة ولا تُغطّي الأدوات الأخرى.
- **✨ حركات الأحداث**: اتصال/فصل البلوتوث، بدء/اكتمال الشحن، انخفاض البطارية، استعادة الشبكة، تذكيرات التقويم/RSS/البريد وغيرها تُعرض على الجزيرة بحركات أنيقة.
- **✦ المظهر والحركة**: 18 سمة جاهزة، ولون تمييز مخصص، وزجاج سائل/أكريليك، واستخراج لون الخلفية، وشريط متحرك، و4 أساليب حركة (نابض iOS وغيرها)، و4 أنماط لموجة الصوت (تنبض مع إيقاع الموسيقى)؛ انسياب غير خطي للتمديد/الطيّ بسلاسة 60 إطارًا في الثانية؛ دعم دقة عالية PerMonitorV2.
- **🖱 التفاعل والذكاء**: إلغاء القفل للسحب مع الالتصاق بالحواف، إخفاء تلقائي في ملء الشاشة/شاشة القفل، أفعال سريعة بنقرة مزدوجة/نقرة الوسط، أزرار أفعال سريعة، محطة نقل الملفات (سحب الملفات إلى الجزيرة وإخراجها إلى تطبيقات أخرى)، «لا تُزعج» ذكي أثناء تسجيل الشاشة، تبديل مجدول للسمة الفاتحة/الداكنة.
- **⚡ الإنتاجية والأتمتة**: مؤقّت بومودورو، مهام، سجل الحافظة، مشغّل سريع، تذكيرات مواعيد؛ مساعد كتم الميكروفون في الاجتماعات، تنبيهات تسجيل/التقاط الشاشة، تقدّم نسخ/تنزيل الملفات على الجزيرة؛ اختصارات عامة ومحرك قواعد.
- **🛡 الخصوصية والأمان**: لا تتبع، لا إعلانات، لا رفع بيانات. يعمل دون اتصال تمامًا ما عدا الكلمات عبر الإنترنت والطقس عند تفعيلهما يدويًا؛ جميع الإعدادات والبيانات محلية في `%APPDATA%\WinIslands`.
- **🔁 التبديل السريع لمصدر الكلمات بنقرة واحدة**: بدّل فوراً بين تلقائي / LRC محلي / AMLL TTML / واجهة Cider / الكلمات عبر الإنترنت، مع إعادة تحميل كلمات الأغنية الحالية مباشرة
- **🛡️ الاسترداد التلقائي عند الأعطال**: بعد خروج غير طبيعي، يشير التشغيل التالي إلى الاسترداد — بلا تجمّد أو شاشة سوداء أو فقدان الحالة
- **⏱️ مدة حركة قابلة للضبط**: شريط تمرير جديد لمدة حركة الفتح/الطي (300–1400ms)


---

## 📥 التحميل (آخر إصدار مستقر 1.2.4)

| النظام | التحميل | ملاحظات |
| --- | --- | --- |
| Windows x64 | [نسخة محمولة x64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-x64.exe) | الخيار الأول لأجهزة 64 بت الشائعة؛ ملف واحد بدون تثبيت، يعمل مباشرة |
| Windows ARM64 | [نسخة محمولة ARM64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-arm64.exe) | لأجهزة Surface Pro X / أجهزة ARM Snapdragon |
| Windows شامل | [مثبّت شامل](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-Setup-1.2.4.exe) | معالج Inno Setup؛ يثبّت x64 / ARM64 حسب البنية |

جميع الإصدارات السابقة وسجل التغييرات الكامل: [GitHub Releases](https://github.com/DMP-Pig/WinIslands/releases).

---

## 📊 مؤشرات الأداء

| المؤشر | القيمة |
| --- | --- |
| الذاكرة الدائمة (Private) | ~72 ميجابايت |
| الإقلاع البارد | < 1 ثانية |
| CPU في الخمول | ≈ 0% |
| معدل الحركة | 60 إطارًا/ثانية |
| عدة نسخ | نسخة واحدة، منع التكرار |
| التتبع عن بُعد | 0 تتبع · بدون رفع · بدون إعلانات |

---

## 🔧 البناء

### المتطلبات
- ويندوز 10 1809+ / ويندوز 11
- .NET 8 SDK

### البناء والاختبار
```powershell
dotnet build WinIslands.slnx -c Release
dotnet test  WinIslands.slnx -c Release
```

### النشر
```powershell
# مستقل (يتضمن وقت تشغيل .NET 8، بدون تثبيت)
.\build\publish.ps1

# معتمد على الإطار (خفيف، يتطلب .NET 8 Desktop Runtime)
.\build\publish.ps1 -FrameworkDependent
```
الملفات الناتجة في `publish\win-x64\`. توضع الإصدارات المستقرة في `releases\<الإصدار>\win-x64\` وتُعاد تسميتها حسب رقم الإصدار.

### المثبّت (اختياري)
ثبّت [Inno Setup 6](https://jrsoftware.org/isinfo.php)، ثم نفّذ `iscc.exe build\release-<الإصدار>.iss` لتوليد `releases\<الإصدار>\WinIslands-Setup-<الإصدار>.exe` (مثبّت شامل، x64 / ARM64 حسب البنية).

---

## 🚀 طريقة الاستخدام

1. شغّل `WinIslands.exe` (يمكن ضبط التشغيل مع بدء تشغيل ويندوز)؛ يظهر رمز في علبة النظام؛ إغلاق النافذة الرئيسية لا يُنهي العملية — يُصغّر إلى العلبة فقط.
2. شغّل أي موسيقى: NetEase Cloud Music وQQ Music وSpotify وApple Music الرسمي وغيرها تظهر تلقائيًا عبر جلسة الوسائط؛ أما Cider فانظر أدناه؛ المشغّلات الأخرى تُكتشف عبر عنوان النافذة.
3. **انقر** على الجزيرة لتمديد البطاقة الكاملة (التمرير لا يمدّد): سحب التقدّم للانتقال (seek)، التحكم في التشغيل، مستوى الصوت، كلمات متزامنة؛ انقر مجددًا للطيّ.
4. قائمة العلبة: إظهار/إخفاء، نافذة كلمات مستقلة، التشغيل مع ويندوز، «لا تُزعج»، البحث عن التحديثات، عرض السجلات، الإعدادات، خروج.
5. اختصارات عامة (كلها قابلة للتخصيص): `Ctrl+Alt+P` تشغيل/إيقاف · `Ctrl+Alt+←/→` السابق/التالي · `Ctrl+Alt+I` إظهار/إخفاء · `Ctrl+Alt+مسافة` تمديد/طيّ · `Ctrl+مسافة` مشغّل سريع · `Ctrl+Alt+V` لوحة سجل الحافظة.
6. وسائط سطر الأوامر الشائعة: `--demo` وضع العرض · `--diagnose` تقرير تشخيصي · `--settings` فتح الإعدادات عند الإقلاع.

### تكامل Cider
1. في إعدادات Cider فعّل «السماح بالتحكم الخارجي» (Allow external control).
2. إعدادات WinIslands ← الوسائط ← تفعيل Cider (المنفذ يُكتشف تلقائيًا `10767` ويُفحص محليًا، أو يُدخل يدويًا).
3. أثناء التشغيل، المصدر على الجزيرة هو `Cider`: الغلاف/التقدّم/الكلمات والتحكم وseek ومستوى الصوت.

---

## ⚙️ شرح الإعدادات

ملف الإعدادات: `%APPDATA%\WinIslands\settings.json` (JSON؛ التغييرات في الواجهة تُطبَّق فورًا، مع إمكانية التصدير/الاستيراد).

| المفتاح | الافتراضي | الوصف |
| --- | --- | --- |
| `Language` | `zh-CN` | لغة الواجهة: `zh-CN` / `en-US` |
| `Position` | `Center` | الموضع: `Center` أعلى المنتصف / `Right` أعلى اليمين |
| `Monitor` | `Primary` | الشاشة: `Primary` الرئيسية / `All` كل الشاشات / `Index` شاشة مختارة |
| `MonitorIndex` | `0` | رقم الشاشة المختارة |
| `OffsetX` / `OffsetY` | `0` / `8` | إزاحة الموضع (بكسل) |
| `Opacity` | `0.92` | الشفافية 0.3–1.0 |
| `Theme` | `Auto` | `Auto` / `Light` / `Dark` |
| `ThemePreset` | `Default` | سمة المظهر (18 سمة جاهزة + Custom) |
| `AccentColor` | `#6C5CE7` | لون التمييز `#RRGGBB` |
| `ThemeScheduledEnabled` | `false` | تبديل فاتح/داكن مجدول (يعمل مع `Theme=Auto` فقط) |
| `ThemeScheduleDarkStartHour` / `EndHour` | `19` / `7` | ساعات بدء/انتهاء الفترة الداكنة |
| `FontScale` / `CornerRadius` | `1.0` / `28` | مقياس الخط 0.8–1.4 / نصف قطر الزوايا 16–40 |
| `AnimationStyle` | `Spring` | أسلوب الحركة: `Spring` / `Soft` / `Elastic` / `Fade` |
| `CompactWidth` / `CompactHeight` | `360` / `72` | الطول / الارتفاع المضغوط |
| `CompactWidthAuto` / `CompactHeightAuto` | `true` | ضبط تلقائي للحجم المضغوط |
| `ExpandedWidth` / `MaxExpandedHeight` | `400` / `384` | الطول الممدد / أقصى ارتفاع ممدد |
| `HideWhenNoMedia` | `true` | إخفاء الجزيرة عند عدم تشغيل وسائط |
| `IslandAlwaysVisible` | `false` | الجزيرة دائمة الظهور (تعرض الأدوات حتى بدون وسائط) |
| `ShowWhenPaused` | `true` | الإظهار عند الإيقاف المؤقت |
| `StartWithWindows` | `false` | التشغيل مع بدء تشغيل ويندوز |
| `IsLocked` | `true` | مقفلة (بعد فتح القفل يمكن للماوس السحب) |
| `EdgeSnapEnabled` | `true` | الالتصاق التلقائي بالحافة/المنتصف عند الإفلات |
| `FullScreenAutoHideEnabled` | `true` | الإخفاء التلقائي في ملء الشاشة |
| `LockScreenAutoHideEnabled` | `true` | إخفاء تلقائي على شاشة القفل (Win+L / فصل سطح المكتب البعيد)، والاستعادة بعد إلغاء القفل |
| `SingleLineMode` | `true` | وضع سطر واحد: كل الأدوات في سطر واحد في الحالة المضغوطة |
| `DoubleClickAction` | `PlayPause` | فعل سريع بالنقرة المزدوجة على الجزيرة |
| `MiddleClickAction` | `PlayPause` | فعل سريع بنقرة الوسط على الجزيرة |
| `CiderEnabled` / `CiderPort` | `true` / `0` | واجهة Cider المحلية (0 = كشف تلقائي) |
| `OnlineLyricsEnabled` | `true` | الكلمات عبر الإنترنت (قابل للتبديل بالنقر الأيمن على الجزيرة؛ واجهة غير رسمية، انتبه لحقوق النشر) |
| `AmllTtmlEnabled` | `true` | كلمات AMLL كلمةً كلمة (api.amll.dev، غير رسمي) |
| `KaraokeHighlight` | `true` | إبراز كاريوكي كلمةً كلمة |
| `StandaloneLyricsWindow` | `false` | نافذة كلمات مستقلة |
| `UseSystemVolume` | `true` | صوت النظام للمصادر غير Cider |
| `LowBatteryThreshold` | `20` | حد تنبيه انخفاض البطارية (٪)، 0 = إيقاف |
| `DoNotDisturbManual` | `false` | «لا تُزعج» يدوي |
| `DoNotDisturbEnabled` | `false` | «لا تُزعج» مجدول (إشعارات صامتة حسب الفترة) |
| `DoNotDisturbStartHour/Minute` | `22` / `0` | بداية «لا تُزعج» (بدقة الدقيقة) |
| `DoNotDisturbEndHour/Minute` | `8` / `0` | نهاية «لا تُزعج» (بدقة الدقيقة) |
| `IslandApiEnabled` / `IslandApiPort` | `true` / `9840` | مفتاح / منفذ واجهة Island |
| `IslandApiToken` | `""` | رمز اختياري لواجهة Island |
| `IslandApiDefaultDuration` | `30` | مدة العرض الافتراضية (ثوانٍ) |
| `WaveStyle` | `Bars` | نمط موجة الصوت: `Bars` / `Spectrum` / `Ring` / `Particles` |
| `WidgetOrder` | `Time,Weather,...` | ترتيب الأدوات (مفاتيح مفصولة بفواصل) |
| `Components` | كائن | خانات «بدون موسيقى / مع موسيقى» لكل أداة |
| `MediaApps` | `[]` | تفعيل/تعطيل وأولوية المشغّلات |
| `Rules` | `[]` | قواعد الأتمتة (شرط + فعل) |
| `ReduceMotion` | `false` | تقليل الحركة (إتاحة/توفير الطاقة) |
| `GlobalHotkeysEnabled` | `true` | مفتاح الاختصارات العامة |
| `LowPowerMode` | `false` | وضع الطاقة المنخفضة: خفض المعدل في الخمول وتبسيط الحركات |

---

## 🏝 واجهة Island (تكامل الأطراف الخارجية)

يمكن لأي برنامج دفع المعلومات إلى الديناميك آيلاند عبر واجهة HTTP / WebSocket المحلية، مثل تكامل Island لتطبيقات iOS الخارجية.

| الطريقة | المسار | الوصف |
| --- | --- | --- |
| POST | `/v1/island/push` | دفع / تحديث بطاقة |
| POST | `/v3/island/push` | فائق لـ v1: صورة / تقدّم متحرك / heartbeat |
| PATCH | `/v3/island/push/{id}` | تحديث جزئي |
| DELETE | `/v1/island/push/{id}` | إزالة بطاقة |
| GET | `/v1/island/active` | الاستعلام عن البطاقة النشطة |
| GET | `/v3/ws` | قناة WebSocket ثنائية الاتجاه |
| GET | `/v1/health` | فحص الصحة |

تدعم: العنوان/النص/الأيقونة/العنوان الفرعي، والتقدّم، والأزرار (فتح رابط / تشغيل برنامج / تنفيذ أمر / رد notify)، وحقول الإدخال، والصور، والتقدّم المتحرك، وتجديد heartbeat، والسمات الفاتحة/الداكنة، ولون تمييز مخصص، وقائمة أولويات. **الدفع لا يغيّر عرض الجزيرة**.

التوثيق الكامل: [docs/IslandAPI.md](docs/IslandAPI.md)

---

## 🛡 الخصوصية والأمان

- **لا تتبع، لا إعلانات، لا رفع بيانات**. باستثناء «الكلمات عبر الإنترنت» و«الطقس» المفعّلين يدويًا، لا يقوم التطبيق بأي طلبات شبكة.
- سيناريوهات الاتصال الوحيدة: تنزيل أغلفة Cider، كلمات AMLL كلمةً كلمة (api.amll.dev)، والكلمات عبر الإنترنت والطقس بعد التفعيل (Open-Meteo).
- جميع البيانات محلية في `%APPDATA%\WinIslands\`؛ السجلات تسجّل معلومات التشغيل المحلية فقط.

---

## ⚠️ القيود المعروفة

- الكاريوكي كلمةً كلمة يعتمد على مصدر الكلمات والتقدّم: مع خط زمني كلمةً كلمة من AMLL TTML / LRC يتم الإبراز كلمةً كلمة، وإلا يُستخدم إبراز الجملة كاملة.
- بعض المشغّلات قد تُبلغ أحيانًا بتقدّم متراجع (مثل إبلاغ Cider/SMTC بالصفر لحظيًا): حارس الموضع يتجاهل التراجعات اللحظية.
- تغطية SMTC تعتمد على تسجيل المشغّل للجلسة العامة؛ بعض المشغّلات القديمة تُكتشف عبر عنوان النافذة فقط (بدون أزرار تحكم).
- Cider 1.x (الواجهة القديمة) غير مدعوم، فقط 2.x وما فوق.
- الكلمات عبر الإنترنت / AMLL / الطقس واجهات غير رسمية وقد تتوقف عن العمل.

---

## ❓ الأسئلة الشائعة

**س: الجزيرة لا تظهر؟**
تأكد من وجود تشغيل؛ `HideWhenNoMedia` مفعّل افتراضيًا، والإخفاء بدون وسائط طبيعي. شغّل `--diagnose` لعرض قائمة الجلسات.

**س: Cider يعرض «غير متصل»؟**
تأكد من تفعيل «السماح بالتحكم الخارجي» في إعدادات Cider، وتحقق من المنفذ (الافتراضي 10767)، وتأكد من تفعيل Cider في إعدادات WinIslands.

**س: أيقونة العلبة باقية بعد الخروج؟**
قائمة العلبة ← خروج؛ إغلاق نافذة الجزيرة يخفيها فقط (الجزيرة مقيمة بالتصميم).

---

## 📄 ترخيص المصدر المفتوح

- التطبيق: MIT (انظر [LICENSE](LICENSE))
- المكونات الخارجية: انظر [THIRD_PARTY.md](THIRD_PARTY.md)

---

- **🎶 Плавное караоке (1.2.4)**: продвижение по буквам теперь использует плавную интерполяцию smoothstep с кросс-переходами между буквами — подсветка течёт слева направо как световая лента, плавно разгоняясь и замедляясь, без скачков.
- **🔋 Постоянный индикатор низкого заряда (1.2.4)**: когда заряд падает ниже порога и устройство не заряжается, в правом верхнем углу острова постоянно отображается небольшая пилюля заряда (красная ≤10 % / иначе оранжевая), обновляясь в реальном времени; она исчезает при подключении к сети или повышении заряда, и её можно отключить в настройках.
- **🎧 Анимация подключения устройств (1.2.4)**: при подключении/отключении Bluetooth показывается анимированная карточка в стиле iOS; при подключении автоматически считывается заряд устройства и отображается «Название устройства · Заряд xx%» (только название, если заряд прочитать нельзя), не блокируя интерфейс.
- **🎤 Настраиваемый стиль текста (1.2.3)**: настройте размер шрифта обычных/текущих строк, межстрочный интервал, скорость караоке и цвета подсветки/основы; изменения применяются мгновенно, отдельное окно текста синхронизируется.
- **📋 История уведомлений (1.2.3)**: просматривайте последние уведомления внизу развёрнутого острова, нажмите, чтобы воспроизвести снова, задайте лимит и очистите одним кликом.
- **🔔 Улучшение уведомлений (1.2.2)**: свёрнутые оповещения показываются одной строкой (иконка + заголовок + однострочное резюме) и обрезаются при длине, не расширяя остров; остров автоматически расширяется, чтобы виджеты справа и текст были полностью видны.
## Русский

# WinIslands — Dynamic Island для Windows

> **Перенесите Dynamic Island из iOS на Windows 11 / 10 — современный, многофункциональный виджет Dynamic Island для Windows.**
> На базе **.NET 8 + WPF**, бесплатно и с открытым кодом (MIT), **без рекламы · без телеметрии · без сбора данных**.

🌐 Сайт: https://WinIslands.JudeKwong.com ｜ GitHub: https://github.com/DMP-Pig/WinIslands

---

## ✨ Ключевые возможности
- **🎶 Плавное караоке (1.2.4)**: продвижение по буквам теперь использует плавную интерполяцию smoothstep с кросс-переходами между буквами — подсветка течёт слева направо как световая лента, плавно разгоняясь и замедляясь, без скачков.
- **🔋 Постоянный индикатор низкого заряда (1.2.4)**: когда заряд падает ниже порога и устройство не заряжается, в правом верхнем углу острова постоянно отображается небольшая пилюля заряда (красная ≤10 % / иначе оранжевая), обновляясь в реальном времени; она исчезает при подключении к сети или повышении заряда, и её можно отключить в настройках.
- **🎧 Анимация подключения устройств (1.2.4)**: при подключении/отключении Bluetooth показывается анимированная карточка в стиле iOS; при подключении автоматически считывается заряд устройства и отображается «Название устройства · Заряд xx%» (только название, если заряд прочитать нельзя), не блокируя интерфейс.
- **🎤 Настраиваемый стиль текста (1.2.3)**: настройте размер шрифта обычных/текущих строк, межстрочный интервал, скорость караоке и цвета подсветки/основы; изменения применяются мгновенно, отдельное окно текста синхронизируется.
- **📋 История уведомлений (1.2.3)**: просматривайте последние уведомления внизу развёрнутого острова, нажмите, чтобы воспроизвести снова, задайте лимит и очистите одним кликом.

- **🌊 Плавная анимация (1.2.1)**: пружинное демпфирование в стиле iOS + каскадные переходы при раскрытии/сворачивании; анимация громкости; умная прозрачность; переход обложки; плавная смена темы; оптимизация производительности.

- **▶ Управление воспроизведением**: нативная интеграция с глобальными мультимедийными сессиями Windows (SMTC), совместимость с NetEase Cloud Music, QQ Music, Spotify, Apple Music, Groove, «Фильмы и ТВ» и др.; дополнительно поддержка локального API Cider; при недоступности — запасной вариант по заголовку окна. Обложка альбома, перетаскиваемая полоса seek, воспроизведение/пауза/следующий/предыдущий — всё включено; при нескольких открытых плеерах источник управления переключается в один клик.
- **♪ Караоке-текст по словам**: развёрнутая карточка прокручивается с синхронной подсветкой и загоранием по словам; четырёхуровневый приоритет источников: локальный `.lrc` → AMLL TTML → API текстов плеера → необязательные онлайн-тексты. Двуязычные тексты, переключатель перевода, копирование текущей строки в один клик; точная подстройка тайминга для каждой песни и отдельное окно текста с регулируемой прозрачностью и блокировкой.
- **▦ Настраиваемая система виджетов**: более 30 виджетов — время, погода, дата (с лунным календарём/сезонами), CPU/GPU/память/диск, скорость сети, батарея, метод ввода, быстрые переключатели (WiFi/Bluetooth/ночной режим/без звука) и др. Каждый виджет поддерживает свою иконку, выбор флажками и перетаскивание для сортировки, режимы в одну/несколько строк.
- **🏝 API Island**: локальный HTTP / WebSocket-интерфейс, позволяющий любому стороннему ПО отправлять информацию на Dynamic Island в реальном времени (как интеграция Island сторонних приложений в iOS). Поддержка изображений, анимированного прогресса, продления по heartbeat, светлой/тёмной темы, действий кнопок и полей ввода; отправки никогда не меняют ширину острова и не перекрывают другие виджеты.
- **✨ Анимации событий**: подключение/отключение Bluetooth, начало/завершение зарядки, низкий заряд, восстановление сети, напоминания календаря/RSS/почты и др. отображаются на острове с изящными анимациями.
- **✦ Внешний вид и анимация**: 18 тем, настраиваемый акцентный цвет, «жидкое стекло»/акрил, извлечение цвета обоев, бегущая строка, 4 типа анимации (пружина iOS и др.), 4 стиля звуковой волны (пульсирует в такт музыке); нелинейное сглаживание разворачивания/сворачивания, плавные 60 fps; поддержка высокого DPI PerMonitorV2.
- **🖱 Взаимодействие и интеллект**: разблокировка для перетаскивания с прилипанием к краям, автоскрытие в полноэкранном режиме и на экране блокировки, быстрые действия по двойному/среднему клику, кнопки быстрых действий, перевалочный пункт файлов (перетаскивание на остров и из него в другие приложения), умный режим «Не беспокоить» во время записи экрана, плановое переключение светлой/тёмной темы.
- **⚡ Производительность и автоматизация**: таймер Pomodoro, задачи, история буфера обмена, быстрый запуск, напоминания расписания; помощник отключения микрофона на созвонах, подсказки записи экрана/скриншотов, прогресс копирования/загрузки файлов на острове; глобальные горячие клавиши и движок правил.
- **🛡 Конфиденциальность и безопасность**: без телеметрии, без рекламы, без отправки данных. Полностью офлайн, кроме включённых вручную онлайн-текстов и погоды; все настройки и данные хранятся локально в `%APPDATA%\WinIslands`.
- **🔁 Переключение источника текста одним нажатием**: мгновенно переключайтесь между «Авто / Локальный LRC / AMLL TTML / API Cider / Онлайн-тексты», текст текущей песни сразу перезагружается
- **🛡️ Автовосстановление после сбоя**: после аварийного завершения следующий запуск уведомляет о восстановлении — без зависаний, чёрного экрана и потери состояния
- **⏱️ Настраиваемая длительность анимации**: ползунок длительности раскрытия/сворачивания (300–1400мс)


---

## 📥 Скачать (последняя стабильная версия 1.2.4)

| Платформа | Скачать | Примечания |
| --- | --- | --- |
| Windows x64 | [Портативная x64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-x64.exe) | Рекомендуется для обычных 64-битных ПК; один файл, без установки, запуск сразу |
| Windows ARM64 | [Портативная ARM64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-arm64.exe) | Для Surface Pro X / устройств ARM Snapdragon |
| Windows универсальная | [Универсальный установщик](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-Setup-1.2.4.exe) | Мастер Inno Setup; устанавливает x64 / ARM64 в зависимости от архитектуры |

Все предыдущие версии и полный журнал изменений: [GitHub Releases](https://github.com/DMP-Pig/WinIslands/releases).

---

## 📊 Производительность

| Показатель | Значение |
| --- | --- |
| Постоянная память (Private) | ~72 МБ |
| Холодный запуск | < 1 с |
| CPU в простое | ≈ 0% |
| Частота анимации | 60 fps |
| Несколько экземпляров | Один экземпляр, защита от дублей |
| Телеметрия | 0 телеметрии · без отправки · без рекламы |

---

## 🔧 Сборка

### Требования
- Windows 10 1809+ / Windows 11
- .NET 8 SDK

### Сборка и тестирование
```powershell
dotnet build WinIslands.slnx -c Release
dotnet test  WinIslands.slnx -c Release
```

### Публикация
```powershell
# Автономная (включает среду выполнения .NET 8, установка не требуется)
.\build\publish.ps1

# Зависимая от платформы (лёгкая, нужен .NET 8 Desktop Runtime)
.\build\publish.ps1 -FrameworkDependent
```
Артефакты находятся в `publish\win-x64\`. Стабильные версии помещаются в `releases\<версия>\win-x64\` и переименовываются по номеру версии.

### Установщик (необязательно)
Установите [Inno Setup 6](https://jrsoftware.org/isinfo.php), затем выполните `iscc.exe build\release-<версия>.iss`, чтобы получить `releases\<версия>\WinIslands-Setup-<версия>.exe` (универсальный установщик, x64 / ARM64 по архитектуре).

---

## 🚀 Использование

1. Запустите `WinIslands.exe` (автозапуск настраивается); в трее появляется значок; закрытие главного окна не завершает процесс — сворачивается в трей.
2. Играйте любую музыку: NetEase Cloud Music, QQ Music, Spotify, официальный Apple Music и др. автоматически отображаются через системную мультимедийную сессию; для Cider см. ниже; другие плееры определяются по заголовку окна.
3. **Клик** по острову разворачивает полную карточку (наведение не разворачивает): перетаскивание seek, управление воспроизведением, громкость, синхронизированные тексты; ещё один клик сворачивает.
4. Меню трея: показать/скрыть, отдельное окно текста, автозапуск, «Не беспокоить», проверка обновлений, просмотр журналов, настройки, выход.
5. Глобальные горячие клавиши (все настраиваются): `Ctrl+Alt+P` воспроизведение/пауза · `Ctrl+Alt+←/→` предыдущий/следующий · `Ctrl+Alt+I` показать/скрыть · `Ctrl+Alt+Пробел` развернуть/свернуть · `Ctrl+Пробел` быстрый запуск · `Ctrl+Alt+V` панель истории буфера обмена.
6. Частые аргументы командной строки: `--demo` демо-режим · `--diagnose` диагностический отчёт · `--settings` открыть настройки при запуске.

### Интеграция Cider
1. В настройках Cider включите «Разрешить внешнее управление» (Allow external control).
2. Настройки WinIslands → Медиа → включить Cider (порт определяется автоматически `10767` и сканируется локально, либо вводится вручную).
3. При воспроизведении источник на острове — `Cider`: обложка/прогресс/текст и управление, seek, громкость.

---

## ⚙️ Описание настроек

Файл конфигурации: `%APPDATA%\WinIslands\settings.json` (JSON; изменения в интерфейсе вступают в силу сразу, экспорт/импорт возможны).

| Ключ | По умолчанию | Описание |
| --- | --- | --- |
| `Language` | `zh-CN` | Язык интерфейса: `zh-CN` / `en-US` |
| `Position` | `Center` | Положение: `Center` верх по центру / `Right` верх справа |
| `Monitor` | `Primary` | Монитор: `Primary` главный / `All` все / `Index` выбранный |
| `MonitorIndex` | `0` | Номер выбранного монитора |
| `OffsetX` / `OffsetY` | `0` / `8` | Смещение положения (пиксели) |
| `Opacity` | `0.92` | Непрозрачность 0.3–1.0 |
| `Theme` | `Auto` | `Auto` / `Light` / `Dark` |
| `ThemePreset` | `Default` | Тема (18 предустановок + Custom) |
| `AccentColor` | `#6C5CE7` | Акцентный цвет `#RRGGBB` |
| `ThemeScheduledEnabled` | `false` | Плановое переключение светлой/тёмной темы (только при `Theme=Auto`) |
| `ThemeScheduleDarkStartHour` / `EndHour` | `19` / `7` | Часы начала/конца тёмного периода |
| `FontScale` / `CornerRadius` | `1.0` / `28` | Масштаб шрифта 0.8–1.4 / радиус углов 16–40 |
| `AnimationStyle` | `Spring` | Тип анимации: `Spring` / `Soft` / `Elastic` / `Fade` |
| `CompactWidth` / `CompactHeight` | `360` / `72` | Длина / высота в компактном режиме |
| `CompactWidthAuto` / `CompactHeightAuto` | `true` | Автоподбор компактного размера |
| `ExpandedWidth` / `MaxExpandedHeight` | `400` / `384` | Длина / максимальная высота в развёрнутом виде |
| `HideWhenNoMedia` | `true` | Скрывать остров, когда нет воспроизведения |
| `IslandAlwaysVisible` | `false` | Остров всегда видим (виджеты показываются и без воспроизведения) |
| `ShowWhenPaused` | `true` | Показывать при паузе |
| `StartWithWindows` | `false` | Автозапуск с Windows |
| `IsLocked` | `true` | Заблокирован (после разблокировки можно перетаскивать мышью) |
| `EdgeSnapEnabled` | `true` | Прилипание к краю / центрирование при отпускании |
| `FullScreenAutoHideEnabled` | `true` | Автоскрытие в полноэкранном режиме |
| `LockScreenAutoHideEnabled` | `true` | Автоскрытие на экране блокировки (Win+L / отключение удалённого рабочего стола), восстановление после разблокировки |
| `SingleLineMode` | `true` | Однострочный режим: все виджеты в одну строку в компактном состоянии |
| `DoubleClickAction` | `PlayPause` | Быстрое действие двойного клика по острову |
| `MiddleClickAction` | `PlayPause` | Быстрое действие среднего клика по острову |
| `CiderEnabled` / `CiderPort` | `true` / `0` | Локальный API Cider (0 = автодетект) |
| `OnlineLyricsEnabled` | `true` | Онлайн-тексты (переключатель правым кликом по острову; неофициальный интерфейс, следите за авторскими правами) |
| `AmllTtmlEnabled` | `true` | Пословные тексты AMLL (api.amll.dev, неофициально) |
| `KaraokeHighlight` | `true` | Пословная подсветка караоке |
| `StandaloneLyricsWindow` | `false` | Отдельное окно текста |
| `UseSystemVolume` | `true` | Системная громкость для источников, отличных от Cider |
| `LowBatteryThreshold` | `20` | Порог предупреждения о низком заряде (%), 0 = выкл. |
| `DoNotDisturbManual` | `false` | Ручной режим «Не беспокоить» |
| `DoNotDisturbEnabled` | `false` | Плановый режим «Не беспокоить» (тихие уведомления по расписанию) |
| `DoNotDisturbStartHour/Minute` | `22` / `0` | Начало «Не беспокоить» (с точностью до минуты) |
| `DoNotDisturbEndHour/Minute` | `8` / `0` | Конец «Не беспокоить» (с точностью до минуты) |
| `IslandApiEnabled` / `IslandApiPort` | `true` / `9840` | Переключатель / порт API Island |
| `IslandApiToken` | `""` | Необязательный токен API Island |
| `IslandApiDefaultDuration` | `30` | Длительность показа по умолчанию (секунды) |
| `WaveStyle` | `Bars` | Стиль звуковой волны: `Bars` / `Spectrum` / `Ring` / `Particles` |
| `WidgetOrder` | `Time,Weather,...` | Порядок виджетов (ключи через запятую) |
| `Components` | объект | Флажки «без музыки / с музыкой» для каждого виджета |
| `MediaApps` | `[]` | Включение/отключение и приоритет плееров |
| `Rules` | `[]` | Правила автоматизации (условие + действие) |
| `ReduceMotion` | `false` | Уменьшить анимацию (доступность/экономия энергии) |
| `GlobalHotkeysEnabled` | `true` | Переключатель глобальных горячих клавиш |
| `LowPowerMode` | `false` | Энергосберегающий режим: снижение частоты в простое, упрощённые анимации |

---

## 🏝 API Island (интеграция сторонних приложений)

Любое ПО может отправлять информацию на Dynamic Island через локальный HTTP / WebSocket-интерфейс, как интеграция Island сторонних приложений в iOS.

| Метод | Путь | Описание |
| --- | --- | --- |
| POST | `/v1/island/push` | Отправить / обновить карточку |
| POST | `/v3/island/push` | Надмножество v1: изображение / анимированный прогресс / heartbeat |
| PATCH | `/v3/island/push/{id}` | Частичное обновление |
| DELETE | `/v1/island/push/{id}` | Удалить карточку |
| GET | `/v1/island/active` | Запрос активной карточки |
| GET | `/v3/ws` | Двунаправленный канал WebSocket |
| GET | `/v1/health` | Проверка работоспособности |

Поддержка: заголовок/текст/иконка/подзаголовок, прогресс, кнопки (открыть ссылку / запустить программу / выполнить команду / обратный вызов notify), поля ввода, изображения, анимированный прогресс, продление по heartbeat, светлая/тёмная тема, настраиваемый акцентный цвет, очередь приоритетов. **Отправка не меняет ширину острова**.

Полная документация: [docs/IslandAPI.md](docs/IslandAPI.md)

---

## 🛡 Конфиденциальность и безопасность

- **Без телеметрии, без рекламы, без отправки данных**. Кроме включённых вручную «онлайн-текстов» и «погоды», приложение не выполняет сетевых запросов.
- Единственные сетевые сценарии: загрузка обложек Cider, пословные тексты AMLL (api.amll.dev), онлайн-тексты и погода после включения (Open-Meteo).
- Все данные хранятся локально в `%APPDATA%\WinIslands\`; журналы записывают только локальную информацию о работе.

---

## ⚠️ Известные ограничения

- Пословное караоке зависит от источника и прогресса текста: при пословной шкале времени AMLL TTML / LRC — подсветка по словам, иначе используется подсветка всей строки.
- Некоторые плееры иногда сообщают откатывающийся прогресс (например, Cider/SMTC кратко сообщают 0): защита позиции игнорирует мгновенные откаты.
- Охват SMTC зависит от регистрации плеером глобальной мультимедийной сессии; отдельные старые плееры определяются только по заголовку окна (без кнопок управления).
- Cider 1.x (старый API) не поддерживается, только 2.x и выше.
- Онлайн-тексты / AMLL / погода — неофициальные интерфейсы, могут перестать работать при изменении версий.

---

## ❓ Частые вопросы

**В: Остров не появляется?**
Проверьте, что что-то воспроизводится; `HideWhenNoMedia` включён по умолчанию, скрытие без медиа — нормально. Запустите `--diagnose`, чтобы увидеть список сессий.

**В: Cider показывает «не подключено»?**
Проверьте, что в настройках Cider включено «Разрешить внешнее управление», проверьте порт (по умолчанию 10767) и убедитесь, что Cider включён в настройках WinIslands.

**В: После выхода значок в трее остаётся?**
Меню трея → Выход; закрытие окна острова лишь скрывает его (остров резидентный по замыслу).

---

## 📄 Открытая лицензия

- Приложение: MIT (см. [LICENSE](LICENSE))
- Сторонние компоненты: см. [THIRD_PARTY.md](THIRD_PARTY.md)

---

- **🎶 Karaokê mais fluido (1.2.4)**: o avanço letra por letra agora usa suavização smoothstep com transições cruzadas entre caracteres — o destaque flui da esquerda para a direita como uma fita de luz, com aceleração/desaceleração, sem passos bruscos.
- **🔋 Indicador persistente de bateria fraca (1.2.4)**: quando a bateria fica abaixo do limite e não está carregando, uma pequena pílula de bateria permanece no canto superior direito da ilha (vermelha ≤10% / laranja caso contrário), atualizando em tempo real; desaparece ao conectar o carregador ou recuperar, e pode ser ativada nas configurações.
- **🎧 Animação de conexão de dispositivos (1.2.4)**: ao conectar/desconectar um dispositivo Bluetooth, um cartão animado estilo iOS é exibido; ao conectar, a bateria do dispositivo é lida automaticamente e mostrada como «Nome do dispositivo · Bateria xx%» (somente o nome se não for possível ler), sem bloquear a interface.
- **🎤 Estilo de letra ajustável (1.2.3)**: personalize o tamanho da fonte das linhas normais/atual, o espaçamento, a velocidade do karaokê e as cores de destaque/base; as mudanças se aplicam na hora e a janela de letras independente acompanha.
- **📋 Histórico de notificações (1.2.3)**: veja as notificações recentes na parte inferior da ilha expandida, clique para reproduzi-las, defina o limite e limpe com um clique.
- **🔔 Notificações aprimoradas (1.2.2)**: alertas recolhidos mostram uma única linha (ícone + título + resumo em uma linha) e são truncados quando longos, sem alargar a ilha; a ilha se alarga automaticamente para que os widgets à direita e o texto fiquem totalmente visíveis.
## Português

# WinIslands — Dynamic Island para Windows

> **Traga o Dynamic Island do iOS para o Windows 11 / 10 — um widget Dynamic Island moderno e multifuncional para Windows.**
> Baseado em **.NET 8 + WPF**, gratuito e de código aberto (MIT), **sem anúncios · sem telemetria · sem coleta de dados**.

🌐 Site: https://WinIslands.JudeKwong.com ｜ GitHub: https://github.com/DMP-Pig/WinIslands

---

## ✨ Destaques
- **🎶 Karaokê mais fluido (1.2.4)**: o avanço letra por letra agora usa suavização smoothstep com transições cruzadas entre caracteres — o destaque flui da esquerda para a direita como uma fita de luz, com aceleração/desaceleração, sem passos bruscos.
- **🔋 Indicador persistente de bateria fraca (1.2.4)**: quando a bateria fica abaixo do limite e não está carregando, uma pequena pílula de bateria permanece no canto superior direito da ilha (vermelha ≤10% / laranja caso contrário), atualizando em tempo real; desaparece ao conectar o carregador ou recuperar, e pode ser ativada nas configurações.
- **🎧 Animação de conexão de dispositivos (1.2.4)**: ao conectar/desconectar um dispositivo Bluetooth, um cartão animado estilo iOS é exibido; ao conectar, a bateria do dispositivo é lida automaticamente e mostrada como «Nome do dispositivo · Bateria xx%» (somente o nome se não for possível ler), sem bloquear a interface.
- **🎤 Estilo de letra ajustável (1.2.3)**: personalize o tamanho da fonte das linhas normais/atual, o espaçamento, a velocidade do karaokê e as cores de destaque/base; as mudanças se aplicam na hora e a janela de letras independente acompanha.
- **📋 Histórico de notificações (1.2.3)**: veja as notificações recentes na parte inferior da ilha expandida, clique para reproduzi-las, defina o limite e limpe com um clique.

- **🌊 Movimento sedoso (1.2.1)**: mola amortecida estilo iOS + transições em cascata para expandir/recolher; animação vinculada ao volume; opacidade inteligente em camadas; transição de capa; troca de tema suave; otimização de animação.

- **▶ Controle de reprodução de mídia**: integração nativa com as sessões de mídia globais do Windows (SMTC), compatível com NetEase Cloud Music, QQ Music, Spotify, Apple Music, Groove, Filmes e TV, etc.; além disso, suporte dedicado à API local do Cider; como último recurso, detecção por título de janela. Capa do álbum, barra de progresso arrastável (seek), reproduzir/pausar/anterior/próxima, tudo incluído; com vários players abertos, alterne a fonte de controle com um clique.
- **♪ Letras karaokê palavra por palavra**: o cartão expandido rola com destaque sincronizado e iluminação palavra por palavra; prioridade de fontes em quatro níveis: `.lrc` local → AMLL TTML → API de letras do player → letras on-line opcionais. Letras bilíngues, botão de tradução, copiar a linha atual com um clique; ajuste fino de tempo por música e janela de letras independente com opacidade e bloqueio ajustáveis.
- **▦ Sistema de widgets personalizável**: mais de 30 widgets — hora, clima, data (com calendário lunar/termos solares), CPU/GPU/RAM/disco, velocidade de rede, bateria, método de entrada, atalhos rápidos (WiFi/Bluetooth/modo noturno/silencioso), etc. Cada widget oferece ícone personalizado, seleção por caixas de marcação e reordenação por arrastar, com modos de uma linha/várias linhas.
- **🏝 API Island**: uma interface HTTP / WebSocket local que permite a qualquer software de terceiros enviar informações para o Dynamic Island em tempo real (como a integração Island de apps de terceiros no iOS). Suporta imagens, progresso animado, renovação por heartbeat, temas claro/escuro, ações de botões e campos de entrada; os envios nunca mudam a largura da ilha nem cobrem outros widgets.
- **✨ Animações de eventos**: conexão/desconexão Bluetooth, início/fim de carregamento, bateria fraca, rede restaurada, lembretes de calendário/RSS/e-mail etc. são exibidos na ilha com animações elegantes.
- **✦ Aparência e movimento**: 18 temas predefinidos, cor de destaque personalizada, acrílico vidro líquido, extração de cor do papel de parede, letreiro, 4 tipos de animação (mola iOS etc.), 4 estilos de onda de áudio (pulsando com a música); easing não linear para expandir/recolher a 60 fps suaves; suporte a alto DPI PerMonitorV2.
- **🖱 Interação e inteligência**: desbloquear para arrastar com ajuste às bordas, ocultar automaticamente em tela cheia/tela de bloqueio, ações rápidas de duplo clique e clique do meio, botões de ação rápida, estação de transferência de arquivos (arrastar arquivos para a ilha e arrastá-los para outros aplicativos), Não perturbe inteligente durante gravação de tela, troca programada de tema claro/escuro.
- **⚡ Produtividade e automação**: timer Pomodoro, tarefas, histórico da área de transferência, iniciador rápido, lembretes de agenda; assistente de mudo em reuniões, avisos de gravação/captura de tela, progresso de cópia/download de arquivos na ilha; atalhos globais e mecanismo de regras.
- **🛡 Privacidade e segurança**: sem telemetria, sem anúncios, sem envio de dados. Totalmente offline exceto pelas letras on-line e clima ativados manualmente; toda a configuração e os dados permanecem locais em `%APPDATA%\WinIslands`.
- **🔁 Troca de fonte da letra com um clique**: alterne instantaneamente entre Automática / LRC local / AMLL TTML / API do Cider / Letras online, e a letra da música atual é recarregada imediatamente
- **🛡️ Recuperação automática após falhas**: após um encerramento anormal, a próxima inicialização avisa a recuperação — sem travamentos, telas pretas ou estado perdido
- **⏱️ Duração de animação ajustável**: novo controle deslizante de duração da animação de expandir/recolher (300–1400ms)


---

## 📥 Download (última versão estável 1.2.4)

| Plataforma | Download | Observações |
| --- | --- | --- |
| Windows x64 | [Portátil x64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-x64.exe) | Recomendado para PCs 64 bits comuns; arquivo único, sem instalação, execução direta |
| Windows ARM64 | [Portátil ARM64](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-1.2.4-win-arm64.exe) | Para Surface Pro X / dispositivos ARM Snapdragon |
| Windows universal | [Instalador universal](https://github.com/DMP-Pig/WinIslands/releases/download/1.2.4/WinIslands-Setup-1.2.4.exe) | Assistente Inno Setup; instala x64 / ARM64 conforme a arquitetura |

Todas as versões anteriores e o changelog completo: [GitHub Releases](https://github.com/DMP-Pig/WinIslands/releases).

---

## 📊 Desempenho

| Métrica | Valor |
| --- | --- |
| Memória residente (Private) | ~72 MB |
| Inicialização a frio | < 1 s |
| CPU em repouso | ≈ 0% |
| Taxa de animação | 60 fps |
| Várias instâncias | Instância única, evita duplicatas |
| Telemetria | 0 telemetria · sem envio · sem anúncios |

---

## 🔧 Compilação

### Requisitos
- Windows 10 1809+ / Windows 11
- SDK .NET 8

### Compilar e testar
```powershell
dotnet build WinIslands.slnx -c Release
dotnet test  WinIslands.slnx -c Release
```

### Publicar
```powershell
# Autossuficiente (inclui o runtime .NET 8, sem instalação)
.\build\publish.ps1

# Dependente de framework (leve, requer .NET 8 Desktop Runtime)
.\build\publish.ps1 -FrameworkDependent
```
Os artefatos ficam em `publish\win-x64\`. As versões estáveis vão para `releases\<versão>\win-x64\` e são renomeadas conforme o número da versão.

### Instalador (opcional)
Instale o [Inno Setup 6](https://jrsoftware.org/isinfo.php), depois execute `iscc.exe build\release-<versão>.iss` para gerar `releases\<versão>\WinIslands-Setup-<versão>.exe` (instalador universal, x64 / ARM64 conforme a arquitetura).

---

## 🚀 Como usar

1. Inicie o `WinIslands.exe` (inicialização automática configurável); o ícone aparece na bandeja; fechar a janela principal não encerra o processo (apenas minimiza para a bandeja).
2. Reproduza qualquer música: NetEase Cloud Music, QQ Music, Spotify, Apple Music oficial etc. aparecem automaticamente pela sessão de mídia do sistema; para Cider, veja abaixo; outros players usam detecção por título de janela.
3. **Clique** na ilha para expandir o cartão completo (passar o mouse não expande): arrastar para seek, controle de reprodução, volume, letras sincronizadas; clique novamente para recolher.
4. Menu da bandeja: mostrar/ocultar, janela de letras independente, iniciar com o Windows, Não perturbe, verificar atualizações, ver logs, configurações, sair.
5. Atalhos globais (todos personalizáveis): `Ctrl+Alt+P` reproduzir/pausar · `Ctrl+Alt+←/→` anterior/próxima · `Ctrl+Alt+I` mostrar/ocultar · `Ctrl+Alt+Espaço` expandir/recolher · `Ctrl+Espaço` iniciador rápido · `Ctrl+Alt+V` painel do histórico da área de transferência.
6. Argumentos de linha de comando comuns: `--demo` modo demonstração · `--diagnose` relatório de diagnóstico · `--settings` abre as configurações na inicialização.

### Integração com Cider
1. Nas configurações do Cider, ative "Permitir controle externo" (Allow external control).
2. Configurações do WinIslands → Mídia → ativar Cider (porta detectada automaticamente `10767` e varredura local, ou digitação manual).
3. Durante a reprodução, a fonte na ilha é `Cider`: capa/progresso/letras e controle, seek, volume.

---

## ⚙️ Opções de configuração

Arquivo de configuração: `%APPDATA%\WinIslands\settings.json` (JSON; alterações na interface valem imediatamente, com exportação/importação).

| Chave | Padrão | Descrição |
| --- | --- | --- |
| `Language` | `zh-CN` | Idioma da interface: `zh-CN` / `en-US` |
| `Position` | `Center` | Posição: `Center` topo centralizado / `Right` topo à direita |
| `Monitor` | `Primary` | Monitor: `Primary` principal / `All` todos / `Index` escolhido |
| `MonitorIndex` | `0` | Número do monitor escolhido |
| `OffsetX` / `OffsetY` | `0` / `8` | Deslocamento de posição (pixels) |
| `Opacity` | `0.92` | Opacidade 0.3–1.0 |
| `Theme` | `Auto` | `Auto` / `Light` / `Dark` |
| `ThemePreset` | `Default` | Tema (18 predefinições + Custom) |
| `AccentColor` | `#6C5CE7` | Cor de destaque `#RRGGBB` |
| `ThemeScheduledEnabled` | `false` | Troca programada claro/escuro (somente com `Theme=Auto`) |
| `ThemeScheduleDarkStartHour` / `EndHour` | `19` / `7` | Horas de início/fim do período escuro |
| `FontScale` / `CornerRadius` | `1.0` / `28` | Escala da fonte 0.8–1.4 / raio dos cantos 16–40 |
| `AnimationStyle` | `Spring` | Tipo de animação: `Spring` / `Soft` / `Elastic` / `Fade` |
| `CompactWidth` / `CompactHeight` | `360` / `72` | Comprimento / altura compactos |
| `CompactWidthAuto` / `CompactHeightAuto` | `true` | Ajuste automático do tamanho compacto |
| `ExpandedWidth` / `MaxExpandedHeight` | `400` / `384` | Comprimento expandido / altura máxima expandida |
| `HideWhenNoMedia` | `true` | Ocultar a ilha quando não há mídia |
| `IslandAlwaysVisible` | `false` | Ilha sempre visível (mostra widgets mesmo sem mídia) |
| `ShowWhenPaused` | `true` | Mostrar ao pausar |
| `StartWithWindows` | `false` | Iniciar com o Windows |
| `IsLocked` | `true` | Bloqueado (após desbloquear, o mouse pode arrastar) |
| `EdgeSnapEnabled` | `true` | Ajustar automaticamente à borda / centralizar ao soltar |
| `FullScreenAutoHideEnabled` | `true` | Ocultar automaticamente em tela cheia |
| `LockScreenAutoHideEnabled` | `true` | Ocultar automaticamente na tela de bloqueio (Win+L / desconexão de área de trabalho remota), restaurar após desbloquear |
| `SingleLineMode` | `true` | Modo de uma linha: todos os widgets em uma linha no estado compacto |
| `DoubleClickAction` | `PlayPause` | Ação rápida de duplo clique na ilha |
| `MiddleClickAction` | `PlayPause` | Ação rápida de clique do meio na ilha |
| `CiderEnabled` / `CiderPort` | `true` / `0` | API local do Cider (0 = detecção automática) |
| `OnlineLyricsEnabled` | `true` | Letras on-line (alternar com clique direito na ilha; interface não oficial, atenção aos direitos autorais) |
| `AmllTtmlEnabled` | `true` | Letras palavra por palavra AMLL (api.amll.dev, não oficial) |
| `KaraokeHighlight` | `true` | Destaque karaokê palavra por palavra |
| `StandaloneLyricsWindow` | `false` | Janela de letras independente |
| `UseSystemVolume` | `true` | Volume do sistema para fontes não-Cider |
| `LowBatteryThreshold` | `20` | Limiar de bateria fraca (%), 0 = desativado |
| `DoNotDisturbManual` | `false` | Não perturbe manual |
| `DoNotDisturbEnabled` | `false` | Não perturbe programado (notificações silenciosas por período) |
| `DoNotDisturbStartHour/Minute` | `22` / `0` | Início do Não perturbe (em minutos) |
| `DoNotDisturbEndHour/Minute` | `8` / `0` | Fim do Não perturbe (em minutos) |
| `IslandApiEnabled` / `IslandApiPort` | `true` / `9840` | Interruptor / porta da API Island |
| `IslandApiToken` | `""` | Token opcional da API Island |
| `IslandApiDefaultDuration` | `30` | Duração padrão de exibição (segundos) |
| `WaveStyle` | `Bars` | Estilo da onda de áudio: `Bars` / `Spectrum` / `Ring` / `Particles` |
| `WidgetOrder` | `Time,Weather,...` | Ordem dos widgets (chaves separadas por vírgula) |
| `Components` | objeto | Caixas "sem música / com música" para cada widget |
| `MediaApps` | `[]` | Ativar/desativar e prioridade dos players |
| `Rules` | `[]` | Regras de automação (condição + ação) |
| `ReduceMotion` | `false` | Reduzir animações (acessibilidade/economia de energia) |
| `GlobalHotkeysEnabled` | `true` | Interruptor de atalhos globais |
| `LowPowerMode` | `false` | Modo de baixo consumo: reduz a taxa em repouso, animações simplificadas |

---

## 🏝 API Island (integração de terceiros)

Qualquer software pode enviar informações ao Dynamic Island pela interface HTTP / WebSocket local, como a integração Island de apps de terceiros no iOS.

| Método | Caminho | Descrição |
| --- | --- | --- |
| POST | `/v1/island/push` | Enviar / atualizar um cartão |
| POST | `/v3/island/push` | Superconjunto do v1: imagem / progresso animado / heartbeat |
| PATCH | `/v3/island/push/{id}` | Atualização parcial |
| DELETE | `/v1/island/push/{id}` | Remover um cartão |
| GET | `/v1/island/active` | Consultar o cartão ativo |
| GET | `/v3/ws` | Canal WebSocket bidirecional |
| GET | `/v1/health` | Verificação de integridade |

Suporta: título/corpo/ícone/subtítulo, progresso, botões (abrir link / iniciar programa / executar comando / retorno notify), campos de entrada, imagens, progresso animado, renovação por heartbeat, temas claro/escuro, cor de destaque personalizada, fila de prioridade. **O envio não altera a largura da ilha**.

Documentação completa: [docs/IslandAPI.md](docs/IslandAPI.md)

---

## 🛡 Privacidade e segurança

- **Sem telemetria, sem anúncios, sem envio de dados**. Exceto por "letras on-line" e "clima" ativados manualmente, o aplicativo não faz nenhuma solicitação de rede.
- Únicas conexões de rede: download de capas do Cider, letras palavra por palavra AMLL (api.amll.dev), letras on-line e clima após ativação (Open-Meteo).
- Todos os dados são armazenados localmente em `%APPDATA%\WinIslands\`; os logs registram apenas informações locais de execução.

---

## ⚠️ Limitações conhecidas

- O karaokê palavra por palavra depende da fonte e do progresso das letras: com linha do tempo palavra por palavra AMLL TTML / LRC, destaque por palavra; caso contrário, usa destaque da frase inteira.
- Alguns players relatam progresso que recua (ex.: Cider/SMTC relatam 0 brevemente): a proteção de posição ignora recuos instantâneos.
- A cobertura SMTC depende de o player registrar a sessão de mídia global; alguns players antigos só são detectados por título de janela (sem botões de controle).
- Cider 1.x (API antiga) não suportado, apenas 2.x e superior.
- Letras on-line / AMLL / clima são interfaces não oficiais e podem parar de funcionar.

---

## ❓ Perguntas frequentes

**P: A ilha não aparece?**
Confirme que algo está tocando; `HideWhenNoMedia` fica ativado por padrão, ocultar sem mídia é normal. Execute `--diagnose` para ver a lista de sessões.

**P: O Cider mostra "não conectado"?**
Verifique se "Permitir controle externo" está ativado nas configurações do Cider, confira a porta (padrão 10767) e confirme que o Cider está ativado nas configurações do WinIslands.

**P: O ícone da bandeja permanece após sair?**
Menu da bandeja → Sair; fechar a janela da ilha apenas a oculta (a ilha fica residente por design).

---

## 📄 Licença de código aberto

- Aplicativo: MIT (veja [LICENSE](LICENSE))
- Componentes de terceiros: veja [THIRD_PARTY.md](THIRD_PARTY.md)