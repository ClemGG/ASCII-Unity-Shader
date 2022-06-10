# ASCII Unity Shader
An Image effect shader drawing ASCII characters instead of meshes, made with Unity 2021.3.

This shader trims the screen UVs depending on the underlying pixel color, and replaces those pixels with a predefined Texture converted from a spritesheet referenced through the Inspector. You can easily replace any of these sprites at runtime to display any kind of character or image. You can also change the text resolution and add it a background color.

This shader also works on Particle Effects and UIs (The UIs need to be either in World Space or Camera Space with a layer assigned on the Camera's Culling Mask.)

![fpsgif](https://user-images.githubusercontent.com/23258134/172984712-f6704abf-037d-48b2-842f-9381bc5989f6.gif)
![cubegif](https://user-images.githubusercontent.com/23258134/172984882-b62d44d0-7a3b-4c17-964b-98ea5b82859b.gif)
![text](https://user-images.githubusercontent.com/23258134/172971719-ed981109-afc7-4a43-87d2-f67ca6152745.jpg)
![grass](https://user-images.githubusercontent.com/23258134/172971743-7c07d1a0-86ef-4fbd-8e37-ec7f54730c49.jpg)
