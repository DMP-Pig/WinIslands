# -*- coding: utf-8 -*-
"""WinIslands SDK 示例：推送一张卡片（Python 标准库实现，无需安装 requests）。

用法:
    python push.py [port] [token] [id]

依赖仅在 Windows 自带环境即可运行；也可把 json 改为 requests 版本。
"""
import json
import sys
import urllib.request

HOST = "127.0.0.1"
PORT = int(sys.argv[1]) if len(sys.argv) > 1 else 9840
TOKEN = sys.argv[2] if len(sys.argv) > 2 else ""
PUSH_ID = sys.argv[3] if len(sys.argv) > 3 else "demo-" + __import__("uuid").uuid4().hex[:8]

payload = {
    "id": PUSH_ID,
    "title": "来自 WinIslands SDK 的消息",
    "subtitle": "push.py 示例",
    "body": "这是一张由 Python 脚本推送的上岛卡片，支持进度、按钮与整卡回跳。",
    "icon": "\ue7f4",
    "type": "info",
    "priority": "high",
    "duration_seconds": 30,
    "progress": 0.6,
    "buttons": [
        {"label": "打开链接", "action": "url", "value": "https://github.com"},
    ],
    "click": {"label": "回跳", "action": "url", "value": "https://github.com"},
}

req = urllib.request.Request(
    "http://%s:%d/v1/island/push" % (HOST, PORT),
    data=json.dumps(payload).encode("utf-8"),
    headers={"Content-Type": "application/json"},
    method="POST",
)
if TOKEN:
    req.add_header("X-WinIslands-Token", TOKEN)

with urllib.request.urlopen(req, timeout=5) as resp:
    info = json.loads(resp.read().decode("utf-8"))

print("推送成功: id=%s position=%s" % (info.get("id"), info.get("position")))
print("移除: DELETE http://%s:%d/v1/island/push/%s" % (HOST, PORT, info.get("id")))
