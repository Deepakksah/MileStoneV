using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Vms.Utilities.Diagnostics;

public class DiagnosticsProviderRepository : IDiagnosticsProviderRepository
{
	public const string InteropTransferBufferPool = "InteropTransferBufferPool";

	private static Lazy<DiagnosticsProviderRepository> instance = new Lazy<DiagnosticsProviderRepository>(() => new DiagnosticsProviderRepository());

	private Dictionary<string, IDiagnosticsProvider> dictionary = new Dictionary<string, IDiagnosticsProvider>();

	public static IDiagnosticsProviderRepository Instance => instance.Value;

	public void Register(string name, IDiagnosticsProvider provider)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		lock (dictionary)
		{
			if (dictionary.ContainsKey(name))
			{
				throw new ArgumentException("Provider with the given name is already registered.", "name");
			}
			dictionary[name] = provider;
		}
	}

	public bool Unregister(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		lock (dictionary)
		{
			return dictionary.Remove(name);
		}
	}

	public IDiagnosticsProvider QueryProvider(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		lock (dictionary)
		{
			if (!dictionary.TryGetValue(name, out var value))
			{
				return null;
			}
			return value;
		}
	}

	public IEnumerable<string> QueryProviderNames()
	{
		lock (dictionary)
		{
			return dictionary.Keys.ToList();
		}
	}
}
