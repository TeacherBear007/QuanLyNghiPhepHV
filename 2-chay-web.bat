@echo off
rem Chay web o cong 7187 (http). Cloudflare Tunnel tro vao dia chi http://localhost:7187
set OUT=C:\nghiphep\publish
if not exist "%OUT%\QuanLyNghiPhepHV.dll" (
  echo Chua co ban publish. Hay chay 1-publish.bat truoc.
  pause
  exit /b 1
)
cd /d "%OUT%"
set ASPNETCORE_ENVIRONMENT=Production
set ASPNETCORE_URLS=http://localhost:7187
set DataProtection__KeyPath=C:\nghiphep\keys
set AllowedHosts=nghiphep.hoanganhplus01.io.vn;localhost
echo Web dang chay tai http://localhost:7187  (giu cua so nay mo)
dotnet QuanLyNghiPhepHV.dll
pause
