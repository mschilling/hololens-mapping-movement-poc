@echo off

echo Starting Holographic Emulator

SETLOCAL
if (%__VERSION%)==() set __VERSION=14393.0
if (%__XDEVER%)==() set __XDEVER=14393.0
"C:\Program Files (x86)\Microsoft XDE\10.0.%__XDEVER%\XDE.exe" /name "HoloLens Emulator 10.0.%__VERSION%.%USERNAME%" /displayName "HoloLens Emulator 10.0.%__VERSION%" /vhd "C:\Program Files (x86)\Windows Kits\10\Emulation\HoloLens\10.0.%__VERSION%\flash.vhd" /video "1268x720" /memsize 1024 /language 409 /creatediffdisk "%LOCALAPPDATA%\Microsoft\XDE\10.0.%__VERSION%\dd.1268x720.2048.vhd" /fastShutdown /sku HDE