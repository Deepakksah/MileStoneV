using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class ServerToServerGroupResource
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
				resourceMan = new ResourceManager("VideoOS.ServerConfigurator.CertificateConfigurator.Presenters.ServerToServerGroupResource", typeof(ServerToServerGroupResource).Assembly);
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

	public static string ServerToServerCommunicationSubtitle => ResourceManager.GetString("ServerToServerCommunicationSubtitle", resourceCulture);

	public static string ServerToServerCommunicationTitle => ResourceManager.GetString("ServerToServerCommunicationTitle", resourceCulture);

	public ServerToServerGroupResource()
	{
	}
}
