@echo off

cd D:\Code\ClozerWoods

dotnet publish -p:PublishProfile=FolderProfile -c:Release

pushd D:\Code\ClozerWoods\ClozerWoods\bin\Release\net6.0\linux-x64\publish

call "C:\Program Files (x86)\WinSCP\WinSCP.com" /ini=nul /script=D:\Code\ClozerWoods\deploy.winscp.txt

popd

echo.
echo Done.