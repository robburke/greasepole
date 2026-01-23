@echo off
REM Post-export script: Ensures canvas has fixed 640x480 dimensions
REM Run this after exporting to web, or integrate into your build process

set "HTML_FILE=%~dp0..\builds\web\index.html"

if not exist "%HTML_FILE%" (
    echo ERROR: index.html not found at %HTML_FILE%
    exit /b 1
)

REM Use PowerShell to do the replacement
powershell -Command "(Get-Content '%HTML_FILE%') -replace '<canvas id=\"canvas\">', '<canvas id=\"canvas\" width=\"640\" height=\"480\">' | Set-Content '%HTML_FILE%'"

echo Fixed canvas dimensions in index.html
