For this project I used Unity as my game engine and my own HDrive for safe storgae before publishing it to Github.

### Dynamic Resolution and Aspect Ratio UI

I've got 2 UI menus; a main menu and a settings menu, the main menu is only avalible at the begining and the settings menu is able to be accessed at all times.

Both are able to accomode a change in screen width and heigh with the use of Auto size for dynamically changing the font size depending on the space avalible but also the use of Overflow set to ellipsis if the font became to small or surpased the minimum font size.

Using anchors and orginsing my layers and compnants I was able to set the images/graphics of the menus to also fit and orientate for the size of the screen or window.

### Graphics Scalability Settings

For resolution and quality settings, I acheieved their changing during runtime from within the settings menu with a dropdown option to switch between Low, Medium and High quality.

I did this with creating multipul render pipline assets and changing their induvidual settings to achieve the different quality levels.

### Action-Based Input System

The input system handles both keyboard and mouse and controller inputs and is able to switch between seamlessly.

Both the player movement and the UI menus are able to respond and recieve inputs from keyboard, mouse and controller.
