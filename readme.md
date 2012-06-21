# ThatsTwinteresting

This is a fun project built during the ConnectedWorld Magazine conference for the Hackathon. Yeah i know, it's the best name ever.
[Project web site](http://jugglingnutcase.github.com/ThatsTwinteresting)

![screenshot](https://github.com/jugglingnutcase/ThatsTwinteresting/raw/gh-pages/images/KatieTweets.png)

## What it does

This web app and background service save Tweets matching queries in [PI AF](http://www.osisoft.com/software-support/what-is-pi/What_Is_PI.aspx) because [Twitter's API doesn't give you data older that 6-9 days](https://dev.twitter.com/docs/using-search). It builds up a database surrounding the queries and then lets the user sort through that data.

## Requirements

* PI AF Server R3 - This project requires OSIsoft's PI database with access to an AF Server. That's the hard part... if you dont have access to one already, or don't know about it, then you may want to let this app pass.
* .NET 4.0
* AFSDK 2.4.0.4431

## Run from the executable

[Download the executable](https://github.com/downloads/jugglingnutcase/ThatsTwinteresting/Twinteresting-alpha.zip) (it's the very alpha version), extract the zip file, edit the batch file with your PI AF connection parameters , and then launch the batch file. Browse to [localhost:1234](http://localhost:1234) to view the application.

## Building and running

1. Set up AF
  1. Create an AF Database for this application (Optional)
  1. Import the Element Template file into your AF Database
  2. Import the EventFrame Template file into your AF Database
2. Open the top level solution and build it
3. Set the command line parameters for each project (they're both the same)
  1. PISystem Name
  2. The application's AF Database Name
  3. User name for PISystem connection (optional)
  4. Password for PISystem connection (optional)

i've put in a lot of work to make it as easy as possible to build and run out of the gate. If i'm missing something or it doesn't work for you, i beg you to let me know! There's github issues or you can tweet me [@jugglingnutcase](https://twitter.com/jugglingnutcase).

## Open Source

i used a few open source libraries to get a few things done faster:
* [Twitter's Bootstrap](http://twitter.github.com/bootstrap) - Thank goodness for this library! Imagine if i had to roll my own HTML and CSS. Yuck.
* [Nancy](http://nancyfx.org) - An easy to use .NET web framework. It's so simple and with tons of hosting options. The main developers are nice to boot :)
* [RestSharp](http://restsharp.org/) - A .NET REST API i use for making calls to Twitter's API.

## Licensing

Copyright 2012 James Sconfitto

This project is licensed under the [Ms-PL](https://github.com/jugglingnutcase/ThatsTwinteresting/blob/master/license).
