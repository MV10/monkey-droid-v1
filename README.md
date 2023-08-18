# monkey-droid

A simple remote-control UI for the [monkey-hi-hat](https://github.com/MV10/monkey-hi-hat) audio visualization app.

Despite the name (we run it on our Android phones), the program is cross-platform -- it should work on Windows 10 1809 or newer, Windows 11, Android, and possibly MacOS / iOS (but I do not use, support, test or even _like_ Apple devices).

Note that this communicates with monkey-hi-hat over the local network, so you must set the `UnsecuredPort` option in the monkey-hi-hat `mhh.conf` configuration file to tell it to listen for commands via TCP.

The program uses the .NET MAUI framework, which currently forces me to use .NET7, which unfortunately is a short-term release. At a minimum, expect an update when .NET8 is released in November 2023. MAUI support has a different [lifecycle](https://dotnet.microsoft.com/en-us/platform/support/policy/maui) policy than .NET itself, thanks to dependencies on Android and Apple libraries.

Stay tuned for details, this is a work-in-progress.
