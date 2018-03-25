@echo off
echo "Win10-x64:"
dotnet publish -c Release -r win-x64
echo "Linux-x64:"
dotnet publish -c Release -r linux-x64
pause
exit