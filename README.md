# SQLFitness
Data reduction using a GA to maximise the semantic comparison of conceptnet word vectors to compare metadata of large flat datasets with Standford CoreNLP to extract the relevant terms in user queries

# Major Version Update
We aim to rewrite the core functionality of this application, in addition to the functionality of https://github.com/cdilga/DBMiner

Currently, one of the hottest loops in the entire pipeline runs over sockets, with the GA running queries to test their fitness through this interface.
In addition to these architectural problems, we seek to address problems with MySQL holding the entire Word2Vec embeddings for ConceptNet. Early testing has seen 1000x speedups possible with a combination of direct disk reads and caching.

We aim to include the following features in the next release:

 - Loading of concept net into memory fast
 - GA implemented using existing high-performance libraries
 - Integration of StanfordCoreNLP for question parsing
 - Updating of fitness and coverage functions [DBMiner] to use performant datatypes in Java
 - Interface with MySQL, excel and csv files to query and return subsets of data
 - Performance improvements, running in a more reasonable time order of minutes not days likely possible
 - Support (perhaps via some framework) to support some kind of validation by running multiple times with different parameters and multiple datasets
 - Adding Java support for SQL queriable logic to query either CSV files or MySQL itself
 - Adding support for "IN" clauses in the set logic 
 - Adding support for "GROUP BY" clauses in set logic
 - Dockerization of project - for rigorous reproducibility and easy validation on Cloud Infrastructure
 - Some documentation on how to setup and run
 - Unit Tests
