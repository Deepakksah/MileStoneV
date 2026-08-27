using System;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public class PVMarshaler : ICustomMarshaler
{
	protected PropVariant m_prop;

	public IntPtr MarshalManagedToNative(object managedObj)
	{
		m_prop = managedObj as PropVariant;
		IntPtr intPtr;
		if (m_prop != null)
		{
			m_prop.Clear();
			intPtr = Marshal.AllocCoTaskMem(GetNativeDataSize());
			if (IntPtr.Size == 4)
			{
				Marshal.WriteInt64(intPtr, 0L);
				Marshal.WriteInt64(intPtr, 8, 0L);
			}
			else
			{
				Marshal.WriteInt64(intPtr, 0L);
				Marshal.WriteInt64(intPtr, 8, 0L);
				Marshal.WriteInt64(intPtr, 16, 0L);
			}
		}
		else
		{
			intPtr = IntPtr.Zero;
		}
		return intPtr;
	}

	public object MarshalNativeToManaged(IntPtr pNativeData)
	{
		Marshal.PtrToStructure(pNativeData, m_prop);
		m_prop = null;
		return m_prop;
	}

	public void CleanUpManagedData(object ManagedObj)
	{
		m_prop = null;
	}

	public void CleanUpNativeData(IntPtr pNativeData)
	{
		Marshal.FreeCoTaskMem(pNativeData);
	}

	public int GetNativeDataSize()
	{
		return Marshal.SizeOf(typeof(PropVariant));
	}

	public static ICustomMarshaler GetInstance(string cookie)
	{
		return new PVMarshaler();
	}
}
