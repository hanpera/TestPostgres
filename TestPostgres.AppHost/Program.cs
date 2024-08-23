var builder = DistributedApplication.CreateBuilder(args);

var azureOpenAiEndpoint = builder.AddParameter("AzureOpeAIEndPoint");
var azureOpenAKey = builder.AddParameter("AzureOpeAIKey");
var azureOpenAiChatCompletionDeploymentName = builder.AddParameter("AzureOpeAIChatCompletionDeploymentName");
var azureOpenAiEmbeddingDeploymentName = builder.AddParameter("AzureOpeAImbeddingDeploymentName");




var postgres = builder.AddPostgres("postgres")
    .WithImage("pgvector/pgvector")
    .WithImageTag("pg16")
    .WithDataVolume("pgdata")
    .WithPgAdmin();

var database = postgres.AddDatabase("TestDB");

var apiService = builder.AddProject<Projects.TestPostgres_ApiService>("apiservice")
        .WithReference(postgres)
        .WithReference(database)
        .WithEnvironment("SemanticKernel__Endpoint", azureOpenAiEndpoint)
        .WithEnvironment("SemanticKernel__ApiKey", azureOpenAKey)

        .WithEnvironment("SemanticKernel__CompletionDeploymentName", azureOpenAiChatCompletionDeploymentName)
        .WithEnvironment("SemanticKernel__EmbeddingDeploymentName", azureOpenAiEmbeddingDeploymentName);

builder.AddProject<Projects.TestPostgres_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(postgres)
    .WithReference(database);

builder.Build().Run();
