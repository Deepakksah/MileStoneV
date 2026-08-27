using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.UI.Mobile;

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
				resourceMan = new ResourceManager("VideoOS.Mobile.UI.Mobile.Strings", typeof(Strings).Assembly);
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

	public static string CertificateSelectionControl_ChooseCertificateWindowMessage => ResourceManager.GetString("CertificateSelectionControl_ChooseCertificateWindowMessage", resourceCulture);

	public static string CertificateSelectionControl_ChooseCertificateWindowTitle => ResourceManager.GetString("CertificateSelectionControl_ChooseCertificateWindowTitle", resourceCulture);

	public static string CertificateSelectionControl_Selection_NoSelectionText => ResourceManager.GetString("CertificateSelectionControl_Selection_NoSelectionText", resourceCulture);

	public static string DUMMY_TEST_STRING => ResourceManager.GetString("DUMMY_TEST_STRING", resourceCulture);

	public Strings()
	{
	}
}
