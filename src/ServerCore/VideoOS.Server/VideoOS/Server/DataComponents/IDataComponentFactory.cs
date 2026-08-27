using VideoOS.Server.DataComponents.ConfigurationObjects;

namespace VideoOS.Server.DataComponents;

public interface IDataComponentFactory
{
	IConfigurationDataComponent CreateConfigurationDataComponent();

	ISiteConfigDataComponent CreateSiteConfigDataComponent();

	IRecorderConfigDataComponent CreateRecorderConfigDataComponent();

	IRecordingServerComponent CreateRecordingServerComponent();

	IHardwareConfigDataComponent CreateHardwareConfigDataComponent();

	IHardwareDefinitionComponent CreateHardwareDefinitionComponent();

	IHardwareDriverComponent CreateHardwareDriverComponent();

	IDeviceDataDistributionComponent CreateDeviceDataDistributionComponent();

	IMIPLicenseRequestComponent CreateMIPLicenseRequestComponent();

	IIntegrationRegistrationsComponent CreateIntegrationRegistrationsComponent();

	ISystemBuilderLicenseRequestComponent CreateSystemBuilderLicenseRequestComponent();

	ISystemSettingsComponent CreateSystemSettingsComponent();

	IDriverConfigDataComponent CreateDriverConfigDataComponent();

	ITaskComponent CreateTaskComponent();

	IDriverNumberComponent CreateDriverNumberComponent();

	IEvidenceLockComponent CreateEvidenceLockComponent();

	ISequencesComponent CreateSequencesComponent();
}
