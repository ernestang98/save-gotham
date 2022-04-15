# CZ4001 Virtual Reality Assignment

CZ4001 VR Game about Climate Change and Sustainability Awareness

Click [here](https://www.youtube.com/watch?v=b2LuaRZW8ws) for demonstration video!

# Dev notes:

### Hardware Settings:

2 ways of playing (We have a script that auto-detects if there is a VR Headset plugged in or not):

  1. VR Headset (Oculus Rift) - When you plugin, if you have appropriate settings and applications set up, Unity IDE + our application should auto configure the controls for you

      ![VR Controller](https://www.rbvi.ucsf.edu/chimerax/docs/user/tutorials/oculus-touch.png)

      - Left thumbstick for movement (forward, backward, strafe left, strafe right)

      - Right thumbstick for turning/changing facing direction

      - Left/Right grip for picking up objects (aim at object, press and hold grip button to grab object, release to let go of object)

      - Left/Right trigger for interacting with a UI object such as a button

      - Primary button for teleportation (press and hold to aim at target location, release to teleport to target location)

  2. Device Simulator - If you don't have a VR Headset, you can use your keyboard and mouse instead. 

      - T to take control of left hand 

      - Y to take control of right hand

      - R to rotate

      - G to grab, you can grab when the red arrow turns white (meaning it faces the object)

      - mouse wheel to move hand front/back

      - press T or Y again to give away control of left or right hand

      - Right click + mouse wheel to move the camera front and back

      - right click + drag to pan the camera

      - turning is done by enabling right hand and then press "A or D key"

      - moving is done by enabling left hand and then press "W, S, A or D key"

      - teleportation is done by enabling left hand or right hand and then press "B key"

  3. Things to note for both hardware settings as we configured it to be similar:

      - You can enable continuous turn provider and disable snap turn provider to get continuous turn instead of 45° turn

### Software Settings:
  
  - XR Interaction Toolkit
  
  - XR Plug-in Management (Enable XR Plug-in Management)

    1. Tick `Mock HMD Loader` if you are using Device Simulator

    2. Tick `Oculus` if you are using VR Headset

  - Oculus Application (download [here](https://www.oculus.com/setup/))

  - Unity Version: 2019.4.35f1 (LTS) (download [here](https://unity3d.com/unity/qa/lts-releases?version=2019.4))

### Miscellaneous:

  - In the event where left hand controller rotation does not work when using Oculus Rift S, please change "XRI LeftHand Rotation" binding path in "XRI Default Input Actions" from deviceRotation [LeftHand XR Controller] to deviceRotation [LeftHand Oculus Touch Controller]

  - Similarly, in the event where right hand controller rotation does not work when using Oculus Rift S, please change "XRI RightHand Rotation" binding path in "XRI Default Input Actions" from deviceRotation [RightHand XR Controller] to deviceRotation [RightHand Oculus Touch Controller]

  - When downloading assets from the asset store, assets maybe rendered as pink colored material, watch [this video](https://www.youtube.com/watch?v=nB0r0c-SIVg) for the explanation

  - To fix, upgrade your materials in the scene by following [this link](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@6.7/manual/Upgrading-To-HDRP.html)

  - Do not develop by downloading repository as zip file as when you commit your code, you will encounter invalid working copy errors. Always run git clone and start from your development from there

  - Handling merge conflicts

    1. Always merge feature-branch into master branch

    2. If there is a merge conflict, always accept master's copy

    3. Follow this [link](https://stackoverflow.com/questions/63623581/how-do-i-accept-git-merge-conflicts-from-their-branch-for-only-a-certain-direc#) for more details

    4. If anything messes up, worst case just force delete commit using this [link](https://stackoverflow.com/questions/3293531/how-to-permanently-remove-few-commits-from-remote-branch) (with huge precaution, always keep backup before doing this)
