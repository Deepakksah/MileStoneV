using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Selection.resources;

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
				resourceMan = new ResourceManager("VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Selection.resources.Strings", typeof(Strings).Assembly);
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

	public static string CenterSelectionMarkerLeftOutOfScopeButtonToolTip => ResourceManager.GetString("CenterSelectionMarkerLeftOutOfScopeButtonToolTip", resourceCulture);

	public static string CenterSelectionMarkerRightOutOfScopeButtonToolTip => ResourceManager.GetString("CenterSelectionMarkerRightOutOfScopeButtonToolTip", resourceCulture);

	public static string FromSelectionMarkerLeftOutOfScopeButtonToolTip => ResourceManager.GetString("FromSelectionMarkerLeftOutOfScopeButtonToolTip", resourceCulture);

	public static string FromSelectionMarkerRightOutOfScopeButtonToolTip => ResourceManager.GetString("FromSelectionMarkerRightOutOfScopeButtonToolTip", resourceCulture);

	public static string SelectionInfoDuration => ResourceManager.GetString("SelectionInfoDuration", resourceCulture);

	public static string SelectionInfoEnd => ResourceManager.GetString("SelectionInfoEnd", resourceCulture);

	public static string SelectionInfoStart => ResourceManager.GetString("SelectionInfoStart", resourceCulture);

	public static string ToSelectionMarkerLeftOutOfScopeButtonToolTip => ResourceManager.GetString("ToSelectionMarkerLeftOutOfScopeButtonToolTip", resourceCulture);

	public static string ToSelectionMarkerRightOutOfScopeButtonToolTip => ResourceManager.GetString("ToSelectionMarkerRightOutOfScopeButtonToolTip", resourceCulture);

	public Strings()
	{
	}
}
