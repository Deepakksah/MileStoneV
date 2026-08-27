using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Platform.SDK.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Strings
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
				resourceMan = new ResourceManager("VideoOS.Platform.SDK.Resources.Strings", typeof(Strings).Assembly);
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

	public static string AlertGroups => ResourceManager.GetString("AlertGroups", resourceCulture);

	public static string AllAlerts => ResourceManager.GetString("AllAlerts", resourceCulture);

	public static string AllCameras => ResourceManager.GetString("AllCameras", resourceCulture);

	public static string AllEvents => ResourceManager.GetString("AllEvents", resourceCulture);

	public static string AllHardware => ResourceManager.GetString("AllHardware", resourceCulture);

	public static string AllInputs => ResourceManager.GetString("AllInputs", resourceCulture);

	public static string AllMatrix => ResourceManager.GetString("AllMatrix", resourceCulture);

	public static string AllMetadataDevices => ResourceManager.GetString("AllMetadataDevices", resourceCulture);

	public static string AllMicrophones => ResourceManager.GetString("AllMicrophones", resourceCulture);

	public static string AllOutputs => ResourceManager.GetString("AllOutputs", resourceCulture);

	public static string AllSpeakers => ResourceManager.GetString("AllSpeakers", resourceCulture);

	public static string AllTransactionSources => ResourceManager.GetString("AllTransactionSources", resourceCulture);

	public static string CameraGroups => ResourceManager.GetString("CameraGroups", resourceCulture);

	public static string EdgeDevice => ResourceManager.GetString("EdgeDevice", resourceCulture);

	public static string EventGroups => ResourceManager.GetString("EventGroups", resourceCulture);

	public static string GenericEvents => ResourceManager.GetString("GenericEvents", resourceCulture);

	public static string InputGroups => ResourceManager.GetString("InputGroups", resourceCulture);

	public static string MetadataDeviceGroups => ResourceManager.GetString("MetadataDeviceGroups", resourceCulture);

	public static string MicrophoneGroups => ResourceManager.GetString("MicrophoneGroups", resourceCulture);

	public static string OutputGroups => ResourceManager.GetString("OutputGroups", resourceCulture);

	public static string SpeakerGroups => ResourceManager.GetString("SpeakerGroups", resourceCulture);

	public Strings()
	{
	}
}
