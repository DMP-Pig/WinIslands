<div align="center">

**🌐 选择语言 / Select Language**

[简体中文](#简体中文) · [繁體中文](#繁體中文) · [English](#english) · [Español](#español) · [Français](#français) · [العربية](#العربية) · [Русский](#русский) · [Português](#português)

</div>

> **说明 / Note**: 以简体中文为标准 · Simplified Chinese is the standard reference.

---

## 简体中文

## WinIslands 1.2.4（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **🎶 卡拉OK逐字平滑重做**：逐字推进改为 smoothstep 缓动 + 字间交叉过渡，高亮像光带一样从左到右连续流动，起笔/收笔有加减速，不再一顿一顿。

- **🔋 低电量常驻指示**：电量低于阈值且未接电源时，灵动岛右上角常驻显示电量胶囊（≤10% 红色 / 其余橙色），随电量实时刷新；接上电源或电量回升后自动消失，设置中可开关。

- **🎧 设备连接动画**：蓝牙设备连接/断开时展示 iOS 风格动画卡片；连接时自动读取设备电量并显示「设备名 · 电量 xx%」，读不到电量时只显示设备名，不阻塞界面。


## WinIslands 1.2.1（正式版 / Stable）

## WinIslands 1.2.2（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **推送消息单行显示**：未展开时，上岛/消息提醒仅显示一行（图标 + 标题 + 单行摘要），超长自动省略，不再撑宽灵动岛；详情（正文/进度/按钮/输入框）在展开后完整展示

- **修复未展开时推送文字显示不全 / 向上偏移 / 超出范围**：有推送时紧凑高度依内容自动伸缩，卡片顶部对齐，不再被上下对称裁切

- **长通知自动调宽**：出现较长推送时，灵动岛自动调节横向长度，右侧的媒体控制、时钟等组件和全部文字完整显示在岛内，不再被边缘裁切

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **弹性阻尼 + 内容交错过渡**：展开/收起改为 iOS 阻尼弹簧（轻微过冲回弹），卡片内各区块依次错峰淡入/淡出，节奏非线性、不生硬，更贴近 iOS 灵动岛

- **音量调节联动动画**：系统音量变化或拖动音量滑杆时，岛上短暂显示「图标 + 音量条 + 百分比」联动动画，几秒后自动淡出

- **智能透明度分层**：紧凑态更通透、展开态更实，透明度随展开/收起平滑过渡；封面取色生效时自动避免叠加

- **封面切换过渡**：切歌时专辑封面交叉淡入 + 轻微缩放，与封面取色背景呼吸互补，换曲衔接丝滑

- **主题切换平滑过渡**：浅色/深色主题切换时背景与边框颜色平滑插值（约 0.3 秒），不再闪变

- **动画性能优化**：波纹等逐帧渲染只更新当前可见的内容，媒体播放时 CPU 占用更低、动画更流畅
## WinIslands 1.2.0（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **多歌词源一键切换**：支持在「自动 / 本地 LRC / AMLL TTML / Cider API / 在线歌词」之间一键循环切换，并立即重新加载当前歌曲歌词
- **崩溃自动恢复**：应用异常退出后，下次启动会提示已自动恢复，不再出现无响应、黑屏、状态丢失
- **动画时长可调**：新增展开/收起动画时长滑杆（300–1400ms），可自由调节动画快慢，适配个人喜好
## WinIslands 1.1.9（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **动画更丝滑连贯**：展开/收起与歌词自动滚动改为时间基准缓动（与帧率无关），在 60fps 与高刷新率（120Hz）显示器上表现一致，快速连续操作不再「动一下停一下」

- **播放时 CPU 占用进一步降低**：进度插值上报频率由 10Hz 降为 5Hz，逐字卡拉OK由控件内部按墙钟 60fps 连续推进——观感不变，后台更省电

- **专辑封面解码缓存**：同一封面只解码一次并复用（自动保留最近 24 张），消除媒体会话每秒上报带来的重复磁盘 IO 与内存抖动

- **歌词滚动细化**：仅在展开且可见时滚动，目标已接近时直接落位，切句不再抖动、空转更少

## WinIslands 1.1.8（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **动画更快更利落**：展开/收起从约 1.1 秒缩短至约 0.7 秒，弹簧更硬朗、回弹更干脆，不再软绵绵、慢吞吞

- **全局动效提速**：组件尺寸过渡、内容错峰淡入、位置重定位（约 430ms → 320ms）、上岛推送、淡出隐藏等所有动画同步加快约 30%–35%，整体响应更快、更跟手

- **动效皮肤统一提速**：iOS 弹簧 / 柔和 / 弹性 / 简洁渐隐四种动效皮肤基准时长同步缩短，保持各自风格但整体更利落


## WinIslands 1.1.7（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **文件中转站**：把任意文件直接拖到灵动岛上，立即生成一个可拖出的文件组件（仅保存路径引用、不复制文件实体）；再次用鼠标把它拖到资源管理器或其他软件即可完成搬运，临时传文件再也不用反复开窗

- **媒体退出自动清理**：媒体应用退出且没有新的播放会话时，灵动岛立即清除残留的歌曲信息，不再显示过期曲目

- **动画更丝滑**：灵动岛重定位与歌词自动滚动改用更柔和的缓动（重定位动画约 430ms、歌词滚动增量放缓），整体更连贯顺畅

- **关于页显示版本号**：设置 → 关于 现在会显示完整版本号（如 WinIslands 1.1.7）

- **稳定性与性能**：媒体会话状态检测增强（播放状态读取失败即视为媒体结束，不再残留）；位置动画增加防并发抖动处理；单文件自包含构建，体积更小

## WinIslands 1.1.6（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **锁屏自动隐藏**：按 Win+L 或远程桌面断开时自动隐藏灵动岛，解锁后自动恢复（设置 → 通用，可开关）

- **定时勿扰升级为分钟级**：勿扰开始 / 结束时间可精确到分钟设置

- **中键快捷动作**：鼠标中键单击灵动岛可执行自定义快捷动作（设置 → 通用可配置）

- **定时明暗主题切换**：设置深色时段起止小时，到点自动切换浅色 / 深色主题（仅自动主题下生效）

- **上岛输入框**：第三方推送可携带输入框，用户在岛上输入后通过 WebSocket 将输入值回传给推送方

- **后台轮询按需启停**：无相关组件或功能启用时停止对应轮询，空闲 CPU 占用更低

- 修复上岛输入框提交问题；修复上岛按钮数据（含输入值）回传丢失问题


## WinIslands 1.1.5（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

**歌词**

- **AMLL 逐字歌词（真·卡拉OK）**：接入 amll.dev 的 Apple Music 风格 TTML 曲库，设置 → 歌词「AMLL 逐字歌词」开关（默认开启）；来源优先级：本地 LRC → AMLL TTML → Cider → 在线歌词
- **逐字高亮引擎重写**：60fps 墙钟连续推进 + 非线性缓动；整行均分兜底；双语歌词按时间轴匹配
- **暂停高亮稳定**：暂停后冻结在暂停时刻，退出重启不跳动；仅活动行驱动动画，空闲近 0 CPU

**修复**

- 紧凑态歌词间距修复：歌词与右侧按钮保持固定间距；空间不足自动扩大岛宽（720→800）
- 稳定性：AMLL 5 秒超时 + 优雅降级；控件重新可见校准墙钟基准


## WinIslands 1.1.4（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **快捷操作按钮**：展开灵动岛卡片底部新增一排可自定义的快捷按钮（锁屏 / 静音 / 播放暂停 / 截图 / 显示桌面 / 任务管理器 / 计算器 / 睡眠 / 音量±）；设置 → 快捷操作 可勾选与 ↑↓ 排序，修改即时生效

- **来电提醒**：检测微信 / QQ 的语音、视频通话窗口，弹出提醒（区分「来电」与「通话中」）；设置 → 通知 可开关并自定义检测应用；仅本机检测，不上传数据

- **上岛命令动作**：第三方上岛推送按钮新增 `action: "command"`，可在本地执行命令行（仅本机回环 API，可配 Token）

- **上岛卡片主题**：第三方推送可携带 `theme: dark / light / auto`，推送卡片自动切换深浅色玻璃样式

- **动画提速**：四种动效皮肤整体时长缩短约 20%，保持 iOS 弹簧缓动与 60fps 丝滑不跳帧

- **后台占用优化**：键盘指示灯仅在组件启用时轮询、全屏监控降频、音频波纹空闲降频

- **稳定性修复**：修复启动时对剪贴板已有内容误弹「已复制」提示（启动基线）；修复日历 .ics 标题转义（\, \; \n）显示为反斜杠的问题

- **性能与稳定性**：蓝牙 / SMTC / 天气接口日志降噪，天气限流指数退避；构建通过，单元与集成测试 104 项全部通过


## WinIslands 1.1.3（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **多播放器切换**：同时打开多个播放器时，展开卡可一键切换当前控制的媒体来源（音符图标 + 来源名 + 下拉箭头），不再只能控制最后启动的播放器

- **封面沉浸**：点击展开卡的专辑封面 / 大图即可打开全屏封面预览，点击 / Esc / 右键淡入淡出关闭

- **歌词时间微调**：展开卡歌词区增设 +0.5s / -0.5s 按钮，按歌曲记忆时间偏移，让歌词与音乐完美对齐

- **桌面歌词增强**：独立歌词小窗支持不透明度调节（默认 0.85）与「锁定」开关（锁定后鼠标穿透、不可拖动）

- **动态主题呼吸**：开启封面取色背景时，展开卡背景色随封面颜色缓慢「呼吸」起伏（约 18 秒一周期），不再是静止的平板色块

- **点击抢先**：按下鼠标立即切换展开 / 收起，不再等待松开才响应，手感更跟手

- **通知操作按钮**：通知横幅支持操作按钮（蓝牙连接提示现带「断开」「设置」按钮，点击立即执行并收起）

- **上岛按钮回调**：第三方上岛的 notify 动作按钮被点击时，通过 WebSocket 向推送方广播 push_button 事件（含 push_id 与按钮文字），推送方自行处理回调

- **性能与稳定性**：动态主题变为按需订阅合成帧（空闲 0 CPU）、渐变画刷缓存复用降低 GC 压力；修复新版本与旧版本实例同时运行互斥体冲突导致无法启动的问题


## WinIslands 1.1.1（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **电量提醒**：低电量提醒（阈值可调），连接电源且充到设定阈值（默认 100%）时弹出「充电完成」提醒，均为本地检测、可开关

- **网络提醒**：断网 / 网络恢复时弹出提示（本地网络状态检测，可开关）

- **新组件**：磁盘剩余空间（系统盘）、输入法状态（中 / 英 + 输入法名称）

- **农历与节气**：日期组件可附加显示农历日期与节气（默认开启，可在设置中关闭）

- **快捷开关组件**：WiFi / 蓝牙 / 夜间模式 / 静音 一键切换（走本地 API，无联网；Radio 状态 2 秒缓存，避免开销）

- **播放来源徽标**：媒体组件上显示当前播放来源（Spotify / Cider / 网易云 / QQ音乐等），一眼可知来自哪个播放器

- **歌词增强**：歌词翻译显示 / 隐藏开关，「复制当前行」按钮一键复制当前歌词

- **组件图标自定义**：每个组件可单独自定义图标（MDL2 图标或 Emoji），不设置则使用默认字形

- **修复歌词缩放跳动**：移除卡拉OK逐字回弹位移导致的歌词「放大又缩小」抖动；展开态当前行歌词的字号 / 透明度改为 300ms 平滑过渡，滚动更丝滑稳定


## WinIslands 1.1.0（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **音量 / 静音临时上岛**：系统音量变化、静音 / 取消静音时，灵动岛短暂显示音量指示（显示时长可调，可在设置中开关）

- **文件复制 / 移动上岛**：检测到资源管理器正在复制 / 移动文件时，灵动岛显示「正在复制文件…」提示（纯本地窗口标题识别，可开关）

- **下载进度上岛**：检测下载目录中的浏览器临时文件（.crdownload / .part / .download 等），显示「正在下载 N 个文件」（默认关闭，可在设置中开启）

- **「使用中」合并胶囊**：设置 → 组件可开启（默认关闭），把「麦克风 / 摄像头 / 会议中 / 录屏」合并为单个「使用中 · …」状态胶囊；可勾选哪些组件参与合并，参与合并的项不再单独显示

- **番茄钟增强**：点击灵动岛上的番茄钟组件可暂停 / 继续计时

- **截图 / 录屏临时上岛**：截图或开始录制时，灵动岛临时显示对应指示（灵动岛隐藏时也能触发）

- **卡拉OK逐字点亮回弹**：每句歌词从第一个字开始平滑点亮，带轻微回弹动效，更流畅自然

- 内部轮询架构优化：隐藏状态下也能触发音量 / 复制 / 下载 / 截图等临时上岛事件


## WinIslands 1.0.9（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **新组件**：GPU 占用、麦克风 / 摄像头使用中、节假日倒计时、会议中；网络组件可显示最近 32 秒迷你曲线

- **双击快捷动作**：设置 → 通用可设双击灵动岛动作为「播放 / 暂停」「打开设置」或「无动作」

- **开会静音助手**：识别会议窗口（Teams / Zoom / 腾讯会议 / 钉钉 / 飞书 / Webex / Slack / Discord / Google Meet），会议中自动勿扰（纯本地启发式）

- **屏幕录制 / 截图提示**：PrintScreen 截图提示 + 录制软件（OBS / Bandicam / Xbox Game Bar 等）检测提示

- **日历事件提醒（.ics）**：解析本地 iCalendar 文件，事件到点（可提前 N 分钟）弹横幅，纯本地

- **RSS 订阅提醒**：轮询 RSS 2.0 / Atom，新条目弹横幅

- **邮件提醒（POP3）**：只读邮件头，新邮件弹横幅，建议使用授权码

- **快速启动器（Spotlight 风格）**：`Ctrl+Space` 搜索应用 / 输入网址打开

- **剪贴板历史面板**：`Ctrl+Alt+V` 独立窗口，点击复制回剪贴板

- **规则（自动化）**：条件（始终 / 未播放 / 播放中 / 时间段 / 指定媒体程序）× 动作（隐藏 / 强制收起 / 强制显示）

- **上岛 API v3**：图片（data URI / http）、动态进度（from/to/duration 自动推进）、心跳续期（heartbeat_seconds）、PATCH 部分更新、WebSocket 通道（/v3/ws）

- **外观**：18 种主题皮肤预设、自定义背景色、4 种动效皮肤、低功耗模式

- 设置页改为 macOS System Settings 风格（左侧导航 + 右侧内容），所有改动即时生效

- **修复深色模式黑字**：为设置界面所有自定义控件模板（按钮 / 复选框 / 输入框 / 下拉框 / 下拉项 / 页签 / 左侧导航等）统一绑定前景色，并新增运行时兜底扫描——深色模式下不再出现个别选项（界面语言、双击动作等下拉框）显示黑字看不清的问题，浅色模式自动恢复深色文字

- **修复设置界面无法打开**：移除重复的 XAML 前景色行导致的 BAML 加载失败

- **动画性能优化**：卡拉OK逐字复用 Run 对象（消除逐帧布局）、稳定 60fps 故事板、日志批量刷新，动画更丝滑

- **移除组件上的红色角标圆点**（按用户要求）


## WinIslands 1.0.8（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

**设置界面全面重构**

- 采用类 macOS 系统设置布局：左侧导航列表 + 右侧内容区，共 13 个分类（通用 / 外观 / 组件 / 媒体 / 媒体信息展示 / 歌词 / Cider / 上岛 API / 效率工具 / 更新 / 关于 / 通知 / 规则）
- 深色 / 浅色模式下所有设置文字颜色自动适配：深色模式白字、浅色模式黑字，不再出现看不清的问题
- 左侧导航文字改为高对比白色，去掉右侧白色分隔线，强化悬停与选中反馈

**媒体播放**

- 新增迷你播放器：独立悬浮小窗，展示专辑封面 / 歌名 / 歌手 / 进度条与播放控制，可自由拖动并记忆位置，随媒体播放自动显示 / 隐藏（可在设置中开启）
- 新增音频输出设备切换：设置 → 媒体 可枚举并切换系统默认播放设备（切换后建议重启播放器生效）
- 播放器来源底层增强：支持枚举全部 SMTC 媒体会话与 Cider 会话，可切换媒体来源

**歌词**

- 新增双语歌词：自动合并相邻时间戳的翻译行（可在设置中关闭）

**外观与动效**

- 新增动效皮肤：4 种动画风格（iOS 弹簧（默认）/ 柔和弹簧 / 弹性回弹 / 简洁渐隐），展开 / 收起使用非线性缓动
- 新增低功耗模式：空闲时降低波纹渲染帧率、简化动画，更省电

**全局快捷键**

- 5 个可自定义组合键：显示 / 隐藏、播放 / 暂停、上一首、下一首、展开 / 收起
- 支持 Ctrl / Alt / Shift / Win + 字母、数字、F1–F24、方向键

**智能规则引擎（设置 → 规则）**

- 按条件自动控制灵动岛显示：始终生效 / 未播放媒体时 / 正在播放媒体时 / 指定时间段 / 指定媒体程序播放时
- 动作：隐藏灵动岛 / 强制收起 / 强制显示；优先级：隐藏 > 折叠 > 强制显示

**通知**

- 通知历史支持：未读红点标记、全部已读、单条删除、点击条目打开来源应用、清空历史
- 新增通知折叠：同来源同标题的重复通知复用同一横幅并累加数量
- 新增勿扰白名单：白名单内的来源（逗号分隔 exe 名）不受勿扰影响，仍正常弹出横幅
- 移除灵动岛上的未读红点角标

**效率工具**

- 复制文本时弹出「已复制」提示
- 自动识别短信验证码并高亮提示
- 大文本复制显示进度动画（按长度估算推进，完成后再显示结果）

**组件**

- 新增节假日倒计时组件：内置 2026–2027 年节假日表（元旦 / 春节 / 清明 / 劳动节 / 端午 / 中秋 / 国庆），显示「XX N 天后」或「今日 XX」，可在组件设置中开关

**上岛 API v2**

- 新增字段：subtitle（副标题）、type（info / success / warning / error）、priority（high / normal / low）、accent（自定义强调色）、click（整卡点击回跳）
- 推送队列：多条推送按优先级高 → 低、先入先出排列；同 id 重复推送保留原队列位置与过期时间
- POST 响应新增 position 字段
- 新增可直接运行的示例脚本 docs/sdk-examples/（push.bat / pull.bat / push.ps1 / push.py / pull.py）

**修复**

- 修复设置窗口无法打开 / 双击无法运行的问题（导航与标签页切换增加空值保护并显式初始化）
- 新增 69 个自动化测试（上岛 API、通知折叠与白名单、规则引擎、验证码识别、LRC 歌词解析），全部通过

**资产 Assets**

- Windows x64 / arm64 便携版（单文件自包含，免安装直接运行）
- Windows 通用安装包（Inno Setup，同时支持 x64 与 ARM64，自动按架构安装）
- > 便携版为独立 exe 文件，不再提供 ZIP 压缩包。


## WinIslands 1.0.7（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- 「声音波纹」升级为**跟随音乐节奏**：通过 WASAPI 环回实时采集系统正在播放的真实音频，节拍强时浪高、安静时浪低，不再是固定音量条

- **60fps 连贯渲染**：起音 25ms / 释放 140ms 指数平滑，波纹起伏连贯、不生硬、不卡顿

- 新增设置（设置 → 外观 → 声音波纹）：跟随音乐节奏开关、灵敏度 0.2–3.0、波纹高度 0.4–1.6，改动即时生效

- 无音频设备 / 音频服务异常时自动降级为节拍模拟，并每 8 秒自动重试恢复实时采集，不卡死、不堆积线程


## WinIslands 1.0.6（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- 新增 6 个灵动岛组件：音量、键盘指示灯（CapsLock）、剪贴板、待办、番茄钟、日程，均支持「无歌曲 / 有歌曲」双列勾选与可拖拽排序

- 新增「效率工具」设置页：剪贴板历史、番茄钟计时、待办列表、日程提醒

- 新增「声音波纹」：播放媒体时，控制按钮左侧随系统音量实时抖动（设置 → 外观可开关）

- 新增 7 种主题预设：默认 / 海洋 / 森林 / 日落 / 霓虹 / 单色 / 葡萄紫

- 外观个性化：自定义字体、字号缩放（0.8–1.4）、胶囊圆角半径（16–40）、展开背景随专辑封面取色、未读通知角标

- 托盘菜单新增：勿扰模式（手动 / 按时段自动静默通知）、检查更新、查看日志

- 设置页现代化改版（圆角 + 液态玻璃），选项改动即时生效，无需手动保存

- 新增更新检查（托盘 / 设置手动检查，可选自动检查，默认关闭）

- 资产 Assets：Windows x64 / arm64 便携版（单文件自包含，免安装直接运行）；Windows 通用安装包（Inno Setup，同时支持 x64 与 ARM64，自动按架构安装）

- > 便携版为独立 exe 文件，不再提供 ZIP 压缩包。


## WinIslands 1.0.5（正式版 / Stable）

一款现代化、多功能的 Windows 灵动岛组件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- 新增「上岛 API」：其他软件可通过本地 HTTP 接口把信息推送到灵动岛（类似 iOS 灵动岛第三方 App 集成），**开发文档见 docs/IslandAPI.md**

-   - `POST /v1/island/push` 推送/更新 · `DELETE /v1/island/push/{id}` 移除 · `GET /v1/island/active` 查询 · `GET /v1/health`

-   - 支持图标、标题、正文、进度、按钮（打开链接/启动程序）、按条自定义显示时长

-   - 设置页提供启用 / 端口 / 可选 Token / 全局默认时长

- 上岛卡片在紧凑态单行展示、不遮挡其它组件、**不影响灵动岛长宽**（自动 / 手动尺寸恒定）

- 尺寸「自动调整」：按内容自适应，手动拖动滑杆会自动关闭对应自动项

- 展开内容支持滚轮滚动（隐藏滚动条）

- 组件上下对齐统一；启动布局 / 字体修复（强制 PerMonitorV2，启动即正常大小）

- 播放媒体不再弹「正在播放」通知

- 修复：展开灵动岛后（约 1~2 秒）卡片回退到紧凑尺寸导致整体黑屏的缺陷

-   - 展开内容改为与紧凑行重叠交叉淡入淡出，动画全程无背景透出

-   - 展开/收起动画完成后显式写回最终卡片尺寸，展开态稳定不缩回

-   - 同步修复展开态点击第三方上岛按钮黑屏的问题

- 资产 Assets：Windows x64 / arm64 便携版（单文件自包含，免安装直接运行）；Windows 通用安装包（Inno Setup，同时支持 x64 与 ARM64，自动按架构安装）

- > 便携版为独立 exe 文件，不再提供 ZIP 压缩包。

---

## 繁體中文

## WinIslands 1.2.4（正式版 / Stable）

一款現代化、多功能的 Windows 動態島元件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **🎶 卡拉OK逐字平滑重做**：逐字推進改為 smoothstep 緩動 + 字間交叉過渡，高亮像光帶一樣從左到右連續流動，起筆/收筆有加減速，不再一頓一頓。

- **🔋 低電量常駐指示**：電量低於閾值且未接電源時，動態島右上角常駐顯示電量膠囊（≤10% 紅色 / 其餘橙色），隨電量即時刷新；接上電源或電量回升後自動消失，設定中可開關。

- **🎧 裝置連線動畫**：藍牙裝置連線/中斷時展示 iOS 風格動畫卡片；連線時自動讀取裝置電量並顯示「裝置名稱 · 電量 xx%」，讀不到電量時只顯示裝置名稱，不阻塞介面。


## WinIslands 1.2.1（正式版 / Stable）

## WinIslands 1.2.2（正式版 / Stable）

一款現代化、多功能的 Windows 動態島元件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **推播訊息單行顯示**：未展開時，上島/訊息提醒僅顯示一行（圖示 + 標題 + 單行摘要），過長自動省略，不再撐寬動態島；詳情（正文/進度/按鈕/輸入框）在展開後完整展示

- **修復未展開時推播文字顯示不全 / 向上偏移 / 超出範圍**：有推播時緊湊高度依內容自動伸縮，卡片頂部對齊，不再被上下對稱裁切

- **長通知自動調寬**：出現較長推播時，動態島自動調整橫向長度，右側的媒體控制、時鐘等元件和全部文字完整顯示在島內，不再被邊緣裁切

一款現代化、多功能的 Windows 動態島元件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **彈性阻尼 + 內容交錯過渡**：展開/收起改為 iOS 阻尼彈簧（輕微過衝回彈），卡片內各區塊依序錯峰淡入/淡出，節奏非線性、不生硬，更貼近 iOS 動態島

- **音量調節連動動畫**：系統音量變化或拖動音量滑桿時，島上短暫顯示「圖示 + 音量條 + 百分比」連動動畫，幾秒後自動淡出

- **智慧透明度分層**：緊湊態更通透、展開態更實，透明度隨展開/收起平滑過渡；封面取色生效時自動避免疊加

- **封面切換過渡**：切歌時專輯封面交叉淡入 + 輕微縮放，與封面取色背景呼吸互補，換曲銜接流暢

- **主題切換平滑過渡**：淺色/深色主題切換時背景與邊框顏色平滑插值（約 0.3 秒），不再閃變

- **動畫效能最佳化**：波紋等逐幀渲染只更新目前可見的內容，媒體播放時 CPU 占用更低、動畫更流暢
## WinIslands 1.2.0（正式版 / Stable）

一款現代化、多功能的 Windows 動態島元件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **多歌詞來源一鍵切換**：支援在「自動 / 本地 LRC / AMLL TTML / Cider API / 線上歌詞」之間一鍵循環切換，並立即重新載入目前歌曲的歌詞
- **當機自動恢復**：應用程式異常結束後，下次啟動會提示已自動恢復，不再出現無回應、黑屏、狀態遺失
- **動畫時長可調**：新增展開/收起動畫時長滑桿（300–1400ms），可自由調整動畫快慢，配合個人喜好
## WinIslands 1.1.9（正式版 / Stable）

一款現代化、多功能的 Windows 動態島元件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **動畫更絲滑連貫**：展開/收起與歌詞自動捲動改為時間基準緩動（與幀率無關），在 60fps 與高更新率（120Hz）螢幕上表現一致，快速連續操作不再「動一下停一下」

- **播放時 CPU 佔用再降低**：進度插值上報頻率由 10Hz 降為 5Hz，逐字卡拉OK由控制項內部依牆鐘 60fps 連續推進——觀感不變，後台更省電

- **專輯封面解碼快取**：同一封面只解碼一次並重複使用（自動保留最近 24 張），消除媒體會話每秒上報造成的重複磁碟 IO 與記憶體抖動

- **歌詞捲動細化**：僅在展開且可見時捲動，目標已接近時直接定位，切句不再抖動、空轉更少

## WinIslands 1.1.8（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島元件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **動畫更快更俐落**：展開/收起從約 1.1 秒縮短至約 0.7 秒，彈簧更硬朗、回彈更乾脆，不再軟綿綿、慢吞吞

- **全域動效提速**：元件尺寸過渡、內容錯峰淡入、位置重定位（約 430ms → 320ms）、上島推送、淡出隱藏等所有動畫同步加快約 30%–35%，整體反應更快、更跟手

- **動效皮膚統一提速**：iOS 彈簧 / 柔和 / 彈性 / 簡潔漸隱四種動效皮膚基準時長同步縮短，保持各自風格但整體更俐落


## WinIslands 1.1.7（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島元件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **檔案中轉站**：把任意檔案直接拖到靈動島上，立即生成一個可拖出的檔案元件（僅保存路徑參照、不複製檔案實體）；再用滑鼠拖到檔案總管或其他軟體即可完成搬運

- **媒體退出自動清理**：媒體應用程式退出且沒有新的播放工作階段時，靈動島立即清除殘留的歌曲資訊，不再顯示過期曲目

- **動畫更流暢**：靈動島重新定位與歌詞自動捲動改用更柔和的緩動（重新定位動畫約 430ms），整體更連貫順暢

- **關於頁顯示版本號**：設定 → 關於 現在會顯示完整版本號（如 WinIslands 1.1.7）

- **穩定性與效能**：媒體工作階段狀態偵測增強（播放狀態讀取失敗即視為媒體結束）；位置動畫增加防並發抖動處理；單檔自包含建置，體積更小

## WinIslands 1.1.6（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **鎖屏自動隱藏**：按 Win+L 或遠程桌面斷開時自動隱藏靈動島，解鎖後自動恢復（設置 → 通用，可開關）

- **定時勿擾升級爲分鐘級**：勿擾開始 / 結束時間可精確到分鐘設置

- **中鍵快捷動作**：鼠標中鍵單擊靈動島可執行自定義快捷動作（設置 → 通用可配置）

- **定時明暗主題切換**：設置深色時段起止小時，到點自動切換淺色 / 深色主題（僅自動主題下生效）

- **上島輸入框**：第三方推送可攜帶輸入框，用戶在島上輸入後通過 WebSocket 將輸入值回傳給推送方

- **後臺輪詢按需啓停**：無相關組件或功能啓用時停止對應輪詢，空閒 CPU 佔用更低

- 修復上島輸入框提交問題；修復上島按鈕數據（含輸入值）回傳丟失問題


## WinIslands 1.1.5（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

**歌詞**

- **AMLL 逐字歌詞（真·卡拉OK）**：接入 amll.dev 的 Apple Music 風格 TTML 曲庫，設置 → 歌詞「AMLL 逐字歌詞」開關（默認開啓）；來源優先級：本地 LRC → AMLL TTML → Cider → 在線歌詞
- **逐字高亮引擎重寫**：60fps 牆鍾連續推進 + 非線性緩動；整行均分兜底；雙語歌詞按時間軸匹配
- **暫停高亮穩定**：暫停後凍結在暫停時刻，退出重啓不跳動；僅活動行驅動動畫，空閒近 0 CPU

**修復**

- 緊湊態歌詞間距修復：歌詞與右側按鈕保持固定間距；空間不足自動擴大島寬（720→800）
- 穩定性：AMLL 5 秒超時 + 優雅降級；控件重新可見校準牆鍾基準


## WinIslands 1.1.4（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **快捷操作按鈕**：展開靈動島卡片底部新增一排可自定義的快捷按鈕（鎖屏 / 靜音 / 播放暫停 / 截圖 / 顯示桌面 / 任務管理器 / 計算器 / 睡眠 / 音量±）；設置 → 快捷操作 可勾選與 ↑↓ 排序，修改即時生效

- **來電提醒**：檢測微信 / QQ 的語音、視頻通話窗口，彈出提醒（區分「來電」與「通話中」）；設置 → 通知 可開關並自定義檢測應用；僅本機檢測，不上傳數據

- **上島命令動作**：第三方上島推送按鈕新增 `action: "command"`，可在本地執行命令行（僅本機迴環 API，可配 Token）

- **上島卡片主題**：第三方推送可攜帶 `theme: dark / light / auto`，推送卡片自動切換深淺色玻璃樣式

- **動畫提速**：四種動效皮膚整體時長縮短約 20%，保持 iOS 彈簧緩動與 60fps 絲滑不跳幀

- **後臺佔用優化**：鍵盤指示燈僅在組件啓用時輪詢、全屏監控降頻、音頻波紋空閒降頻

- **穩定性修復**：修復啓動時對剪貼板已有內容誤彈「已複製」提示（啓動基線）；修復日曆 .ics 標題轉義（\, \; \n）顯示爲反斜槓的問題

- **性能與穩定性**：藍牙 / SMTC / 天氣接口日誌降噪，天氣限流指數退避；構建通過，單元與集成測試 104 項全部通過


## WinIslands 1.1.3（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **多播放器切換**：同時打開多個播放器時，展開卡可一鍵切換當前控制的媒體來源（音符圖標 + 來源名 + 下拉箭頭），不再只能控制最後啓動的播放器

- **封面沉浸**：點擊展開卡的專輯封面 / 大圖即可打開全屏封面預覽，點擊 / Esc / 右鍵淡入淡出關閉

- **歌詞時間微調**：展開卡歌詞區增設 +0.5s / -0.5s 按鈕，按歌曲記憶時間偏移，讓歌詞與音樂完美對齊

- **桌面歌詞增強**：獨立歌詞小窗支持不透明度調節（默認 0.85）與「鎖定」開關（鎖定後鼠標穿透、不可拖動）

- **動態主題呼吸**：開啓封面取色背景時，展開卡背景色隨封面顏色緩慢「呼吸」起伏（約 18 秒一周期），不再是靜止的平板色塊

- **點擊搶先**：按下鼠標立即切換展開 / 收起，不再等待鬆開才響應，手感更跟手

- **通知操作按鈕**：通知橫幅支持操作按鈕（藍牙連接提示現帶「斷開」「設置」按鈕，點擊立即執行並收起）

- **上島按鈕回調**：第三方上島的 notify 動作按鈕被點擊時，通過 WebSocket 向推送方廣播 push_button 事件（含 push_id 與按鈕文字），推送方自行處理回調

- **性能與穩定性**：動態主題變爲按需訂閱合成幀（空閒 0 CPU）、漸變畫刷緩存復用降低 GC 壓力；修復新版本與舊版本實例同時運行互斥體衝突導致無法啓動的問題


## WinIslands 1.1.1（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **電量提醒**：低電量提醒（閾值可調），連接電源且充到設定閾值（默認 100%）時彈出「充電完成」提醒，均爲本地檢測、可開關

- **網絡提醒**：斷網 / 網絡恢復時彈出提示（本地網絡狀態檢測，可開關）

- **新組件**：磁盤剩餘空間（系統盤）、輸入法狀態（中 / 英 + 輸入法名稱）

- **農曆與節氣**：日期組件可附加顯示農曆日期與節氣（默認開啓，可在設置中關閉）

- **快捷開關組件**：WiFi / 藍牙 / 夜間模式 / 靜音 一鍵切換（走本地 API，無聯網；Radio 狀態 2 秒緩存，避免開銷）

- **播放來源徽標**：媒體組件上顯示當前播放來源（Spotify / Cider / 網易雲 / QQ音樂等），一眼可知來自哪個播放器

- **歌詞增強**：歌詞翻譯顯示 / 隱藏開關，「複製當前行」按鈕一鍵複製當前歌詞

- **組件圖標自定義**：每個組件可單獨自定義圖標（MDL2 圖標或 Emoji），不設置則使用默認字形

- **修復歌詞縮放跳動**：移除卡拉OK逐字回彈位移導致的歌詞「放大又縮小」抖動；展開態當前行歌詞的字號 / 透明度改爲 300ms 平滑過渡，滾動更絲滑穩定


## WinIslands 1.1.0（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **音量 / 靜音臨時上島**：系統音量變化、靜音 / 取消靜音時，靈動島短暫顯示音量指示（顯示時長可調，可在設置中開關）

- **文件複製 / 移動上島**：檢測到資源管理器正在複製 / 移動文件時，靈動島顯示「正在複製文件…」提示（純本地窗口標題識別，可開關）

- **下載進度上島**：檢測下載目錄中的瀏覽器臨時文件（.crdownload / .part / .download 等），顯示「正在下載 N 個文件」（默認關閉，可在設置中開啓）

- **「使用中」合併膠囊**：設置 → 組件可開啓（默認關閉），把「麥克風 / 攝像頭 / 會議中 / 錄屏」合併爲單個「使用中 · …」狀態膠囊；可勾選哪些組件參與合併，參與合併的項不再單獨顯示

- **番茄鍾增強**：點擊靈動島上的番茄鍾組件可暫停 / 繼續計時

- **截圖 / 錄屏臨時上島**：截圖或開始錄製時，靈動島臨時顯示對應指示（靈動島隱藏時也能觸發）

- **卡拉OK逐字點亮回彈**：每句歌詞從第一個字開始平滑點亮，帶輕微回彈動效，更流暢自然

- 內部輪詢架構優化：隱藏狀態下也能觸發音量 / 複製 / 下載 / 截圖等臨時上島事件


## WinIslands 1.0.9（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- **新組件**：GPU 佔用、麥克風 / 攝像頭使用中、節假日倒計時、會議中；網絡組件可顯示最近 32 秒迷你曲線

- **雙擊快捷動作**：設置 → 通用可設雙擊靈動島動作爲「播放 / 暫停」「打開設置」或「無動作」

- **開會靜音助手**：識別會議窗口（Teams / Zoom / 騰訊會議 / 釘釘 / 飛書 / Webex / Slack / Discord / Google Meet），會議中自動勿擾（純本地啟發式）

- **屏幕錄製 / 截圖提示**：PrintScreen 截圖提示 + 錄製軟件（OBS / Bandicam / Xbox Game Bar 等）檢測提示

- **日曆事件提醒（.ics）**：解析本地 iCalendar 文件，事件到點（可提前 N 分鐘）彈橫幅，純本地

- **RSS 訂閱提醒**：輪詢 RSS 2.0 / Atom，新條目彈橫幅

- **郵件提醒（POP3）**：只讀郵件頭，新郵件彈橫幅，建議使用授權碼

- **快速啓動器（Spotlight 風格）**：`Ctrl+Space` 搜索應用 / 輸入網址打開

- **剪貼板歷史面板**：`Ctrl+Alt+V` 獨立窗口，點擊複製回剪貼板

- **規則（自動化）**：條件（始終 / 未播放 / 播放中 / 時間段 / 指定媒體程序）× 動作（隱藏 / 強制收起 / 強制顯示）

- **上島 API v3**：圖片（data URI / http）、動態進度（from/to/duration 自動推進）、心跳續期（heartbeat_seconds）、PATCH 部分更新、WebSocket 通道（/v3/ws）

- **外觀**：18 種主題皮膚預設、自定義背景色、4 種動效皮膚、低功耗模式

- 設置頁改爲 macOS System Settings 風格（左側導航 + 右側內容），所有改動即時生效

- **修復深色模式黑字**：爲設置界面所有自定義控件模板（按鈕 / 複選框 / 輸入框 / 下拉框 / 下拉項 / 頁籤 / 左側導航等）統一綁定前景色，並新增運行時兜底掃描——深色模式下不再出現個別選項（界面語言、雙擊動作等下拉框）顯示黑字看不清的問題，淺色模式自動恢復深色文字

- **修復設置界面無法打開**：移除重複的 XAML 前景色行導致的 BAML 加載失敗

- **動畫性能優化**：卡拉OK逐字復用 Run 對象（消除逐幀布局）、穩定 60fps 故事板、日誌批量刷新，動畫更絲滑

- **移除組件上的紅色角標圓點**（按用戶要求）


## WinIslands 1.0.8（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

**設置界面全面重構**

- 採用類 macOS 系統設置布局：左側導航列表 + 右側內容區，共 13 個分類（通用 / 外觀 / 組件 / 媒體 / 媒體信息展示 / 歌詞 / Cider / 上島 API / 效率工具 / 更新 / 關於 / 通知 / 規則）
- 深色 / 淺色模式下所有設置文字顏色自動適配：深色模式白字、淺色模式黑字，不再出現看不清的問題
- 左側導航文字改爲高對比白色，去掉右側白色分隔線，強化懸停與選中反饋

**媒體播放**

- 新增迷你播放器：獨立懸浮小窗，展示專輯封面 / 歌名 / 歌手 / 進度條與播放控制，可自由拖動並記憶位置，隨媒體播放自動顯示 / 隱藏（可在設置中開啓）
- 新增音頻輸出設備切換：設置 → 媒體 可枚舉並切換系統默認播放設備（切換後建議重啓播放器生效）
- 播放器來源底層增強：支持枚舉全部 SMTC 媒體會話與 Cider 會話，可切換媒體來源

**歌詞**

- 新增雙語歌詞：自動合併相鄰時間戳的翻譯行（可在設置中關閉）

**外觀與動效**

- 新增動效皮膚：4 種動畫風格（iOS 彈簧（默認）/ 柔和彈簧 / 彈性回彈 / 簡潔漸隱），展開 / 收起使用非線性緩動
- 新增低功耗模式：空閒時降低波紋渲染幀率、簡化動畫，更省電

**全局快捷鍵**

- 5 個可自定義組合鍵：顯示 / 隱藏、播放 / 暫停、上一首、下一首、展開 / 收起
- 支持 Ctrl / Alt / Shift / Win + 字母、數字、F1–F24、方向鍵

**智能規則引擎（設置 → 規則）**

- 按條件自動控制靈動島顯示：始終生效 / 未播放媒體時 / 正在播放媒體時 / 指定時間段 / 指定媒體程序播放時
- 動作：隱藏靈動島 / 強制收起 / 強制顯示；優先級：隱藏 > 摺疊 > 強制顯示

**通知**

- 通知歷史支持：未讀紅點標記、全部已讀、單條刪除、點擊條目打開來源應用、清空歷史
- 新增通知摺疊：同來源同標題的重複通知復用同一橫幅並累加數量
- 新增勿擾白名單：白名單內的來源（逗號分隔 exe 名）不受勿擾影響，仍正常彈出橫幅
- 移除靈動島上的未讀紅點角標

**效率工具**

- 複製文本時彈出「已複製」提示
- 自動識別短信驗證碼並高亮提示
- 大文本複製顯示進度動畫（按長度估算推進，完成後再顯示結果）

**組件**

- 新增節假日倒計時組件：內置 2026–2027 年節假日表（元旦 / 春節 / 清明 / 勞動節 / 端午 / 中秋 / 國慶），顯示「XX N 天后」或「今日 XX」，可在組件設置中開關

**上島 API v2**

- 新增字段：subtitle（副標題）、type（info / success / warning / error）、priority（high / normal / low）、accent（自定義強調色）、click（整卡點擊回跳）
- 推送隊列：多條推送按優先級高 → 低、先入先出排列；同 id 重複推送保留原隊列位置與過期時間
- POST 響應新增 position 字段
- 新增可直接運行的示例腳本 docs/sdk-examples/（push.bat / pull.bat / push.ps1 / push.py / pull.py）

**修復**

- 修復設置窗口無法打開 / 雙擊無法運行的問題（導航與標籤頁切換增加空值保護並顯式初始化）
- 新增 69 個自動化測試（上島 API、通知摺疊與白名單、規則引擎、驗證碼識別、LRC 歌詞解析），全部通過

**資產 Assets**

- Windows x64 / arm64 便攜版（單文件自包含，免安裝直接運行）
- Windows 通用安裝包（Inno Setup，同時支持 x64 與 ARM64，自動按架構安裝）
- > 便攜版爲獨立 exe 文件，不再提供 ZIP 壓縮包。


## WinIslands 1.0.7（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- 「聲音波紋」升級爲**跟隨音樂節奏**：通過 WASAPI 環回實時採集系統正在播放的真實音頻，節拍強時浪高、安靜時浪低，不再是固定音量條

- **60fps 連貫渲染**：起音 25ms / 釋放 140ms 指數平滑，波紋起伏連貫、不生硬、不卡頓

- 新增設置（設置 → 外觀 → 聲音波紋）：跟隨音樂節奏開關、靈敏度 0.2–3.0、波紋高度 0.4–1.6，改動即時生效

- 無音頻設備 / 音頻服務異常時自動降級爲節拍模擬，並每 8 秒自動重試恢復實時採集，不卡死、不堆積線程


## WinIslands 1.0.6（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- 新增 6 個靈動島組件：音量、鍵盤指示燈（CapsLock）、剪貼板、待辦、番茄鍾、日程，均支持「無歌曲 / 有歌曲」雙列勾選與可拖拽排序

- 新增「效率工具」設置頁：剪貼板歷史、番茄鍾計時、待辦列表、日程提醒

- 新增「聲音波紋」：播放媒體時，控制按鈕左側隨系統音量實時抖動（設置 → 外觀可開關）

- 新增 7 種主題預設：默認 / 海洋 / 森林 / 日落 / 霓虹 / 單色 / 葡萄紫

- 外觀個性化：自定義字體、字號縮放（0.8–1.4）、膠囊圓角半徑（16–40）、展開背景隨專輯封面取色、未讀通知角標

- 託盤菜單新增：勿擾模式（手動 / 按時段自動靜默通知）、檢查更新、查看日誌

- 設置頁現代化改版（圓角 + 液態玻璃），選項改動即時生效，無需手動保存

- 新增更新檢查（託盤 / 設置手動檢查，可選自動檢查，默認關閉）

- 資產 Assets：Windows x64 / arm64 便攜版（單文件自包含，免安裝直接運行）；Windows 通用安裝包（Inno Setup，同時支持 x64 與 ARM64，自動按架構安裝）

- > 便攜版爲獨立 exe 文件，不再提供 ZIP 壓縮包。


## WinIslands 1.0.5（正式版 / Stable）

一款現代化、多功能的 Windows 靈動島組件。A modern, multi-functional Dynamic Island widget for Windows.

### 更新內容

- 新增「上島 API」：其他軟件可通過本地 HTTP 接口把信息推送到靈動島（類似 iOS 靈動島第三方 App 集成），**開發文檔見 docs/IslandAPI.md**

-   - `POST /v1/island/push` 推送/更新 · `DELETE /v1/island/push/{id}` 移除 · `GET /v1/island/active` 查詢 · `GET /v1/health`

-   - 支持圖標、標題、正文、進度、按鈕（打開鏈接/啓動程序）、按條自定義顯示時長

-   - 設置頁提供啓用 / 端口 / 可選 Token / 全局默認時長

- 上島卡片在緊湊態單行展示、不遮擋其它組件、**不影響靈動島長寬**（自動 / 手動尺寸恆定）

- 尺寸「自動調整」：按內容自適應，手動拖動滑杆會自動關閉對應自動項

- 展開內容支持滾輪滾動（隱藏滾動條）

- 組件上下對齊統一；啓動布局 / 字體修復（強制 PerMonitorV2，啓動即正常大小）

- 播放媒體不再彈「正在播放」通知

- 修復：展開靈動島後（約 1~2 秒）卡片回退到緊湊尺寸導致整體黑屏的缺陷

-   - 展開內容改爲與緊湊行重疊交叉淡入淡出，動畫全程無背景透出

-   - 展開/收起動畫完成後顯式寫回最終卡片尺寸，展開態穩定不縮回

-   - 同步修復展開態點擊第三方上島按鈕黑屏的問題

- 資產 Assets：Windows x64 / arm64 便攜版（單文件自包含，免安裝直接運行）；Windows 通用安裝包（Inno Setup，同時支持 x64 與 ARM64，自動按架構安裝）

- > 便攜版爲獨立 exe 文件，不再提供 ZIP 壓縮包。

---

## English

## WinIslands 1.2.4（正式版 / Stable）

A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **🎶 Smoother karaoke highlighting**: character-by-character progress now uses smoothstep easing with cross-fades between letters — the highlight flows continuously left to right like a light band, easing in and out instead of stepping abruptly.

- **🔋 Persistent low-battery indicator**: when the battery drops below the threshold and is not charging, a small battery pill stays in the island's top-right corner (red ≤10% / orange otherwise), refreshing in real time; it disappears automatically when plugged in or recovered, and can be toggled in settings.

- **🎧 Device connection animation**: Bluetooth connect/disconnect now shows an iOS-style animated card; on connect, the device's battery is read automatically and shown as "Device name · Battery xx%" (name only when battery can't be read), without blocking the UI.


