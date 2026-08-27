using System.Collections.Generic;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface ICapabilities
{
	Dictionary<string, string> Capabilities { get; }
}
