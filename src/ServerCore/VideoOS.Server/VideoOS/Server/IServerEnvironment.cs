using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Rules.Interfaces;
using VideoOS.SNMPCOMCommBridge;
using VideoOS.Server.CommandHandler;
using VideoOS.Server.DataComponents;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Server.ServiceChannel;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server;

public interface IServerEnvironment
{
	IServerConfig ServerConfig { get; }

	IServer Server { get; }

	IServerConfigurationLookup ServerConfigurationLookup { get; }

	IRuleEngine RuleEngine { get; }

	IDataComponentFactory DataComponentFactory { get; }

	IConfigurationDataLoader ConfigurationDataLoader { get; }

	IConfigurationDataSetComponent ConfigurationDataSetComponent { get; }

	ILicenseDataSetComponent LicenseDataSetComponent { get; }

	IRecorderCommManager RecorderCommManager { get; }

	IMessageManager MessageManager { get; }

	IServiceChannelManager ServiceChannelManager { get; }

	ILicenseManager LicenseManager { get; }

	IIdpAdminClient IdpAdminClient { get; }

	ISNMPManager SnmpManager { get; }

	IServerLogManager ServerLogManager { get; }

	ILogAuditLogger AuditLogger { get; }

	ILocalCommandProcessor LocalCommandProcessor { get; }

	ISqlLogFactory SqlLogFactory { get; }

	ITaskHandling TaskHandling { get; }

	IServerFileStorage ServerFileStorage { get; }

	IFileUploadSynchronizer FileUploadSynchronizer { get; }

	IVmoClientFactory VmoClientFactory { get; }

	ILocalizedLogger AuditAndSystemLogger { get; }
}
