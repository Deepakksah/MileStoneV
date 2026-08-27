using System;

namespace VideoOS.Management.Server;

public interface IHardwareDefinitionComponent : IDisposable
{
	string QueryDefinition(string digest);
}
