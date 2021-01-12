# demos-dotnet-graphdemo
Demo of a CQRS-patterned, event-driven solution for inserting nodes into a CosmosDB Graph, and exposing the data through GraphQL.

## Writes
Writes are triggered by an event-grid event fired when a file is dropped in the specified blob-container.
The querys contained in the file are read and executed seq. by an Azure Function.

## Reads
Reads are exposed via an dotnet api-app, through an GraphQL endpoint, visualized with GraphiQL.

## The data
The example data is based on a subset of the airport data compiled by Kelvin Lawrence here: https://github.com/krlawrence/graph, with some added staff data to show the
capability to store any arbitrary data on nodes and edges, and add edges between nodes of differing "type".

## Setup and pre-reqs.
### Pre-req:
* An Azure subscription with the following:
  * Storage account
  * CosmosDB with Gremlin API
  * Function App
  * App Service

### Setup
* Publish the Function App and Api
* Set up an Event Grid System Topic listening for blob creation on the blob container or your choosing.
* Add an Event Subscription that points to the "BlobReader" function.
* Set up the different variables needed in the "BlobReader" and "RepositoryConfiguration"
