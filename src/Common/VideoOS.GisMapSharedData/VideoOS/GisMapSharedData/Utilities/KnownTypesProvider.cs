using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.GisMapSharedData.Utilities;

public static class KnownTypesProvider
{
	private static readonly ConcurrentDictionary<Type, List<Type>> _knownTypesCollection = new ConcurrentDictionary<Type, List<Type>>();

	public static IEnumerable<Type> GetKnownTypes<T>()
	{
		if (!_knownTypesCollection.TryGetValue(typeof(T), out var value))
		{
			value = (from type in typeof(T).Assembly.GetTypes()
				where type.IsSubclassOf(typeof(T))
				select type).ToList();
			_knownTypesCollection.TryAdd(typeof(T), value);
		}
		return value.AsReadOnly();
	}
}
