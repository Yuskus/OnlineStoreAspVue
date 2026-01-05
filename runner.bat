@echo off

setlocal enabledelayedexpansion

set "directory=%CD%"
set "backend=OnlineStore.Server"

docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo Docker desktop is not running. Please start it and try again.
    exit /b 1
)

if not exist "%directory%\%backend%" (
    echo Backend not found.
    exit /b 1
)

cd %backend%

docker-compose down
docker-compose build
docker-compose up -d

endlocal