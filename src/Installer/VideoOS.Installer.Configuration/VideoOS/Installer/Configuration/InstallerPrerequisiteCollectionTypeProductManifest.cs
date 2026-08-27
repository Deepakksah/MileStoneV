using Microsoft.Installer.ProductConfig;

namespace VideoOS.Installer.Configuration;

public class InstallerPrerequisiteCollectionTypeProductManifest
{
	private Product _prerequisiteConfig;

	public Product PrerequisitData
	{
		get
		{
			return _prerequisiteConfig;
		}
		set
		{
			_prerequisiteConfig = value;
		}
	}
}
