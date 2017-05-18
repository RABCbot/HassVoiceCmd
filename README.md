# HassVoiceCmd

This is an Universal Windows Platform (UWP) application that bootstraps [Home-Assistant](http://Home-Assistant.io">http://Home-Assistant.io) and registers each service/entity as custom Cortana commands.<br/>
![Screen](HassVoiceCmd-Screen.png)

## Requirements

This application was built with Microsoft Visual Studio 2015 Community Edition [Visual Studio 2015](https://www.visualstudio.com/vs/community)<br/>
Using Template10 [Template10](https://github.com/Windows-XAML/Template10)<br/>
It requires Newtonsoft.Json [Newtonsoft.Json](http://www.newtonsoft.com/json">http://www.newtonsoft.com/json)<br/>

## How it works

You can run the application within Visual Studio<br/>
After launched. Click Settings, enter your Home-assistant web address, a prefix for cortana and the entities you want filtered out.<br/>
Click Home.<br/>
After bootstrap completes, it display a list of all the available commands.<br/>
To call a command, just speak the prefix together with the phrase as shown in the UI.<br/>
For example: "Hey Cortana, Please turn on the garage light".<br/>
