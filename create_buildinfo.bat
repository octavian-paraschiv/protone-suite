@echo off
set CUR_YYYY=%date:~10,4%
set CUR_MM=%date:~4,2%
set CUR_DD=%date:~7,2%
set DATE=%CUR_YYYY%-%CUR_MM%-%CUR_DD%
set FILE="ProTONE Suite %APPVEYOR_BUILD_VERSION%.buildinfo.txt"
echo %DATE% > %FILE%

