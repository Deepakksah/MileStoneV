using System;

namespace VideoOS.Toolkit;

public interface IProperty
{
	byte[] GetData();

	IntPtr GetNativeData(out ulong length);
}
