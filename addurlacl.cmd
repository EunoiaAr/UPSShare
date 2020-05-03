@ECHO off
REM see: https://stackoverflow.com/a/4115328/41236

if '%1'=='' GOTO Error
if '%2'=='' GOTO Error

netsh http add urlacl url=%1 user=%2
GOTO End

:Error
ECHO USAGE: addurlacl ^<urltoadd^> ^<username^>
ECHO    i.e: addurlacl http://+:80/MyUri DOMAIN\user
ECHO.
ECHO    where: urltoadd = http://+:80/MyUri
ECHO      and: username = DOMAIN\user
:End
