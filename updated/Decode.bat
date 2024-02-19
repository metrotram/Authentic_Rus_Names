@echo off
setlocal

REM Pfade setzen
set "programm=CS2 Translator 1.0.exe"
set "ordner=%~dp0"

REM Prüfen, ob die Datei existiert
if not exist "%ordner%%programm%" (
    echo Die Datei "%programm%" wurde nicht gefunden.
    exit /b 1
)

REM Schleife für alle .loc-Dateien im Ordner
for %%f in ("%ordner%*.loc") do (
    REM Ausführen des Programms mit dem Parameter
    start "" "%ordner%%programm%" "%%~nxf"
)

endlocal
