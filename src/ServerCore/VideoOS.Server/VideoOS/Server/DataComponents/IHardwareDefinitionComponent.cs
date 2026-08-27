using System;

namespace VideoOS.Server.DataComponents;

public interface IHardwareDefinitionComponent : IDisposable
{
	string QueryDefinition(string digest);
}
