using System;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Server;

public interface IConfigurationDataSetComponent
{
	ReaderWriterLockSlim ConfigurationDataSetCacheLock { get; }

	ConfigurationDataSet LoadConfigurationDataSetByRecorder(Guid recorderId, Guid recorderServiceId);

	ConfigurationDataSet LoadConfigurationDataSetByHardware(Guid hardwareId);

	void WriteConfigurationDataSetChangesToDatabase(ConfigurationDataSet changes, ICallContextInfo callContextInfo);

	void WriteConfigurationDataSetChangesToDatabaseForced(ConfigurationDataSet changes, ICallContextInfo callContextInfo);

	void WriteConfigurationDataSetChangesForReplaceHardware(ConfigurationDataSet changes, ICallContextInfo callContextInfo);

	SecurityDataSet LoadSecurityDataSetByHardware(Guid hardwareId);
}
