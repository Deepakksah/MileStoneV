using System;
using System.Data;
using System.Runtime.Caching;

namespace VideoOS.Management.Server;

public sealed class HardwareDefinitionComponent : GeminiResourceComponent, IHardwareDefinitionComponent, IDisposable
{
	public class HardwareDefinitionCache : IDisposable
	{
		private MemoryCache cache;

		private TimeSpan slidingExpirationTimeout;

		public HardwareDefinitionCache()
		{
			cache = new MemoryCache("HardwareDefinitionsCache");
			if (!int.TryParse(Config.Instance.GetAppSetting("HardwareDefinitionsCache_SlidingExpirationMinutes", "1440"), out var result))
			{
				slidingExpirationTimeout = TimeSpan.FromMinutes(1440.0);
			}
			else if (result <= 0)
			{
				slidingExpirationTimeout = ObjectCache.NoSlidingExpiration;
			}
			else
			{
				slidingExpirationTimeout = TimeSpan.FromMinutes(result);
			}
		}

		public string Get(string digest)
		{
			if (digest == null)
			{
				throw new ArgumentNullException("digest");
			}
			return (string)cache.Get(digest);
		}

		public void Add(string digest, string definition)
		{
			if (digest == null)
			{
				throw new ArgumentNullException("digest");
			}
			if (definition == null)
			{
				throw new ArgumentNullException("definition");
			}
			CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
			cacheItemPolicy.SlidingExpiration = slidingExpirationTimeout;
			cache.Add(digest, definition, cacheItemPolicy);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && cache != null)
			{
				cache.Dispose();
				cache = null;
			}
		}
	}

	private static HardwareDefinitionCache DefinitionCache = new HardwareDefinitionCache();

	public string QueryDefinition(string digest)
	{
		if (digest == null)
		{
			throw new ArgumentNullException("digest");
		}
		string definition = DefinitionCache.Get(digest);
		if (definition != null)
		{
			return definition;
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_definition");
			BindString("@digest", digest, 44, allowNull: false, SqlDbType.Char);
			object obj = ExecuteScalar();
			if (obj == null)
			{
				throw new HardwareDefinitionNotFoundException();
			}
			definition = (string)obj;
			if (definition != null)
			{
				DefinitionCache.Add(digest, definition);
			}
			return definition;
		});
	}
}
