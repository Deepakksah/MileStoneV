using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public interface ISiteConfigDataComponent : IDisposable
{
	Site QueryHomeSite();
}
