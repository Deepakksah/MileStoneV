using VideoOS.Server.DataComponents.ConfigurationObjects;

namespace VideoOS.Server.DataComponents;

public sealed class DataComponentFactory : IDataComponentFactory
{
	public IConfigurationDataComponent CreateConfigurationDataComponent()
	{
		return new ConfigurationDataComponent();
	}

	public ISiteConfigDataComponent CreateSiteConfigDataComponent()
	{
		return new SiteComponent();
	}

	public IRecorderConfigDataComponent CreateRecorderConfigDataComponent()
	{
		return new RecorderConfigDataComponent();
	}

	public IRecordingServerComponent CreateRecordingServerComponent()
	{
		return new RecordingServerComponent();
	}

	public IHardwareConfigDataComponent CreateHardwareConfigDataComponent()
	{
		return new HardwareConfigDataComponent(Server.Instance.SqlLogFactory, Server.Instance.CorporateEncryptionSymmetric);
	}

	public IDeviceDataDistributionComponent CreateDeviceDataDistributionComponent()
	{
		return new DeviceDataDistribution.DeviceDataDistributionComponent();
	}

	public IMIPLicenseRequestComponent CreateMIPLicenseRequestComponent()
	{
		return new MIPLicenseRequestComponent();
	}

	public IIntegrationRegistrationsComponent CreateIntegrationRegistrationsComponent()
	{
		return new IntegrationRegistrationsComponent();
	}

	public ISystemBuilderLicenseRequestComponent CreateSystemBuilderLicenseRequestComponent()
	{
		return new SystemBuilderLicenseRequestComponent();
	}

	public ISystemSettingsComponent CreateSystemSettingsComponent()
	{
		return new SystemSettingsComponent();
	}

	public IHardwareDefinitionComponent CreateHardwareDefinitionComponent()
	{
		return new HardwareDefinitionComponent();
	}

	public IHardwareDriverComponent CreateHardwareDriverComponent()
	{
		return new HardwareDriverComponent();
	}

	public IDriverConfigDataComponent CreateDriverConfigDataComponent()
	{
		return new DriverConfigDataComponent();
	}

	public ITaskComponent CreateTaskComponent()
	{
		return new TaskComponent(Server.Instance.CorporateEncryptionSymmetric);
	}

	public IDriverNumberComponent CreateDriverNumberComponent()
	{
		return new DriverNumberComponent();
	}

	public IEvidenceLockComponent CreateEvidenceLockComponent()
	{
		return new EvidenceLockComponent();
	}

	public ISequencesComponent CreateSequencesComponent()
	{
		return new SequencesComponent();
	}
}
