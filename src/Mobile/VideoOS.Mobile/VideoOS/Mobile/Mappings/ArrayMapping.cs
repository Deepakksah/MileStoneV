using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Mappings;

public class ArrayMapping
{
	public Func<string, string> NameFunc { get; }

	public string PropCountName { get; }

	public List<Mapping> Mappings { get; }

	public bool Indexed { get; set; } = true;

	public Func<Dictionary<string, string>, int, bool> CheckValidity { get; set; }

	public string PropIdName { get; }

	public ArrayMapping(string propCountName, string propIdName, Func<string, string> nameFunc, List<Mapping> propMappings)
	{
		CheckValidity = null;
		NameFunc = nameFunc;
		Mappings = propMappings;
		PropCountName = propCountName;
		PropIdName = propIdName;
	}
}
