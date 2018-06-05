# Merp
An open source, event-based Micro ERP developed by Andrea Saltarello using ASP.NET Core 2.1

**Release notes**
05/06/2018
- web project migrated to ASP .NET Core 2.1
- web project's area moved to Razor UI class libraries
- added Martin @ Merp, a bot built on top of LUIS and Microsoft's Bot Framework

21/10/2017
- AzureCloudServices environment removed
- All projects but the web app were migrated to .NET Standard 2.0
- Read models are now backed by Entity Framework Core 2.0 and had to be refactored accordingly
- VIES Acl rebuilt from scratch using updated WCF Connected Services VS extension
- More UI polishing
- Started implementing invoice search

13/10/2017
- Supported environment names are now: Staging, Production, OnPremises, AzureCosmosDB, AzureMongoDB, AzureCloudServices
- A tad bit of UI polishing