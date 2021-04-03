# Primus.ModTool
Currently is meant for in-house modding enabled builds. As experience builds up, creating an external tool for independant modders might be considered.


## Primus.ModTool.Core
Where fundemantals of any functionality like management of input (_InputAction_), camera, etc. are stored.

### CameraSwitcher
Switching cameras was found in this YouTube [video](https://www.youtube.com/watch?v=vZXGbTdk8gA).


## Primus.ModTool.Manager

### ModToolManager
A _GenericMonoSingleton_. Where coordination of functionality is done. Input settings are set at [ModToolInput](./Core/ModToolInput.inputactions). Note that serialization of both [Core](./Core/) and [Functionality](./Functionality) classes makes in-editor use easy. 


## Primus.ModTool.Functionality
Where any piece of functionality build on top of [Primus.Core](../Core/) or [Primus.ModTool.Core](./Core/) reside. These can be combined into creating a development or modding tool within a subclass of [ModToolManager](./Manager/ModToolManager.cs).

### PositionSelector
Let's modder create an array of 2D coordinates with their corresponding enum values. An implementation with a dictionary could suite better.