## WinIslands 1.2.1 (Stable)

## WinIslands 1.2.2 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### What's New

- **Single-line compact alerts**: while collapsed, push notifications and reminders show just one line (icon + title + one-line summary) and truncate when too long, so they no longer widen the island; full details (body/progress/buttons/input) appear when expanded

- **Fixed truncated/shifted notification text when collapsed**: with an active push, the compact height grows with the content and the card aligns to the top — no more vertical clipping

- **Auto-widen island for long notifications**: when a long notification arrives, the island auto-expands horizontally so media controls, the clock and all text stay fully inside the island instead of being clipped at the edge

A modern, multi-functional Dynamic Island widget for Windows.

### What's New

- **Spring damping + staggered transitions**: expand/collapse now uses an iOS-style damped spring (with a subtle overshoot bounce), and the card's sections fade in/out one by one with a non-linear rhythm — closer to the iOS Dynamic Island

- **Volume-change linked animation**: when the system volume changes or you drag the volume slider, the island briefly shows an animated "icon + volume bar + percentage" indicator that fades out on its own after a few seconds

- **Smart opacity layering**: the compact state is more see-through and the expanded state more solid, transitioning smoothly; automatically avoids double-layering when cover tinting is active

- **Cover-switch transition**: when the track changes, the album art cross-fades with a subtle zoom, complementing the breathing cover-tint background

- **Smooth theme switching**: switching between light/dark themes now interpolates the card background and border colors over ~0.3s instead of flashing

- **Animation performance**: per-frame effects (such as the wave) only update the currently visible panel, lowering CPU usage during playback for smoother animation
## WinIslands 1.2.0 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### What's New

- **One-tap lyric-source switching**: cycle instantly between Auto / Local LRC / AMLL TTML / Cider API / Online lyrics, and the current song's lyrics reload right away
- **Automatic crash recovery**: after an abnormal exit, the next launch shows a recovery notice — no more hangs, black screens, or lost state
- **Adjustable animation duration**: a new expand/collapse animation-duration slider (300–1400ms) lets you fine-tune animation speed to your liking
## WinIslands 1.1.9 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### What's New

- **Silkier, more coherent animations**: expand/collapse and lyric auto-scroll now use time-based easing (frame-rate independent), identical on 60fps and high-refresh (120Hz) displays — rapid consecutive actions no longer stutter or jerk

- **Even lower CPU while playing**: progress interpolation is reported at 5Hz while per-word karaoke advances continuously at 60fps from wall-clock inside the control — same visuals, less background power

- **Album-art decoding cache**: each cover is decoded once and reused (the most recent 24 are auto-evicted), eliminating repeated disk I/O and memory churn from per-second media-session updates

- **Lyric-scroll refinements**: scrolling only runs when expanded and visible, and snaps to the target when nearly reached — no jitter on fast line changes, less idle ticking

## WinIslands 1.1.8 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### What's New

- **Snappier, faster animations**: expand/collapse shortened from ~1.1s to ~0.7s; the spring is firmer with a crisper settle — no more sluggish, mushy motion

- **Global motion speed-up**: component size transitions, staggered content fades, repositioning (~430ms → 320ms), island pushes, and fade-out hiding are all ~30%–35% faster — snappier and more responsive

- **All animation skins accelerated**: iOS Spring / Soft / Elastic / Fade share the same reduced base durations, keeping their character but feeling snappier overall


## WinIslands 1.1.7 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- **File Transfer Station**: Drop any file directly onto the island and it instantly becomes a draggable file item (path reference only, no file copy). Drag it out to File Explorer or any other app to move it on the fly — no more opening folders back and forth

- **Media exit cleanup**: When a media app quits and no new playback session exists, the island immediately clears the stale track info

- **Smoother animations**: Repositioning and lyric auto-scroll use gentler easing (reposition ~430ms, slower scroll increment) for a more fluid feel

- **Version on About page**: Settings → About now shows the full version (e.g. WinIslands 1.1.7)

- **Stability & performance**: Enhanced media session state detection (unreadable playback state is treated as media ended); anti-concurrent-jitter protection for position animation; smaller single-file self-contained build

## WinIslands 1.1.6 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- **Auto-hide on lock screen**: The island auto-hides when you press Win+L or the remote desktop disconnects, and restores after unlock (Settings → General, toggleable)

- **Scheduled Do Not Disturb is now minute-level**: Start/end times for Do Not Disturb can be set down to the minute

- **Middle-click quick action**: Middle-clicking the island performs a custom quick action (configurable in Settings → General)

- **Scheduled light/dark theme switching**: Set start/end hours for the dark period, and the theme switches automatically (only takes effect with Theme=Auto)

- **Island input fields**: Third-party pushes can include an input field; when the user types on the island, the value is sent back to the pusher via WebSocket

- **On-demand background polling**: Polling stops when no related widget/feature is enabled, reducing idle CPU usage

- Fixed island input submission; fixed lost push-button data (including input value) callbacks


## WinIslands 1.1.5 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

**Lyrics**

- **AMLL word-by-word lyrics (true karaoke)**: Integrates amll.dev Apple Music-style TTML library; Settings → Lyrics, “AMLL word-by-word lyrics” toggle (on by default); source priority: local LRC → AMLL TTML → Cider → online lyrics
- **Word-highlight engine rewritten**: continuous 60fps wall-clock advancement + non-linear easing; whole-line fallback; bilingual lyrics matched by timeline
- **Stable pause highlight**: freezing at the pause moment after pausing; no jumping after exit and restart; only the active line drives animation, near 0 CPU when idle

**Fixes**

- Compact-state lyrics spacing fix: fixed spacing between lyrics and the right-side buttons; island width auto-grows when space is insufficient (720→800)
- Stability: AMLL 5s timeout + graceful fallback; wall-clock baseline recalibrated when controls become visible again


