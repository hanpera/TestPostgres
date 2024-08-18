using TestPostgres.ApiService.Models;

namespace TestPostgres.ApiService.Services
{
    public class PostgresDBService : IDBService
    {
        public Task<Session> InsertSessionAsync(Session session)
        {
            throw new NotImplementedException();
        }

        public Task<Message> InsertMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<List<Session>> GetSessionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Message>> GetSessionMessagesAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<Session> UpdateSessionAsync(Session session)
        {
            throw new NotImplementedException();
        }

        public Task<Session> GetSessionAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task UpsertSessionBatchAsync(params dynamic[] messages)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSessionAndMessagesAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCacheAsync(float[] vectors, double similarityScore)
        {
            throw new NotImplementedException();
        }

        public Task CachePutAsync(CacheItem cacheItem)
        {
            throw new NotImplementedException();
        }

        public Task CacheRemoveAsync(float[] vectors)
        {
            throw new NotImplementedException();
        }

        public Task CacheClearAsync()
        {
            throw new NotImplementedException();
        }
    }
}
