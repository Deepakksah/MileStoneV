using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class EventServerGroupResource
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
				resourceMan = new ResourceManager("VideoOS.ServerConfigurator.CertificateConfigurator.Presenters.EventServerGroupResource", typeof(EventServerGroupResource).Assembly);
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

	public static string EventServerCommunicationSubtitle => ResourceManager.GetString("EventServerCommunicationSubtitle", resourceCulture);

	public static string EventServerCommunicationTitle => ResourceManager.GetString("EventServerCommunicationTitle", resourceCulture);

	public EventServerGroupResource()
	{
	}
}
