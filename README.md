# Merp
An event-based Micro ERP, developed by Andrea Saltarello using ASP.NET Core 2.0

**Release notes** 
03/05/2018
Add DataSeeder class.

**IMPORTANT**: Remember to insert your email and password.
(TIP: Use a partial class of DataSeeder)

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
