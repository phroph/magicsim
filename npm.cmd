@IF EXIST "%~dp0\nodeexe.exe" (
  "%~dp0\nodeexe.exe"  "%~dp0\required_node_modules\npm\bin\npm-cli.js" %*
) ELSE (
  @SETLOCAL
  @SET PATHEXT=%PATHEXT:;.JS;=;%
  node  "%~dp0\required_node_modules\npm\bin\npm-cli.js" %*
)