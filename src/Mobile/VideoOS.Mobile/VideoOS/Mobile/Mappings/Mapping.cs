using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Mappings;

public class Mapping
{
	public string ReadableName { get; }

	public string Key { get; }

	public Func<Dictionary<string, string>, int, string, string> Mapper { get; }

	public bool IsSensitive { get; set; }

	public Mapping(string readableName, string key, Func<Dictionary<string, string>, int, string, string> mapper = null, bool isSensitive = false)
	{
		ReadableName = readableName;
		Key = key;
		Mapper = mapper;
		IsSensitive = isSensitive;
	}
}
