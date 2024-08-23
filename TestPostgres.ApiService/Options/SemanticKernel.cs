namespace TestPostgres.ApiService.Options;

public record SemanticKernel
{
    public required string Endpoint { get; init; }
    public string ApiKey { get; set; }

    public required string CompletionDeploymentName { get; init; }

    public required string EmbeddingDeploymentName { get; init; }
}
