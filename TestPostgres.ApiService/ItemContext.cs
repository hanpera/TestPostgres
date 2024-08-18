using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using TestPostgres.ApiService.Models;

namespace TestPostgres.ApiService
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");

            //modelBuilder.Entity<Item>()
            //    .HasIndex(i => i.Embedding)
            //    .HasMethod("hnsw")
            //    .HasOperators("vector_l2_ops")
            //    .HasStorageParameter("m", 16)
            //    .HasStorageParameter("ef_construction", 64);

            modelBuilder.Entity<CacheItem>()
                .HasIndex(i => i.Embeddings)
                .HasMethod("hnsw")
                .HasOperators("vector_l2_ops")
                .HasStorageParameter("m", 16)
                .HasStorageParameter("ef_construction", 64);
        }

        //public DbSet<Item> Items { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<CacheItem> Cache { get; set; }

    }

    [Table("efcore_items")]
    public class Item
    {
        public int Id { get; set; }

        [Column("embedding", TypeName = "vector(3)")]
        public Vector? Embedding { get; set; }

        [Column("half_embedding", TypeName = "halfvec(3)")]
        public HalfVector? HalfEmbedding { get; set; }

        [Column("binary_embedding", TypeName = "bit(3)")]
        public BitArray? BinaryEmbedding { get; set; }

        [Column("sparse_embedding", TypeName = "sparsevec(3)")]
        public SparseVector? SparseEmbedding { get; set; }
    }
}
