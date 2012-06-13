# ThatsTwinteresting

Yeah, it's the best name ever. i know.

This is a fun project built during the ConnectedWorld Magazine conference for the Hackathon. 

## What it does

This web app and background service save Twitter queries away in [PI AF](http://www.osisoft.com/software-support/what-is-pi/What_Is_PI.aspx) in order to save those tweets. It builds up a database surrounding your queries and then lets the user sort through that data.

## Building

This project requires OSIsoft's PI database with access to an AF Server. That's the hard part... if you dont have one already, or don't know about it, then you may want to let this one pass.

Just open the project in Visual Studio and get going. This project may require some other small apps i'm building... but it's still early and i'll figure it out.

## Open Source

i used a few open source libraries to get a few things done faster:
* [Twitter's Bootstrap](http://twitter.github.com/bootstrap) - Thank goodness for this library! Imagine if i had to roll my own HTML and CSS. Yuck.
* [Nancy](http://nancyfx.org) - An easy to use .NET web framework. It's so simple and with tons of hosting options. The main developers are nice to boot :)
* [RestSharp](http://restsharp.org/) - A .NET REST API i use for making calls to Twitter's API.

## Licensing

Copyright 2012 James Sconfitto

This project is licensed under the [Ms-PL](https://github.com/jugglingnutcase/ThatsTwinteresting/blob/master/license).
