# monkey-droid <img src="https://github.com/MV10/volts-laboratory/blob/master/misc/mhh-icon.png" height="32px"/>

A simple remote-control UI for the [monkey-hi-hat](https://github.com/MV10/monkey-hi-hat) audio visualization app.

> The Version 1.x repository has been archived. Version 2.x is a complete from-scratch rewrite. 

Version 1.0.1 has been released! Install from the monkey-hi-hat [release](https://github.com/MV10/monkey-hi-hat/releases) page. Requires version 1.1.0 or newer of monkey-hi-hat.

Note that this communicates with monkey-hi-hat over the local network, so you must set the `UnsecuredPort` option in the monkey-hi-hat `mhh.conf` configuration file to tell it to listen for commands via TCP.

Despite the name (we run it on our Android phones), the program is cross-platform -- it should work on Windows 10 1809 or newer, Windows 11, Android (supposedly v5 or newer, though I can only test against v13), and possibly MacOS / iOS (but I do not use, support, test on, build for, or even _like_ Apple devices).

The program uses the .NET MAUI framework, which currently forces me to use .NET7, which unfortunately is a short-term release. At a minimum, expect an update when .NET8 is released in November 2023. MAUI support has a different [lifecycle](https://dotnet.microsoft.com/en-us/platform/support/policy/maui) policy than .NET itself, thanks to dependencies on Android and Apple libraries.

### Known issues:
* "Busy spinner" doesn't block access to tabs / toolbar
* Font glyphs clipped on Android ([reported](https://github.com/dotnet/maui/issues/16880))
* Font glyph poor quality on Windows ([reported](https://github.com/dotnet/maui/issues/6043#issuecomment-1685032632))
* Android convenience: Make Util page auto-prefix command with "--"
