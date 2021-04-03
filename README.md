# Primus
Unity assets to develop games faster and consistently in the long run. Most scripts and prefabs are homebrew, with exception of referenced code/assets.

## Asset Store
Currently library is not apt to be put in the Asset Store.

## Maxims
### 0. Serial Comma
Make use of [serial comma](https://en.wikipedia.org/wiki/Serial_comma) (a.k.a. Oxford comma) in documentation. This is purely personal preference of the repository owner.
### 1. Generics
Whenever possible make use of Generics. With Unity 2020 adding in-editor manupilation of abstract generic arrays/lists, this is prioritized.
### 2. Samples
For every prefab catalogued, there need be a sample to simplify documentation, testing, and user experience. 

## Namespaces

### Primus.Core
Implementations of fundemental concepts, i.e. building blocks. GenericSingleton class, and Bibliotheca (object pooling) are few examples implemeted.
### Primus.Biblion
For cataloguing generic [Biblion](./Core/Bibliotheca/README.md/##Biblion) scripts, their respective BiblionPrefabs are stored under Primus.Samples namespace.
### Primus.Sample
Where working demonstrative code and BiblionPrefabs are stored. BiblionPrefabs need to be duplicated into original prefabs and their scripts replaced with one's in-game scripts in a similar fashion to the provided sample.
### Primus.ModTool
Where modding-related tools are stored. Here too, generic base implementations from which per game adaptations are made are the main focus.

See subfolders for more detail.