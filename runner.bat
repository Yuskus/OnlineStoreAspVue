@echo off

setlocal enabledelayedexpansion

set "directory=%CD%"
set "backend=OnlineStore.Server"
set "frontend=onlinestore.client"

docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo Docker desktop is not running. Please start it and try again.
    exit /b 1
)

if not exist "%directory%\%backend%" (
    echo Backend not found.
    exit /b 1
)

if not exist "%directory%\%frontend%" (
    echo Frontend not found.
    exit /b 1
)

cd %backend%

dotnet --version >nul 2>&1
if %errorlevel% == 0 (
    dotnet restore
    dotnet build
)

docker-compose down
docker-compose build --no-cache
docker-compose up -d

cd ..

cd %frontend%

set "output="
for /f "delims=" %%i in ('npm install 2^>^&1') do (
    set "output=!output!%%i"
)

echo !output!

docker-compose down
docker-compose build --no-cache
docker-compose up -d

endlocal