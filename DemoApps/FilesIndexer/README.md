## ﻿Demo Application Files Indexer

This App demonstrates the base principles of working with Flucene:
* String field mapping
* Boosting for field
* Collection mapping
* Aggregating object mapping

After starting, the application makes following steps:

1. Registers used components in IoC container (autofac library is used).
2. Indexes content of subfolder named "Samples". At this step you can see mapping from whole object to the lucene's Document.
3. Runs main interaction cycle. Enter the query or "exit" ("quit") command to exit. At this step you can see mapping from lucene's Document to our aggregated object.

The demo models and mappings are placed in Models subfolder. Mapping
classes named as *Map.

Map class is a specialization of the generic DocumentMap class. In the
constructor of such specialization, we can describe mapping rules, with
using of fluent interface. Start from Map or Reference methods and describe
mapping details using methods' chain.
