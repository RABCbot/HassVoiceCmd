##HassVoiceCmd

This is an Universal Windows Platform (UWP) application that bootstraps <a href="http://Home-Assistant.io">http://Home-Assistant.io</a> and registers each service/entity as custom Cortana commands.

##Requirements

This application was built with Microsoft Visual Studio 2015 Community Edition <a href="https://www.visualstudio.com/vs/community/">https://www.visualstudio.com/vs/community/</a>.<br>
It requires Newtonsoft.Json <a href="http://www.newtonsoft.com/json">http://www.newtonsoft.com/json</a>

##How it works

You can run the application within Visual Studio, after launched.<br>
Click the menu button and enter your Home-assistant web address, you can also enter a prefix word.<br>
Click Save.<br>
After bootstraping, it display a list of all the available commands.
To call a command, just speak the prefix together with the phrase as shown in the UI.<br>
For example: "Please turn on the garage light".
