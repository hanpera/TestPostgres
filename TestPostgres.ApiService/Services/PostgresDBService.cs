using Azure;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using TestPostgres.ApiService.Models;

namespace TestPostgres.ApiService.Services
{
    public class PostgresDBService
        : IDBService
    {
        private readonly ItemContext _dbContext;
        public PostgresDBService(ItemContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Session> InsertSessionAsync(Session session)
        {
            _dbContext.Sessions.Add(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }

        public async Task<Message> InsertMessageAsync(Message message)
        {
            try
            {
                _dbContext.Messages.Add(message);
                await _dbContext.SaveChangesAsync();
                return message;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Session>> GetSessionsAsync()
        {
            return await _dbContext.Sessions.ToListAsync();
        }

        public async Task<List<Message>> GetSessionMessagesAsync(string sessionId)
        {
            try
            {
                var messages = await _dbContext.Messages.Where(m => m.SessionId == sessionId).ToListAsync();
                return messages;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Session> UpdateSessionAsync(Session session)
        {
            _dbContext.Sessions.Update(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }

        public async Task<Session?> GetSessionAsync(string sessionId)
        {
            return await _dbContext.Sessions.FindAsync(sessionId);
        }

        public async Task UpdateSessionAndMessages(Session session, Message[] messages)
        {

            var curSession = _dbContext.Sessions.Find(session.Id);
            if (curSession == null)
            {
                throw new ArgumentException("Session not found.");
            }
            _dbContext.Sessions.Update(session);
            var curMessages = await _dbContext.Messages.Where(m => m.SessionId == messages.First().SessionId).ToListAsync();
            foreach (var message in messages)
            {
                var existingMessage = curMessages.Find(m => m.Id == message.Id);
                if (existingMessage == null)
                {
                    _dbContext.Messages.Add(message);
                }
                else
                {
                    _dbContext.Messages.Update(message);
                }

            }
            await _dbContext.SaveChangesAsync();
        }

        //private Task UpsertSessionBatchAsync(params Message[] messages)
        //{
        //    if (messages.Select(m => m.SessionId).Distinct().Count() > 1)
        //    {
        //        throw new ArgumentException("All items must have the same partition key.");
        //    }
        //    var curMessages = _dbContext.Messages.Where(m => m.SessionId == messages.First().SessionId);
        //    _dbContext.Messages
        //    throw new NotImplementedException();
        //}

        public async Task DeleteSessionAndMessagesAsync(string sessionId)
        {
            _dbContext.Messages.RemoveRange(_dbContext.Messages.Where(m => m.SessionId == sessionId));
            _dbContext.Sessions.Remove(_dbContext.Sessions.Find(sessionId)!);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetCacheAsync(float[] vectors, double similarityScore)
        {
            try
            {
                var embedding = new Vector(vectors);
                var tmp = await _dbContext.Cache.Select(x => new { Value = x, Distance = x.Embeddings.CosineDistance(embedding) }).ToListAsync();

                var response = await _dbContext.Cache
                    .Where(c => c.Embeddings.CosineDistance(embedding) < similarityScore)
                    .OrderBy(c => c.Embeddings.CosineDistance(embedding))
                    .ToListAsync();
                string cacheResponse = "";

                foreach (CacheItem item in response)
                {
                    cacheResponse = item.Completion;
                }
                return cacheResponse;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task CachePutAsync(CacheItem cacheItem)
        {
            var item = _dbContext.Cache.Find(cacheItem.Id);
            if (item == null)
            {
                _dbContext.Cache.Add(cacheItem);
            }
            else
            {
                _dbContext.Cache.Update(cacheItem);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task CacheRemoveAsync(float[] vectors)
        {
            double similarityScore = 0.99;
            var embedding = new Vector(vectors);
            var response = await _dbContext.Cache
                .Where(c => c.Embeddings.CosineDistance(embedding) > similarityScore)
                .OrderByDescending(c => c.Embeddings.CosineDistance(embedding))
                .ToListAsync();
            _dbContext.Cache.RemoveRange(response);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CacheClearAsync()
        {
            await _dbContext.Cache.ExecuteDeleteAsync();
        }

        //public Task UpsertSessionBatchAsync(Session session, Message[] messages)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
