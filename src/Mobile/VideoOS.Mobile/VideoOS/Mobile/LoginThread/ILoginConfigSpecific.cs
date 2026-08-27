using System;

namespace VideoOS.Mobile.LoginThread;

public interface ILoginConfigSpecific
{
	Guid ServerTypeId { get; }

	Guid ServerPluginId { get; }

	Guid ServerItemType { get; }

	string CommunicationAlias { get; }

	string ServerVersion { get; }

	string ServerDescription { get; }
}
