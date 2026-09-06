# ============================================================
#  WinIslands SDK 示例：推送一张卡片（PowerShell）
#  用法：.\push.ps1 [-Port 9840] [-Token "xxx"] [-Id "my-id"]
# ============================================================
param(
    [int]$Port = 9840,
    [string]$Token = "",
    [string]$Id = ""
)
$ErrorActionPreference = "Stop"

if (-not $Id) { $Id = "demo-" + [Guid]::NewGuid().ToString("N").Substring(0, 8) }

$body = @{
    id               = $Id
    title            = "来自 WinIslands SDK 的消息"
    subtitle         = "push.ps1 示例"
    body             = "这是一张由 PowerShell 脚本推送的上岛卡片，支持进度与按钮。"
    icon             = "\uE7F4"
    type             = "info"
    priority         = "high"
    duration_seconds = 30
    progress         = 0.6
    buttons          = @(
        @{ label = "打开链接"; action = "url";    value = "https://github.com" }
    )
    click            = @{ label = "回跳"; action = "url"; value = "https://github.com" }
} | ConvertTo-Json -Depth 6

$uri = "http://127.0.0.1:$Port/v1/island/push"
$headers = @{}
if ($Token) { $headers["X-WinIslands-Token"] = $Token }

$resp = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $body -ContentType "application/json"
Write-Host ("推送成功: id={0} position={1}" -f $resp.id, $resp.position)
Write-Host "提示: 用 .\pull.ps1 查询，或用以下命令移除："
Write-Host ("Invoke-RestMethod -Uri 'http://127.0.0.1:{0}/v1/island/push/{1}' -Method Delete" -f $Port, $resp.id)
