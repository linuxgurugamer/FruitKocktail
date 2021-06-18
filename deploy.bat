
@echo off

echo  Solution deploy.bat

rem H is the destination game folder
rem GAMEDIR is the name of the mod folder (usually the mod name)
rem GAMEDATA is the name of the local GameData
rem VERSIONFILE is the name of the version file, usually the same as GAMEDATA,
rem    but not always

set H=%KSPDIR%
set GAMEDIR=FruitKocktail
set GAMEDATA=GameData
set VERSIONFILE=%GAMEDIR%.version


copy /Y %1%2.dll "%GAMEDATA%\%GAMEDIR%\%3\Plugins"
copy /Y %1%2.pdb "%GAMEDATA%\%GAMEDIR%\%3\Plugins"


copy /y  %3\%3.version  %GAMEDATA%\%GAMEDIR%\%3
copy /y  %3\README.md %GAMEDATA%\%GAMEDIR%\%3

rem xcopy /y /s /I %GAMEDATA%\%GAMEDIR% "%H%\GameData\%GAMEDIR%"

rem pause