using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.resources;

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
				resourceMan = new ResourceManager("VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.resources.Strings", typeof(Strings).Assembly);
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

	public static string LegendTitle => ResourceManager.GetString("LegendTitle", resourceCulture);

	public static string LegendTooltip => ResourceManager.GetString("LegendTooltip", resourceCulture);

	public static string PlaySelectionContextMenuItemText => ResourceManager.GetString("PlaySelectionContextMenuItemText", resourceCulture);

	public static string SelectSelectionCenterTimeContextMenuItemText => ResourceManager.GetString("SelectSelectionCenterTimeContextMenuItemText", resourceCulture);

	public static string SelectSelectionFromTimeContextMenuItemText => ResourceManager.GetString("SelectSelectionFromTimeContextMenuItemText", resourceCulture);

	public static string SelectSelectionToTimeContextMenuItemText => ResourceManager.GetString("SelectSelectionToTimeContextMenuItemText", resourceCulture);

	public static string SelectTimeContextMenuItemText => ResourceManager.GetString("SelectTimeContextMenuItemText", resourceCulture);

	public static string SetSelectionCenterContextMenuItemText => ResourceManager.GetString("SetSelectionCenterContextMenuItemText", resourceCulture);

	public static string SetSelectionFromContextMenuItemText => ResourceManager.GetString("SetSelectionFromContextMenuItemText", resourceCulture);

	public static string SetSelectionToContextMenuItemText => ResourceManager.GetString("SetSelectionToContextMenuItemText", resourceCulture);

	public static string SpanContextMenuItemText => ResourceManager.GetString("SpanContextMenuItemText", resourceCulture);

	public static string ToggleTimeSelectionStateContextMenuItem => ResourceManager.GetString("ToggleTimeSelectionStateContextMenuItem", resourceCulture);

	public Strings()
	{
	}
}
