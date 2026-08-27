using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Mappings;

public class IdGroupMapping
{
	public string KeyPrefix { get; }

	public List<Mapping> Mappings { get; }

	public Func<string, string> NameFunc { get; }

	public Func<Dictionary<string, string>, bool> CheckValidity { get; set; }

	public IdGroupMapping(string keyPrefix, Func<string, string> nameFunc, List<Mapping> propMappings)
	{
		CheckValidity = null;
		NameFunc = nameFunc;
		Mappings = propMappings;
		KeyPrefix = keyPrefix;
	}
}
