@echo off

REM TODO: Maybe change this to a Powershell script so we can accept the
REM       password as an argument where the typed characters are obscured.

cd D:\Code\ClozerWoods

dotnet publish -p:PublishProfile=FolderProfile -c:Release

pushd D:\Code\ClozerWoods\ClozerWoods\bin\Release\net6.0\linux-x64\publish

call "C:\Program Files (x86)\WinSCP\WinSCP.com" /ini=nul /script=D:\Code\ClozerWoods\deploy.winscp.txt

popd

putty.exe root@45.76.24.90 -i "C:\Users\micha\.ssh\clozerwoods.ppk" -m "deploy.putty.txt"

echo.
echo Done.