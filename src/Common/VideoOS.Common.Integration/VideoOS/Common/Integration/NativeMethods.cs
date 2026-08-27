using System;
using System.Runtime.InteropServices;

namespace VideoOS.Common.Integration;

public static class NativeMethods
{
	public struct SLensDescription
	{
		public IntPtr RPL;

		public IntPtr Name;
	}

	[DllImport("imv1.dll", EntryPoint = "?StaticGetLensDescriptionCount@IMV_CameraInterface@@SAHXZ", ExactSpelling = true)]
	public static extern int StaticGetLensDescriptionCount();

	[DllImport("imv1.dll", EntryPoint = "?StaticGetLensDescription@IMV_CameraInterface@@SAPEBUSLensDescription@@XZ", ExactSpelling = true)]
	public static extern IntPtr StaticGetLensDescription();
}