## WinIslands 1.1.4 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- **Quick-action buttons**: a new row of customizable quick buttons at the bottom of the expanded island card (lock screen / mute / play-pause / screenshot / show desktop / task manager / calculator / sleep / volume±); Settings → Quick Actions lets you check and reorder with ↑↓, changes take effect immediately

- **Incoming-call alerts**: detects voice/video call windows of WeChat / QQ and shows an alert (distinguishing “incoming call” from “in call”); Settings → Notifications toggles it and lets you customize detected apps; local detection only, no data upload

- **Island command action**: third-party push buttons support `action: "command"` to run a command line locally (loopback-only API, Token configurable)

- **Island card theme**: third-party pushes can carry `theme: dark / light / auto`; pushed cards auto-switch between light/dark glass styles

- **Faster animations**: overall duration of the four motion skins shortened by ~20%, keeping iOS spring easing and smooth 60fps without dropped frames

- **Background usage optimization**: keyboard indicators poll only when the widget is enabled, fullscreen monitor is throttled, audio wave is throttled when idle

- **Stability fixes**: fixed spurious “Copied” toast on startup with pre-existing clipboard content (startup baseline); fixed calendar .ics title escaping (\, \; \n) showing as backslashes

- **Performance & stability**: reduced log noise for Bluetooth / SMTC / weather APIs, exponential backoff for weather rate limiting; build passes, all 104 unit & integration tests pass


## WinIslands 1.1.3 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- **Multi-player switching**: with several players open, the expanded card can switch the controlled media source with one click (note icon + source name + dropdown arrow); no longer limited to the last-launched player

- **Immersive cover**: click the album art / large image on the expanded card to open a fullscreen cover preview; click / Esc / right-click fades it out

- **Lyric time fine-tuning**: +0.5s / -0.5s buttons in the lyric area of the expanded card; per-song offset is remembered to align lyrics perfectly with music

- **Desktop lyrics enhanced**: standalone lyrics window supports opacity adjustment (default 0.85) and a “Lock” toggle (locked = click-through, not draggable)

- **Dynamic theme breathing**: with cover-color background enabled, the expanded card background slowly “breathes” with the cover color (about an 18s cycle) instead of a static flat block

- **Click-to-act first**: pressing the mouse immediately toggles expand/collapse without waiting for release, feeling more responsive

- **Notification action buttons**: notification banners support action buttons (Bluetooth connect alert now has “Disconnect” and “Settings” buttons, executing immediately and collapsing)

- **Island button callback**: when a notify action button from a third-party push is clicked, the push_button event (with push_id and button text) is broadcast to the pusher over WebSocket for it to handle

- **Performance & stability**: dynamic theme subscribes to composition frames on demand (0 CPU idle), gradient brushes cached/reused to lower GC pressure; fixed mutex conflict preventing startup when new and old versions run at the same time


## WinIslands 1.1.1 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- **Battery alerts**: low-battery alert (adjustable threshold); “Charging complete” alert when plugged in and charged to the set threshold (default 100%); all local detection, toggleable

- **Network alerts**: alert on network loss / restoration (local network detection, toggleable)

- **New widgets**: disk free space (system drive), input method status (CN / EN + IME name)

- **Lunar calendar & solar terms**: the date widget can optionally show lunar date and solar terms (on by default, can be disabled in settings)

- **Quick-toggle widgets**: one-click toggle for WiFi / Bluetooth / night mode / mute (local APIs, no network; Radio state cached for 2s to avoid overhead)

- **Playback source badge**: the media widget shows the current playback source (Spotify / Cider / NetEase Cloud / QQ Music, etc.), so you know which player it comes from at a glance

- **Lyrics enhancements**: show/hide lyrics translation toggle; one-click “Copy current line” button

- **Custom widget icons**: each widget can have its own custom icon (MDL2 icon or Emoji); default glyph used if unset

- **Fixed lyric scale jumping**: removed the “grow then shrink” jitter caused by karaoke word bounce offset; font size/opacity of the current lyric line in expanded state now transitions smoothly over 300ms for smoother scrolling


## WinIslands 1.1.0 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- **Volume / mute temporary island**: when system volume changes or mute toggles, the island briefly shows a volume indicator (display duration adjustable, toggleable in settings)

- **File copy / move on island**: when Explorer is copying/moving files, the island shows a “Copying files…” hint (pure local window-title detection, toggleable)

- **Download progress on island**: detects browser temp files in the download directory (.crdownload / .part / .download, etc.) and shows “Downloading N file(s)” (off by default, can be enabled in settings)

- **“In use” merged capsule**: Settings → Widgets can enable (off by default) merging “Mic / Camera / In meeting / Recording” into a single “In use · …” status capsule; choose which widgets participate; merged ones no longer show separately

- **Pomodoro enhancements**: clicking the Pomodoro widget on the island pauses / resumes the timer

- **Screenshot / recording temporary island**: when taking a screenshot or starting a recording, the island temporarily shows the corresponding indicator (works even when the island is hidden)

- **Karaoke word lighting with bounce**: each lyric line lights up smoothly from the first word with a slight bounce effect, more fluid and natural

- Internal polling architecture optimized: temporary island events such as volume / copy / download / screenshot can be triggered while hidden


## WinIslands 1.0.9 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- **New widgets**: GPU usage, mic / camera in use, holiday countdown, in meeting; network widget can show a mini 32-second curve

- **Double-click quick action**: Settings → General lets you set the double-click action to “Play / Pause”, “Open Settings”, or “None”

- **Meeting mute assistant**: recognizes meeting windows (Teams / Zoom / Tencent Meeting / DingTalk / Feishu / Webex / Slack / Discord / Google Meet) and auto-DND during meetings (purely local heuristics)

- **Screen recording / screenshot alerts**: PrintScreen screenshot toast + recording software (OBS / Bandicam / Xbox Game Bar, etc.) detection toast

- **Calendar event reminders (.ics)**: parses local iCalendar files; banner alert when an event is due (with optional N-minute lead time); fully local

- **RSS subscription alerts**: polls RSS 2.0 / Atom, banner on new entries

- **Email alerts (POP3)**: reads only message headers, banner on new mail; an authorization code is recommended

- **Quick launcher (Spotlight-style)**: `Ctrl+Space` searches apps / opens URLs

- **Clipboard history panel**: `Ctrl+Alt+V` opens a standalone window; click to copy back to the clipboard

- **Rules (automation)**: conditions (always / not playing / playing / time range / specific media app) × actions (hide / force collapse / force show)

- **Island API v3**: images (data URI / http), animated progress (from/to/duration auto-advancing), heartbeat renewal (heartbeat_seconds), PATCH partial updates, WebSocket channel (/v3/ws)

- **Appearance**: 18 theme presets, custom background color, 4 motion skins, low-power mode

- Settings page restyled to a macOS System Settings look (left nav + right content); all changes take effect immediately

- **Fixed black text in dark mode**: unified foreground-color binding for all custom control templates in settings (buttons / checkboxes / inputs / dropdowns / dropdown items / tabs / left nav, etc.) plus a runtime fallback scan — dark mode no longer shows individual options (interface language, double-click action dropdowns, etc.) with unreadable black text; light mode automatically restores dark text

- **Fixed settings page failing to open**: removed duplicate XAML foreground-color lines that caused BAML load failure

- **Animation performance optimization**: karaoke word highlighting reuses Run objects (eliminating per-frame layout), stabilized 60fps storyboards, batched log refresh — smoother animations

- **Removed red badge dots on widgets** (per user request)


## WinIslands 1.0.8 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

**Settings UI full rebuild**

- macOS-like System Settings layout: left navigation list + right content area, 13 categories (General / Appearance / Widgets / Media / Media info display / Lyrics / Cider / Island API / Productivity / Update / About / Notifications / Rules)
- All settings text colors auto-adapt to dark/light mode: white in dark mode, black in light mode; no more unreadable text
- Left nav text changed to high-contrast white, right white divider removed, stronger hover & selection feedback

**Media playback**

- New mini player: standalone floating window showing album art / title / artist / progress bar and playback controls; freely draggable with remembered position; auto shows/hides with media playback (enable in settings)
- New audio output device switching: Settings → Media can enumerate and switch the system default playback device (restarting the player is recommended after switching)
- Player-source backend enhanced: enumerates all SMTC media sessions and Cider sessions; can switch media sources

**Lyrics**

- New bilingual lyrics: automatically merges adjacent translation lines by timestamp (can be disabled in settings)

**Appearance & motion**

- New motion skins: 4 animation styles (iOS spring (default) / soft spring / elastic bounce / simple fade), non-linear easing for expand/collapse
- New low-power mode: lowers wave render rate and simplifies animations when idle to save power

**Global hotkeys**

- 5 customizable key combinations: show/hide, play/pause, previous, next, expand/collapse
- Supports Ctrl / Alt / Shift / Win + letters, digits, F1–F24, arrow keys

**Smart rules engine (Settings → Rules)**

- Auto-controls island visibility by condition: always / when no media is playing / when media is playing / specific time range / when a specific media app is playing
- Actions: hide island / force collapse / force show; priority: hide > collapse > force show

**Notifications**

- Notification history support: unread red-dot marker, mark all read, delete single item, click item to open source app, clear history
- New notification collapsing: repeated notifications from the same source and title reuse one banner and accumulate the count
- New DND whitelist: sources in the whitelist (comma-separated exe names) are unaffected by DND and still show banners normally
- Removed the unread red-dot badge on the island

**Productivity**

- “Copied” toast when copying text
- Auto-detects SMS verification codes and highlights them
- Large-text copy shows a progress animation (estimated by length, shows result after finishing)

**Widgets**

- New holiday countdown widget: built-in 2026–2027 holiday table (New Year / Spring Festival / Qingming / Labor Day / Dragon Boat / Mid-Autumn / National Day), showing “XX in N days” or “Today XX”; toggleable in widget settings

**Island API v2**

- New fields: subtitle, type (info / success / warning / error), priority (high / normal / low), accent (custom accent color), click (whole-card click callback)
- Push queue: multiple pushes ordered high → low priority, first-in first-out; re-pushing same id keeps original queue position and expiry
- POST response now includes a position field
- New ready-to-run sample scripts in docs/sdk-examples/ (push.bat / pull.bat / push.ps1 / push.py / pull.py)

**Fixes**

- Fixed settings window failing to open / double-click failing to run (null-value guards and explicit initialization for nav & tab switching)
- Added 69 automated tests (Island API, notification collapsing & whitelist, rules engine, verification-code recognition, LRC parsing), all passing

**Assets**

- Windows x64 / arm64 portable builds (single-file self-contained, no install, run directly)
- Windows universal installer (Inno Setup, supports x64 and ARM64, installs by architecture)
- > Portable builds are standalone exe files; ZIP archives are no longer provided.


## WinIslands 1.0.7 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- The “sound wave” is upgraded to **follow the music rhythm**: real-time capture of actual system audio via WASAPI loopback; tall waves on strong beats, low waves when quiet — no longer a fixed volume bar

- **60fps continuous rendering**: exponential smoothing with 25ms attack / 140ms release; wave motion is continuous, not stiff or choppy

- New settings (Settings → Appearance → Sound wave): follow-music-rhythm toggle, sensitivity 0.2–3.0, wave height 0.4–1.6; changes take effect immediately

- When there is no audio device / the audio service is abnormal, it gracefully falls back to beat simulation and retries restoring live capture every 8 seconds — no freezes, no thread buildup


## WinIslands 1.0.6 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- Added 6 island widgets: volume, keyboard indicators (CapsLock), clipboard, to-dos, Pomodoro, schedule — all support “without song / with song” two-column checkboxes and drag-to-reorder

- Added “Productivity” settings page: clipboard history, Pomodoro timer, to-do list, schedule reminders

- Added “sound wave”: while media plays, the area left of the control buttons pulses with the system volume in real time (toggleable in Settings → Appearance)

- Added 7 theme presets: Default / Ocean / Forest / Sunset / Neon / Monochrome / Grape

- Appearance personalization: custom font, font-size scale (0.8–1.4), capsule corner radius (16–40), expanded background tinted from album art, unread notification badge

- Tray menu additions: Do Not Disturb mode (manual / scheduled silent notifications), check for updates, view logs

- Modernized settings page (rounded corners + liquid glass); changes take effect immediately, no manual save

- New update check (manual from tray / settings, optional auto-check, off by default)

- Assets: Windows x64 / arm64 portable builds (single-file self-contained, no install, run directly); Windows universal installer (Inno Setup, supports x64 and ARM64, installs by architecture)

- > Portable builds are standalone exe files; ZIP archives are no longer provided.


## WinIslands 1.0.5 (Stable)

A modern, multi-functional Dynamic Island widget for Windows.

### Changelog

- Added “Island API”: other software can push information to the island via a local HTTP API (like third-party App integration with the iOS island); **developer docs at docs/IslandAPI.md**

-   - `POST /v1/island/push` push/update · `DELETE /v1/island/push/{id}` remove · `GET /v1/island/active` query · `GET /v1/health`

-   - Supports icon, title, body, progress, buttons (open link / launch program), per-item custom display duration

-   - Settings page provides enable / port / optional Token / global default duration

- Pushed cards show on a single line in compact state, never cover other widgets, and **do not affect island width or height** (auto / manual sizes stay constant)

- Size “auto-adjust”: adapts to content; manually dragging a slider auto-disables the corresponding auto option

- Expanded content supports mouse-wheel scrolling (scrollbar hidden)

- Widget top/bottom alignment unified; startup layout / font fixes (PerMonitorV2 enforced, correct size from launch)

- No more “Now Playing” toast when media plays

- Fixed: after expanding the island (about 1–2s), the card reverting to compact size caused a full black screen

-   - Expanded content now cross-fades overlapped with the compact row; no background shows through during animations

-   - Final card size is explicitly written back after expand/collapse animations complete; expanded state stays stable and does not shrink

-   - Also fixed the black screen when clicking third-party island buttons in expanded state

- Assets: Windows x64 / arm64 portable builds (single-file self-contained, no install, run directly); Windows universal installer (Inno Setup, supports x64 and ARM64, installs by architecture)

- > Portable builds are standalone exe files; ZIP archives are no longer provided.

---

## Español

## WinIslands 1.2.4（正式版 / Stable）

Una moderna y multifuncional Dynamic Island para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **🎶 Resaltado de karaoke más fluido**: el avance letra a letra ahora usa suavizado smoothstep con transiciones cruzadas entre caracteres: el resaltado fluye de izquierda a derecha como una cinta de luz, con aceleración/deceleración, sin pasos bruscos.

- **🔋 Indicador de batería baja persistente**: cuando la batería baja del umbral y no está cargando, una pequeña píldora de batería permanece en la esquina superior derecha de la isla (roja ≤10 % / naranja en otros casos), actualizándose en tiempo real; desaparece al conectar la carga o recuperarse, y se puede activar en los ajustes.

- **🎧 Animación de conexión de dispositivos**: al conectar/desconectar un dispositivo Bluetooth se muestra una tarjeta animada estilo iOS; al conectar se lee automáticamente la batería y se muestra «Nombre del dispositivo · Batería xx %» (solo el nombre si no se puede leer), sin bloquear la interfaz.


## WinIslands 1.2.1 (Estable)

## WinIslands 1.2.2 (Estable)

Una moderna y multifuncional Dynamic Island para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Novedades

- **Alertas compactas de una sola línea**: plegadas, las notificaciones y recordatorios muestran una sola línea (icono + título + resumen de una línea) y se truncan si son muy largas, sin ensanchar la isla; los detalles completos aparecen al expandir

- **Corregido el texto truncado/desplazado al estar plegado**: con una notificación activa, la altura compacta crece con el contenido y la tarjeta se alinea arriba — sin recortes verticales

- **La isla se ensancha automáticamente con notificaciones largas**: al llegar una notificación larga, la isla expande su ancho para que los controles de medios, el reloj y todo el texto queden dentro, sin recortarse en el borde

Una moderna y multifuncional Dynamic Island para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Novedades

- **Amortiguación elástica + transiciones escalonadas**: expandir/contraer ahora usa un resorte amortiguado estilo iOS (con un pequeño rebote) y las secciones de la tarjeta entran/salen escalonadamente con un ritmo no lineal — más cerca de la Dynamic Island de iOS

- **Animación vinculada al volumen**: cuando cambia el volumen del sistema o arrastras el control deslizante, la isla muestra brevemente una animación de "icono + barra de volumen + porcentaje" que se desvanece sola tras unos segundos

- **Capas de opacidad inteligentes**: el estado compacto es más transparente y el expandido más sólido, con transición suave; evita la doble capa cuando el tinte de portada está activo

- **Transición al cambiar de portada**: al cambiar de canción, la portada se funde con un zoom sutil, complementando el fondo de tinte respirante

- **Cambio de tema suave**: al cambiar entre temas claro/oscuro, el fondo y el borde de la tarjeta interpolaran en ~0,3 s en lugar de parpadear

- **Optimización de animación**: los efectos por fotograma (como la onda) solo actualizan el panel visible, reduciendo el uso de CPU durante la reproducción para una animación más fluida
## WinIslands 1.2.0 (Estable)

Una moderna y multifuncional Dynamic Island para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Novedades

- **Cambio de fuente de letras con un clic**: alterna al instante entre Automática / LRC local / AMLL TTML / API de Cider / Letras en línea, y las letras de la canción actual se recargan de inmediato
- **Recuperación automática tras fallos**: después de un cierre anómalo, el siguiente inicio muestra un aviso de recuperación — sin bloqueos, pantallas negras ni estados perdidos
- **Duración de animación ajustable**: nuevo control deslizante de duración de expandir/contraer (300–1400ms) para ajustar la velocidad a tu gusto
## WinIslands 1.1.9 (Estable)

Una moderna y multifuncional Dynamic Island para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Novedades

- **Animaciones más fluidas y coherentes**: expandir/contraer y el desplazamiento automático de la letra usan ahora una interpolación basada en el tiempo (independiente de la frecuencia de cuadros), idéntica en pantallas de 60fps y de alta frecuencia (120Hz) — las acciones rápidas consecutivas ya no se entrecortan

- **Menor uso de CPU durante la reproducción**: la interpolación del progreso se notifica a 5Hz mientras el karaoke palabra por palabra avanza continuamente a 60fps mediante el reloj interno del control — misma apariencia, menos consumo en segundo plano

- **Caché de decodificación de carátulas**: cada carátula se decodifica una sola vez y se reutiliza (las 24 más recientes se eliminan automáticamente), eliminando la E/S de disco repetida y la fluctuación de memoria de las actualizaciones por segundo de la sesión multimedia

- **Mejoras en el desplazamiento de la letra**: solo se desplaza cuando está expandido y visible, y se asienta al alcanzar el objetivo — sin tirones al cambiar de línea, con menos repeticiones innecesarias

## WinIslands 1.1.8 (Estable)

Un widget moderno y multifuncional de Dynamic Island para Windows.

### Novedades

- **Animaciones más rápidas y ágiles**: expandir/contraer se reduce de ~1,1 s a ~0,7 s; el resorte es más firme y con un asentamiento más nítido, sin movimiento lento ni blando

- **Aceleración global del movimiento**: las transiciones de tamaño de componentes, fundidos escalonados, reposicionamiento (~430 ms → 320 ms), pulsaciones en la isla y desvanecimientos se aceleran ~30–35 %: más ágiles y receptivas

- **Todos los estilos de animación acelerados**: iOS Spring / Soft / Elastic / Fade comparten las mismas duraciones base reducidas, manteniendo su carácter pero con una sensación más ágil


## WinIslands 1.1.7 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- **Estación de transferencia de archivos**: suelta cualquier archivo directamente sobre la isla y se convierte al instante en un elemento de archivo arrastrable (solo referencia de ruta, sin copiar el archivo). Arrástralo al Explorador de archivos o a cualquier otra aplicación para moverlo sobre la marcha

- **Limpieza al salir del reproductor**: cuando una aplicación multimedia se cierra y no hay una nueva sesión de reproducción, la isla borra inmediatamente la información de la canción obsoleta

- **Animaciones más fluidas**: el reposicionamiento y el desplazamiento automático de la letra usan una suavización más suave (reposicionamiento ~430 ms) para un movimiento más continuo

- **Versión en la página Acerca de**: Configuración → Acerca de ahora muestra la versión completa (p. ej., WinIslands 1.1.7)

- **Estabilidad y rendimiento**: detección mejorada del estado de la sesión multimedia; protección contra vibraciones concurrentes en la animación de posición; compilación autónoma de un solo archivo más pequeña

## WinIslands 1.1.6 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- **Ocultar automáticamente en la pantalla de bloqueo**: la isla se oculta automáticamente al pulsar Win+L o al desconectarse el escritorio remoto, y se restaura tras desbloquear (Configuración → General, activable)

- **No molestar programado ahora a nivel de minutos**: las horas de inicio/fin de No molestar se pueden fijar con precisión de minuto

- **Acción rápida de clic central**: hacer clic central en la isla ejecuta una acción rápida personalizada (configurable en Configuración → General)

- **Cambio programado de tema claro/oscuro**: fija las horas de inicio/fin del periodo oscuro y el tema cambia automáticamente (solo con Theme=Auto)

- **Campos de entrada en la isla**: los envíos de terceros pueden incluir un campo de entrada; cuando el usuario escribe en la isla, el valor se devuelve al emisor por WebSocket

- **Sondeo en segundo plano bajo demanda**: el sondeo se detiene cuando no hay ningún widget/función relacionado activado, reduciendo la CPU en reposo

- Corregido el envío de campos de entrada de la isla; corregida la pérdida de datos de los botones de envío (incluido el valor de entrada) en las devoluciones de llamada


## WinIslands 1.1.5 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

**Letras**

- **Letras AMLL palabra por palabra (karaoke real)**: integra la biblioteca TTML estilo Apple Music de amll.dev; Configuración → Letras, interruptor «Letras AMLL palabra por palabra» (activado por defecto); prioridad de fuentes: LRC local → AMLL TTML → Cider → letras en línea
- **Motor de resaltado de palabras reescrito**: avance continuo de 60 fps por reloj de pared + easing no lineal; respaldo por línea completa; letras bilingües emparejadas por línea de tiempo
- **Resaltado estable en pausa**: se congela en el momento de la pausa; sin saltos al salir y reiniciar; solo la línea activa impulsa la animación, CPU casi 0 en reposo

**Correcciones**

- Corregido el espaciado de letras en estado compacto: distancia fija entre las letras y los botones de la derecha; el ancho de la isla crece automáticamente si falta espacio (720→800)
- Estabilidad: tiempo de espera de AMLL de 5 s + degradación elegante; recalibración de la referencia de reloj al volver a verse los controles


## WinIslands 1.1.4 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- **Botones de acción rápida**: nueva fila de botones rápidos personalizables en la parte inferior de la tarjeta expandida (bloqueo de pantalla / silencio / reproducir-pausar / captura / mostrar escritorio / administrador de tareas / calculadora / suspender / volumen±); Configuración → Acciones rápidas permite marcarlos y ordenarlos con ↑↓, con efecto inmediato

- **Avisos de llamada entrante**: detecta ventanas de llamada de voz/vídeo de WeChat / QQ y muestra un aviso (distingue «llamada entrante» de «en llamada»); Configuración → Notificaciones lo activa y permite personalizar las apps detectadas; detección local, sin subir datos

- **Acción de comando en la isla**: los botones de envío de terceros admiten `action: "command"` para ejecutar una línea de comandos localmente (API solo de bucle local, Token configurable)

- **Tema de la tarjeta de la isla**: los envíos de terceros pueden incluir `theme: dark / light / auto`; las tarjetas cambian automáticamente entre estilos de vidrio claro/oscuro

- **Animaciones más rápidas**: la duración total de las cuatro pieles de movimiento se reduce ~20 %, manteniendo el easing de muelle iOS y 60 fps fluidos sin fotogramas perdidos

- **Optimización del uso en segundo plano**: los indicadores de teclado solo sondean cuando el widget está activado, el monitor de pantalla completa se reduce de frecuencia y la onda de audio se reduce en reposo

- **Correcciones de estabilidad**: corregido el aviso falso «Copiado» al iniciar con contenido ya en el portapapeles (línea base de inicio); corregido el escape de títulos .ics del calendario (\, \; \n) que se mostraban como barras invertidas

- **Rendimiento y estabilidad**: menos ruido en los registros de Bluetooth / SMTC / clima, retroceso exponencial para la limitación del clima; compilación correcta, las 104 pruebas unitarias y de integración pasan


## WinIslands 1.1.3 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- **Cambio entre varios reproductores**: con varios reproductores abiertos, la tarjeta expandida cambia la fuente de medios controlada con un clic (icono de nota + nombre de la fuente + flecha desplegable); ya no se limita al último reproductor iniciado

- **Portada inmersiva**: clic en la portada / imagen grande de la tarjeta expandida abre una vista previa de portada a pantalla completa; clic / Esc / clic derecho la cierra con fundido

- **Ajuste fino del tiempo de letras**: botones +0,5s / -0,5s en el área de letras de la tarjeta expandida; el desplazamiento por canción se recuerda para alinear las letras perfectamente

- **Letras de escritorio mejoradas**: la ventana de letras independiente admite ajuste de opacidad (predeterminada 0,85) e interruptor «Bloquear» (bloqueada = clic a través, no arrastrable)

- **Tema dinámico que respira**: con el fondo de color de portada activado, el fondo de la tarjeta expandida «respira» lentamente con el color de la portada (ciclo de ~18 s) en lugar de un bloque plano estático

- **Clic a la primera**: pulsar el ratón cambia inmediatamente expandir/contraer sin esperar a soltar, más ágil

- **Botones de acción en notificaciones**: los banners de notificación admiten botones de acción (el aviso de conexión Bluetooth ahora tiene botones «Desconectar» y «Configuración», que se ejecutan y ocultan al instante)

- **Devolución de llamada de botones de la isla**: al hacer clic en un botón de acción notify de un envío de terceros, el evento push_button (con push_id y texto del botón) se difunde al emisor por WebSocket para que lo gestione

- **Rendimiento y estabilidad**: el tema dinámico se suscribe a los fotogramas de composición bajo demanda (0 CPU en reposo), pinceles de degradado en caché para reducir la presión del GC; corregido el conflicto de mutex que impedía iniciar cuando las versiones nueva y antigua se ejecutan a la vez


## WinIslands 1.1.1 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- **Avisos de batería**: aviso de batería baja (umbral ajustable); aviso «Carga completada» al enchufar y cargar hasta el umbral fijado (predeterminado 100 %); detección local, activables

- **Avisos de red**: aviso al perder / restaurar la red (detección local de red, activable)

