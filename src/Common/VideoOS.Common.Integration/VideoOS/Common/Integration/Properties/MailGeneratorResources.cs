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
public class MailGeneratorResources
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
				resourceMan = new ResourceManager("VideoOS.Common.Integration.Properties.MailGeneratorResources", typeof(MailGeneratorResources).Assembly);
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

	public static string Mail1 => ResourceManager.GetString("Mail1", resourceCulture);

	public static string Mail2 => ResourceManager.GetString("Mail2", resourceCulture);

	public static string Mail3 => ResourceManager.GetString("Mail3", resourceCulture);

	public static string MailMediaErrorCodecDoesNotSupportThisMedia => ResourceManager.GetString("MailMediaErrorCodecDoesNotSupportThisMedia", resourceCulture);

	public static string MailMediaErrorConnectionBroken => ResourceManager.GetString("MailMediaErrorConnectionBroken", resourceCulture);

	public static string MailMediaErrorInConfiguration => ResourceManager.GetString("MailMediaErrorInConfiguration", resourceCulture);

	public static string MailMediaErrorNoCodecSelected => ResourceManager.GetString("MailMediaErrorNoCodecSelected", resourceCulture);

	public static string MailMediaErrorNoConfiguration => ResourceManager.GetString("MailMediaErrorNoConfiguration", resourceCulture);

	public static string MailMediaErrorNoImagesAvailable => ResourceManager.GetString("MailMediaErrorNoImagesAvailable", resourceCulture);

	public static string MailMediaErrorUnknownError => ResourceManager.GetString("MailMediaErrorUnknownError", resourceCulture);

	public static string Undefined => ResourceManager.GetString("Undefined", resourceCulture);

	public MailGeneratorResources()
	{
	}
}
