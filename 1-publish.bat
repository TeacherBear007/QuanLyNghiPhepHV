@echo off
rem Build ban Release vao C:\nghiphep\publish. Tat web (2-chay-web.bat) truoc khi chay file nay.
cd /d "%~dp0"
set OUT=C:\nghiphep\publish
echo Dang build vao %OUT% ...
dotnet publish QuanLyNghiPhepHV.csproj -c Release -o "%OUT%"
if errorlevel 1 (
  echo.
  echo LOI: build that bai. Xem thong bao o tren.
  pause
  exit /b 1
)
echo.
echo Xong. Hay chay file 2-chay-web.bat de mo web.
pause
