MultiRobotic Interface In Unity3D

********** WORK STILL IN PROGRESS ************
******* WILL KEEP THIS PROJECT UPDATED *******

This project studies the capabilities of the Unity3D game engine to implement a user interface with multiple robots running ROS with DDS layer to manage communications. It makes use of ROSSharp (https://github.com/siemens/ros-sharp) and DDS VortexOpenSplice from ADLinkTech. Since this project is still in active development it is far from a complete product. This project is still considered a proof of concept that we can integrate all this into the Unity game engine. The IngameDebugConsole is an imported asset from https://github.com/yasirkula/UnityIngameDebugConsole and will be modified in the final product.

You will need ROS in your system with a rosbridge package installed. In order to communicate simply run rosbridge on an available port and reference it the ROS connectors in each robot. Inside the Third-Party folder reside the Mapbox API and ROSSharp.
In RosSharp\Scripts\RosBridgeClient you will find the various publisher and subscriber scripts, responsible for the communication of each topic type. ROSSharp dll was changed in order to support a lot more topic types with personalized types as well. Topics that require a lot of bandwith like Image should be tied to a separate websocket port (RosConnector).

For Mapbox you will only need an API key that you submit in the Mapbox/Setup tab

The included map mesh Porto_ISEP, was obtained by using OSM2World

There are 3 scenes in this project for now:

- MainScene for the Mapbox environment
- PCL Viewer for the debugging of the PCL importation
- LSA for the render of the inside of ISEP's LSA water tank and two robots (EVA and ROAZII)


PointClouds:

Import ASCII point clouds with RGB channels or plain xyz data by choosing the dropdown option File/Open PCL File. 
