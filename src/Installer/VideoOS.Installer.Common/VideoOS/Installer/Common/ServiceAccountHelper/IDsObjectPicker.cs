using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

[ComImport]
[Guid("0C87E64E-3B7A-11D2-B9E0-00C04FD8DBF7")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IDsObjectPicker
{
	void Initialize(ref DSOP_INIT_INFO pInitInfo);

	void InvokeDialog(IntPtr HWND, out IDataObject lpDataObject);
}
