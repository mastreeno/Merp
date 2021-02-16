# Merp
An open source, event-based Micro ERP developed by Andrea Saltarello using ASP.NET Core 3.1

**Release notes**
16/02/2021
- Improvements in Blazor-based WASM client app (Merp.Wasm.App)

25/11/2020
- Started migrating auth server to IdentityServer4 v4
- Started working on Blazor-based WASM client app

10/05/2020
- Experimental support for AWS as an ASP .NET Core environment

17/4/2020
- Solution ported to .NET Core 3.1
- Registry read model major overhaul

12/03/2019
- Started working on public API to support integration scenarios

30/01/2019
- Martin has been reimplemented from scratch using Bot Framework v4
- Martin is now available as an Alexa skill as well

27/09/2018
- Solution was ported to .NET Core SDK 2.1.4 since the latter is an LTS release
- UI was rewritten from scratch becoming a VUE.js-based SPA
- Bounded context logic is now provided by services implemented in ad hoc web apps
- Basic Task Management feature added to Merp
- Added invoice multi lines management and invoice details pages
- Run MongoDb patches in patches/MongoDb folder
- Run Update-Database on Merp.Accountancy.QueryStack
- Run the following Entity Framework Core migrations defined within the project named Merp.Web.Auth (these migrations create the tables needed by both ASP.NET Core's Identity and IdentityServer to persist clients, API resources and configuration data.):
    - *EntityFrameworkCore\Update-Database -Context PersistedGrantDbContext*
    - *EntityFrameworkCore\Update-Database -Context ConfigurationDbContext*
    - *EntityFrameworkCore\Update-Database -Context ApplicationDbContext*
- Run *npm run build-vendor-dev* and *npm run build-app-dev* from a prompt pointing at the src/Merp.Web.App folder
    
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