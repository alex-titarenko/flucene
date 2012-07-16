Demo Application Files Indexer

This App demonstrates the base principles of working with Flucene:
• String field mapping
• Boosting for field
• Collection mapping
• Aggregating object mapping

After starting, the application makes following steps:
1. Registers used components in IoC container (autofac library is used).
2. Indexes content of subfolder named "Samples". At this step you can 
   see mapping from whole object to the lucene's Document.
3. Runs main interaction cycle. Enter the query or "exit" ("quit") command 
   to exit. At this step you can see mapping from lucene's Document to our
   aggregated object.
