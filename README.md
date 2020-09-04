# unityMultiplayer

Please do not use this in production. This is just a demo of "how simple it can be using PHP and C# rather using third party plugins".
If you really want to use this in production, 
- Make sure to add API key system so other people can't access your index.php
- Use password_hash instead of MD5 in login.php
- Use PDO instead of MYSQLI but this is optional since people seem to have different opinion about each of them
- You need to optimize C# code (lot of repetitive code)
- May be updating DB every 0.3 sec is not a good idea
