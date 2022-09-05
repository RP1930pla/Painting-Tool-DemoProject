# Painting Tool / Demo Unity Project
This is the downloadable demo for the Painting Tool I created for a game studio as a technical art test. *This are the project files!*

## How to use the tool
### 1. Preparing the meshes to be painted (before placing on scene)

|![Image](https://user-images.githubusercontent.com/47044476/188498189-2c018eb5-e165-4f89-84df-d70ca4ad8c6d.png)|**Before doing anything, check that your mesh has Read/Write enabled and that the mesh has a second uv channel with lightmap UV’s. If you didn’t create the second uv channel in your modeling application, just check the generate lightmap UVs option.**|
|--|--|

### 2. Preparing the meshes to be painted (on the scene)
- Change the object layer to “Paintable”. Add a mesh collider (Necessary to get lightmap coords using a raycast). Create and set a Toon Standard Paintable material, or instead add a texture2D to your custom shader with the reference _PaintMap.
- Make sure that you maintain constant texel size between lightmaps to make the brush size stable.

![image](https://user-images.githubusercontent.com/47044476/188499271-e1bf3d0d-338d-4785-b2f5-590049576aa4.png)

![image](https://user-images.githubusercontent.com/47044476/188499515-8eb30261-c9a8-481e-b4f4-1aa6ca532026.png)

![image](https://user-images.githubusercontent.com/47044476/188499527-70648102-4199-406b-840a-371739fb9dc4.png)

### 3. Grass Tool Prefab and settings
- The only script you need to modify in order to change the tools settings is the last one, Runtime Settings.
Make sure that you reference your main camera on Scene Camera, the grass mesh in Grass Mesh and the material in Grass Material.

- You can change the brush texture on the brush texture reference.
- Max Texture Brush Size and Min Texture Brush Size changes the maximum and minimum size of the brush paint, set it to values that match the grass instancer radius.
- Brush Size is controlled on the UI Sliders, Instances defines the density of the grass instancing and normal mask is a value that allows painting on only certain angles (also changed in UI).

![image](https://user-images.githubusercontent.com/47044476/188499892-42d973fe-71fa-4ac0-a338-3a77b75ef81d.png)
