using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls.resources;

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
				resourceMan = new ResourceManager("VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls.resources.Strings", typeof(Strings).Assembly);
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

	public static string TooltipNavigateFirstButton => ResourceManager.GetString("TooltipNavigateFirstButton", resourceCulture);

	public static string TooltipNavigateLastButton => ResourceManager.GetString("TooltipNavigateLastButton", resourceCulture);

	public static string TooltipNavigateNextButton => ResourceManager.GetString("TooltipNavigateNextButton", resourceCulture);

	public static string TooltipNavigateNextSequenceButton => ResourceManager.GetString("TooltipNavigateNextSequenceButton", resourceCulture);

	public static string TooltipNavigatePreviousButton => ResourceManager.GetString("TooltipNavigatePreviousButton", resourceCulture);

	public static string TooltipNavigatePreviousSequenceButton => ResourceManager.GetString("TooltipNavigatePreviousSequenceButton", resourceCulture);

	public static string TooltipPlaybackBackwardButton => ResourceManager.GetString("TooltipPlaybackBackwardButton", resourceCulture);

	public static string TooltipPlaybackForwardButton => ResourceManager.GetString("TooltipPlaybackForwardButton", resourceCulture);

	public static string TooltipPlaybackPauseButton => ResourceManager.GetString("TooltipPlaybackPauseButton", resourceCulture);

	public static string TooltipPlaybackSlider => ResourceManager.GetString("TooltipPlaybackSlider", resourceCulture);

	public static string TooltipPlaybackSpeed => ResourceManager.GetString("TooltipPlaybackSpeed", resourceCulture);

	public static string TooltipTimeSelectionModeSetTimeButton => ResourceManager.GetString("TooltipTimeSelectionModeSetTimeButton", resourceCulture);

	public static string TooltipTimeSelectionModeToggleButton => ResourceManager.GetString("TooltipTimeSelectionModeToggleButton", resourceCulture);

	public Strings()
	{
	}
}
