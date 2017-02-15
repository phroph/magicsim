reg Query "HKLM\Hardware\Description\System\CentralProcessor\0" | find /i "x86" > NUL && set OS=32BIT || set OS=64BIT
if %OS%==32BIT .\nodeexe.exe %*
if %OS%==64BIT .\nodeexe64.exe %*

