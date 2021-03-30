# Primus.Bibliotheca
An analogy of borrowing and returning a book/media from a library is made to object pooling. This is done so to have a more precise wording for actors and relations in a pooling system. Apperently to not to defeat the purpose, _Bibliotheca_ and _Biblion_ are choosen
 over "Library" and "Book". Also, _CheckOut_ and _CheckIn_ mean respectively to borrow, and to return.
## Biblion
A _MonoBehaviour_, implements _IBiblion_. Requires to be in a _TitleCatalog_. Each _Biblion_ has a _BiblionTitle_. It is attached to any GameObject that can be checked in/out. Abstract generic implementation can be found at [GenericBaseBiblion](./GenericBaseBiblion.cs) and its respective sample.
## BiblionPrefab
A _GameObject_, onto which a _Biblion_ is attached. While _Prefab_ can be any pre-fabricated _GameObject_, a _BiblionPrefab_ must have an _Biblion_ component attached to be able to checked in/out. Samples include one when necessary.
## BiblionInstance
A _GameObject_, which is a duplicate instance of its respective _BiblionPrefab_. These are what is being checked in/out at a _Bibliotheca_.
## BiblionTitle / Title Catalog
A _System.Enum_, where _Biblion_ types (titles) are enlisted. As a whole _BiblionTitle_ Enum is the "Title Catalog" which includes the comprehensive list of _Biblion_ types one can check in/out.

"Title Catalog" is a necessity to define a _Bibliotheca_, its _Shelf_ and _Biblion_ inventory. More than one "Title Catalog" can be used in a game, however, one should define a new _Bibliotheca_ per "Title Catalog".

Where such usage might prove useful is unkown at the time of writing.
## Bibliotheca
A _MonoBehaivour_. Has an inventory of _Shelf_, with each _Shelf_ dedicated to a single _BiblionTitle_. Any _BiblionInstance_ is manifactured, checked in/out here. It is essential, that the respective sample _BiblionPrefab_ is put into _Bibliotheca_'s inventory. Abstract generic implementation can be found at [GenericBaseBibliotheca](./GenericBaseBibliotheca.cs) and its respective sample.
## Shelf
 Is a generic container of _Biblion_ of which _BiblionType_ is known to the _Shelf_ (i.e. enlisted in the same "Title Catalog"). Abstract generic implementation can be found at [GenericShelf](./GenericShelf.cs) and its respective sample.

 ## Inventory
 A _Bibliotheca_'s collection of _Shelf_. Although the _Shelf_ does not impose such restriction, a _Bibliotheca_ only puts instances of a single _BiblionTitle_ into only its respective _Shelf_. Thus, inventory is also the total set of _Biblions_ a _Bibliotheca_ can check in/out. Note that unless all _BiblionPrefabs_ corresponding to all _BiblionTitles_ a "Title Catalog" includes are provided to a _Bibliotheca_, then its inventory is smaller than the catalog available.
