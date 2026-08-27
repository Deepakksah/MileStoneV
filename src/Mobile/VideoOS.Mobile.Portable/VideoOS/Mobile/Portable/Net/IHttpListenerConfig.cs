using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Net;

public interface IHttpListenerConfig
{
	void SetPrefixes(IEnumerable<string> prefixes);
}
