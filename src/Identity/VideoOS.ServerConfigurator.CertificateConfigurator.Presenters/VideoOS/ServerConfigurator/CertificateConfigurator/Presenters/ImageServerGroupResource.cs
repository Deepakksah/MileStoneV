using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class ImageServerGroupResource
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.ServerConfigurator.CertificateConfigurator.Presenters.ImageServerGroupResource", typeof(ImageServerGroupResource).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ImageServerCommunicationSubtitle => ResourceManager.GetString("ImageServerCommunicationSubtitle", resourceCulture);

	public static string ImageServerCommunicationTitle => ResourceManager.GetString("ImageServerCommunicationTitle", resourceCulture);

	public ImageServerGroupResource()
	{
	}
}
