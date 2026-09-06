@echo off
REM ============================================================
REM  WinIslands SDK example: query active pushes (curl)
REM  Usage: pull.bat [port] [token] [id]
REM    no id  -> GET /v1/island/active  (list active cards)
REM    id     -> DELETE /v1/island/push/{id}
REM ============================================================
setlocal
set PORT=%1
if "%PORT%"=="" set PORT=9840
set TOKEN=%2
set ID=%3

if "%ID%"=="" (
  echo GET http://127.0.0.1:%PORT%/v1/island/active
  if "%TOKEN%"=="" (
    curl -s "http://127.0.0.1:%PORT%/v1/island/active"
  ) else (
    curl -s "http://127.0.0.1:%PORT%/v1/island/active" -H "X-WinIslands-Token: %TOKEN%"
  )
) else (
  echo DELETE http://127.0.0.1:%PORT%/v1/island/push/%ID%
  if "%TOKEN%"=="" (
    curl -s -X DELETE "http://127.0.0.1:%PORT%/v1/island/push/%ID%"
  ) else (
    curl -s -X DELETE "http://127.0.0.1:%PORT%/v1/island/push/%ID%" -H "X-WinIslands-Token: %TOKEN%"
  )
)
echo.
endlocal
