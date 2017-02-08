reg Query "HKLM\Hardware\Description\System\CentralProcessor\0" | find /i "x86" > NUL && set OS=32BIT || set OS=64BIT
if %OS%==32BIT .\npm.cmd install & .\nodeexe.exe %*
if %OS%==64BIT .\npm64.cmd install & .\nodeexe64.exe %*

