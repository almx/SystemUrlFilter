SystemUrlFilter
==============

SystemUrlFilter is a small application that is made to filter out URL system calls on Windows. For example, if a game or application wants to spawn a browser window, which might be a nag, the application can be setup to interact the call and ignore it.

Technical Background
--------------

When a system call with an HTTP or HTTPS URL is made, for example from an application or even via Start->Run, the application for handling the URL is found in the registry. Where depends on the browser you have installed. For example, for Firefox, the executable call is found in this key:

HKEY_CLASSES_ROOT\FirefoxURL\shell\open\command

This should be something like this by default:

"C:\Program Files (x86)\Mozilla Firefox\firefox.exe" -osint -url "%1"

The parameter %1 is the URL.

Examples of calls that go through this path when spawning an URL include Excel, Adobe Flash updater, EverQuest's Free to Play nag browser window upon logging out, many install/setup programs, and more. The tool was specifically made for filtering EverQuest's nag browser window.

Setup
==============

1) Build or download SystemUrlFilter and place it in a directory of your choice.
2) Find the registry key relevant to your browser. The keys should be (only tested via Firefox):

**Firefox**: HKEY_CLASSES_ROOT\FirefoxURL\shell\open\command

**Chrome**: HKEY_CLASSES_ROOT\ChromeHTML\shell\open\command

Change the value of the (Default) key to:

"C:\<Path>\SystemUrlFilter.exe" "%1"

That's it.

Filters
==============

Filters can be configured in the Filters.txt file found along with the executable. One filter per line.

You can prefix and/or postfix an URL you want to match with a *, which acts as a wildcard. As an example, the Filters.txt file contains the following line:

*everquest.com/free-to-play

This will make the application block the URL "https://www.everquest.com/free-to-play" and similar, and prevent it from being opened in your browser.

Configuration and Logging
==============

The file SystemUrlFilter.exe.config contains a few configurable options:

- Logging: Set to "true" to turn on logging. Set to "false" to turn off logging. With logging turned on, each intercepted URL call, whether blocked or not, will be logged along with a timestamp to a file named SystemUrlFilter.log, by default placed in the same directory as the executable. Note that this might cause trouble if you have UAC turned on and have the application placed in C:\Program Files or similar.
- LoggingDirectoryOverride: Specify a directory for logging, instead of by default logging in the same directory as the executable.
- BrowserExecutablePath: The full path to your browser. By default, set to a typical path to Firefox.
- BrowserArgumentsBeforeUrl: Arguments to your browser executable, before the URL. By default, set to "-osint -url ", which is suitable for Firefox