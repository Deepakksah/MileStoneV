using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CLRMetaHost;

[ComImport]
[CompilerGenerated]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("9FD93CCF-3280-4391-B3A9-96E1CDE77C8D")]
[TypeIdentifier]
public interface ICLRStrongName
{
	void _VtblGap1_20();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	sbyte StrongNameSignatureVerificationEx([In][MarshalAs(UnmanagedType.LPWStr)] string pwzFilePath, [In] sbyte fForceVerification);
}
