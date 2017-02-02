@IF EXIST "%~dp0\nodeexe.exe" (
  "%~dp0\nodeexe.exe"  "%~dp0\bnode_modules\npm\bin\npm-cli.js" %*
) ELSE (
  @SETLOCAL
  @SET PATHEXT=%PATHEXT:;.JS;=;%
  node  "%~dp0\bmodules\npm\bin\npm-cli.js" %*
)