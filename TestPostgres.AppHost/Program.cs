var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImage("pgvector/pgvector")
    .WithImageTag("0.7.3-pg16")
    .WithDataVolume("pgdata")
    .WithPgAdmin();

var database = postgres.AddDatabase("TestDB")

 ;

var apiService = builder.AddProject<Projects.TestPostgres_ApiService>("apiservice")
        .WithReference(postgres)
    .WithReference(database); ;

builder.AddProject<Projects.TestPostgres_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(postgres)
    .WithReference(database);

builder.Build().Run();
