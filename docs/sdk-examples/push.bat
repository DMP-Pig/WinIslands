@echo off
REM ============================================================
REM  WinIslands SDK example: push a card (curl)
REM  Usage: push.bat [port] [token]
REM  Default port: 9840
REM ============================================================
setlocal
set PORT=%1
if "%PORT%"=="" set PORT=9840
set TOKEN=%2

set BODY={"title":"Hello from WinIslands SDK","body":"This card was pushed by push.bat","icon":"\ue7f4","duration_seconds":30,"buttons":[{"label":"Open","action":"url","value":"https://github.com"}]}

echo POST http://127.0.0.1:%PORT%/v1/island/push
if "%TOKEN%"=="" (
  curl -s -X POST "http://127.0.0.1:%PORT%/v1/island/push" -H "Content-Type: application/json" -d "%BODY%"
) else (
  curl -s -X POST "http://127.0.0.1:%PORT%/v1/island/push" -H "Content-Type: application/json" -H "X-WinIslands-Token: %TOKEN%" -d "%BODY%"
)
echo.
endlocal
