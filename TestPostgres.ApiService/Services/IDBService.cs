using TestPostgres.ApiService.Models;

namespace TestPostgres.ApiService.Services;

public interface IDBService
{
    /// <summary>
    /// Creates a new chat session.
    /// </summary>
    /// <param name="session">Chat session item to create.</param>
    /// <returns>Newly created chat session item.</returns>
    Task<Session> InsertSessionAsync(Session session);

    /// <summary>
    /// Creates a new chat message.
    /// </summary>
    /// <param name="message">Chat message item to create.</param>
    /// <returns>Newly created chat message item.</returns>
    Task<Message> InsertMessageAsync(Message message);

    /// <summary>
    /// Gets a list of all current chat sessions.
    /// </summary>
    /// <returns>List of distinct chat session items.</returns>
    Task<List<Session>> GetSessionsAsync();

    /// <summary>
    /// Gets a list of all current chat messages for a specified session identifier.
    /// </summary>
    /// <param name="sessionId">Chat session identifier used to filter messsages.</param>
    /// <returns>List of chat message items for the specified session.</returns>
    Task<List<Message>> GetSessionMessagesAsync(string sessionId);

    /// <summary>
    /// Updates an existing chat session.
    /// </summary>
    /// <param name="session">Chat session item to update.</param>
    /// <returns>Revised created chat session item.</returns>
    Task<Session> UpdateSessionAsync(Session session);

    /// <summary>
    /// Returns an existing chat session.
    /// </summary>
    /// <param name="sessionId">Chat session id for the session to return.</param>
    /// <returns>Chat session item.</returns>
    Task<Session?> GetSessionAsync(string sessionId);

    /// <summary>
    /// Batch create chat message and update session.
    /// </summary>
    /// <param name="messages">Chat message and session items to create or replace.</param>
    Task UpdateSessionAndMessages(Session session, Message[] messages);

    /// <summary>
    /// Batch deletes an existing chat session and all related messages.
    /// </summary>
    /// <param name="sessionId">Chat session identifier used to flag messages and sessions for deletion.</param>
    Task DeleteSessionAndMessagesAsync(string sessionId);

    /// <summary>
    /// Find a cache item.
    /// Select Top 1 to get only get one result.
    /// OrderBy DESC to return the highest similary score first.
    /// Use a subquery to get the similarity score so we can then use in a WHERE clause
    /// </summary>
    /// <param name="vectors">Vectors to do the semantic search in the cache.</param>
    /// <param name="similarityScore">Value to determine how similar the vectors. >0.99 is exact match.</param>
    Task<string> GetCacheAsync(float[] vectors, double similarityScore);

    /// <summary>
    /// Add a new cache item.
    /// </summary>
    /// <param name="vectors">Vectors used to perform the semantic search.</param>
    /// <param name="prompt">Text value of the vectors in the search.</param>
    /// <param name="completion">Text value of the previously generated response to return to the user.</param>
    Task CachePutAsync(CacheItem cacheItem);

    /// <summary>
    /// Remove a cache item using its vectors.
    /// </summary>
    /// <param name="vectors">Vectors used to perform the semantic search. Similarity Score is set to 0.99 for exact match</param>
    Task CacheRemoveAsync(float[] vectors);

    /// <summary>
    /// Clear the cache of all cache items.
    /// </summary>
    Task CacheClearAsync();

}