- **Nuevos widgets**: espacio libre del disco (unidad del sistema), estado del método de entrada (CN / EN + nombre del IME)

- **Calendario lunar y términos solares**: el widget de fecha puede mostrar además la fecha lunar y los términos solares (activado por defecto, desactivable en configuración)

- **Widgets de conmutación rápida**: activar/desactivar WiFi / Bluetooth / modo nocturno / silencio con un clic (API locales, sin red; estado de Radio en caché 2 s para evitar gastos)

- **Insignia de fuente de reproducción**: el widget de medios muestra la fuente de reproducción actual (Spotify / Cider / NetEase Cloud / QQ Music, etc.) para saber de qué reproductor proviene de un vistazo

- **Mejoras de letras**: interruptor mostrar/ocultar traducción de letras; botón «Copiar línea actual» de un clic

- **Iconos de widgets personalizados**: cada widget puede tener su propio icono (MDL2 o Emoji); glifo predeterminado si no se fija

- **Corregido el salto de escala de letras**: eliminada la vibración de «crecer y encoger» causada por el desplazamiento de rebote de las palabras del karaoke; el tamaño/opacidad de la línea actual en estado expandido transiciona suavemente en 300 ms para un desplazamiento más fluido


## WinIslands 1.1.0 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- **Volumen / silencio temporal en la isla**: al cambiar el volumen del sistema o el silencio, la isla muestra brevemente un indicador de volumen (duración ajustable, activable en configuración)

- **Copia / movimiento de archivos en la isla**: cuando el Explorador copia/mueve archivos, la isla muestra «Copiando archivos…» (detección local por título de ventana, activable)

- **Progreso de descarga en la isla**: detecta archivos temporales del navegador en el directorio de descargas (.crdownload / .part / .download, etc.) y muestra «Descargando N archivo(s)» (desactivado por defecto, activable en configuración)

- **Cápsula combinada «En uso»**: Configuración → Widgets permite activar (desactivado por defecto) la combinación de «Micrófono / Cámara / En reunión / Grabando» en una sola cápsula «En uso · …»; elige qué widgets participan; los combinados ya no se muestran por separado

- **Mejoras de Pomodoro**: clic en el widget Pomodoro de la isla pausa / reanuda el temporizador

- **Captura / grabación temporal en la isla**: al tomar una captura o iniciar una grabación, la isla muestra temporalmente el indicador correspondiente (funciona aunque la isla esté oculta)

- **Iluminación de palabras de karaoke con rebote**: cada línea se ilumina suavemente desde la primera palabra con un ligero rebote, más fluido y natural

- Arquitectura de sondeo interno optimizada: los eventos temporales de la isla como volumen / copia / descarga / captura pueden activarse mientras está oculta


## WinIslands 1.0.9 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- **Nuevos widgets**: uso de GPU, micrófono / cámara en uso, cuenta regresiva de festivos, en reunión; el widget de red puede mostrar una mini curva de 32 segundos

- **Acción rápida de doble clic**: Configuración → General permite fijar el doble clic en «Reproducir / Pausar», «Abrir configuración» o «Ninguna»

- **Asistente de silencio en reuniones**: reconoce ventanas de reunión (Teams / Zoom / Tencent Meeting / DingTalk / Feishu / Webex / Slack / Discord / Google Meet) y activa No molestar automáticamente durante reuniones (heurística puramente local)

- **Avisos de grabación / captura de pantalla**: aviso de captura por PrintScreen + detección de software de grabación (OBS / Bandicam / Xbox Game Bar, etc.)

- **Recordatorios de eventos de calendario (.ics)**: analiza archivos iCalendar locales; banner cuando un evento llega (con antelación opcional de N minutos); totalmente local

- **Avisos de suscripciones RSS**: sondea RSS 2.0 / Atom, banner para entradas nuevas

- **Avisos de correo (POP3)**: lee solo las cabeceras, banner para correo nuevo; se recomienda código de autorización

- **Lanzador rápido (estilo Spotlight)**: `Ctrl+Espacio` busca aplicaciones / abre URLs

- **Panel de historial del portapapeles**: `Ctrl+Alt+V` abre una ventana independiente; clic para copiar de vuelta

- **Reglas (automatización)**: condiciones (siempre / sin reproducción / reproduciendo / rango horario / app de medios específica) × acciones (ocultar / forzar contraer / forzar mostrar)

- **Island API v3**: imágenes (data URI / http), progreso animado (from/to/duration automático), renovación por heartbeat (heartbeat_seconds), actualizaciones parciales PATCH, canal WebSocket (/v3/ws)

- **Apariencia**: 18 temas preestablecidos, color de fondo personalizado, 4 pieles de movimiento, modo de bajo consumo

- La página de configuración se rediseñó al estilo de Configuración del sistema macOS (navegación izquierda + contenido derecho); todos los cambios se aplican de inmediato

- **Corregido el texto negro en modo oscuro**: enlace unificado del color de primer plano en todas las plantillas de controles personalizados de configuración (botones / casillas / entradas / desplegables / elementos desplegables / pestañas / navegación izquierda, etc.) más un escaneo de respaldo en tiempo de ejecución — el modo oscuro ya no muestra opciones individuales (idioma de la interfaz, desplegables de acciones de doble clic, etc.) con texto negro ilegible; el modo claro restaura automáticamente el texto oscuro

- **Corregido que la configuración no se abriera**: se eliminaron las líneas duplicadas de color de primer plano XAML que causaban el fallo de carga de BAML

- **Optimización del rendimiento de animación**: el resaltado de palabras karaoke reutiliza objetos Run (eliminando el diseño por fotograma), storyboards estables a 60 fps, refresco de registros por lotes — animaciones más fluidas

- **Eliminados los puntos rojos de insignia en los widgets** (por petición del usuario)


## WinIslands 1.0.8 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

**Reconstrucción completa de la interfaz de configuración**

- Diseño tipo Configuración del sistema macOS: lista de navegación izquierda + área de contenido derecha, 13 categorías (General / Apariencia / Widgets / Medios / Visualización de información de medios / Letras / Cider / Island API / Productividad / Actualización / Acerca de / Notificaciones / Reglas)
- Los colores del texto de configuración se adaptan automáticamente al modo oscuro/claro: blanco en oscuro, negro en claro; sin texto ilegible
- El texto de la navegación izquierda pasa a blanco de alto contraste, se elimina la línea blanca divisoria derecha y se refuerza la retroalimentación de hover y selección

**Reproducción de medios**

- Nuevo mini reproductor: ventana flotante independiente con portada / título / artista / barra de progreso y controles de reproducción; arrastrable con posición recordada; se muestra/oculta con la reproducción (activable en configuración)
- Nuevo cambio de dispositivo de salida de audio: Configuración → Medios puede enumerar y cambiar el dispositivo de reproducción predeterminado del sistema (se recomienda reiniciar el reproductor tras el cambio)
- Backend de fuentes de reproductores mejorado: enumera todas las sesiones SMTC y de Cider; puede cambiar la fuente de medios

**Letras**

- Nuevas letras bilingües: combina automáticamente las líneas de traducción adyacentes por marca de tiempo (desactivable en configuración)

**Apariencia y movimiento**

- Nuevas pieles de movimiento: 4 estilos de animación (muelle iOS (predeterminado) / muelle suave / rebote elástico / fundido simple), easing no lineal para expandir/contraer
- Nuevo modo de bajo consumo: reduce la tasa de renderizado de la onda y simplifica las animaciones en reposo para ahorrar energía

**Atajos globales**

- 5 combinaciones de teclas personalizables: mostrar/ocultar, reproducir/pausar, anterior, siguiente, expandir/contraer
- Admite Ctrl / Alt / Shift / Win + letras, dígitos, F1–F24, teclas de dirección

**Motor de reglas inteligente (Configuración → Reglas)**

- Controla automáticamente la visibilidad de la isla por condición: siempre / sin reproducción / reproduciendo / rango horario específico / app de medios específica reproduciendo
- Acciones: ocultar isla / forzar contraer / forzar mostrar; prioridad: ocultar > contraer > forzar mostrar

**Notificaciones**

- Historial de notificaciones: marcador de punto rojo no leído, marcar todo como leído, eliminar un elemento, clic para abrir la app de origen, borrar historial
- Nuevo plegado de notificaciones: notificaciones repetidas de la misma fuente y título reutilizan un banner y acumulan el recuento
- Nueva lista blanca de No molestar: las fuentes en la lista blanca (nombres de exe separados por comas) no se ven afectadas por No molestar y siguen mostrando banners normalmente
- Eliminada la insignia de punto rojo no leído de la isla

**Productividad**

- Aviso «Copiado» al copiar texto
- Detecta automáticamente códigos de verificación SMS y los resalta
- La copia de textos grandes muestra una animación de progreso (estimada por longitud, muestra el resultado al terminar)

**Widgets**

- Nuevo widget de cuenta regresiva de festivos: tabla de festivos 2026–2027 incorporada (Año Nuevo / Fiesta de Primavera / Qingming / Día del Trabajo / Festival del Barco Dragón / Festival del Medio Otoño / Día Nacional), muestra «XX en N días» o «Hoy XX»; activable en la configuración de widgets

**Island API v2**

- Nuevos campos: subtitle (subtítulo), type (info / success / warning / error), priority (high / normal / low), accent (color de acento personalizado), click (devolución de llamada de clic de toda la tarjeta)
- Cola de envíos: múltiples envíos ordenados por prioridad alta → baja, primero en entrar, primero en salir; reenviar el mismo id conserva la posición y la expiración originales
- La respuesta POST ahora incluye un campo position
- Nuevos scripts de ejemplo listos para ejecutar en docs/sdk-examples/ (push.bat / pull.bat / push.ps1 / push.py / pull.py)

**Correcciones**

- Corregido que la ventana de configuración no se abriera / que el doble clic no funcionara (protecciones de valores nulos e inicialización explícita en la navegación y el cambio de pestañas)
- Se añadieron 69 pruebas automatizadas (Island API, plegado de notificaciones y lista blanca, motor de reglas, reconocimiento de códigos de verificación, análisis de LRC), todas pasan

**Activos**

- Versiones portátiles de Windows x64 / arm64 (archivo único autocontenido, sin instalar, ejecución directa)
- Instalador universal de Windows (Inno Setup, soporta x64 y ARM64, instala según la arquitectura)
- > Las versiones portátiles son archivos exe independientes; ya no se ofrecen archivos ZIP.


## WinIslands 1.0.7 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- La «onda de sonido» se actualiza para **seguir el ritmo de la música**: captura en tiempo real del audio real del sistema mediante bucle de retorno WASAPI; ondas altas en ritmos fuertes, bajas en silencio — ya no es una barra de volumen fija

- **Renderizado continuo a 60 fps**: suavizado exponencial con ataque de 25 ms / liberación de 140 ms; el movimiento de la onda es continuo, no rígido ni entrecortado

- Nuevos ajustes (Configuración → Apariencia → Onda de sonido): interruptor seguir ritmo de música, sensibilidad 0,2–3,0, altura de onda 0,4–1,6; cambios con efecto inmediato

- Sin dispositivo de audio / servicio de audio anómalo, degrada suavemente a simulación de ritmo y reintenta restaurar la captura en vivo cada 8 segundos — sin congelaciones ni acumulación de hilos


## WinIslands 1.0.6 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- Se añadieron 6 widgets de isla: volumen, indicadores de teclado (CapsLock), portapapeles, tareas, Pomodoro, agenda — todos admiten casillas de dos columnas «sin canción / con canción» y reordenación por arrastre

- Nueva página de configuración «Productividad»: historial del portapapeles, temporizador Pomodoro, lista de tareas, recordatorios de agenda

- Nueva «onda de sonido»: mientras se reproducen medios, el área a la izquierda de los botones de control pulsa con el volumen del sistema en tiempo real (activable en Configuración → Apariencia)

- Se añadieron 7 temas preestablecidos: Predeterminado / Océano / Bosque / Atardecer / Neón / Monocromo / Uva

- Personalización de apariencia: fuente personalizada, escala de tamaño de fuente (0,8–1,4), radio de esquinas de la cápsula (16–40), fondo expandido teñido con la portada, insignia de notificaciones no leídas

- Nuevo en el menú de la bandeja: modo No molestar (manual / notificaciones silenciosas programadas), buscar actualizaciones, ver registros

- Página de configuración modernizada (esquinas redondeadas + vidrio líquido); los cambios se aplican de inmediato, sin guardar manualmente

- Nueva comprobación de actualizaciones (manual desde bandeja / configuración, comprobación automática opcional, desactivada por defecto)

- Activos: versiones portátiles de Windows x64 / arm64 (archivo único autocontenido, sin instalar, ejecución directa); instalador universal de Windows (Inno Setup, soporta x64 y ARM64, instala según la arquitectura)

- > Las versiones portátiles son archivos exe independientes; ya no se ofrecen archivos ZIP.


## WinIslands 1.0.5 (Estable)

Un widget Dynamic Island moderno y multifuncional para Windows.

### Registro de cambios

- Nueva «Island API»: otros programas pueden enviar información a la isla mediante una API HTTP local (como la integración de apps de terceros con la isla de iOS); **documentación para desarrolladores en docs/IslandAPI.md**

-   - `POST /v1/island/push` enviar/actualizar · `DELETE /v1/island/push/{id}` eliminar · `GET /v1/island/active` consultar · `GET /v1/health`

-   - Admite icono, título, cuerpo, progreso, botones (abrir enlace / iniciar programa), duración de visualización personalizada por elemento

-   - La página de configuración ofrece activar / puerto / Token opcional / duración predeterminada global

- Las tarjetas enviadas se muestran en una sola línea en estado compacto, no cubren otros widgets y **no afectan al ancho ni a la altura de la isla** (los tamaños automático/manual permanecen constantes)

- «Ajuste automático» del tamaño: se adapta al contenido; arrastrar manualmente un control deslizante desactiva automáticamente la opción automática correspondiente

- El contenido expandido admite desplazamiento con la rueda del ratón (barra oculta)

- Alineación superior/inferior de widgets unificada; correcciones de diseño/fuente al inicio (PerMonitorV2 obligatorio, tamaño correcto desde el arranque)

- Ya no se muestra el aviso «Reproduciendo ahora» al reproducir medios

- Corregido: tras expandir la isla (unos 1–2 s), la tarjeta que volvía al tamaño compacto causaba una pantalla negra total

-   - El contenido expandido ahora se funde en superposición con la fila compacta; no se ve el fondo durante las animaciones

-   - El tamaño final de la tarjeta se escribe explícitamente al terminar las animaciones de expandir/contraer; el estado expandido permanece estable y no se encoge

-   - También corregida la pantalla negra al hacer clic en botones de terceros en la isla en estado expandido

- Activos: versiones portátiles de Windows x64 / arm64 (archivo único autocontenido, sin instalar, ejecución directa); instalador universal de Windows (Inno Setup, soporta x64 y ARM64, instala según la arquitectura)

- > Las versiones portátiles son archivos exe independientes; ya no se ofrecen archivos ZIP.

---

## Français

## WinIslands 1.2.4（正式版 / Stable）

Une Dynamic Island moderne et polyvalente pour Windows. A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **🎶 Karaoké fluide affiné** : la progression mot à mot utilise désormais un lissage smoothstep avec transitions croisées entre les lettres — la surbrillance coule de gauche à droite comme un faisceau de lumière, avec accélération/décélération, sans à-coups.

- **🔋 Indicateur de batterie faible persistant** : lorsque la batterie descend sous le seuil sans charge, une petite pilule de batterie reste affichée en haut à droite de l'île (rouge ≤10 % / orange sinon), mise à jour en temps réel ; elle disparaît automatiquement une fois branché ou rechargé, et peut être activée dans les réglages.

- **🎧 Animation de connexion d'appareils** : la connexion/déconnexion Bluetooth affiche désormais une carte animée style iOS ; à la connexion, le niveau de batterie de l'appareil est lu automatiquement et affiché comme « Nom de l'appareil · Batterie xx % » (nom seul si la batterie n'est pas lisible), sans bloquer l'interface.


## WinIslands 1.2.1 (Stable)

## WinIslands 1.2.2 (Stable)

Une Dynamic Island moderne et polyvalente pour Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Nouveautés

- **Alertes compactes sur une seule ligne** : repliées, les notifications et rappels affichent une seule ligne (icône + titre + résumé sur une ligne) et sont tronquées si trop longues, sans élargir l'île ; les détails complets apparaissent au déploiement

- **Correction du texte tronqué/décalé à l'état replié** : avec une notification active, la hauteur compacte s'adapte au contenu et la carte s'aligne en haut — plus de rognage vertical

- **Élargissement automatique pour les longues notifications** : lorsqu'une longue notification arrive, l'île élargit automatiquement sa largeur afin que les contrôles média, l'horloge et tout le texte restent entièrement visibles, sans rognage au bord

Une Dynamic Island moderne et polyvalente pour Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Nouveautés

- **Amortissement élastique + transitions échelonnées** : le déploiement/repli utilise désormais un ressort amorti façon iOS (avec un léger rebond) et les sections de la carte apparaissent/disparaissent en cascade avec un rythme non linéaire — plus proche de la Dynamic Island d'iOS

- **Animation liée au volume** : lorsque le volume système change ou que vous faites glisser le curseur, l'île affiche brièvement une animation « icône + barre de volume + pourcentage » qui s'estompe d'elle-même après quelques secondes

- **Opacité intelligente en couches** : l'état compact est plus transparent et l'état déployé plus opaque, avec une transition douce ; évite la double superposition lorsque la teinte de la pochette est active

- **Transition de pochette** : au changement de chanson, la pochette se fond avec un léger zoom, complétant l'arrière-plan de teinte qui respire

- **Changement de thème fluide** : passer du thème clair au sombre interpole maintenant les couleurs du fond et de la bordure de la carte en ~0,3 s au lieu de clignoter

- **Optimisation de l'animation** : les effets par image (comme la vague) ne mettent à jour que le panneau visible, réduisant l'utilisation du CPU pendant la lecture pour une animation plus fluide
## WinIslands 1.2.0 (Stable)

Une Dynamic Island moderne et polyvalente pour Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Nouveautés

- **Changement de source des paroles en un clic** : basculez instantanément entre Auto / LRC local / AMLL TTML / API Cider / Paroles en ligne, et les paroles de la chanson en cours sont rechargées aussitôt
- **Récupération automatique après un plantage** : après une fermeture anormale, le prochain lancement affiche un avis de récupération — plus de blocages, d'écran noir ni d'état perdu
- **Durée d'animation réglable** : un nouveau curseur de durée d'animation déploiement/repli (300–1400ms) permet d'ajuster la vitesse à vos préférences
## WinIslands 1.1.9 (Stable)

Une Dynamic Island moderne et polyvalente pour Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Nouveautés

