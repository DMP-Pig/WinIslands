# -*- coding: utf-8 -*-
"""WinIslands SDK 示例：查询当前活跃卡片（Python 标准库）。

用法:
    python pull.py [port] [token]
"""
import json
import sys
import urllib.request

HOST = "127.0.0.1"
PORT = int(sys.argv[1]) if len(sys.argv) > 1 else 9840
TOKEN = sys.argv[2] if len(sys.argv) > 2 else ""

req = urllib.request.Request(
    "http://%s:%d/v1/island/active" % (HOST, PORT),
    method="GET",
)
if TOKEN:
    req.add_header("X-WinIslands-Token", TOKEN)

with urllib.request.urlopen(req, timeout=5) as resp:
    cards = json.loads(resp.read().decode("utf-8"))

print("活跃卡片 %d 条:" % len(cards))
for i, c in enumerate(cards, 1):
    print("%d. [%s] %s  (position=%d, expires_at=%s)" % (
        i, c.get("priority", "normal"), c.get("title"), i, c.get("expires_at")))
