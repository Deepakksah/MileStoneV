using System;
using System.Collections.Generic;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public class Feature : IFeature
{
	public string Name { get; private set; }

	public Dictionary<string, string> Parameters { get; private set; }

	public bool Visible { get; set; }

	public Feature(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		Name = name;
		Parameters = new Dictionary<string, string>();
	}
}
