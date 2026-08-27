using System;
using System.Linq;
using System.Windows.Media;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class ProgressViewModel
{
	public string ApplicationTitle { get; }

	public ImageSource ApplicationIcon { get; }

	public bool Register { get; }

	public bool EnableEncryption { get; }

	public bool DisableEncryption { get; }

	public ProgressViewModel(IApplicationResources applicationResources, bool register, bool enableEncryption, bool disableEncryption)
	{
		if (applicationResources == null)
		{
			throw new ArgumentNullException("applicationResources");
		}
		if (new bool[3] { register, enableEncryption, disableEncryption }.Count((bool b) => b) != 1)
		{
			throw new ArgumentException("Exactly one of the boolean arguments must be set");
		}
		ApplicationTitle = applicationResources.ServerConfiguratorApplicationName;
		ApplicationIcon = applicationResources.ServerConfiguratorIcon.GetImageSource(16.0, () => (ImageSource)null);
		Register = register;
		EnableEncryption = enableEncryption;
		DisableEncryption = disableEncryption;
	}
}
