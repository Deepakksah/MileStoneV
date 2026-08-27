using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaLower.Controls.resources;

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
				resourceMan = new ResourceManager("VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaLower.Controls.resources.Strings", typeof(Strings).Assembly);
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

	public static string TooltipSetupButton => ResourceManager.GetString("TooltipSetupButton", resourceCulture);

	public static string TooltipSpanSlider => ResourceManager.GetString("TooltipSpanSlider", resourceCulture);

	public static string TooltipSpanSliderDecreaseButton => ResourceManager.GetString("TooltipSpanSliderDecreaseButton", resourceCulture);

	public static string TooltipSpanSliderIncreaseButton => ResourceManager.GetString("TooltipSpanSliderIncreaseButton", resourceCulture);

	public Strings()
	{
	}
}
