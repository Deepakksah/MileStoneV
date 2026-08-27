using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Platform.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Images
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
				resourceMan = new ResourceManager("VideoOS.Platform.Resources.Images", typeof(Images).Assembly);
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

	public static Icon Attention16ni8 => (Icon)ResourceManager.GetObject("Attention16ni8", resourceCulture);

	public static Icon Disabled16ni8 => (Icon)ResourceManager.GetObject("Disabled16ni8", resourceCulture);

	public static Icon Running16ni8 => (Icon)ResourceManager.GetObject("Running16ni8", resourceCulture);

	public static Icon SDK_general => (Icon)ResourceManager.GetObject("SDK_general", resourceCulture);

	public Images()
	{
	}
}
