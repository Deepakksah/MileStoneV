using System;

namespace VideoOS.Toolkit;

public interface IData : IDisposable
{
	DateTime EndTimeStamp { get; }

	DateTime BeginTimeStamp { get; }

	byte[] GetData();

	IntPtr GetNativeData(out ulong length);

	IProperty GetProperty(string propertyName);

	IData Clone();
}
