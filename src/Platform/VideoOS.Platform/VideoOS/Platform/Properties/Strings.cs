using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Platform.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Platform.Properties.Strings", typeof(Strings).Assembly);
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

	public static string PlaybackWpfUserControlMini_FirstSequence_Tooltip => ResourceManager.GetString("PlaybackWpfUserControlMini_FirstSequence_Tooltip", resourceCulture);

	public static string PlaybackWpfUserControlMini_LastSequence_Tooltip => ResourceManager.GetString("PlaybackWpfUserControlMini_LastSequence_Tooltip", resourceCulture);

	public static string PlaybackWpfUserControlMini_NextFrame_Tooltip => ResourceManager.GetString("PlaybackWpfUserControlMini_NextFrame_Tooltip", resourceCulture);

	public static string PlaybackWpfUserControlMini_NextSequence_Tooltip => ResourceManager.GetString("PlaybackWpfUserControlMini_NextSequence_Tooltip", resourceCulture);

	public static string PlaybackWpfUserControlMini_PlayBackward_Tooltip => ResourceManager.GetString("PlaybackWpfUserControlMini_PlayBackward_Tooltip", resourceCulture);

	public static string PlaybackWpfUserControlMini_PlayForward_Tooltip => ResourceManager.GetString("PlaybackWpfUserControlMini_PlayForward_Tooltip", resourceCulture);

	public static string PlaybackWpfUserControlMini_PreviousFrame_Tooltip => ResourceManager.GetString("PlaybackWpfUserControlMini_PreviousFrame_Tooltip", resourceCulture);

	public static string PlaybackWpfUserControlMini_PreviousSequence_Tooltip => ResourceManager.GetString("PlaybackWpfUserControlMini_PreviousSequence_Tooltip", resourceCulture);

	public static string SearchResultStateLoading => ResourceManager.GetString("SearchResultStateLoading", resourceCulture);

	public Strings()
	{
	}
}
