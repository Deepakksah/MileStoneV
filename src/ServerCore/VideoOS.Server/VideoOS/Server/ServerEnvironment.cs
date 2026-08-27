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

public sealed class ServerEnvironment : IServerEnvironment
{
	public IServerConfig ServerConfig => VideoOS.Server.ServerConfig.Instance;

	public IServer Server
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance;
		}
	}

	public IServerConfigurationLookup ServerConfigurationLookup
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.ConfigurationLookup == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.ConfigurationLookup;
		}
	}

	public IRuleEngine RuleEngine
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.RuleEngine == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.RuleEngine;
		}
	}

	public IDataComponentFactory DataComponentFactory
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.DataComponentFactory == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.DataComponentFactory;
		}
	}

	public IConfigurationDataLoader ConfigurationDataLoader
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.Data == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.Data;
		}
	}

	public IConfigurationDataSetComponent ConfigurationDataSetComponent
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.Data == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.Data;
		}
	}

	public ILicenseDataSetComponent LicenseDataSetComponent
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.Data == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.Data;
		}
	}

	public IRecorderCommManager RecorderCommManager
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.RecorderCommManager == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.RecorderCommManager;
		}
	}

	public IMessageManager MessageManager
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.MessageManager == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.MessageManager;
		}
	}

	public IServiceChannelManager ServiceChannelManager
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.ServiceChannelManager == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.ServiceChannelManager;
		}
	}

	public ILicenseManager LicenseManager
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.LicenseManager == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.LicenseManager;
		}
	}

	public IIdpAdminClient IdpAdminClient
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.IdpAdminClient == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.IdpAdminClient;
		}
	}

	public ISNMPManager SnmpManager
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null || VideoOS.Server.Server.Instance.SNMPManager == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.SNMPManager;
		}
	}

	public ILogAuditLogger AuditLogger => VideoOS.Server.ServerLogManager.Instance;

	public IServerLogManager ServerLogManager => VideoOS.Server.ServerLogManager.Instance;

	public ILocalCommandProcessor LocalCommandProcessor => VideoOS.Common.Integration.Command.LocalCommandProcessor.Instance;

	public ISqlLogFactory SqlLogFactory => VideoOS.Server.Server.Instance.SqlLogFactory;

	public ITaskHandling TaskHandling => VideoOS.Server.TaskHandling.Instance;

	public IVmoClientFactory VmoClientFactory
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.VmoClientFactory;
		}
	}

	public IServerFileStorage ServerFileStorage
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.ServerFileStorage;
		}
	}

	public IFileUploadSynchronizer FileUploadSynchronizer
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.FileUploadSynchronizer;
		}
	}

	public ILocalizedLogger AuditAndSystemLogger
	{
		get
		{
			if (VideoOS.Server.Server.Instance == null)
			{
				throw new InvalidDomainException("Use of server environment property before server is initialized");
			}
			return VideoOS.Server.Server.Instance.AuditAndSystemLogger;
		}
	}
}
