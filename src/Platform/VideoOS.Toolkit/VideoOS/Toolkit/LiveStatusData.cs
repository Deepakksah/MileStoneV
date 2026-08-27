using System;
using System.Runtime.CompilerServices;
using NmToolkit;

namespace VideoOS.Toolkit;

public unsafe class LiveStatusData(ImLiveStatusData* data) : OutOfBandData((ImData*)data), ILiveStatusData
{
	private unsafe ImLiveStatusData* m_pNativeData = data;

	public virtual DateTime GetStatusTime()
	{
		return BeginTimeStamp;
	}

	public unsafe virtual ILiveStatusData.StatusValue GetCameraLiveFeedStarted()
	{
		//IL_0018: Expected I, but got I8
		ImLiveStatusData* pNativeData = m_pNativeData;
		long num = *(long*)(*(long*)pNativeData + 64);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, byte>)num)((nint)pNativeData, &flag) != 0)
		{
			GC.KeepAlive(this);
			return flag ? ILiveStatusData.StatusValue.True : ILiveStatusData.StatusValue.False;
		}
		return ILiveStatusData.StatusValue.NotAvailable;
	}

	public unsafe virtual ILiveStatusData.StatusValue GetCameraLiveMotion()
	{
		//IL_0018: Expected I, but got I8
		ImLiveStatusData* pNativeData = m_pNativeData;
		long num = *(long*)(*(long*)pNativeData + 72);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, byte>)num)((nint)pNativeData, &flag) != 0)
		{
			GC.KeepAlive(this);
			return flag ? ILiveStatusData.StatusValue.True : ILiveStatusData.StatusValue.False;
		}
		return ILiveStatusData.StatusValue.NotAvailable;
	}

	public unsafe virtual ILiveStatusData.StatusValue GetCameraRecording()
	{
		//IL_0018: Expected I, but got I8
		ImLiveStatusData* pNativeData = m_pNativeData;
		long num = *(long*)(*(long*)pNativeData + 80);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, byte>)num)((nint)pNativeData, &flag) != 0)
		{
			GC.KeepAlive(this);
			return flag ? ILiveStatusData.StatusValue.True : ILiveStatusData.StatusValue.False;
		}
		return ILiveStatusData.StatusValue.NotAvailable;
	}

	public unsafe virtual ILiveStatusData.StatusValue GetCameraEventNotification()
	{
		//IL_0018: Expected I, but got I8
		ImLiveStatusData* pNativeData = m_pNativeData;
		long num = *(long*)(*(long*)pNativeData + 88);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, byte>)num)((nint)pNativeData, &flag) != 0)
		{
			GC.KeepAlive(this);
			return flag ? ILiveStatusData.StatusValue.True : ILiveStatusData.StatusValue.False;
		}
		return ILiveStatusData.StatusValue.NotAvailable;
	}

	public unsafe virtual ILiveStatusData.StatusValue GetCameraConnectionLost()
	{
		//IL_0018: Expected I, but got I8
		ImLiveStatusData* pNativeData = m_pNativeData;
		long num = *(long*)(*(long*)pNativeData + 96);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, byte>)num)((nint)pNativeData, &flag) != 0)
		{
			GC.KeepAlive(this);
			return flag ? ILiveStatusData.StatusValue.True : ILiveStatusData.StatusValue.False;
		}
		return ILiveStatusData.StatusValue.NotAvailable;
	}

	public unsafe virtual ILiveStatusData.StatusValue GetCameraDatabaseFail()
	{
		//IL_0018: Expected I, but got I8
		ImLiveStatusData* pNativeData = m_pNativeData;
		long num = *(long*)(*(long*)pNativeData + 104);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, byte>)num)((nint)pNativeData, &flag) != 0)
		{
			GC.KeepAlive(this);
			return flag ? ILiveStatusData.StatusValue.True : ILiveStatusData.StatusValue.False;
		}
		return ILiveStatusData.StatusValue.NotAvailable;
	}

	public unsafe virtual ILiveStatusData.StatusValue GetServerRunningOutOfDiskSpace()
	{
		//IL_0018: Expected I, but got I8
		ImLiveStatusData* pNativeData = m_pNativeData;
		long num = *(long*)(*(long*)pNativeData + 112);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, byte>)num)((nint)pNativeData, &flag) != 0)
		{
			GC.KeepAlive(this);
			return flag ? ILiveStatusData.StatusValue.True : ILiveStatusData.StatusValue.False;
		}
		return ILiveStatusData.StatusValue.NotAvailable;
	}

	public unsafe virtual ILiveStatusData.StatusValue GetClientLiveFeedStopped()
	{
		//IL_0018: Expected I, but got I8
		ImLiveStatusData* pNativeData = m_pNativeData;
		long num = *(long*)(*(long*)pNativeData + 120);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, byte>)num)((nint)pNativeData, &flag) != 0)
		{
			GC.KeepAlive(this);
			return flag ? ILiveStatusData.StatusValue.True : ILiveStatusData.StatusValue.False;
		}
		return ILiveStatusData.StatusValue.NotAvailable;
	}
}
