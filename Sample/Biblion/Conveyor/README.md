# Primus.Sample.Conveyor

An in-Editor interactible asset, and a [_Biblion_](../../Core/Bibliotheca/README.md). Has _Roller_ and _Belt_ parts, which are provided with the prefab. Those parts are currently needed to be attached to the conveyor script within a game. A conveyor script needs to extend [_GenericConveyor_](../../Biblion/Conveyor/GenericConveyor.cs) with no need to override any functionality. Also mind that as a _Biblion_ its _BiblionPrefab_ needs a ["Title Catalog"](../../Core/Bibliotheca/README.md) to be provided when being extended.

Usage:

        // Where MyTitleCatalog is a System.Enum enlisting BiblionTitles.
        public class MyConveyor : GenericConveyor<MyTitleCatalog>
        {
        }