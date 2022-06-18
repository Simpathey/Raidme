# Raidme

Stream overlay game that triggers when you get raided

## The Game

##### General Game Flow

1. You get raided and a timer shows up on you stream.
2. The raiders and people in your chat can type one of two commands to participate in a battle.
3. When the timer hits zero units spawn in for every person that typed the command.
4. The units battle each other until one side has no units left.
5. The game ends in either a draw or one of the two sides winning.

##### Special Rules

1. The strength of units spawning in is equal to the percentage participation of the two groups.
2. If you get another raid while the timer is still counting down the two raids merge and extra time is added.
3. If you get another raid while the battle is happening that raid will not trigger a game.
4. If the number of people that typed the raid command is larger then the total number of raiders, there is a debuff on the excess units.

## What you need to run RaidMe

- Your own bot
- To register your bot (easier then it sounds)
- To set up and allow access to your bot
- OBS

## Setting up your bot

##### Registering your bot

- Create a bot account on twitch (for example mine is Simpabot)
- Go to https://dev.twitch.tv/ and sign in with your own personal account (for example I am Simpathey)

Once logged in, click on Dashboard, then Apps, then Register Your Application. You’ll then need to fill a few fields. Give your application a name (this isn’t too important, your viewers won’t see it, it’ll just be the name you see if you come in to look at this page again). In the OAuth Redirect URI field, just add http://localhost, as you will be running the game locally on your own computer while you stream.

Once you press submit, the same page should show you two pieces of information. Click on Manage next to the application you just added. You will need both the Client ID and the Client Secret. Make a note of these, but do not share them (especially the Client Secret) with anybody else. If your secret doesn’t show, click the “New Secret” button and it’ll show you a code. This can be used to make a new secret should you accidentally share it with someone. Click submit and you are then done.

##### Giving access to your bot

- First step here is to log into Twitch on your bot account. You may want to do this in Incognito mode so that you don’t have to log out of your personal Twitch account. Once logged in, visit https://twitchtokengenerator.com/. It’ll present you with two options, click on the Bot Chat Token option. At this point you’ll need to authorize Twitch Token Generator to use your account. You’ll then be presented with both an Access Token and a Refresh Token. Make note of these, and again keep these private. If you lose them, you can come back to this site and re-generate new ones. Once you’ve got these codes, you can close the webpage.

## Running The Game

- Download the latest build and unzip the file
- Launch Raidme.exe
- Set up your config (only needs to be done once)
- Click APPLY CHANGES in top right
- Hit escape to hide the config canvas
- You can at this point minimize the game and it will run in the background of the stream

## Configuring Your Raidme Game

- **Twitch Channel Name:** Fill with your twitch channel name
- **Bot Name:** Fill with your twitch bot name
- **Client ID:** Fill with client ID you got from Registering your bot
- **Client Secret:** Fill with client secret you got from Registering your bot
- **Bot Access Token:** Fill With access token from twitch token generator
- **Bot Refresh Token:** Fill With refresh token from twitch token generator
- **Text Color:** Push square button to expand panel, sets the color of text in the game
- **Outline Color:** Push square button to expand panel, sets the color of text outline in the game
- **Raider Limit:** Enter an integer to set a limit to how many people it takes raiding you to trigger the game. Must be a whole number 
- **Raider Command:** The command raiders type to join the battle, defaults to raid, no need to include the ! that is added automatically
- **Defender Command:** The command current chatters type to join the battle, defaults to defend, no need to include the ! that is added automatically
- **Community Name:** What ever you call you community, defaults to defenders
- **Seconds Before Battle:** How much time is on the clock between the time you get raided and the time the battle starts, must be whole number
- **Defender Sprite:** To change the default defender png replace the png in the folder >Raidme_Data/StreamingAssets/DefenderSprite | Size 512x512 png with transparent background (defenders attack right to left)
- **Raider Sprite:** To change the default raider png replace the png in the folder >Raidme_Data/StreamingAssets/RaiderSprite | Size 512x512 png with transparent background (raiders attack left to right)

## Capturing in OBS

- Create a new Game Capture
- Select the window that has Raidme running
- Check the box that says "Allow Transparency" This is under the scale resolution setting if your having trouble finding it
- Make sure Raidme is the top of your scene so it will show up on top when game starts
