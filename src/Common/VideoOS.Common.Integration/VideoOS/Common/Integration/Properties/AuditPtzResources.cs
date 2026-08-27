using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Common.Integration.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class AuditPtzResources
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
				resourceMan = new ResourceManager("VideoOS.Common.Integration.Properties.AuditPtzResources", typeof(AuditPtzResources).Assembly);
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

	public static string DirectionDown => ResourceManager.GetString("DirectionDown", resourceCulture);

	public static string DirectionLeft => ResourceManager.GetString("DirectionLeft", resourceCulture);

	public static string DirectionLeftDown => ResourceManager.GetString("DirectionLeftDown", resourceCulture);

	public static string DirectionLeftUp => ResourceManager.GetString("DirectionLeftUp", resourceCulture);

	public static string DirectionRight => ResourceManager.GetString("DirectionRight", resourceCulture);

	public static string DirectionRightDown => ResourceManager.GetString("DirectionRightDown", resourceCulture);

	public static string DirectionRightUp => ResourceManager.GetString("DirectionRightUp", resourceCulture);

	public static string DirectionUp => ResourceManager.GetString("DirectionUp", resourceCulture);

	public static string DirectionZoomIn => ResourceManager.GetString("DirectionZoomIn", resourceCulture);

	public static string DirectionZoomOut => ResourceManager.GetString("DirectionZoomOut", resourceCulture);

	public AuditPtzResources()
	{
	}
}
