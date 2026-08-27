using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IFeature
{
	string Name { get; }

	Dictionary<string, string> Parameters { get; }

	bool Visible { get; set; }
}