- **Animations plus fluides et cohérentes** : le déploiement/repli et le défilement automatique des paroles utilisent désormais une interpolation basée sur le temps (indépendante du nombre d'images), identique sur les écrans 60fps et haute fréquence (120Hz) — les actions rapides et successives ne saccadent plus

- **CPU encore réduit pendant la lecture** : l'interpolation de la progression est signalée à 5Hz tandis que le karaoké mot à mot avance en continu à 60fps via l'horloge interne du contrôle — même rendu, moins d'énergie en arrière-plan

- **Cache de décodage des pochettes** : chaque pochette est décodée une seule fois et réutilisée (les 24 plus récentes sont automatiquement évincées), éliminant les E/S disque répétées et les fluctuations de mémoire des mises à jour par seconde de la session multimédia

- **Améliorations du défilement des paroles** : le défilement ne s'exécute que lorsque la carte est déployée et visible, avec un positionnement précis à l'approche de la cible — pas de saccades lors des changements de ligne rapides

## WinIslands 1.1.8 (Stable)

Un widget moderne et multifonctionnel de Dynamic Island pour Windows.

### Nouveautés

- **Animations plus rapides et plus nettes** : l'expansion/réduction passe d'environ 1,1 s à environ 0,7 s ; le ressort est plus ferme avec un arrêt plus net — fini le mouvement lent et mollasson

- **Accélération globale du mouvement** : transitions de taille des composants, fondus décalés, repositionnement (~430 ms → 320 ms), notifications sur l'île et disparition en fondu sont ~30–35 % plus rapides — plus réactif

- **Tous les styles d'animation accélérés** : iOS Spring / Soft / Elastic / Fade partagent les mêmes durées de base réduites, conservant leur caractère avec une sensation plus vive


## WinIslands 1.1.7 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- **Station de transfert de fichiers** : déposez n'importe quel fichier directement sur l'île, il devient instantanément un élément de fichier déplaçable (référence de chemin uniquement, aucune copie). Faites-le glisser vers l'Explorateur ou toute autre application pour le déplacer à la volée

- **Nettoyage à la fermeture du lecteur** : quand une application multimédia se ferme et qu'aucune nouvelle session de lecture n'existe, l'île efface immédiatement les anciennes informations de la chanson

- **Animations plus fluides** : le repositionnement et le défilement automatique des paroles utilisent une courbe d'accélération plus douce (repositionnement ~430 ms) pour un mouvement plus continu

- **Version sur la page À propos** : Paramètres → À propos affiche désormais la version complète (ex. WinIslands 1.1.7)

- **Stabilité et performances** : détection améliorée de l'état de la session multimédia ; protection contre les vibrations concurrentes de l'animation de position ; build autonome en un seul fichier plus léger

## WinIslands 1.1.6 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- **Masquage automatique sur l'écran de verrouillage** : l'île se masque automatiquement à Win+L ou à la déconnexion du bureau à distance, et se restaure après déverrouillage (Paramètres → Général, activable)

- **Ne pas déranger programmé au niveau de la minute** : les heures de début/fin de Ne pas déranger se règlent à la minute près

- **Action rapide au clic central** : un clic central sur l'île exécute une action rapide personnalisée (configurable dans Paramètres → Général)

- **Bascule programmée du thème clair/sombre** : définissez les heures de début/fin de la période sombre, le thème bascule automatiquement (uniquement avec Theme=Auto)

- **Champs de saisie sur l'île** : les envois tiers peuvent inclure un champ de saisie ; lorsque l'utilisateur saisit sur l'île, la valeur est renvoyée à l'émetteur via WebSocket

- **Sondage en arrière-plan à la demande** : le sondage s'arrête quand aucun widget/fonction associé n'est activé, réduisant la CPU en veille

- Correction de l'envoi des champs de saisie de l'île ; correction de la perte des données des boutons d'envoi (y compris la valeur de saisie) dans les rappels


## WinIslands 1.1.5 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

**Paroles**

- **Paroles AMLL mot à mot (véritable karaoké)** : intègre la bibliothèque TTML style Apple Music d'amll.dev ; Paramètres → Paroles, interrupteur « Paroles AMLL mot à mot » (activé par défaut) ; priorité des sources : LRC local → AMLL TTML → Cider → paroles en ligne
- **Moteur de surbrillance mot à mot réécrit** : progression continue à 60 fps sur horloge murale + easing non linéaire ; repli sur toute la ligne ; paroles bilingues appariées par ligne de temps
- **Surbrillance stable en pause** : gelée au moment de la pause ; aucun saut après sortie et redémarrage ; seule la ligne active pilote l'animation, CPU quasi nul au repos

**Correctifs**

- Correctif de l'espacement des paroles à l'état compact : distance fixe entre les paroles et les boutons de droite ; la largeur de l'île s'agrandit automatiquement si l'espace manque (720→800)
- Stabilité : délai d'expiration AMLL de 5 s + repli élégant ; recalibrage de la référence d'horloge quand les contrôles redeviennent visibles


## WinIslands 1.1.4 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- **Boutons d'action rapide** : nouvelle rangée de boutons rapides personnalisables en bas de la carte dépliée (verrouillage / silencieux / lecture-pause / capture / bureau / gestionnaire des tâches / calculatrice / veille / volume±) ; Paramètres → Actions rapides permet de les cocher et de les trier avec ↑↓, effet immédiat

- **Alertes d'appel entrant** : détecte les fenêtres d'appel vocal/vidéo de WeChat / QQ et affiche une alerte (distingue « appel entrant » de « en appel ») ; Paramètres → Notifications pour activer et personnaliser les applications détectées ; détection locale uniquement, aucun envoi de données

- **Action de commande sur l'île** : les boutons d'envoi tiers prennent en charge `action: "command"` pour exécuter une ligne de commande localement (API boucle locale uniquement, Token configurable)

- **Thème de la carte de l'île** : les envois tiers peuvent inclure `theme: dark / light / auto` ; les cartes basculent automatiquement entre les styles de verre clair/sombre

- **Animations plus rapides** : durée totale des quatre peaux de mouvement réduite d'environ 20 %, en conservant l'easing ressort iOS et des 60 fps fluides sans image perdue

- **Optimisation de l'utilisation en arrière-plan** : les voyants du clavier ne sondent que lorsque le widget est activé, le moniteur plein écran est réduit en fréquence, l'onde audio est réduite au repos

- **Correctifs de stabilité** : corrigé le faux message « Copié » au démarrage avec un contenu déjà dans le presse-papiers (ligne de base de démarrage) ; corrigé l'échappement des titres .ics du calendrier (\, \; \n) affichés comme des barres obliques inverses

- **Performance et stabilité** : bruit de journal réduit pour les API Bluetooth / SMTC / météo, backoff exponentiel pour le rate limiting de la météo ; compilation réussie, les 104 tests unitaires et d'intégration passent


## WinIslands 1.1.3 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- **Bascule entre plusieurs lecteurs** : avec plusieurs lecteurs ouverts, la carte dépliée change de source multimédia contrôlée en un clic (icône de note + nom de la source + flèche déroulante) ; plus limité au dernier lecteur lancé

- **Pochette immersive** : un clic sur la pochette / grande image de la carte dépliée ouvre un aperçu plein écran ; clic / Échap / clic droit le referme en fondu

- **Réglage fin du minutage des paroles** : boutons +0,5 s / -0,5 s dans la zone des paroles de la carte dépliée ; le décalage par chanson est mémorisé pour aligner parfaitement les paroles

- **Paroles de bureau améliorées** : la fenêtre de paroles indépendante prend en charge le réglage de l'opacité (0,85 par défaut) et l'interrupteur « Verrouiller » (verrouillée = clic traversant, non déplaçable)

- **Thème dynamique « respirant »** : avec le fond de couleur de pochette activé, le fond de la carte dépliée « respire » lentement avec la couleur de la pochette (cycle d'environ 18 s) au lieu d'un bloc plat statique

- **Réponse au clic immédiate** : appuyer sur la souris bascule immédiatement déplier/replier sans attendre le relâchement, plus réactif

- **Boutons d'action sur les notifications** : les bannières de notification prennent en charge les boutons d'action (l'alerte de connexion Bluetooth a maintenant les boutons « Déconnecter » et « Paramètres », exécutés et repliés immédiatement)

- **Rappel des boutons de l'île** : lorsqu'un bouton d'action notify d'un envoi tiers est cliqué, l'événement push_button (avec push_id et le texte du bouton) est diffusé à l'émetteur via WebSocket pour qu'il le traite

- **Performance et stabilité** : le thème dynamique s'abonne aux images composites à la demande (0 CPU au repos), pinceaux dégradés mis en cache pour réduire la pression GC ; corrigé le conflit de mutex empêchant le démarrage quand des versions nouvelles et anciennes tournent en même temps


## WinIslands 1.1.1 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- **Alertes de batterie** : alerte de batterie faible (seuil réglable) ; alerte « Charge terminée » lorsqu'elle est branchée et chargée au seuil défini (100 % par défaut) ; détection locale, activables

- **Alertes réseau** : alerte à la perte / restauration du réseau (détection locale, activable)

- **Nouveaux widgets** : espace disque libre (lecteur système), état de la méthode de saisie (CN / EN + nom de l'IME)

- **Calendrier lunaire et termes solaires** : le widget de date peut afficher en plus la date lunaire et les termes solaires (activé par défaut, désactivable dans les paramètres)

- **Widgets de bascule rapide** : activation/désactivation WiFi / Bluetooth / mode nuit / silencieux en un clic (API locales, sans réseau ; état Radio mis en cache 2 s pour éviter les frais)

- **Badge de source de lecture** : le widget multimédia affiche la source de lecture actuelle (Spotify / Cider / NetEase Cloud / QQ Music, etc.), pour savoir d'où elle vient en un coup d'œil

- **Améliorations des paroles** : interrupteur afficher/masquer la traduction ; bouton « Copier la ligne actuelle » en un clic

- **Icônes de widgets personnalisées** : chaque widget peut avoir sa propre icône (MDL2 ou Emoji) ; glyphe par défaut sinon

- **Corrigé le saut d'échelle des paroles** : supprimé l'oscillation « grandir puis rétrécir » causée par le décalage de rebond des mots du karaoké ; la taille/opacité de la ligne actuelle à l'état déplié transite en douceur sur 300 ms pour un défilement plus fluide


## WinIslands 1.1.0 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- **Volume / silencieux temporaire sur l'île** : au changement de volume système ou de silencieux, l'île affiche brièvement un indicateur de volume (durée réglable, activable dans les paramètres)

- **Copie / déplacement de fichiers sur l'île** : quand l'Explorateur copie/déplace des fichiers, l'île affiche « Copie de fichiers… » (détection locale par titre de fenêtre, activable)

- **Progression de téléchargement sur l'île** : détecte les fichiers temporaires du navigateur dans le dossier de téléchargements (.crdownload / .part / .download, etc.) et affiche « Téléchargement de N fichier(s) » (désactivé par défaut, activable dans les paramètres)

- **Capsule fusionnée « En cours d'utilisation »** : Paramètres → Widgets permet d'activer (désactivé par défaut) la fusion de « Micro / Caméra / En réunion / Enregistrement » en une seule capsule « En cours · … » ; choisissez les widgets participants ; les éléments fusionnés ne sont plus affichés séparément

- **Améliorations Pomodoro** : un clic sur le widget Pomodoro de l'île met en pause / reprend le minuteur

- **Capture / enregistrement temporaire sur l'île** : lors d'une capture ou du début d'un enregistrement, l'île affiche temporairement l'indicateur correspondant (fonctionne même si l'île est masquée)

- **Allumage des mots du karaoké avec rebond** : chaque ligne s'allume en douceur depuis le premier mot avec un léger rebond, plus fluide et naturel

- Architecture de sondage interne optimisée : les événements temporaires de l'île (volume / copie / téléchargement / capture) peuvent être déclenchés pendant qu'elle est masquée


## WinIslands 1.0.9 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- **Nouveaux widgets** : utilisation GPU, micro / caméra en cours d'utilisation, compte à rebours des jours fériés, en réunion ; le widget réseau peut afficher une mini courbe de 32 secondes

- **Action rapide au double-clic** : Paramètres → Général permet de définir le double-clic sur « Lecture / Pause », « Ouvrir les paramètres » ou « Aucune »

- **Assistant de mise en sourdine en réunion** : reconnaît les fenêtres de réunion (Teams / Zoom / Tencent Meeting / DingTalk / Feishu / Webex / Slack / Discord / Google Meet) et active Ne pas déranger pendant les réunions (heuristique purement locale)

- **Alertes d'enregistrement / capture d'écran** : alerte de capture PrintScreen + détection des logiciels d'enregistrement (OBS / Bandicam / Xbox Game Bar, etc.)

- **Rappels d'événements du calendrier (.ics)** : analyse les fichiers iCalendar locaux ; bannière quand un événement arrive (avec délai optionnel de N minutes) ; entièrement local

- **Alertes d'abonnements RSS** : sonde RSS 2.0 / Atom, bannière pour les nouvelles entrées

- **Alertes e-mail (POP3)** : lit uniquement les en-têtes, bannière pour les nouveaux e-mails ; un code d'autorisation est recommandé

- **Lanceur rapide (style Spotlight)** : `Ctrl+Espace` recherche des applications / ouvre des URL

- **Panneau d'historique du presse-papiers** : `Ctrl+Alt+V` ouvre une fenêtre indépendante ; clic pour recopier dans le presse-papiers

- **Règles (automatisation)** : conditions (toujours / pas de lecture / en lecture / plage horaire / application multimédia spécifique) × actions (masquer / forcer le repli / forcer l'affichage)

- **Island API v3** : images (data URI / http), progression animée (from/to/duration automatique), renouvellement par heartbeat (heartbeat_seconds), mises à jour partielles PATCH, canal WebSocket (/v3/ws)

- **Apparence** : 18 thèmes prédéfinis, couleur de fond personnalisée, 4 peaux de mouvement, mode basse consommation

- La page de paramètres est repensée à la macOS System Settings (navigation à gauche + contenu à droite) ; tous les changements prennent effet immédiatement

- **Texte noir en mode sombre corrigé** : liaison unifiée de la couleur de premier plan dans tous les modèles de contrôles personnalisés des paramètres (boutons / cases / champs / listes déroulantes / éléments de listes / onglets / navigation gauche, etc.) plus un balayage de secours à l'exécution — le mode sombre n'affiche plus certaines options (langue de l'interface, listes déroulantes des actions double-clic, etc.) en texte noir illisible ; le mode clair restaure automatiquement le texte sombre

- **Ouverture des paramètres corrigée** : suppression des lignes de couleur de premier plan XAML dupliquées à l'origine de l'échec de chargement BAML

- **Optimisation des performances d'animation** : la surbrillance mot à mot du karaoké réutilise les objets Run (éliminant la mise en page image par image), storyboards stables à 60 fps, rafraîchissement des journaux par lots — animations plus fluides

- **Points d'insigne rouges supprimés des widgets** (à la demande de l'utilisateur)


## WinIslands 1.0.8 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

**Refonte complète de l'interface des paramètres**

- Disposition type Configuration système macOS : liste de navigation à gauche + zone de contenu à droite, 13 catégories (Général / Apparence / Widgets / Média / Affichage des informations média / Paroles / Cider / Island API / Productivité / Mise à jour / À propos / Notifications / Règles)
- Les couleurs du texte des paramètres s'adaptent automatiquement au mode sombre/clair : blanc en sombre, noir en clair ; plus de texte illisible
- Le texte de la navigation gauche passe en blanc à contraste élevé, la ligne blanche de séparation droite est supprimée, retour visuel au survol et à la sélection renforcé

**Lecture multimédia**

- Nouveau mini-lecteur : fenêtre flottante indépendante avec pochette / titre / artiste / barre de progression et contrôles de lecture ; déplaçable avec position mémorisée ; affichage/masquage automatique avec la lecture (activable dans les paramètres)
- Nouveau changement de périphérique de sortie audio : Paramètres → Média peut énumérer et changer le périphérique de lecture par défaut du système (redémarrage du lecteur recommandé après le changement)
- Backend des sources de lecteurs amélioré : énumère toutes les sessions SMTC et Cider ; peut changer la source multimédia

**Paroles**

- Nouvelles paroles bilingues : fusionne automatiquement les lignes de traduction adjacentes par horodatage (désactivable dans les paramètres)

**Apparence et animation**

- Nouvelles peaux de mouvement : 4 styles d'animation (ressort iOS (défaut) / ressort doux / rebond élastique / fondu simple), easing non linéaire pour déplier/replier
- Nouveau mode basse consommation : réduit le taux de rendu de l'onde et simplifie les animations au repos pour économiser l'énergie

**Raccourcis globaux**

- 5 combinaisons de touches personnalisables : afficher/masquer, lecture/pause, précédent, suivant, déplier/replier
- Prend en charge Ctrl / Alt / Maj / Win + lettres, chiffres, F1–F24, touches fléchées

**Moteur de règles intelligent (Paramètres → Règles)**

- Contrôle automatiquement la visibilité de l'île par condition : toujours / sans lecture / en lecture / plage horaire spécifique / application multimédia spécifique en lecture
- Actions : masquer l'île / forcer le repli / forcer l'affichage ; priorité : masquer > replier > forcer l'affichage

**Notifications**

- Historique des notifications : point rouge non lu, tout marquer comme lu, supprimer un élément, clic pour ouvrir l'application source, vider l'historique
- Nouveau regroupement des notifications : les notifications répétées de même source et titre réutilisent une bannière et cumulent le compteur
- Nouvelle liste blanche Ne pas déranger : les sources de la liste blanche (noms d'exe séparés par des virgules) ne sont pas affectées par Ne pas déranger et affichent normalement les bannières
- Badge de point rouge non lu supprimé de l'île

**Productivité**

- Message « Copié » lors de la copie de texte
- Détecte automatiquement les codes de vérification SMS et les met en évidence
- La copie de textes volumineux affiche une animation de progression (estimée par la longueur, affiche le résultat à la fin)

**Widgets**

- Nouveau widget de compte à rebours des jours fériés : table des jours fériés 2026–2027 intégrée (Nouvel An / Fête du Printemps / Qingming / Fête du Travail / Dragon Boat / Mi-Automne / Fête nationale), affichant « XX dans N jours » ou « Aujourd'hui XX » ; activable dans les paramètres des widgets

**Island API v2**

- Nouveaux champs : subtitle (sous-titre), type (info / success / warning / error), priority (high / normal / low), accent (couleur d'accent personnalisée), click (rappel au clic sur toute la carte)
- File d'envois : plusieurs envois triés par priorité haute → basse, premier entré premier sorti ; le renvoi du même id conserve la position et l'expiration d'origine
- La réponse POST inclut désormais un champ position
- Nouveaux scripts d'exemple prêts à l'exécution dans docs/sdk-examples/ (push.bat / pull.bat / push.ps1 / push.py / pull.py)

**Correctifs**

- Corrigé l'impossibilité d'ouvrir la fenêtre des paramètres / le non-fonctionnement du double-clic (protections de valeurs nulles et initialisation explicite de la navigation et du changement d'onglets)
- Ajout de 69 tests automatisés (Island API, regroupement des notifications et liste blanche, moteur de règles, reconnaissance des codes de vérification, analyse LRC), tous réussis

**Ressources**

- Versions portables Windows x64 / arm64 (fichier unique autonome, sans installation, exécution directe)
- Installeur universel Windows (Inno Setup, prend en charge x64 et ARM64, installe selon l'architecture)
- > Les versions portables sont des fichiers exe autonomes ; les archives ZIP ne sont plus fournies.


## WinIslands 1.0.7 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- L'« onde sonore » passe en **suivi du rythme musical** : capture en temps réel de l'audio système réel via la boucle WASAPI ; ondes hautes sur les temps forts, basses dans le silence — plus une barre de volume fixe

- **Rendu continu à 60 fps** : lissage exponentiel avec attaque de 25 ms / relâchement de 140 ms ; le mouvement de l'onde est continu, ni rigide ni saccadé

- Nouveaux réglages (Paramètres → Apparence → Onde sonore) : interrupteur suivre le rythme musical, sensibilité 0,2–3,0, hauteur d'onde 0,4–1,6 ; changements immédiats

- Sans périphérique audio / service audio anormal, repli élégant sur la simulation de rythme avec nouvelle tentative de restauration de la capture en direct toutes les 8 secondes — pas de gel ni d'accumulation de fils


## WinIslands 1.0.6 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- Ajout de 6 widgets d'île : volume, voyants du clavier (CapsLock), presse-papiers, tâches, Pomodoro, agenda — tous avec cases à deux colonnes « sans chanson / avec chanson » et réorganisation par glisser-déposer

- Nouvelle page de paramètres « Productivité » : historique du presse-papiers, minuteur Pomodoro, liste de tâches, rappels d'agenda

- Nouvelle « onde sonore » : pendant la lecture, la zone à gauche des boutons de contrôle pulse avec le volume système en temps réel (activable dans Paramètres → Apparence)

- Ajout de 7 thèmes prédéfinis : Défaut / Océan / Forêt / Coucher de soleil / Néon / Monochrome / Raisin

- Personnalisation de l'apparence : police personnalisée, échelle de taille de police (0,8–1,4), rayon des coins de la capsule (16–40), fond déplié teinté de la pochette, badge de notifications non lues

- Nouveautés du menu de la barre d'état : mode Ne pas déranger (manuel / notifications silencieuses programmées), rechercher les mises à jour, voir les journaux

- Page de paramètres modernisée (coins arrondis + verre liquide) ; les changements prennent effet immédiatement, sans enregistrement manuel

- Nouvelle vérification des mises à jour (manuelle depuis la barre d'état / les paramètres, vérification automatique facultative, désactivée par défaut)

- Ressources : versions portables Windows x64 / arm64 (fichier unique autonome, sans installation, exécution directe) ; installeur universel Windows (Inno Setup, prend en charge x64 et ARM64, installe selon l'architecture)

- > Les versions portables sont des fichiers exe autonomes ; les archives ZIP ne sont plus fournies.


## WinIslands 1.0.5 (Stable)

Un widget Dynamic Island moderne et multifonctionnel pour Windows.

### Journal des modifications

- Nouvelle « Island API » : d'autres logiciels peuvent envoyer des informations à l'île via une API HTTP locale (comme l'intégration des apps tierces avec l'île iOS) ; **documentation développeur dans docs/IslandAPI.md**

-   - `POST /v1/island/push` envoyer/mettre à jour · `DELETE /v1/island/push/{id}` supprimer · `GET /v1/island/active` interroger · `GET /v1/health`

-   - Prend en charge icône, titre, corps, progression, boutons (ouvrir un lien / lancer un programme), durée d'affichage personnalisée par élément

-   - La page de paramètres propose activer / port / Token facultatif / durée par défaut globale

- Les cartes envoyées s'affichent sur une seule ligne à l'état compact, ne couvrent pas les autres widgets et **n'affectent pas la largeur ni la hauteur de l'île** (tailles auto/manuelle constantes)

- « Ajustement automatique » de la taille : s'adapte au contenu ; glisser manuellement un curseur désactive automatiquement l'option auto correspondante

- Le contenu déplié prend en charge le défilement à la molette (barre masquée)

- Alignement haut/bas des widgets unifié ; corrections de disposition/police au démarrage (PerMonitorV2 imposé, taille correcte dès le lancement)

- Plus de message « Lecture en cours » lors de la lecture de médias

- Corrigé : après l'expansion de l'île (environ 1–2 s), la carte revenant à la taille compacte provoquait un écran noir complet

-   - Le contenu déplié est désormais en fondu croisé superposé à la ligne compacte ; aucun fond ne transparaît pendant les animations

-   - La taille finale de la carte est explicitement réécrite après les animations d'expansion/repli ; l'état déplié reste stable et ne rétrécit pas

-   - Également corrigé l'écran noir au clic sur les boutons tiers de l'île à l'état déplié

- Ressources : versions portables Windows x64 / arm64 (fichier unique autonome, sans installation, exécution directe) ; installeur universel Windows (Inno Setup, prend en charge x64 et ARM64, installe selon l'architecture)

- > Les versions portables sont des fichiers exe autonomes ; les archives ZIP ne sont plus fournies.

---

## العربية

## WinIslands 1.2.4（正式版 / Stable）

جزيرة ديناميكية حديثة ومتعددة الوظائف لنظام ويندوز. A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **🎶 تحسين انسياب كلمات الكاريوكي**: أصبح تقدّم الحروف يستخدم تدرّجًا سلسًا (smoothstep) مع انتقالات متداخلة بين الأحرف — يشعّ التمييز من اليسار إلى اليمين كشريط ضوئي متواصل، مع تسارع/تباطؤ في البداية والنهاية، دون توقّفات مفاجئة.

- **🔋 مؤشر انخفاض البطارية الدائم**: عندما تنخفض البطارية دون الحدّ المحدد دون شحن، تبقى حبّة بطارية صغيرة في الزاوية العلوية اليمنى من الجزيرة (حمراء ≤10% / برتقالية في غير ذلك)، وتتحدّث لحظيًا؛ تختفي تلقائيًا عند التوصيل بالشاحن أو ارتفاع الشحن، ويمكن إيقافها من الإعدادات.

- **🎧 حركة اتصال الأجهزة**: عند اتصال/انقطاع جهاز بلوتوث تظهر بطاقة متحركة بأسلوب iOS؛ وعند الاتصال تُقرأ بطارية الجهاز تلقائيًا ويُعرض «اسم الجهاز · البطارية xx%»، وعند تعذّر القراءة يُعرض الاسم فقط، دون تعطيل الواجهة.


## WinIslands 1.2.1 (مستقر)

## WinIslands 1.2.2 (مستقر)

جزيرة ديناميكية حديثة ومتعددة الوظائف لنظام ويندوز. A modern, multi-functional Dynamic Island widget for Windows.

### ما الجديد

- **تنبيهات مضغوطة بسطر واحد**: في الحالة المطوية، تعرض الإشعارات والتذكيرات سطرًا واحدًا (أيقونة + عنوان + ملخص سطر واحد) وتُقتطع تلقائيًا عند الطول، فلا توسّع الجزيرة؛ تظهر التفاصيل الكاملة عند التوسيع

- **إصلاح نص الإشعار المقطوع/المنزاح في الحالة المطوية**: مع وجود إشعار نشط، ينمو الارتفاع المضغوط مع المحتوى وتُحاذى البطاقة للأعلى — بلا قصّ رأسي

- **توسيع الجزيرة تلقائيًا للإشعارات الطويلة**: عند وصول إشعار طويل، توسّع الجزيرة عرضها تلقائيًا لتبقى عناصر التحكم بالوسائط والساعة وكل النص داخل الجزيرة كاملًا بدل قطعها عند الحافة

جزيرة ديناميكية حديثة ومتعددة الوظائف لنظام ويندوز. A modern, multi-functional Dynamic Island widget for Windows.

### ما الجديد

- **تخميد نابض + انتقالات متتالية**: الفتح/الطي يستخدم الآن زنبركاً مخمداً بأسلوب iOS (مع ارتداد خفيف) وتظهر/تختفي أقسام البطاقة تباعاً بإيقاع غير خطي — أقرب إلى الجزيرة الديناميكية في iOS

- **حركة مرتبطة بمستوى الصوت**: عندما يتغيّر مستوى صوت النظام أو تسحب شريط التمرير، تعرض الجزيرة لفترة وجيزة حركة «أيقونة + شريط صوت + نسبة مئوية» تتلاشى تلقائياً بعد ثوانٍ

- **طبقات شفافية ذكية**: الحالة المضغوطة أكثر شفافية والحالة الموسعة أكثر صلابة، مع انتقال سلس؛ وتتجنب الطبقات المزدوجة عند تفعيل صبغة الغلاف

- **انتقال الغلاف**: عند تغيير الأغنية، يتلاشى غلاف الألبوم مع تكبير خفيف، بما يكمل خلفية الصبغة المتنفسة

- **تبديل ثيم سلس**: عند التبديل بين الثيم الفاتح والداكن، تتحول ألوان خلفية البطاقة وحدودها خلال ~0.3 ثانية بدلاً من الوميض

- **تحسين أداء الحركة**: تأثيرات الإطارات (كموجة الصوت) تحدّث اللوحة الظاهرة فقط، ما يقلل استهلاك المعالج أثناء التشغيل لحركة أكثر سلاسة
## WinIslands 1.2.0 (مستقر)

جزيرة ديناميكية حديثة ومتعددة الوظائف لنظام ويندوز. A modern, multi-functional Dynamic Island widget for Windows.

### ما الجديد

- **التبديل السريع لمصدر الكلمات بنقرة واحدة**: بدّل فوراً بين تلقائي / LRC محلي / AMLL TTML / واجهة Cider / الكلمات عبر الإنترنت، مع إعادة تحميل كلمات الأغنية الحالية مباشرة
- **الاسترداد التلقائي عند الأعطال**: بعد خروج غير طبيعي، يعرض التشغيل التالي إشعار استرداد — لا مزيد من التجمّد أو الشاشة السوداء أو فقدان الحالة
- **مدة حركة قابلة للضبط**: شريط تمرير جديد لمدة حركة الفتح/الطي (300–1400ms) يسمح بضبط السرعة حسب الذوق
## WinIslands 1.1.9 (مستقر)

جزيرة ديناميكية حديثة ومتعددة الوظائف لنظام ويندوز. A modern, multi-functional Dynamic Island widget for Windows.

### ما الجديد

- **حركات أكثر سلاسة واتساقاً**: فتح/طي الجزيرة والتمرير التلقائي للكلمات يستخدمان الآن استيفاءً زمنياً (مستقلاً عن معدل الإطارات)، بنفس النتيجة على شاشات 60fps وعالية التحديث (120Hz) — لا مزيد من التوقف المتقطع عند تنفيذ إجراءات متتابعة سريعة

- **خفض إضافي لاستهلاك المعالج أثناء التشغيل**: يتم إبلاغ استيفاء التقدم بمعدل 5Hz بينما يتقدم الكاريوكي حرفاً بحرف بشكل مستمر بمعدل 60fps عبر الساعة الداخلية للعنصر — نفس المظهر مع استهلاك أقل في الخلفية

- **تخزين مؤقت لفك أغلفة الألبومات**: يُفكّ كل غلاف مرة واحدة ويُعاد استخدامه (يتم إخراج أحدث 24 تلقائياً)، مما يلغي عمليات القراءة المتكررة من القرص وتقلب الذاكرة الناتجة عن تحديثات جلسة الوسائط كل ثانية

- **تحسينات تمرير الكلمات**: يتم التمرير فقط عند فتح الجزيرة وظهورها، مع تثبيت دقيق عند الاقتراب من الهدف — بلا اهتزاز عند تغيير الأسطر السريع

## WinIslands 1.1.8 (مستقر)

أداة Dynamic Island عصرية ومتعددة الوظائف لنظام Windows.

### المستجدات

- **حركات أسرع وأكثر حسمًا**: توسيع/طي الأداة يقلّ من ~1.1 ثانية إلى ~0.7 ثانية؛ الربيع أصبح أكثر صلابة واستقرارًا أكثر وضوحًا — بلا حركة بطيئة أو مترهلة

- **تسريع شامل للحركة**: انتقالات حجم المكوّنات، وتلاشي المحتوى المتدرج، وإعادة تحديد الموضع (~430 مللي ثانية ← 320 مللي ثانية)، ودفع المحتوى إلى الجزيرة، والإخفاء بالتلاشي — كلها أسرع بنحو 30–35%

- **تسريع جميع أنماط الحركة**: أنماط iOS Spring / Soft / Elastic / Fade تشترك في مدد أساسية أقصر، مع الحفاظ على طابعها لكن بإحساس أسرع


## WinIslands 1.1.7 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- **محطة نقل الملفات**: اسحب أي ملف مباشرة إلى الجزيرة فيتحول فورًا إلى عنصر ملف قابل للسحب (إشارة إلى المسار فقط، دون نسخ الملف)؛ ثم اسحبه إلى مستكشف الملفات أو أي تطبيق آخر لنقله بسهولة

- **تنظيف تلقائي عند خروج التطبيق**: عند إغلاق تطبيق الوسائط وعدم وجود جلسة تشغيل جديدة، تمسح الجزيرة فورًا معلومات الأغنية القديمة

- **رسوم متحركة أكثر سلاسة**: إعادة التموضع والتمرير التلقائي للكلمات يستخدمان تليينًا أنعم (إعادة التموضع ~430 مللي ثانية) لحركة أكثر استمرارية

- **إظهار الإصدار في صفحة حول**: الإعدادات ← حول يعرض الآن الإصدار الكامل (مثل WinIslands 1.1.7)

- **الاستقرار والأداء**: تحسين اكتشاف حالة جلسة الوسائط (فشل قراءة حالة التشغيل يُعتبر نهاية الوسائط)؛ حماية من الاهتزاز المتزامن في حركة الموقع؛ ملف مستقل أصغر

## WinIslands 1.1.6 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- **إخفاء تلقائي عند قفل الشاشة**: يتم إخفاء الجزيرة تلقائيًا عند الضغط على Win+L أو انقطاع الاتصال بسطح المكتب البعيد، وتعود تلقائيًا بعد فتح القفل (الإعدادات ← عام، قابل للتبديل)

- **ترقية جدولة عدم الإزعاج إلى مستوى الدقائق**: يمكن ضبط وقت بدء/انتهاء عدم الإزعاج بدقة حتى الدقيقة

- **إجراء سريع بزر الفأرة الأوسط**: النقر بزر الفأرة الأوسط على الجزيرة ينفّذ إجراءً سريعًا مخصصًا (قابل للتكوين في الإعدادات ← عام)

- **تبديل مجدول بين السمة الفاتحة والداكنة**: اضبط ساعات بداية ونهاية الفترة الداكنة، وتتحول السمة تلقائيًا في الموعد (يسري فقط مع السمة التلقائية)

- **حقل إدخال على الجزيرة**: يمكن أن تحمل إرسالات الطرف الثالث حقل إدخال؛ عندما يكتب المستخدم على الجزيرة، تُعاد القيمة إلى المُرسِل عبر WebSocket

- **الاستقصاء الخلفي عند الطلب**: يتوقف الاستقصاء عند عدم تفعيل أي مكوّن أو ميزة ذات صلة، مما يقلل استخدام CPU في وضع الخمول

- إصلاح مشكلة إرسال حقل الإدخال على الجزيرة؛ وإصلاح فقدان بيانات أزرار الإرسال (بما في ذلك قيمة الإدخال) في الاستدعاءات


## WinIslands 1.1.5 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

**الكلمات**

- **كلمات AMLL حرفًا بحرف (كاراوكي حقيقي)**: يدمج مكتبة TTML بأسلوب Apple Music من amll.dev؛ الإعدادات ← الكلمات، مفتاح «كلمات AMLL حرفًا بحرف» (مفعل افتراضيًا)؛ أولوية المصادر: LRC محلي ← AMLL TTML ← Cider ← كلمات عبر الإنترنت
- **إعادة كتابة محرك الإضاءة حرفًا بحرف**: تقدم مستمر 60fps بوقت الحائط + تسهيل غير خطي؛ احتياطي بتوزيع كامل السطر؛ الكلمات ثنائية اللغة تُطابق حسب الخط الزمني
- **ثبات الإضاءة عند الإيقاف المؤقت**: تُجمَّد عند لحظة الإيقاف، ولا تقفز بعد الخروج وإعادة التشغيل؛ السطر النشط فقط يحرّك الرسم، CPU قريب من الصفر في وضع الخمول

**الإصلاحات**

- إصلاح تباعد الكلمات في الوضع المضغوط: مسافة ثابتة بين الكلمات والأزرار اليمنى؛ يزداد عرض الجزيرة تلقائيًا عند عدم كفاية المساحة (720←800)
- الاستقرار: مهلة AMLL 5 ثوانٍ + تراجع أنيق؛ إعادة معايرة أساس وقت الحائط عند عودة عناصر التحكم للظهور


## WinIslands 1.1.4 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- **أزرار إجراءات سريعة**: صف جديد من الأزرار السريعة القابلة للتخصيص أسفل بطاقة الجزيرة الموسعة (قفل الشاشة / كتم الصوت / تشغيل-إيقاف / لقطة شاشة / إظهار سطح المكتب / إدارة المهام / الآلة الحاسبة / الإسبات / الصوت±)؛ الإعدادات ← إجراءات سريعة للاختيار والترتيب بـ ↑↓، وتُطبَّق التغييرات فورًا

- **تنبيهات المكالمات الواردة**: يكتشف نوافذ المكالمات الصوتية/المرئية من WeChat / QQ ويعرض تنبيهًا (يميز «مكالمة واردة» عن «في مكالمة»)؛ الإعدادات ← الإشعارات للتبديل وتخصيص التطبيقات المكتشفة؛ كشف محلي فقط، دون رفع أي بيانات

- **إجراء أوامر على الجزيرة**: أزرار إرسال الطرف الثالث تدعم `action: "command"` لتنفيذ سطر أوامر محليًا (واجهة برمجية محلية فقط، مع إمكانية ضبط Token)

- **سمة بطاقة الجزيرة**: يمكن أن تحمل إرسالات الطرف الثالث `theme: dark / light / auto`؛ تتحول بطاقات الإرسال تلقائيًا بين أنماط الزجاج الفاتح والداكن

- **تسريع الحركات**: تقصير المدة الإجمالية لأنماط الحركة الأربعة بنحو 20%، مع الحفاظ على مرونة زنبرك iOS و60fps سلسة دون فقدان إطارات

- **تحسين الاستخدام الخلفي**: مؤشرات لوحة المفاتيح تُستقصى فقط عند تفعيل المكوّن، وخفض تردد مراقبة ملء الشاشة، وخفض تردد موجة الصوت في وضع الخمول

- **إصلاحات الاستقرار**: إصلاح ظهور «تم النسخ» الخاطئ عند بدء التشغيل مع محتوى موجود في الحافظة (خط الأساس عند البدء)؛ إصلاح مشكلة ظهور أحرف ترميز .ics في التقويم (\, \; \n) كخطوط مائلة عكسية

- **الأداء والاستقرار**: تقليل ضجيج سجلات Bluetooth / SMTC / الطقس، وتراجع أسي لتقييد الطقس؛ البناء ناجح، وجميع اختبارات الوحدة والتكامل الـ 104 نجحت


## WinIslands 1.1.3 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- **التبديل بين مشغلات متعددة**: عند فتح عدة مشغلات، يمكن للبطاقة الموسعة تبديل مصدر الوسائط المتحكم به بنقرة واحدة (أيقونة نغمة + اسم المصدر + سهم قائمة منسدلة)؛ لم يعد مقصورًا على آخر مشغل بدأ

- **انغماس الغلاف**: انقر على غلاف الألبوم / الصورة الكبيرة في البطاقة الموسعة لفتح معاينة غلاف بملء الشاشة؛ أغلقها بالنقر / Esc / الزر الأيمن مع تلاشٍ

- **ضبط دقيق لوقت الكلمات**: زرا +0.5s / -0.5s في منطقة الكلمات بالبطاقة الموسعة؛ يُحفظ إزاحة الوقت لكل أغنية لمحاذاة الكلمات مع الموسيقى تمامًا

- **تحسين كلمات سطح المكتب**: نافذة الكلمات المستقلة تدعم ضبط الشفافية (الافتراضي 0.85) ومفتاح «قفل» (عند القفل: يمرّ الماوس ولا يمكن السحب)

- **تنفّس السمة الديناميكية**: عند تفعيل خلفية أخذ اللون من الغلاف، يتنفس لون خلفية البطاقة الموسعة ببطء مع لون الغلاف (دورة نحو 18 ثانية) بدلًا من كتلة مسطحة ثابتة

- **النقر أولًا**: بمجرد الضغط على الماوس يتبدل التوسيع/الطي فورًا دون انتظار رفع الزر، استجابة أسرع

- **أزرار إجراءات الإشعارات**: لافتات الإشعارات تدعم أزرار إجراءات (تنبيه اتصال Bluetooth يحمل الآن زرّي «فصل» و«إعدادات»، اضغط للتنفيذ الفوري والطي)

- **استدعاء أزرار الجزيرة**: عند النقر على زر إجراء notify في إرسال طرف ثالث، يُبث حدث push_button إلى المُرسِل عبر WebSocket (يتضمن push_id ونص الزر)، ويعالج المُرسِل الاستدعاء بنفسه

- **الأداء والاستقرار**: تحول السمة الديناميكية إلى الاشتراك في إطارات التركيب عند الطلب (0 CPU في الخمول) وإعادة استخدام مخازن الفرشاة المتدرجة لتقليل ضغط GC؛ إصلاح تعارض كائن المزامنة بين النسخة الجديدة والقديمة الذي يمنع بدء التشغيل


## WinIslands 1.1.1 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- **تنبيهات البطارية**: تنبيه انخفاض البطارية (عتبة قابلة للضبط)، وعند توصيل الطاقة والوصول إلى العتبة المحددة (الافتراضي 100%) يظهر تنبيه «اكتمل الشحن»؛ كل ذلك كشف محلي وقابل للتبديل

- **تنبيهات الشبكة**: يظهر تنبيه عند انقطاع الشبكة / استعادتها (كشف محلي لحالة الشبكة، قابل للتبديل)

- **مكوّنات جديدة**: المساحة المتبقية على القرص (قرص النظام)، وحالة طريقة الإدخال (صينية / إنجليزية + اسم طريقة الإدخال)

- **التقويم القمري والمواسم**: يمكن لمكوّن التاريخ عرض التاريخ القمري والمواسم الشمسية إضافيًا (مفعل افتراضيًا، ويمكن إيقافه في الإعدادات)

- **مكوّن المفاتيح السريعة**: تبديل WiFi / Bluetooth / الوضع الليلي / كتم الصوت بنقرة واحدة (عبر واجهة محلية دون اتصال؛ حالة الراديو مخزنة مؤقتًا ثانيتين لتجنب التكلفة)

- **شارة مصدر التشغيل**: يعرض مكوّن الوسائط مصدر التشغيل الحالي (Spotify / Cider / NetEase Cloud / QQ Music وغيرها)، فتعرف المشغّل من نظرة واحدة

- **تحسين الكلمات**: مفتاح إظهار/إخفاء ترجمة الكلمات، وزر «نسخ السطر الحالي» لنسخ الكلمات الحالية بنقرة واحدة

- **تخصيص أيقونة المكوّن**: يمكن تخصيص أيقونة كل مكوّن على حدة (أيقونة MDL2 أو Emoji)؛ عند عدم التحديد يُستخدم الشكل الافتراضي

- **إصلاح قفزات تكبير الكلمات**: إزالة اهتزاز «تكبير ثم تصغير» الناتج عن إزاحة النطّاط الحرفية في الكاراوكي؛ تحويل حجم/شفافية سطر الكلمات الحالي في الوضع الموسع إلى انتقال سلس 300ms، تمرير أكثر سلاسة وثباتًا


## WinIslands 1.1.0 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- **صوت/كتم مؤقت على الجزيرة**: عند تغير مستوى الصوت أو كتم/إلغاء كتم، تعرض الجزيرة مؤشر الصوت لفترة قصيرة (مدة العرض قابلة للضبط وقابلة للتبديل في الإعدادات)

- **نسخ/نقل الملفات على الجزيرة**: عند اكتشاف نسخ/نقل ملفات في مستكشف الملفات، تعرض الجزيرة تنبيه «جارٍ نسخ الملفات…» (تحديد نافذة محلي بحت، قابل للتبديل)

- **تقدم التنزيل على الجزيرة**: اكتشاف ملفات مؤقتة للتنزيل في دليل التنزيل (.crdownload / .part / .download وغيرها)، وعرض «جارٍ تنزيل N ملفات» (مغلق افتراضيًا ويمكن تفعيله في الإعدادات)

- **كبسولة «قيد الاستخدام» المدمجة**: يمكن تفعيلها في الإعدادات ← المكوّنات (مغلقة افتراضيًا)، لدمج «الميكروفون / الكاميرا / في اجتماع / تسجيل الشاشة» في كبسولة واحدة «قيد الاستخدام · …»؛ يمكن اختيار المكوّنات المشاركة في الدمج، ولا تظهر العناصر المدمجة منفردة

- **تحسين مؤقت بومودورو**: النقر على مكوّن بومودورو على الجزيرة يوقف/يستأنف المؤقت

- **لقطة شاشة/تسجيل مؤقت على الجزيرة**: عند التقاط شاشة أو بدء تسجيل، تعرض الجزيرة المؤشر المقابل مؤقتًا (يعمل حتى عند إخفاء الجزيرة)

- **إضاءة حرفية مرتدة في الكاراوكي**: كل سطر كلمات يُضاء بسلاسة من الحرف الأول مع ارتداد خفيف، أكثر سلاسة وطبيعية

- تحسين بنية الاستقصاء الداخلية: يمكن تشغيل أحداث مؤقتة مثل الصوت / النسخ / التنزيل / لقطة الشاشة حتى في حالة الإخفاء


## WinIslands 1.0.9 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- **مكوّنات جديدة**: استخدام GPU، الميكروفون / الكاميرا قيد الاستخدام، عدّاد تنازلي للعطلات، في اجتماع؛ يمكن لمكوّن الشبكة عرض منحنى مصغر لآخر 32 ثانية

- **إجراء سريع بالنقر المزدوج**: الإعدادات ← عام: عيّن النقر المزدوج على الجزيرة إلى «تشغيل / إيقاف» أو «فتح الإعدادات» أو «بدون إجراء»

- **مساعد كتم الاجتماعات**: يتعرف على نوافذ الاجتماعات (Teams / Zoom / Tencent Meeting / DingTalk / Feishu / Webex / Slack / Discord / Google Meet) ويقوم بعدم الإزعاج تلقائيًا أثناء الاجتماع (استدلال محلي بحت)

- **تنبيهات تسجيل الشاشة / لقطة الشاشة**: تنبيه لقطة PrintScreen + اكتشاف برامج التسجيل (OBS / Bandicam / Xbox Game Bar وغيرها)

- **تذكيرات أحداث التقويم (.ics)**: يحلل ملفات iCalendar المحلية، وعند وصول الحدث (مع إمكانية التبكير N دقيقة) تظهر لافتة؛ محلي بحت

- **تذكيرات اشتراكات RSS**: استقصاء RSS 2.0 / Atom، وعند وجود عناصر جديدة تظهر لافتة

- **تذكيرات البريد (POP3)**: قراءة رؤوس البريد فقط، وعند وجود بريد جديد تظهر لافتة؛ يُنصح باستخدام كود تفويض

- **مطلق سريع (بأسلوب Spotlight)**: `Ctrl+Space` للبحث عن التطبيقات / إدخال رابط لفتحه

- **لوحة سجل الحافظة**: `Ctrl+Alt+V` نافذة مستقلة، انقر للنسخ إلى الحافظة

- **القواعد (الأتمتة)**: شروط (دائمًا / بدون تشغيل / أثناء التشغيل / فترة زمنية / برنامج وسائط محدد) × إجراءات (إخفاء / طي إجباري / إظهار إجباري)

- **واجهة الجزيرة API v3**: صور (data URI / http)، تقدم ديناميكي (from/to/duration تقدم تلقائيًا)، تجديد نبضة (heartbeat_seconds)، تحديث جزئي PATCH، قناة WebSocket (/v3/ws)

- **المظهر**: 18 سمة جاهزة، خلفية مخصصة، 4 أنماط حركة، وضع الطاقة المنخفضة

- إعادة تصميم صفحة الإعدادات بأسلوب macOS System Settings (شريط تنقل يسار + محتوى يمين)، جميع التغييرات تسري فورًا

- **إصلاح النص الأسود في الوضع الداكن**: ربط لون المقدمة بشكل موحد لكل قوالب عناصر التحكم المخصصة في واجهة الإعدادات (أزرار / خانات اختيار / حقول إدخال / قوائم منسدلة / عناصر قائمة منسدلة / تبويبات / تنقل يساري…)، وإضافة فحص احتياطي وقت التشغيل — لم تعد تظهر خيارات معينة (لغة الواجهة، إجراء النقر المزدوج وغيرها من القوائم المنسدلة) بنص أسود غير مقروء في الوضع الداكن، وفي الوضع الفاتح تعود النصوص الداكنة تلقائيًا

- **إصلاح عدم فتح واجهة الإعدادات**: إزالة سطر لون المقدمة المكرر في XAML الذي تسبب في فشل تحميل BAML

- **تحسين أداء الحركات**: إعادة استخدام كائنات Run للكلمات حرفًا بحرف (إزالة تخطيط كل إطار)، وقصص 60fps ثابتة، وتحديث السجلات دفعة واحدة، حركات أنعم

- **إزالة النقاط الحمراء على المكوّنات** (حسب طلب المستخدم)


## WinIslands 1.0.8 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

**إعادة بناء شاملة لواجهة الإعدادات**

- تخطيط بأسلوب إعدادات نظام macOS: قائمة تنقل يسار + منطقة محتوى يمين، 13 فئة (عام / المظهر / المكوّنات / الوسائط / عرض معلومات الوسائط / الكلمات / Cider / واجهة الجزيرة API / أدوات الإنتاجية / التحديث / حول / الإشعارات / القواعد)
- تتبنى ألوان نصوص الإعدادات تلقائيًا بين الوضع الداكن/الفاتح: نص أبيض في الداكن، أسود في الفاتح؛ لا مشكلة في القراءة بعد الآن
- نص التنقل يسار أصبح أبيض عالي التباين، وأُزيل الخط الأبيض الفاصل يمين، وتقوية التغذية الراجعة عند التمرير والاختيار

**تشغيل الوسائط**

- مشغّل مصغر جديد: نافذة عائمة مستقلة تعرض غلاف الألبوم / اسم الأغنية / الفنان / شريط التقدم وضوابط التشغيل؛ قابلة للسحب بحرية مع حفظ الموقع؛ تظهر/تختفي تلقائيًا مع تشغيل الوسائط (يمكن تفعيلها في الإعدادات)
- تبديل جهاز إخراج الصوت الجديد: الإعدادات ← الوسائط يمكنها تعداد وتبديل جهاز التشغيل الافتراضي للنظام (يُنصح بإعادة تشغيل المشغّل بعد التبديل)
- تعزيز البنية التحتية لمصدر المشغّل: دعم تعداد جميع جلسات SMTC وجلسات Cider، مع إمكانية تبديل مصدر الوسائط

**الكلمات**

- كلمات ثنائية اللغة جديدة: دمج أسطر الترجمة المتجاورة تلقائيًا حسب الطوابع الزمنية (يمكن إيقافها في الإعدادات)

**المظهر والحركة**

- أنماط حركة جديدة: 4 أنماط حركة (زنبرك iOS (الافتراضي) / زنبرك ناعم / ارتداد مرن / تلاشٍ بسيط)، والتوسيع/الطي يستخدم تسهيلًا غير خطي
- وضع طاقة منخفض جديد: تقليل معدل إطارات الموجة وتبسيط الحركات في وضع الخمول لتوفير الطاقة

**اختصارات لوحة المفاتيح العامة**

- 5 توافيق مفاتيح قابلة للتخصيص: إظهار/إخفاء، تشغيل/إيقاف، السابق، التالي، توسيع/طي
- يدعم Ctrl / Alt / Shift / Win + حروف وأرقام وF1–F24 وأسهم الاتجاهات

**محرك القواعد الذكية (الإعدادات ← القواعد)**

- تحكم تلقائي في ظهور الجزيرة حسب الشروط: دائمًا / عند عدم تشغيل وسائط / أثناء تشغيل وسائط / فترة زمنية محددة / عند تشغيل برنامج وسائط محدد
- الإجراءات: إخفاء الجزيرة / طي إجباري / إظهار إجباري؛ الأولوية: إخفاء > طي > إظهار إجباري

**الإشعارات**

- دعم سجل الإشعارات: علامة النقطة الحمراء غير المقروءة، تحديد الكل كمقروء، حذف عنصر، النقر لفتح التطبيق المصدر، مسح السجل
- طي الإشعارات الجديد: الإشعارات المكررة من نفس المصدر والعنوان تعيد استخدام نفس اللافتة وتجمع العدد
- قائمة بيضاء جديدة لعدم الإزعاج: المصادر في القائمة البيضاء (أسماء exe مفصولة بفواصل) لا تتأثر بعدم الإزعاج وتظل تظهر كالمعتاد
- إزالة شارة النقطة الحمراء غير المقروءة من الجزيرة

**أدوات الإنتاجية**

- تلميح «تم النسخ» عند نسخ نص
- اكتشاف تلقائي لرموز التحقق عبر الرسائل القصيرة وإبرازها
- نسخ النص الكبير يعرض حركة تقدم (تقدير حسب الطول، ويعرض النتيجة بعد الانتهاء)

**المكوّنات**

- مكوّن عدّاد العطلات الجديد: جدول عطلات 2026–2027 مدمج (رأس السنة / عيد الربيع / كينغمينغ / عيد العمال / مهرجان قوارب التنين / منتصف الخريف / اليوم الوطني)، يعرض «XX بعد N من الأيام» أو «اليوم XX»؛ قابل للتبديل في إعدادات المكوّنات

**واجهة الجزيرة API v2**

- حقول جديدة: subtitle (عنوان فرعي)، type (info / success / warning / error)، priority (high / normal / low)، accent (لون تمييز مخصص)، click (استدعاء بالنقر على البطاقة كاملة)
- قائمة الانتظار: ترتيب عدة إرسالات من الأولوية الأعلى ← الأدنى، أولًا يدخل أولًا يخرج؛ إعادة إرسال نفس id تحتفظ بموقع القائمة الأصلي ووقت الانتهاء
- استجابة POST تتضمن الآن حقل position
- نصوص برمجية جاهزة للتشغيل في docs/sdk-examples/ (push.bat / pull.bat / push.ps1 / push.py / pull.py)

**الإصلاحات**

- إصلاح مشكلة عدم فتح نافذة الإعدادات / عدم التشغيل بالنقر المزدوج (حماية من القيم الفارغة وتهيئة صريحة للتنقل وتبديل التبويبات)
- إضافة 69 اختبارًا آليًا (واجهة الجزيرة API، طي الإشعارات والقائمة البيضاء، محرك القواعد، التعرف على رموز التحقق، تحليل LRC)، جميعها ناجحة

**الأصول (Assets)**

- إصدارات محمولة Windows x64 / arm64 (ملف مفرد ذاتي الاحتواء، بدون تثبيت، يعمل مباشرة)
- مثبّت Windows الشامل (Inno Setup، يدعم x64 وARM64، يُثبت حسب البنية)
- > الإصدارات المحمولة ملفات exe مستقلة؛ لم يعد يتم توفير أرشيفات ZIP.


## WinIslands 1.0.7 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- «موجة الصوت» ترقت إلى **متابعة إيقاع الموسيقى**: التقاط حقيقي للصوت الجاري تشغيله عبر WASAPI loopback؛ الموجة عالية عند الإيقاع القوي ومنخفضة عند الهدوء — لم تعد شريط حجم ثابتًا

- **عرض مستمر 60fps**: تنعيم أسي 25ms للهجوم / 140ms للتحرير؛ حركة الموجة مستمرة، غير قاسية ولا متقطعة

- إعدادات جديدة (الإعدادات ← المظهر ← موجة الصوت): مفتاح متابعة الإيقاع، الحساسية 0.2–3.0، ارتفاع الموجة 0.4–1.6؛ تسري التغييرات فورًا

- عند عدم وجود جهاز صوت / خلل في خدمة الصوت، يتم التراجع تلقائيًا إلى محاكاة الإيقاع مع إعادة محاولة الاستعادة كل 8 ثوانٍ — لا تجمد ولا تراكم للخيوط


## WinIslands 1.0.6 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- إضافة 6 مكوّنات للجزيرة: الصوت، مؤشرات لوحة المفاتيح (CapsLock)، الحافظة، المهام، بومودورو، الجدول — جميعها تدعم خانات «بدون أغنية / مع أغنية» ثنائية وترتيبًا بالسحب

- إضافة صفحة إعدادات «أدوات الإنتاجية»: سجل الحافظة، مؤقت بومودورو، قائمة المهام، تذكيرات الجدول

- إضافة «موجة الصوت»: عند تشغيل الوسائط، تنبض المنطقة يسار أزرار التحكم مع مستوى صوت النظام في الوقت الفعلي (قابلة للتبديل في الإعدادات ← المظهر)

- إضافة 7 سمات جاهزة: الافتراضية / المحيط / الغابة / الغروب / النيون / أحادية اللون / العنب

- تخصيص المظهر: خط مخصص، مقياس حجم الخط (0.8–1.4)، نصف قطر زوايا الكبسولة (16–40)، خلفية موسعة مأخوذة من غلاف الألبوم، شارة إشعارات غير مقروءة

- إضافات قائمة الدرج: وضع عدم الإزعاج (يدوي / كتم تلقائي حسب الفترة)، فحص التحديثات، عرض السجلات

- إعادة تصميم حديثة لصفحة الإعدادات (زوايا دائرية + زجاج سائل)؛ تسري التغييرات فورًا دون حفظ يدوي

- فحص تحديثات جديد (يدوي من الدرج/الإعدادات، فحص تلقائي اختياري، مغلق افتراضيًا)

- الأصول: إصدارات محمولة Windows x64 / arm64 (ملف مفرد ذاتي الاحتواء، بدون تثبيت، يعمل مباشرة)؛ مثبّت Windows الشامل (Inno Setup، يدعم x64 وARM64، يُثبت حسب البنية)

- > الإصدارات المحمولة ملفات exe مستقلة؛ لم يعد يتم توفير أرشيفات ZIP.


## WinIslands 1.0.5 (إصدار مستقر)

أداة Dynamic Island حديثة ومتعددة الوظائف لنظام Windows.

### سجل التغييرات

- إضافة «واجهة الجزيرة API»: يمكن للبرامج الأخرى دفع المعلومات إلى الجزيرة عبر واجهة HTTP محلية (مثل تكامل تطبيقات الطرف الثالث مع جزيرة iOS)؛ **وثائق المطور في docs/IslandAPI.md**

-   - `POST /v1/island/push` دفع/تحديث · `DELETE /v1/island/push/{id}` إزالة · `GET /v1/island/active` استعلام · `GET /v1/health`

-   - يدعم الأيقونة والعنوان والنص والتقدم والأزرار (فتح رابط / تشغيل برنامج) ومدة عرض مخصصة لكل عنصر

-   - توفر صفحة الإعدادات: التفعيل / المنفذ / Token اختياري / المدة الافتراضية العامة

- بطاقة الإرسال تُعرض في سطر واحد في الوضع المضغوط، ولا تغطي المكوّنات الأخرى، **ولا تؤثر على طول وعرض الجزيرة** (الأحجام التلقائية/اليدوية ثابتة)

- «الضبط التلقائي» للأحجام: يتكيف مع المحتوى؛ سحب شريط التمرير يدويًا يغلق الخيار التلقائي المقابل

- المحتوى الموسع يدعم التمرير بعجلة الماوس (شريط التمرير مخفي)

- توحيد المحاذاة الرأسية للمكوّنات؛ إصلاحات تخطيط/خط البدء (فرض PerMonitorV2، الحجم الصحيح من البداية)

- تشغيل الوسائط لا يعرض إشعار «الآن قيد التشغيل»

- إصلاح: بعد توسيع الجزيرة (نحو 1–2 ثانية) كانت البطاقة تعود إلى الحجم المضغوط مسببة شاشة سوداء كاملة

-   - أصبح المحتوى الموسع يتلاشى متداخلًا مع السطر المضغوط؛ لا يظهر الخلفية أثناء الحركات

-   - الحجم النهائي للبطاقة يُكتب صراحةً بعد اكتمال حركات التوسيع/الطي؛ الحالة الموسعة ثابتة ولا تنكمش

-   - إصلاح متزامن للشاشة السوداء عند النقر على أزرار الجزيرة لطرف ثالث في الوضع الموسع

- الأصول: إصدارات محمولة Windows x64 / arm64 (ملف مفرد ذاتي الاحتواء، بدون تثبيت، يعمل مباشرة)؛ مثبّت Windows الشامل (Inno Setup، يدعم x64 وARM64، يُثبت حسب البنية)

- > الإصدارات المحمولة ملفات exe مستقلة؛ لم يعد يتم توفير أرشيفات ZIP.

---

## Русский

## WinIslands 1.2.4（正式版 / Stable）

Современный многофункциональный Dynamic Island для Windows. A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **🎶 Плавное караоке**: продвижение по буквам теперь использует плавную интерполяцию smoothstep с кросс-переходами между буквами — подсветка течёт слева направо как световая лента, плавно разгоняясь и замедляясь, без скачков.

- **🔋 Постоянный индикатор низкого заряда**: когда заряд падает ниже порога и устройство не заряжается, в правом верхнем углу острова постоянно отображается небольшая пилюля заряда (красная ≤10 % / иначе оранжевая), обновляясь в реальном времени; она исчезает при подключении к сети или повышении заряда, и её можно отключить в настройках.

- **🎧 Анимация подключения устройств**: при подключении/отключении Bluetooth показывается анимированная карточка в стиле iOS; при подключении автоматически считывается заряд устройства и отображается «Название устройства · Заряд xx%» (только название, если заряд прочитать нельзя), не блокируя интерфейс.


## WinIslands 1.2.1 (Stable)

## WinIslands 1.2.2 (Stable)

Современный многофункциональный Dynamic Island для Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Что нового

- **Компактные уведомления в одну строку**: в свёрнутом виде уведомления и напоминания показываются одной строкой (иконка + заголовок + однострочное резюме) и обрезаются при переизбытке длины, больше не расширяя остров; полные детали появляются при развёртывании

- **Исправлен обрезанный/смещённый текст в свёрнутом виде**: при активном уведомлении компактная высота растёт вместе с содержимым, а карточка выравнивается сверху — без вертикальной обрезки

- **Автоматическое расширение острова для длинных уведомлений**: при длинном уведомлении остров автоматически расширяется по горизонтали, чтобы элементы управления медиа, часы и весь текст полностью оставались внутри острова, а не обрезались у края

Современный многофункциональный Dynamic Island для Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Что нового

- **Пружинное демпфирование + каскадные переходы**: раскрытие/сворачивание теперь использует пружину в стиле iOS (с лёгким отскоком), а секции карточки появляются/исчезают поочерёдно с нелинейным ритмом — ближе к Dynamic Island из iOS

- **Связанная с громкостью анимация**: при изменении системной громкости или перетаскивании ползунка остров кратко показывает анимацию «иконка + полоса громкости + проценты», которая сама плавно исчезает через несколько секунд

- **Умная прозрачность по слоям**: компактное состояние прозрачнее, развёрнутое — плотнее, с плавным переходом; автоматически избегает двойного наложения при активной подцветке обложки

- **Переход смены обложки**: при смене трека обложка плавно проявляется с лёгким масштабированием, дополняя «дышащий» фон подцветки

- **Плавное переключение темы**: при переключении светлой/тёмной темы цвет фона и рамки карточки интерполируется за ~0,3 с вместо мигания

- **Оптимизация производительности**: покадровые эффекты (например, волна) обновляют только видимую панель, снижая нагрузку на ЦП во время воспроизведения для более плавной анимации
## WinIslands 1.2.0 (Stable)

Современный многофункциональный Dynamic Island для Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Что нового

- **Переключение источника текста одним нажатием**: мгновенно переключайтесь между «Авто / Локальный LRC / AMLL TTML / API Cider / Онлайн-тексты», и текст текущей песни сразу перезагружается
- **Автоматическое восстановление после сбоя**: после аварийного завершения следующий запуск покажет уведомление о восстановлении — без зависаний, чёрного экрана и потери состояния
- **Настраиваемая длительность анимации**: новый ползунок длительности анимации раскрытия/сворачивания (300–1400мс) позволяет подстроить скорость под себя
## WinIslands 1.1.9 (Stable)

Современный многофункциональный Dynamic Island для Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Что нового

- **Более плавные и согласованные анимации**: раскрытие/сворачивание и автоматическая прокрутка текста теперь используют временную интерполяцию (независимую от частоты кадров), одинаковую на 60fps и дисплеях с высокой частотой (120Гц) — быстрые последовательные действия больше не дёргаются

- **Ещё ниже расход CPU при воспроизведении**: интерполяция прогресса отдаётся на 5Гц, а покадровый караоке продвигается непрерывно на 60fps по внутренним часам элемента — тот же вид, меньше энергии в фоне

- **Кэш декодирования обложек**: каждая обложка декодируется один раз и переиспользуется (последние 24 автоматически вытесняются), устраняя повторные операции ввода-вывода и скачки памяти от посекундных обновлений медиа-сессии

- **Улучшения прокрутки текста**: прокрутка выполняется только при раскрытой и видимой карточке, с точным позиционированием при приближении к цели — без дрожания при быстрой смене строк

## WinIslands 1.1.8 (Stable)

Современный многофункциональный виджет Dynamic Island для Windows.

### Что нового

- **Быстрее и чётче**: разворачивание/сворачивание сокращено с ~1,1 с до ~0,7 с; пружина стала жёстче, отклик чётче — никакой вялости и плавности «каши»

- **Ускорение всех анимаций**: переходы размеров компонентов, каскадное появление контента, перемещение (~430 мс → 320 мс), push-уведомления на остров и скрытие с затуханием ускорены примерно на 30–35 %

- **Все скины анимации ускорены**: iOS Spring / Soft / Elastic / Fade используют одинаковые сокращённые базовые длительности, сохраняя свой характер, но ощущаются более отзывчивыми


## WinIslands 1.1.7 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- **Перевалочный пункт файлов**: перетащите любой файл прямо на остров — он мгновенно превратится в перетаскиваемый элемент (только ссылка на путь, без копирования файла); затем перетащите его в проводник или любое другое приложение, чтобы быстро переместить

- **Очистка при выходе из приложения**: когда медиаприложение закрывается и нет новой сессии воспроизведения, остров сразу очищает устаревшую информацию о треке

- **Более плавные анимации**: перемещение и автопрокрутка текста песен используют более мягкое сглаживание (перемещение ~430 мс) для более непрерывного движения

- **Версия на странице «О программе»**: Настройки → О программе теперь показывает полную версию (например, WinIslands 1.1.7)

- **Стабильность и производительность**: улучшено определение состояния медиасессии (невозможность чтения состояния воспроизведения считается завершением медиа); защита от одновременных колебаний анимации положения; меньше автономный однофайловый сборка

## WinIslands 1.1.6 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- **Автоскрытие на экране блокировки**: остров автоматически скрывается при нажатии Win+L или отключении удалённого рабочего стола и восстанавливается после разблокировки (Настройки → Общие, можно отключить)

- **Плановый «Не беспокоить» теперь с точностью до минуты**: время начала/окончания «Не беспокоить» можно задавать с точностью до минуты

- **Быстрое действие среднего клика**: средний клик по острову выполняет настраиваемое быстрое действие (настраивается в Настройки → Общие)

- **Плановое переключение светлой/тёмной темы**: задайте часы начала/конца тёмного периода, и тема переключится автоматически (действует только при Theme=Auto)

- **Поля ввода на острове**: сторонние отправки могут включать поле ввода; когда пользователь вводит текст на острове, значение возвращается отправителю через WebSocket

- **Фоновый опрос по требованию**: опрос останавливается, если не включены связанные виджеты/функции, снижая нагрузку на CPU в простое

- Исправлена отправка полей ввода острова; исправлена потеря данных кнопок отправки (включая введённое значение) в обратных вызовах


## WinIslands 1.1.5 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

**Тексты песен**

- **Пословные тексты AMLL (настоящее караоке)**: интеграция библиотеки TTML в стиле Apple Music с amll.dev; Настройки → Тексты, переключатель «Пословные тексты AMLL» (включён по умолчанию); приоритет источников: локальный LRC → AMLL TTML → Cider → онлайн-тексты
- **Движок пословной подсветки переписан**: непрерывное продвижение по часам 60 fps + нелинейное сглаживание; запасной вариант — равномерная строка; двуязычные тексты сопоставляются по шкале времени
- **Стабильная подсветка при паузе**: заморозка в момент паузы; без прыжков после выхода и перезапуска; только активная строка управляет анимацией, почти 0 CPU в простое

**Исправления**

- Исправлены отступы текстов в компактном режиме: фиксированное расстояние между текстами и кнопками справа; при нехватке места ширина острова увеличивается автоматически (720→800)
- Стабильность: тайм-аут AMLL 5 с + корректное понижение; калибровка базовой точки часов при повторном появлении элементов управления


## WinIslands 1.1.4 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- **Кнопки быстрых действий**: новый ряд настраиваемых кнопок внизу развёрнутой карточки острова (блокировка / без звука / воспроизведение-пауза / скриншот / рабочий стол / диспетчер задач / калькулятор / сон / громкость±); Настройки → Быстрые действия позволяют отмечать и сортировать с помощью ↑↓, изменения применяются сразу

- **Уведомления о входящих звонках**: определяет окна голосовых/видеозвонков WeChat / QQ и показывает уведомление (различает «входящий звонок» и «идёт разговор»); Настройки → Уведомления позволяют включить и настроить приложения; только локальное определение, без передачи данных

- **Командное действие острова**: кнопки сторонних отправок поддерживают `action: "command"` для локального выполнения командной строки (только loopback-API, Token настраивается)

- **Тема карточки острова**: сторонние отправки могут включать `theme: dark / light / auto`; карточки автоматически переключают тёмный/светлый стеклянный стиль

- **Ускорение анимаций**: общая длительность четырёх типов анимации сокращена примерно на 20 %, с сохранением пружинного сглаживания iOS и плавных 60 fps без пропуска кадров

- **Оптимизация фоновой нагрузки**: индикаторы клавиатуры опрашиваются только при включённом виджете, монитор полноэкранного режима снижает частоту, звуковая волна снижает частоту в простое

- **Исправления стабильности**: исправлено ложное уведомление «Скопировано» при запуске с уже имеющимся содержимым буфера обмена (базовая точка запуска); исправлено экранирование заголовков .ics календаря (\, \; \n), отображавшихся как обратные слэши

- **Производительность и стабильность**: снижен шум журналов Bluetooth / SMTC / погоды, экспоненциальная задержка при ограничении погоды; сборка проходит, все 104 модульных и интеграционных теста успешны


## WinIslands 1.1.3 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- **Переключение между несколькими плеерами**: при нескольких открытых плеерах развёрнутая карточка переключает управляемый медиа-источник одним кликом (значок ноты + имя источника + стрелка выпадающего списка); больше не ограничено последним запущенным плеером

- **Иммерсивная обложка**: клик по обложке / большому изображению на развёрнутой карточке открывает полноэкранный предпросмотр; клик / Esc / правый клик закрывает с затуханием

- **Точная настройка времени текстов**: кнопки +0,5 с / -0,5 с в области текстов развёрнутой карточки; смещение запоминается для каждой песни, чтобы идеально выровнять тексты

- **Улучшенные тексты на рабочем столе**: отдельное окно текстов поддерживает настройку прозрачности (по умолчанию 0,85) и переключатель «Блокировка» (при блокировке — клики насквозь, нельзя перетаскивать)

- **«Дышащая» динамическая тема**: при включённом фоне из цвета обложки фон развёрнутой карточки медленно «дышит» в такт цвету обложки (цикл около 18 с) вместо статичной плоской заливки

- **Отклик сразу по нажатию**: нажатие кнопки мыши немедленно переключает разворачивание/сворачивание, не дожидаясь отпускания, — отзывчивее

- **Кнопки действий в уведомлениях**: баннеры уведомлений поддерживают кнопки действий (уведомление о подключении Bluetooth теперь имеет кнопки «Отключить» и «Настройки», выполняемые и сворачиваемые сразу)

- **Обратный вызов кнопок острова**: при нажатии notify-кнопки сторонней отправки событие push_button (с push_id и текстом кнопки) транслируется отправителю через WebSocket для обработки

- **Производительность и стабильность**: динамическая тема подписывается на композиционные кадры по требованию (0 CPU в простое), градиентные кисти кэшируются для снижения нагрузки GC; исправлен конфликт мьютексов, не позволявший запускаться при одновременной работе новых и старых версий


## WinIslands 1.1.1 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- **Уведомления о заряде**: предупреждение о низком заряде (порог настраивается); уведомление «Зарядка завершена» при подключении к питанию и достижении заданного порога (по умолчанию 100 %); всё локально, можно отключить

- **Сетевые уведомления**: уведомление о потере / восстановлении сети (локальное определение, можно отключить)

- **Новые виджеты**: свободное место на диске (системный диск), состояние метода ввода (RU / EN + название IME)

- **Лунный календарь и сезоны**: виджет даты может дополнительно показывать лунную дату и сезоны (включено по умолчанию, отключается в настройках)

- **Виджеты быстрого переключения**: WiFi / Bluetooth / ночной режим / без звука одним кликом (локальные API, без интернета; состояние Radio кэшируется на 2 с, чтобы избежать нагрузки)

- **Значок источника воспроизведения**: медиа-виджет показывает текущий источник (Spotify / Cider / NetEase Cloud / QQ Music и т. д.), чтобы сразу было понятно, из какого плеера

- **Улучшения текстов**: переключатель показа/скрытия перевода текстов; кнопка «Копировать текущую строку» одним кликом

- **Пользовательские иконки виджетов**: каждый виджет может иметь свою иконку (MDL2 или Emoji); по умолчанию используется стандартный глиф

- **Исправлено дрожание масштаба текстов**: убрано дрожание «увеличение и уменьшение», вызванное смещением отскока слов караоке; размер/прозрачность текущей строки в развёрнутом виде теперь плавно меняются за 300 мс


## WinIslands 1.1.0 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- **Временный индикатор громкости / без звука**: при изменении системной громкости или переключении без звука остров кратко показывает индикатор громкости (длительность настраивается, можно отключить в настройках)

- **Копирование / перемещение файлов на острове**: когда Проводник копирует/перемещает файлы, остров показывает «Копирование файлов…» (только локальное определение по заголовку окна, можно отключить)

- **Прогресс загрузки на острове**: определяет временные файлы браузера в папке загрузок (.crdownload / .part / .download и т. д.) и показывает «Загружается N файл(ов)» (по умолчанию выключено, можно включить в настройках)

- **Объединённая капсула «Используется»**: Настройки → Виджеты позволяют включить (по умолчанию выключено) объединение «Микрофон / Камера / В собрании / Запись» в одну капсулу «Используется · …»; выберите, какие виджеты участвуют; объединённые больше не показываются отдельно

- **Улучшения Pomodoro**: клик по виджету Pomodoro на острове ставит на паузу / возобновляет таймер

- **Временный индикатор скриншота / записи**: при скриншоте или начале записи остров временно показывает соответствующий индикатор (работает даже при скрытом острове)

- **Пословная подсветка караоке с отскоком**: каждая строка плавно загорается с первого слова с лёгким отскоком — плавнее и естественнее

- Оптимизирована внутренняя архитектура опроса: временные события острова (громкость / копирование / загрузка / скриншот) могут срабатывать в скрытом состоянии


## WinIslands 1.0.9 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- **Новые виджеты**: загрузка GPU, микрофон / камера используются, обратный отсчёт праздников, в собрании; сетевой виджет может показывать мини-кривую за 32 секунды

- **Быстрое действие двойного клика**: Настройки → Общие позволяют задать двойной клик как «Воспроизведение / Пауза», «Открыть настройки» или «Без действия»

- **Помощник тишины в собраниях**: распознаёт окна собраний (Teams / Zoom / Tencent Meeting / DingTalk / Feishu / Webex / Slack / Discord / Google Meet) и включает «Не беспокоить» во время собраний (только локальная эвристика)

- **Индикаторы записи / скриншотов**: уведомление о скриншоте PrintScreen + определение программ записи (OBS / Bandicam / Xbox Game Bar и т. д.)

- **Напоминания о событиях календаря (.ics)**: разбирает локальные файлы iCalendar; баннер при наступлении события (с возможным опережением на N минут); полностью локально

- **Уведомления RSS**: опрос RSS 2.0 / Atom, баннер для новых записей

- **Уведомления почты (POP3)**: читает только заголовки, баннер для новых писем; рекомендуется код авторизации

- **Быстрый запуск (в стиле Spotlight)**: `Ctrl+Пробел` поиск приложений / открытие URL

- **Панель истории буфера обмена**: `Ctrl+Alt+V` открывает отдельное окно; клик копирует обратно

- **Правила (автоматизация)**: условия (всегда / не воспроизводится / воспроизводится / временной диапазон / конкретное медиа-приложение) × действия (скрыть / принудительно свернуть / принудительно показать)

- **Island API v3**: изображения (data URI / http), анимированный прогресс (from/to/duration автоматически), продление по heartbeat (heartbeat_seconds), частичные обновления PATCH, канал WebSocket (/v3/ws)

- **Внешний вид**: 18 тем, настраиваемый цвет фона, 4 типа анимации, режим низкого энергопотребления

- Страница настроек переработана в стиле системных настроек macOS (левая навигация + правая область); все изменения применяются сразу

- **Исправлен чёрный текст в тёмном режиме**: единая привязка цвета текста для всех пользовательских шаблонов элементов настроек (кнопки / флажки / поля / выпадающие списки / элементы списков / вкладки / левая навигация и т. д.) плюс резервное сканирование во время выполнения — в тёмном режиме отдельные опции (язык интерфейса, выпадающие списки действий двойного клика и т. д.) больше не отображаются нечитаемым чёрным текстом; в светлом режиме автоматически возвращается тёмный текст

- **Исправлено открытие страницы настроек**: удалены дублирующиеся строки цвета текста XAML, вызывавшие сбой загрузки BAML

- **Оптимизация производительности анимации**: пословная подсветка караоке переиспользует объекты Run (устраняя покадровую разметку), стабильные раскадровки 60 fps, пакетное обновление журналов — анимации плавнее

- **Удалены красные точки-значки с виджетов** (по запросу пользователя)


## WinIslands 1.0.8 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

**Полная переработка интерфейса настроек**

- Макет в стиле системных настроек macOS: список навигации слева + область содержимого справа, 13 категорий (Общие / Внешний вид / Виджеты / Медиа / Отображение медиа-информации / Тексты / Cider / Island API / Инструменты / Обновление / О программе / Уведомления / Правила)
- Цвета текста настроек автоматически адаптируются к тёмному/светлому режиму: белый в тёмном, чёрный в светлом; нечитаемого текста больше нет
- Текст левой навигации стал высококонтрастным белым, удалена белая разделительная линия справа, усилена реакция на наведение и выбор

**Воспроизведение медиа**

- Новый мини-плеер: отдельное плавающее окно с обложкой / названием / исполнителем / полосой прогресса и управлением; свободно перетаскивается с запоминанием позиции; автоматически показывается/скрывается при воспроизведении (включается в настройках)
- Новое переключение устройства вывода звука: Настройки → Медиа может перечислять и менять системное устройство воспроизведения по умолчанию (рекомендуется перезапустить плеер после смены)
- Улучшена внутренняя основа источников плееров: перечисляет все сессии SMTC и Cider; можно переключать источник медиа

**Тексты**

- Новые двуязычные тексты: автоматическое объединение соседних строк перевода по временным меткам (отключается в настройках)

**Внешний вид и анимация**

- Новые типы анимации: 4 стиля (пружина iOS (по умолчанию) / мягкая пружина / эластичный отскок / простое затухание), нелинейное сглаживание разворачивания/сворачивания
- Новый режим низкого энергопотребления: снижение частоты рендеринга волны и упрощение анимаций в простое

**Глобальные горячие клавиши**

- 5 настраиваемых комбинаций: показать/скрыть, воспроизведение/пауза, предыдущий, следующий, развернуть/свернуть
- Поддержка Ctrl / Alt / Shift / Win + буквы, цифры, F1–F24, стрелки

**Умный движок правил (Настройки → Правила)**

- Автоматическое управление видимостью острова по условию: всегда / когда нет воспроизведения / когда воспроизводится / заданный временной диапазон / когда играет конкретное медиа-приложение
- Действия: скрыть остров / принудительно свернуть / принудительно показать; приоритет: скрыть > свернуть > принудительно показать

**Уведомления**

- История уведомлений: метка непрочитанного красной точкой, отметить всё прочитанным, удалить одну запись, клик для открытия исходного приложения, очистить историю
- Новое сворачивание уведомлений: повторные уведомления от одного источника и с одним заголовком используют один баннер и накапливают счётчик
- Новый белый список «Не беспокоить»: источники из белого списка (имена exe через запятую) не затрагиваются режимом «Не беспокоить» и показывают баннеры как обычно
- Удалён значок непрочитанного с острова

**Инструменты**

- Уведомление «Скопировано» при копировании текста
- Автоматическое распознавание SMS-кодов подтверждения с подсветкой
- Копирование больших текстов показывает анимацию прогресса (оценка по длине, результат после завершения)

**Виджеты**

- Новый виджет обратного отсчёта праздников: встроенная таблица праздников 2026–2027 (Новый год / Весенний фестиваль / Цинмин / День труда / Праздник драконьих лодок / Праздник середины осени / Национальный день), показывает «XX через N дней» или «Сегодня XX»; включается в настройках виджетов

**Island API v2**

- Новые поля: subtitle (подзаголовок), type (info / success / warning / error), priority (high / normal / low), accent (настраиваемый акцентный цвет), click (обратный вызов клика по всей карточке)
- Очередь отправок: несколько отправок сортируются по приоритету высокий → низкий, FIFO; повторная отправка того же id сохраняет исходную позицию и срок действия
- Ответ POST теперь включает поле position
- Новые готовые примеры скриптов в docs/sdk-examples/ (push.bat / pull.bat / push.ps1 / push.py / pull.py)

**Исправления**

- Исправлена невозможность открыть окно настроек / неработающий двойной клик (защита от пустых значений и явная инициализация навигации и переключения вкладок)
- Добавлено 69 автоматических тестов (Island API, сворачивание уведомлений и белый список, движок правил, распознавание кодов подтверждения, разбор LRC), все успешны

**Артефакты**

- Портативные сборки Windows x64 / arm64 (один файл, автономный, без установки, запуск сразу)
- Универсальный установщик Windows (Inno Setup, x64 и ARM64, установка по архитектуре)
- > Портативные сборки — отдельные exe-файлы; ZIP-архивы больше не предоставляются.


## WinIslands 1.0.7 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- «Звуковая волна» обновлена до **следования ритму музыки**: захват реального системного звука в реальном времени через WASAPI loopback; высокие волны на сильных долях, низкие в тишине — больше не фиксированная полоса громкости

- **Непрерывный рендеринг 60 fps**: экспоненциальное сглаживание с атакой 25 мс / спадом 140 мс; движение волны непрерывное, не жёсткое и не прерывистое

- Новые настройки (Настройки → Внешний вид → Звуковая волна): переключатель следования ритму, чувствительность 0,2–3,0, высота волны 0,4–1,6; изменения применяются сразу

- При отсутствии аудиоустройства / сбое аудиослужбы — корректное понижение до имитации ритма с повторной попыткой восстановления захвата каждые 8 секунд; без зависаний и накопления потоков


## WinIslands 1.0.6 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- Добавлено 6 виджетов острова: громкость, индикаторы клавиатуры (CapsLock), буфер обмена, задачи, Pomodoro, расписание — все с двухколоночными флажками «без песни / с песней» и перетаскиванием для сортировки

- Новая страница настроек «Инструменты»: история буфера обмена, таймер Pomodoro, список задач, напоминания расписания

- Новая «звуковая волна»: во время воспроизведения область слева от кнопок управления пульсирует с системной громкостью в реальном времени (включается в Настройках → Внешний вид)

- Добавлено 7 тем: По умолчанию / Океан / Лес / Закат / Неон / Монохром / Виноград

- Персонализация внешнего вида: собственный шрифт, масштаб размера шрифта (0,8–1,4), радиус углов капсулы (16–40), фон развёрнутого вида, окрашенный обложкой, значок непрочитанных уведомлений

- Новое в меню трея: режим «Не беспокоить» (ручной / тихие уведомления по расписанию), проверка обновлений, просмотр журналов

- Современная страница настроек (скруглённые углы + жидкое стекло); изменения применяются сразу, без ручного сохранения

- Новая проверка обновлений (вручную из трея / настроек, необязательная автоматическая, по умолчанию выключена)

- Артефакты: портативные сборки Windows x64 / arm64 (один файл, автономный, без установки, запуск сразу); универсальный установщик Windows (Inno Setup, x64 и ARM64, установка по архитектуре)

- > Портативные сборки — отдельные exe-файлы; ZIP-архивы больше не предоставляются.


## WinIslands 1.0.5 (Стабильный)

Современный, многофункциональный виджет Dynamic Island для Windows.

### Журнал изменений

- Новая «Island API»: другие программы могут отправлять информацию на остров через локальный HTTP-интерфейс (как интеграция сторонних приложений с островом iOS); **документация для разработчиков в docs/IslandAPI.md**

-   - `POST /v1/island/push` отправить/обновить · `DELETE /v1/island/push/{id}` удалить · `GET /v1/island/active` запрос · `GET /v1/health`

-   - Поддержка иконки, заголовка, текста, прогресса, кнопок (открыть ссылку / запустить программу), индивидуальной длительности показа

-   - На странице настроек: включение / порт / необязательный Token / глобальная длительность по умолчанию

- Отправленные карточки показываются в одну строку в компактном состоянии, не перекрывают другие виджеты и **не влияют на ширину и высоту острова** (авто/ручной размеры постоянны)

- «Автоподбор» размера: адаптируется к содержимому; ручное перетаскивание ползунка автоматически отключает соответствующую авто-опцию

- Развёрнутое содержимое поддерживает прокрутку колесом (полоса скрыта)

- Выравнивание виджетов по верхнему/нижнему краю унифицировано; исправлены раскладка/шрифт при запуске (принудительный PerMonitorV2, правильный размер с первого мгновения)

- При воспроизведении медиа больше не появляется уведомление «Сейчас играет»

- Исправлено: после разворачивания острова (примерно через 1–2 с) возврат карточки к компактному размеру вызывал полный чёрный экран

-   - Развёрнутое содержимое теперь перекрёстно растворяется с компактной строкой; фон не просвечивает во время анимаций

-   - После завершения анимаций разворачивания/сворачивания финальный размер карточки явно записывается; развёрнутое состояние стабильно и не уменьшается

-   - Также исправлен чёрный экран при клике по сторонним кнопкам острова в развёрнутом состоянии

- Артефакты: портативные сборки Windows x64 / arm64 (один файл, автономный, без установки, запуск сразу); универсальный установщик Windows (Inno Setup, x64 и ARM64, установка по архитектуре)

- > Портативные сборки — отдельные exe-файлы; ZIP-архивы больше не предоставляются.

---

## Português

## WinIslands 1.2.4（正式版 / Stable）

Um Dynamic Island moderno e multifuncional para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### 更新内容

- **🎶 Karaokê mais fluido**: o avanço letra por letra agora usa suavização smoothstep com transições cruzadas entre caracteres — o destaque flui da esquerda para a direita como uma fita de luz, com aceleração/desaceleração, sem passos bruscos.

- **🔋 Indicador persistente de bateria fraca**: quando a bateria fica abaixo do limite e não está carregando, uma pequena pílula de bateria permanece no canto superior direito da ilha (vermelha ≤10% / laranja caso contrário), atualizando em tempo real; desaparece ao conectar o carregador ou recuperar, e pode ser ativada nas configurações.

- **🎧 Animação de conexão de dispositivos**: ao conectar/desconectar um dispositivo Bluetooth, um cartão animado estilo iOS é exibido; ao conectar, a bateria do dispositivo é lida automaticamente e mostrada como «Nome do dispositivo · Bateria xx%» (somente o nome se não for possível ler), sem bloquear a interface.


## WinIslands 1.2.1 (Estável)

## WinIslands 1.2.2 (Estável)

Um Dynamic Island moderno e multifuncional para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Novidades

- **Alertas compactos em uma única linha**: recolhidas, as notificações e lembretes mostram uma única linha (ícone + título + resumo em uma linha) e são truncados quando longos, sem alargar a ilha; os detalhes completos aparecem ao expandir

- **Corrigido texto truncado/deslocado no estado recolhido**: com uma notificação ativa, a altura compacta cresce com o conteúdo e a parte interna se alinha ao topo — sem cortes verticais

- **Ilha se alarga automaticamente para notificações longas**: quando chega uma notificação longa, a ilha expande sua largura automaticamente para que os controles de mídia, o relógio e todo o texto fiquem totalmente dentro, sem cortes na borda

Um Dynamic Island moderno e multifuncional para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Novidades

- **Amortecimento elástico + transições escalonadas**: expandir/recolher agora usa uma mola amortecida no estilo iOS (com um leve repique) e as seções da parte interna entram/saem em cascata com ritmo não linear — mais próximo da Dynamic Island do iOS

- **Animação vinculada ao volume**: quando o volume do sistema muda ou você arrasta o controle deslizante, a ilha mostra brevemente uma animação de "ícone + barra de volume + porcentagem" que se desvanece sozinha após alguns segundos

- **Opacidade inteligente em camadas**: o estado compacto é mais transparente e o expandido mais sólido, com transição suave; evita sobreposição dupla quando a tonalidade da capa está ativa

- **Transição ao trocar de capa**: ao trocar de música, a capa se funde com um leve zoom, complementando o fundo de tonalidade "respirante"

- **Troca de tema suave**: ao alternar entre o tema claro e escuro, as cores de fundo e borda da parte interna interpolam em ~0,3 s em vez de piscar

- **Otimização de animação**: efeitos por quadro (como a onda) atualizam apenas o painel visível, reduzindo o uso de CPU durante a reprodução para uma animação mais fluida
## WinIslands 1.2.0 (Estável)

Um Dynamic Island moderno e multifuncional para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Novidades

- **Troca de fonte da letra com um clique**: alterne instantaneamente entre Automática / LRC local / AMLL TTML / API do Cider / Letras online, e a letra da música atual é recarregada imediatamente
- **Recuperação automática após falhas**: após um encerramento anormal, a próxima inicialização mostra um aviso de recuperação — sem travamentos, telas pretas ou estado perdido
- **Duração de animação ajustável**: novo controle deslizante de duração da animação de expandir/recolher (300–1400ms) para ajustar a velocidade ao seu gosto
## WinIslands 1.1.9 (Estável)

Um Dynamic Island moderno e multifuncional para Windows. A modern, multi-functional Dynamic Island widget for Windows.

### Novidades

- **Animações mais suaves e coerentes**: expandir/recolher e a rolagem automática da letra agora usam interpolação baseada em tempo (independente da taxa de quadros), idêntica em telas de 60fps e de alta taxa (120Hz) — ações rápidas e consecutivas não engasgam mais

- **Menos uso de CPU durante a reprodução**: a interpolação do progresso é reportada a 5Hz enquanto o karaokê palavra por palavra avança continuamente a 60fps pelo relógio interno do controle — mesma aparência, menos consumo em segundo plano

- **Cache de decodificação de capas**: cada capa é decodificada uma única vez e reutilizada (as 24 mais recentes são removidas automaticamente), eliminando I/O repetido de disco e oscilação de memória das atualizações por segundo da sessão de mídia

- **Melhorias na rolagem da letra**: rola apenas quando expandida e visível, com posicionamento preciso ao se aproximar do alvo — sem tremores em mudanças rápidas de linha

## WinIslands 1.1.8 (Estável)

Um widget moderno e multifuncional de Dynamic Island para Windows.

### Novidades

- **Animações mais rápidas e ágeis**: expandir/contrair foi reduzido de ~1,1 s para ~0,7 s; a mola está mais firme com um assentamento mais nítido — sem movimento lento ou mole

- **Aceleração global do movimento**: transições de tamanho dos componentes, fades escalonados de conteúdo, reposicionamento (~430 ms → 320 ms), pushes na ilha e ocultação por fade ficam ~30–35% mais rápidos

- **Todos os estilos de animação acelerados**: iOS Spring / Soft / Elastic / Fade compartilham as mesmas durações básicas reduzidas, mantendo o seu caráter, mas com sensação mais ágil


## WinIslands 1.1.7 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- **Estação de transferência de arquivos**: solte qualquer arquivo diretamente na ilha e ele se torna instantaneamente um item de arquivo arrastável (somente referência de caminho, sem copiar o arquivo); arraste-o para o Explorador de Arquivos ou qualquer outro aplicativo para movê-lo rapidamente

- **Limpeza ao sair do reprodutor**: quando um aplicativo de mídia fecha e não há nova sessão de reprodução, a ilha limpa imediatamente as informações antigas da música

- **Animações mais suaves**: o reposicionamento e a rolagem automática da letra usam uma suavização mais suave (reposicionamento ~430 ms) para um movimento mais contínuo

- **Versão na página Sobre**: Configurações → Sobre agora mostra a versão completa (ex.: WinIslands 1.1.7)

- **Estabilidade e desempenho**: detecção aprimorada do estado da sessão de mídia (falha ao ler o estado de reprodução é tratada como fim da mídia); proteção contra vibração concorrente na animação de posição; build autônomo de arquivo único menor

## WinIslands 1.1.6 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- **Ocultar automaticamente na tela de bloqueio**: a ilha se oculta automaticamente ao pressionar Win+L ou desconectar do desktop remoto e é restaurada após desbloquear (Configurações → Geral, ativável)

- **Não perturbe programado em nível de minutos**: os horários de início/fim do Não perturbe podem ser definidos com precisão de minuto

- **Ação rápida de clique do meio**: clicar no meio da ilha executa uma ação rápida personalizada (configurável em Configurações → Geral)

- **Troca programada de tema claro/escuro**: defina os horários de início/fim do período escuro e o tema alterna automaticamente (somente com Theme=Auto)

- **Campos de entrada na ilha**: envios de terceiros podem incluir um campo de entrada; quando o usuário digita na ilha, o valor é devolvido ao remetente via WebSocket

- **Sondagem em segundo plano sob demanda**: a sondagem para quando nenhum widget/recurso relacionado está ativado, reduzindo a CPU em repouso

- Corrigido o envio dos campos de entrada da ilha; corrigida a perda de dados dos botões de envio (incluindo o valor digitado) nos retornos de chamada


## WinIslands 1.1.5 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

**Letras**

- **Letras AMLL palavra por palavra (karaokê de verdade)**: integra a biblioteca TTML estilo Apple Music do amll.dev; Configurações → Letras, botão “Letras AMLL palavra por palavra” (ativo por padrão); prioridade de fontes: LRC local → AMLL TTML → Cider → letras on-line
- **Mecanismo de destaque de palavras reescrito**: avanço contínuo de 60 fps por relógio de parede + easing não linear; fallback de linha inteira; letras bilíngues combinadas por linha do tempo
- **Destaque estável em pausa**: congela no momento da pausa; sem saltos ao sair e reiniciar; apenas a linha ativa dirige a animação, CPU quase 0 em repouso

**Correções**

- Espaçamento das letras no estado compacto corrigido: distância fixa entre as letras e os botões à direita; a largura da ilha cresce automaticamente quando falta espaço (720→800)
- Estabilidade: tempo limite AMLL de 5 s + fallback elegante; recalibração da referência de relógio quando os controles voltam a ficar visíveis


## WinIslands 1.1.4 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- **Botões de ação rápida**: nova fileira de botões rápidos personalizáveis na parte inferior do cartão expandido (bloquear tela / silenciar / reproduzir-pausar / captura / mostrar área de trabalho / gerenciador de tarefas / calculadora / suspender / volume±); Configurações → Ações rápidas permite marcar e ordenar com ↑↓, efeito imediato

- **Alertas de chamada recebida**: detecta janelas de chamada de voz/vídeo do WeChat / QQ e mostra um alerta (distingue “chamada recebida” de “em chamada”); Configurações → Notificações ativa e permite personalizar os apps detectados; detecção local, sem envio de dados

- **Ação de comando na ilha**: botões de envio de terceiros suportam `action: "command"` para executar uma linha de comando localmente (API somente de loopback, Token configurável)

- **Tema do cartão da ilha**: envios de terceiros podem incluir `theme: dark / light / auto`; os cartões alternam automaticamente entre estilos de vidro claro/escuro

- **Animações mais rápidas**: duração total das quatro skins de movimento reduzida em ~20%, mantendo o easing de mola iOS e 60 fps suaves sem frames perdidos

- **Otimização de uso em segundo plano**: os indicadores de teclado são sondados apenas quando o widget está ativado, o monitor de tela cheia tem frequência reduzida e a onda de áudio é reduzida em repouso

- **Correções de estabilidade**: corrigido o aviso falso “Copiado” ao iniciar com conteúdo já na área de transferência (linha de base de inicialização); corrigido o escape de títulos .ics do calendário (\, \; \n) exibidos como barras invertidas

- **Desempenho e estabilidade**: menos ruído nos logs de Bluetooth / SMTC / clima, backoff exponencial para limite de frequência do clima; build passa, todos os 104 testes unitários e de integração passam


## WinIslands 1.1.3 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- **Troca entre vários players**: com vários players abertos, o cartão expandido troca a fonte de mídia controlada com um clique (ícone de nota + nome da fonte + seta do menu); não fica mais limitado ao último player iniciado

- **Capa imersiva**: clicar na capa / imagem grande do cartão expandido abre uma prévia em tela cheia; clique / Esc / clique direito fecha com fade

- **Ajuste fino do tempo das letras**: botões +0,5s / -0,5s na área de letras do cartão expandido; o deslocamento por música é lembrado para alinhar perfeitamente as letras

- **Letras da área de trabalho aprimoradas**: a janela de letras independente suporta ajuste de opacidade (padrão 0,85) e o botão “Bloquear” (bloqueada = clique através, não arrastável)

- **Tema dinâmico respirando**: com o fundo de cor da capa ativado, o fundo do cartão expandido “respira” lentamente com a cor da capa (ciclo de cerca de 18 s) em vez de um bloco plano estático

- **Clique na hora**: pressionar o mouse alterna imediatamente expandir/recolher sem esperar soltar, mais responsivo

- **Botões de ação em notificações**: banners de notificação suportam botões de ação (o alerta de conexão Bluetooth agora tem botões “Desconectar” e “Configurações”, executados e recolhidos imediatamente)

- **Retorno de chamada dos botões da ilha**: quando um botão de ação notify de um envio de terceiros é clicado, o evento push_button (com push_id e o texto do botão) é transmitido ao remetente via WebSocket para ele tratar

- **Desempenho e estabilidade**: o tema dinâmico assina frames de composição sob demanda (0 CPU em repouso), pincéis de gradiente em cache para reduzir a pressão do GC; corrigido o conflito de mutex que impedia a inicialização quando versões novas e antigas rodam ao mesmo tempo


## WinIslands 1.1.1 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- **Alertas de bateria**: alerta de bateria fraca (limiar ajustável); alerta “Carga concluída” ao conectar e carregar até o limiar definido (padrão 100%); detecção local, ativáveis

- **Alertas de rede**: alerta na perda / restauração da rede (detecção local de rede, ativável)

- **Novos widgets**: espaço livre no disco (unidade do sistema), estado do método de entrada (PT / EN + nome do IME)

- **Calendário lunar e termos solares**: o widget de data pode mostrar também a data lunar e os termos solares (ativo por padrão, desativável nas configurações)

- **Widgets de alternância rápida**: ligar/desligar WiFi / Bluetooth / modo noturno / silencioso com um clique (APIs locais, sem rede; estado do Radio em cache por 2 s para evitar custo)

- **Distintivo da fonte de reprodução**: o widget de mídia mostra a fonte atual (Spotify / Cider / NetEase Cloud / QQ Music etc.), para saber de qual player vem de relance

- **Letras aprimoradas**: botão mostrar/ocultar tradução das letras; botão “Copiar linha atual” com um clique

- **Ícones de widgets personalizados**: cada widget pode ter seu próprio ícone (MDL2 ou Emoji); glifo padrão se não definido

- **Corrigido o salto de escala das letras**: removida a vibração de “crescer e encolher” causada pelo deslocamento de rebote das palavras do karaokê; o tamanho/opacidade da linha atual no estado expandido transita suavemente em 300 ms para rolagem mais fluida


## WinIslands 1.1.0 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- **Volume / silencioso temporário na ilha**: ao mudar o volume do sistema ou o silencioso, a ilha mostra brevemente um indicador de volume (duração ajustável, ativável nas configurações)

- **Cópia / movimentação de arquivos na ilha**: quando o Explorer copia/move arquivos, a ilha mostra “Copiando arquivos…” (detecção local por título de janela, ativável)

- **Progresso de download na ilha**: detecta arquivos temporários do navegador na pasta de downloads (.crdownload / .part / .download etc.) e mostra “Baixando N arquivo(s)” (desativado por padrão, ativável nas configurações)

- **Cápsula combinada “Em uso”**: Configurações → Widgets permite ativar (desativado por padrão) a combinação de “Microfone / Câmera / Em reunião / Gravando” em uma única cápsula “Em uso · …”; escolha quais widgets participam; os combinados não aparecem mais separadamente

- **Pomodoro aprimorado**: clicar no widget Pomodoro da ilha pausa / retoma o temporizador

- **Captura / gravação temporária na ilha**: ao capturar a tela ou iniciar uma gravação, a ilha mostra temporariamente o indicador correspondente (funciona mesmo com a ilha oculta)

- **Iluminação de palavras do karaokê com rebote**: cada linha acende suavemente desde a primeira palavra com um leve rebote, mais fluido e natural

- Arquitetura de sondagem interna otimizada: eventos temporários da ilha, como volume / cópia / download / captura, podem ser acionados enquanto ela está oculta


## WinIslands 1.0.9 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- **Novos widgets**: uso de GPU, microfone / câmera em uso, contagem regressiva de feriados, em reunião; o widget de rede pode mostrar uma mini-curva de 32 segundos

- **Ação rápida de duplo clique**: Configurações → Geral permite definir o duplo clique como “Reproduzir / Pausar”, “Abrir configurações” ou “Nenhuma”

- **Assistente de silêncio em reuniões**: reconhece janelas de reunião (Teams / Zoom / Tencent Meeting / DingTalk / Feishu / Webex / Slack / Discord / Google Meet) e ativa o Não perturbe automaticamente durante reuniões (heurística puramente local)

- **Alertas de gravação / captura de tela**: aviso de captura PrintScreen + detecção de software de gravação (OBS / Bandicam / Xbox Game Bar etc.)

- **Lembretes de eventos do calendário (.ics)**: analisa arquivos iCalendar locais; banner quando um evento chega (com antecedência opcional de N minutos); totalmente local

- **Alertas de assinaturas RSS**: sonda RSS 2.0 / Atom, banner para novas entradas

- **Alertas de e-mail (POP3)**: lê apenas cabeçalhos, banner para e-mails novos; recomenda-se código de autorização

- **Iniciador rápido (estilo Spotlight)**: `Ctrl+Espaço` pesquisa aplicativos / abre URLs

- **Painel do histórico da área de transferência**: `Ctrl+Alt+V` abre uma janela independente; clique para copiar de volta

- **Regras (automação)**: condições (sempre / sem reprodução / reproduzindo / intervalo de tempo / app de mídia específico) × ações (ocultar / forçar recolher / forçar mostrar)

- **Island API v3**: imagens (data URI / http), progresso animado (from/to/duration automático), renovação por heartbeat (heartbeat_seconds), atualizações parciais PATCH, canal WebSocket (/v3/ws)

- **Aparência**: 18 temas predefinidos, cor de fundo personalizada, 4 skins de movimento, modo de baixo consumo

- A página de configurações foi reformulada no estilo Configurações do sistema macOS (navegação à esquerda + conteúdo à direita); todas as mudanças valem imediatamente

- **Texto preto no modo escuro corrigido**: vinculação unificada da cor do primeiro plano em todos os modelos de controles personalizados das configurações (botões / caixas / campos / menus suspensos / itens de menu / guias / navegação à esquerda etc.) mais uma varredura de fallback em tempo de execução — o modo escuro não mostra mais opções individuais (idioma da interface, menus suspensos de ação de duplo clique etc.) com texto preto ilegível; o modo claro restaura automaticamente o texto escuro

- **Abertura das configurações corrigida**: removidas as linhas duplicadas de cor de primeiro plano XAML que causavam falha de carregamento BAML

- **Otimização do desempenho de animação**: o destaque de palavras do karaokê reutiliza objetos Run (eliminando o layout por frame), storyboards estáveis a 60 fps, atualização de logs em lote — animações mais suaves

- **Pontos vermelhos removidos dos widgets** (a pedido do usuário)


## WinIslands 1.0.8 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

**Reconstrução completa da interface de configurações**

- Layout estilo Configurações do sistema macOS: lista de navegação à esquerda + área de conteúdo à direita, 13 categorias (Geral / Aparência / Widgets / Mídia / Exibição de informações de mídia / Letras / Cider / Island API / Produtividade / Atualização / Sobre / Notificações / Regras)
- As cores do texto das configurações se adaptam automaticamente ao modo escuro/claro: branco no escuro, preto no claro; sem texto ilegível
- O texto da navegação à esquerda ficou branco de alto contraste, a linha divisória branca à direita foi removida e o feedback de hover e seleção foi reforçado

**Reprodução de mídia**

- Novo mini player: janela flutuante independente com capa / título / artista / barra de progresso e controles de reprodução; arrastável com posição memorizada; mostra/oculta automaticamente com a reprodução (ativável nas configurações)
- Nova troca de dispositivo de saída de áudio: Configurações → Mídia pode enumerar e trocar o dispositivo de reprodução padrão do sistema (reiniciar o player é recomendado após a troca)
- Backend de fontes de players aprimorado: enumera todas as sessões SMTC e do Cider; pode trocar a fonte de mídia

**Letras**

- Novas letras bilíngues: combina automaticamente as linhas de tradução adjacentes por carimbo de tempo (desativável nas configurações)

**Aparência e movimento**

- Novas skins de movimento: 4 estilos de animação (mola iOS (padrão) / mola suave / rebote elástico / fade simples), easing não linear para expandir/recolher
- Novo modo de baixo consumo: reduz a taxa de renderização da onda e simplifica as animações em repouso para economizar energia

**Atalhos globais**

- 5 combinações de teclas personalizáveis: mostrar/ocultar, reproduzir/pausar, anterior, próxima, expandir/recolher
- Suporta Ctrl / Alt / Shift / Win + letras, dígitos, F1–F24, teclas de seta

**Mecanismo de regras inteligente (Configurações → Regras)**

- Controla automaticamente a visibilidade da ilha por condição: sempre / sem reprodução de mídia / com reprodução / intervalo de tempo específico / app de mídia específico reproduzindo
- Ações: ocultar ilha / forçar recolher / forçar mostrar; prioridade: ocultar > recolher > forçar mostrar

**Notificações**

- Histórico de notificações: marcador de ponto vermelho não lido, marcar tudo como lido, excluir um item, clicar para abrir o app de origem, limpar histórico
- Novo agrupamento de notificações: notificações repetidas da mesma fonte e título reutilizam um banner e acumulam o contador
- Nova lista de permissões do Não perturbe: fontes na lista (nomes de exe separados por vírgulas) não são afetadas pelo Não perturbe e continuam mostrando banners normalmente
- Removido o distintivo de ponto vermelho não lido da ilha

**Produtividade**

- Aviso “Copiado” ao copiar texto
- Detecta automaticamente códigos de verificação por SMS e os destaca
- Copiar textos grandes mostra uma animação de progresso (estimada pelo comprimento, mostra o resultado ao final)

**Widgets**

- Novo widget de contagem regressiva de feriados: tabela de feriados 2026–2027 embutida (Ano Novo / Festival da Primavera / Qingming / Dia do Trabalho / Festival do Barco-Dragão / Festival do Meio do Outono / Dia Nacional), mostrando “XX em N dias” ou “Hoje XX”; ativável nas configurações de widgets

**Island API v2**

- Novos campos: subtitle (subtítulo), type (info / success / warning / error), priority (high / normal / low), accent (cor de destaque personalizada), click (retorno de chamada de clique no cartão inteiro)
- Fila de envios: vários envios ordenados por prioridade alta → baixa, primeiro a entrar, primeiro a sair; reenviar o mesmo id mantém a posição e a expiração originais
- A resposta POST agora inclui um campo position
- Novos scripts de exemplo prontos para executar em docs/sdk-examples/ (push.bat / pull.bat / push.ps1 / push.py / pull.py)

**Correções**

- Corrigida a impossibilidade de abrir a janela de configurações / o duplo clique não funcionar (proteções de valores nulos e inicialização explícita na navegação e na troca de guias)
- Adicionados 69 testes automatizados (Island API, agrupamento de notificações e lista de permissões, mecanismo de regras, reconhecimento de códigos de verificação, análise LRC), todos passando

**Ativos**

- Versões portáteis Windows x64 / arm64 (arquivo único autossuficiente, sem instalação, execução direta)
- Instalador universal Windows (Inno Setup, suporta x64 e ARM64, instala pela arquitetura)
- > As versões portáteis são arquivos exe independentes; arquivos ZIP não são mais fornecidos.


## WinIslands 1.0.7 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- A “onda de som” foi atualizada para **seguir o ritmo da música**: captura em tempo real do áudio real do sistema via loopback WASAPI; ondas altas em batidas fortes, baixas no silêncio — não é mais uma barra de volume fixa

- **Renderização contínua a 60 fps**: suavização exponencial com ataque de 25 ms / liberação de 140 ms; o movimento da onda é contínuo, sem rigidez ou cortes

- Novas configurações (Configurações → Aparência → Onda de som): botão seguir ritmo da música, sensibilidade 0,2–3,0, altura da onda 0,4–1,6; mudanças imediatas

- Sem dispositivo de áudio / serviço de áudio anormal, cai suavemente para simulação de batida e tenta restaurar a captura ao vivo a cada 8 segundos — sem travas ou acúmulo de threads


## WinIslands 1.0.6 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- Adicionados 6 widgets de ilha: volume, indicadores de teclado (CapsLock), área de transferência, tarefas, Pomodoro, agenda — todos com caixas de duas colunas “sem música / com música” e reordenação por arrastar

- Nova página de configurações “Produtividade”: histórico da área de transferência, temporizador Pomodoro, lista de tarefas, lembretes de agenda

- Nova “onda de som”: durante a reprodução de mídia, a área à esquerda dos botões de controle pulsa com o volume do sistema em tempo real (ativável em Configurações → Aparência)

- Adicionados 7 temas predefinidos: Padrão / Oceano / Floresta / Pôr do sol / Neon / Monocromático / Uva

- Personalização de aparência: fonte personalizada, escala de tamanho da fonte (0,8–1,4), raio dos cantos da cápsula (16–40), fundo expandido tingido pela capa, distintivo de notificações não lidas

- Novidades no menu da bandeja: modo Não perturbe (manual / notificações silenciosas programadas), verificar atualizações, ver logs

- Página de configurações modernizada (cantos arredondados + vidro líquido); mudanças valem imediatamente, sem salvar manualmente

- Nova verificação de atualizações (manual pela bandeja / configurações, verificação automática opcional, desativada por padrão)

- Ativos: versões portáteis Windows x64 / arm64 (arquivo único autossuficiente, sem instalação, execução direta); instalador universal Windows (Inno Setup, suporta x64 e ARM64, instala pela arquitetura)

- > As versões portáteis são arquivos exe independentes; arquivos ZIP não são mais fornecidos.


## WinIslands 1.0.5 (Estável)

Um widget Dynamic Island moderno e multifuncional para Windows.

### Registro de alterações

- Nova “Island API”: outros softwares podem enviar informações à ilha via API HTTP local (como a integração de apps de terceiros com a ilha do iOS); **documentação para desenvolvedores em docs/IslandAPI.md**

-   - `POST /v1/island/push` enviar/atualizar · `DELETE /v1/island/push/{id}` remover · `GET /v1/island/active` consultar · `GET /v1/health`

-   - Suporta ícone, título, corpo, progresso, botões (abrir link / iniciar programa), duração de exibição personalizada por item

-   - A página de configurações oferece ativar / porta / Token opcional / duração padrão global

- Os cartões enviados aparecem em uma linha no estado compacto, não cobrem outros widgets e **não afetam a largura nem a altura da ilha** (tamanhos automático/manual constantes)

- “Ajuste automático” do tamanho: adapta-se ao conteúdo; arrastar manualmente um controle deslizante desativa automaticamente a opção automática correspondente

- O conteúdo expandido suporta rolagem com a roda do mouse (barra oculta)

- Alinhamento superior/inferior dos widgets unificado; correções de layout/fonte na inicialização (PerMonitorV2 forçado, tamanho correto desde o início)

- Não mostra mais o aviso “Tocando agora” ao reproduzir mídia

- Corrigido: após expandir a ilha (cerca de 1–2 s), o cartão voltando ao tamanho compacto causava tela preta total

-   - O conteúdo expandido agora faz cross-fade sobreposto à linha compacta; nenhum fundo transparece durante as animações

-   - O tamanho final do cartão é gravado explicitamente após as animações de expandir/recolher; o estado expandido fica estável e não encolhe

-   - Também corrigida a tela preta ao clicar em botões de terceiros na ilha no estado expandido

- Ativos: versões portáteis Windows x64 / arm64 (arquivo único autossuficiente, sem instalação, execução direta); instalador universal Windows (Inno Setup, suporta x64 e ARM64, instala pela arquitetura)

- > As versões portáteis são arquivos exe independentes; arquivos ZIP não são mais fornecidos.
