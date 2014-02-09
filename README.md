HawkenExhaust
=============

**HawkenExhaust is no longer actively developed due to Hawken having Steamworks integration. Feel free to use the source as SimpleGameReLauncher implementation example.** 


HawkenExhaust is a simple utility to enable use of the Steam Overlay in [Hawken](http://playhawken.com) without having to sacrifice use of the Hawken Launcher.

Usage
-----
Add HawkenExhaust.exe as an Non-Steam Game. The Hawken Launcher will start. When the Hawken game starts, the Steam Overlay will be enabled. Note, Hawken will take about 2 seconds longer to start than normal. This is because HawkenExhaust closes and relaunches HawkenGame-Win32-Shipping.exe as a child process so that Steam can apply it's overlay. For some reason, HawkenLauncher.exe does not launch HawkenGame-Win32-Shipping.exe as a child process, so this does that for it, enabling the Steam Overlay.

License
-------
HawkenExhaust is licensed under GNU GPL v3.
