using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup.resources;

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
				resourceMan = new ResourceManager("VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup.resources.Strings", typeof(Strings).Assembly);
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

	public static string GoToButtonLabel => ResourceManager.GetString("GoToButtonLabel", resourceCulture);

	public static string OKButtonLabel => ResourceManager.GetString("OKButtonLabel", resourceCulture);

	public static string SelectTimeHeaderCenterTime => ResourceManager.GetString("SelectTimeHeaderCenterTime", resourceCulture);

	public static string SelectTimeHeaderFromTime => ResourceManager.GetString("SelectTimeHeaderFromTime", resourceCulture);

	public static string SelectTimeHeaderSelectTime => ResourceManager.GetString("SelectTimeHeaderSelectTime", resourceCulture);

	public static string SelectTimeHeaderToTime => ResourceManager.GetString("SelectTimeHeaderToTime", resourceCulture);

	public Strings()
	{
	}
}
