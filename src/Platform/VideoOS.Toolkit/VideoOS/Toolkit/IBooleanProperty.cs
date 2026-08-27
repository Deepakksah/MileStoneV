using System.Runtime.InteropServices;

namespace VideoOS.Toolkit;

public interface IBooleanProperty : IProperty
{
	bool Value
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get;
	}
}
