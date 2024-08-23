using System.ComponentModel.DataAnnotations;
using Pgvector;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestPostgres.ApiService.Models;

[Table("CacheItems")]
public record CacheItem(float[] Vectors, string Prompts, string Completion)
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    [Key]
    public int Id { get; set; } 

    [Column(TypeName = "vector(1536)")]
    public Vector? Embeddings { get; set; } = new(Vectors);

    //public float[] Vectors { get; set; } = Vectors;
    public string Prompts { get; set; } = Prompts;

    public string Completion { get; set; } = Completion;
}
