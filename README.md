# UnityBCI

Unity application for communicating to BCI2000. Can send local/remote commands to control BCI2000's operator layer as well as set/get parameters/states to replace the BCI2000 native application layer with Unity.

![alt text](/Media/GUI.PNG)
![alt text](/Media/1.PNG)
![alt text](/Media/3.PNG)
Assets from Mega Fantasy Props Pack



Consider this project just barely stable

# To run:
- Download the SteamVR plugin from the Asset Store
- Load the "Start" scene
- Click 'Traditional', 'SignalGenerator', 'ARSignalProcessing', and 'CursorTask' and then 'Configure'
Will need to download SteamVR plugin from Unity Asset Store (maybe VRTK as well)
Might need to install Leap Motion modules

This will load a simple scene with a cursor moving between two targets via BCI2000

Now using ![BCI2000Web](https://github.com/cronelab/bci2000web). Need to install via npm.

# To do:
- Add Leap Motion, Hue, Roku, and TP-Link libraries/scripts
- Cleanup prefabs for 1/2D SMR based tasks


This work can be found in it's academic form ![here](https://ieeexplore.ieee.org/document/8302482/)
