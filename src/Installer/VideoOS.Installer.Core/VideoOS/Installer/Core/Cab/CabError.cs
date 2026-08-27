using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

[StructLayout(LayoutKind.Sequential)]
public class CabError
{
	private int erfOper;

	private int erfType;

	private bool fError;

	public FDIError FdiErrorCode => (FDIError)erfOper;

	public int ErrorType
	{
		get
		{
			return erfType;
		}
		set
		{
			erfType = value;
		}
	}

	public bool HasError => fError;
}
