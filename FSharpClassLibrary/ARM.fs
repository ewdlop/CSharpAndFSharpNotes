module ARM

open Farmer
open Farmer.Builders
open Farmer.CosmosDb

let plan = servicePlan {
    name "theFarm"
    sku WebApp.Sku.F1
}

let ai = appInsights {
    name "insights"
}

let cosmosDb = cosmosDb {
    name "Tasks"
    account_name "isaac-to-do-app-cosmos"
    consistency_policy Session
}

let planets = [ "jupiter"; "mars"; "pluto"; "venus" ]

let webApps : IBuilder list = [
    for planet in planets do
        webApp {
            name ("mywebapp-" + planet)
            link_to_service_plan plan
            link_to_app_insights ai
        }
]

// Create a storage account with a container
let myStorageAccount = storageAccount {
    name "myTestStorage"
    add_public_container "myContainer"
}

// Create a web app with application insights that's connected to the storage account.
let myWebApp = webApp {
    name "myTestWebApp"
    setting "storageKey" myStorageAccount.Key
}

let database = sqlDb {
    name "isaacparseddata"
    sku Sql.DtuSku.S1
}

let transactionalDb = sqlServer {
    name "isaacetlserver"
    admin_username "theadministrator"
    add_databases [ database ]
}

let etlProcessor = functions {
    name "isaacetlprocessor"
    storage_account_name "isaacmydata"
    setting "sql-conn" (transactionalDb.ConnectionString database)
}

// Create an ARM template
let deployment = arm {
    location Location.NorthEurope
    add_resources [
        plan
        ai
        myStorageAccount
        myWebApp
        transactionalDb
        etlProcessor
        cosmosDb
    ]
    add_resources webApps
}


// Deploy it to Azure!
deployment
|> Writer.quickWrite "myResourceGroup"