# monkey-droid

A simple remote-control UI for the [monkey-hi-hat](https://github.com/MV10/monkey-hi-hat) audio visualization app.

Status: Working, but not released yet.

Despite the name (we run it on our Android phones), the program is cross-platform -- it should work on Windows 10 1809 or newer, Windows 11, Android, and possibly MacOS / iOS (but I do not use, support, test or even _like_ Apple devices).

Note that this communicates with monkey-hi-hat over the local network, so you must set the `UnsecuredPort` option in the monkey-hi-hat `mhh.conf` configuration file to tell it to listen for commands via TCP.

The program uses the .NET MAUI framework, which currently forces me to use .NET7, which unfortunately is a short-term release. At a minimum, expect an update when .NET8 is released in November 2023. MAUI support has a different [lifecycle](https://dotnet.microsoft.com/en-us/platform/support/policy/maui) policy than .NET itself, thanks to dependencies on Android and Apple libraries.

Known issues:
* "Busy spinner" doesn't block access to tabs / toolbar
* About -> Delete cache doesn't clear memory on Android
* Font glyphs clipped on Android ([reported](https://github.com/dotnet/maui/issues/16880))
* Font glyph poor quality on Windows ([reported](https://github.com/dotnet/maui/issues/6043#issuecomment-1685032632))
* Make Util page auto-prefix command with "--" (Android convenience)
* Background updates of visualizer details don't immediately show on-screen
