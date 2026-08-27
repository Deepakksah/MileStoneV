using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.Providers.Strategies;

namespace VideoOS.Platform.AccessControl.Internal.Providers;

public class GuidProvider : IGuidProvider
{
	private static GuidProvider _instance;

	private static readonly object Locker = new object();

	private static HashSet<IGuidProviderStrategy> _guidProviderStrategies = new HashSet<IGuidProviderStrategy>
	{
		new Sha1ProviderStrategy()
	};

	public static GuidProvider Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (Locker)
				{
					if (_instance == null)
					{
						_instance = new GuidProvider();
					}
				}
			}
			return _instance;
		}
	}

	public HashSet<IGuidProviderStrategy> GuidProviderStrategies => _guidProviderStrategies;

	private GuidProvider()
	{
	}

	public void SetStrategies(HashSet<IGuidProviderStrategy> guidProviderStrategies)
	{
		lock (Locker)
		{
			if (guidProviderStrategies == null)
			{
				throw new ArgumentNullException("guidProviderStrategies");
			}
			if (!guidProviderStrategies.Any())
			{
				throw new ArgumentException("you must pass at least 1 guidProviderStrategy", "guidProviderStrategies");
			}
			_guidProviderStrategies = guidProviderStrategies;
		}
	}
}
