# VR-Trash-Sorting-Game

Welcome to Waste Watchers, the educational yet fun experience where you are tasked with sorting the trash at Western's Waste Management Facility. This game challenges players by giving them a variety of different items to sort into the applicable disposal bins. Some items will have additional objectives like pouring out liquids from cups, or flattening cardboard boxes to save space. Try your best to get a high score and rack up a good combo!

INSTRUCTIONS
-------------
Note: This was built in Unity version 2020.3.21f, so we recommend sticking to that to avoid compatibility issues.

- To access the game, download the Unity project. The game runs under the "Main Level" scene, so navigate there to access the main gameplay area.
- We have provided both a VR Controller and First Person Controller camera object which can be found in the scene hierarchy.
-- You can switch between modes by enabling/disabling the above controllers. Do not keep both active simultaneously.
- To build the game, navigate to the Unity build settings panel, ensure Main Level is set as the only scene, and configure your settings before building.

CONTROLS
-------------
Desktop:
- Left Click to pick up items or press buttons
- Left Click to throw held items
- Left Click (over sink) to pour liquids -- hold for extended effect
- Right Click to gently drop held items
- Z to zoom camera -- hold for extended effect

VR:
- Lower Trigger to pick up items -- hold to keep item in hand
- Release Lower Trigger to throw
- Everything else is controlled by manual movement of VR hands and headset

FEATURES
-------------
- A fully detailed stage set in a waste management facility, bringing the whole experience to life.
- Fun waste sorting gameplay, requiring players to sort items into one of 3 bins (landfill, containers, paper) as they come through a conveyor system.
-- Custom items are easy to import! Add a new trash prefab variant and add it to the Conveyor Object Spawner component of the "ConveyorSpawner (2)" object.
- Liquid pouring; Some items such as cups and soda cans will be full of liquid, dump those contents over the sink to get bonus points.
- Cardboard flattening; Boxes take up a lot of space in bins, so flattening them first is good practice. Use the hydraulic press to flatten applicable objects for bonus points.
- Awesome music (thanks Dylan!)
- The ultimate mascot, Razz the Raccoon! Complete with cute animations.

WISHLIST
-------------
Throughout the development process, we had to reprioritize and scrap / modify some features that we would have loved to have ready by May 3rd. Some of these include:
- A tutorial where Razz guides new players on how to sort, pour liquids, or flatten items.
- Voice lines for Razz giving additional feedback as well as extra information regarding waste sorting for educational purposes.
- Dynamically scaling difficulty with faster item spawns and more complexity added over time.
- Additional sounds and visual effects providing even more feedback to players.
