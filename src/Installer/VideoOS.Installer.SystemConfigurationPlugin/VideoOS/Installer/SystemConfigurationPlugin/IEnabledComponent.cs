namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface IEnabledComponent
{
	bool Enabled { get; set; }

	string Name { get; set; }
}
