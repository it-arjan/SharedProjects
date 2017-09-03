# Shared MyData project

This repository holds the data Api. Authenticated using OAUTH against identityserver3. 

To see how data Api fits in the overall system, see https://messagequeuefrontend.azurewebsites.net/systemlayout

The data project defines an IData interface, and 2 implementations: Entity Framework and Remote Data Api (written using NancyFx). This data project is used in MvcFrontend (api implementation) and in the nancy Api (ETF implementation). So The frontend sends json to the data api which commits it into the Azure Sql Db using ETF.

# Most interesting
It's all pretty basic, it uses a factory and the repository pattern of course, to avoid LING statements bleeding into the client app. It does not have a fake implementation for unit testing yet.

IData also defines async version for all methods.

You can find the two inpementation in the Etf and NancyApi folders.


