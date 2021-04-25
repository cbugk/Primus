# Primus.ModTool
Currently is meant for in-house modding via external configuration tools executables.


## Primus.ModTool.Core

### IFunctionality
An _Interface_ for any base or mod functionality.

### IFunctionalityManager
An _Interface_ where implementations of _IFunctionality_ are composed into a modding tool.

## Primus.ModTool.Functionality
Where any piece of functionality build on top of Primus resides.

### CameraSwitcher
Switching cameras was found in this [video](https://www.youtube.com/watch?v=vZXGbTdk8gA).

### MouseToWorldPosition
Raycasting from mouseposition to get 3D coordinates of a hit was found in this [video](https://www.youtube.com/watch?v=JfpMIUDa-Mk).
