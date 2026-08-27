using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using _003FA0x4cdda686;
using NmGenericByteData;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class CmManagedVideoDataAccessProxy
{
	[return: MarshalAs(UnmanagedType.U1)]
	protected delegate bool GetVideoDataDelegate(IntPtr ipTimeStamp, int timeMatch, int bSyncPointOnly, IntPtr ippGenericByteData);

	[return: MarshalAs(UnmanagedType.U1)]
	protected delegate bool HasVideoDataDelegate(IntPtr ipTimeStamp, int timeMatch, int bSyncPointOnly);

	[return: MarshalAs(UnmanagedType.U1)]
	protected delegate bool GetVideoDataTimeDelegate(IntPtr ipTimeStamp, int timeMatch, int bSyncPointOnly, IntPtr ipTimeStampOut);

	protected IVideoDataAccess m_pAccess;

	protected GetVideoDataDelegate m_GetVideoData;

	protected HasVideoDataDelegate m_HasVideoData;

	protected GetVideoDataTimeDelegate m_GetVideoDataTime;

	protected unsafe CmUnmanagedVideoDataAccessProxy* m_pUnmanagedProxy;

	protected Exception m_pException;

	protected GenericByteData m_pByteDataKeepAlive;

	public unsafe CmManagedVideoDataAccessProxy(IVideoDataAccess pAccess, CmUnmanagedVideoDataAccessProxy* pUnmanagedProxy)
	{
		//IL_00d5: Expected I8, but got I
		//IL_00e1: Expected I8, but got I
		//IL_00ed: Expected I8, but got I
		m_pAccess = pAccess;
		m_GetVideoData = null;
		m_HasVideoData = null;
		m_GetVideoDataTime = null;
		m_pUnmanagedProxy = pUnmanagedProxy;
		m_pException = null;
		m_pByteDataKeepAlive = null;
		base._002Ector();
		if (m_pAccess != null && m_pUnmanagedProxy != null)
		{
			m_GetVideoData = GetVideoData;
			m_HasVideoData = HasVideoData;
			m_GetVideoDataTime = GetVideoDataTime;
			IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate(m_GetVideoData);
			IntPtr functionPointerForDelegate2 = Marshal.GetFunctionPointerForDelegate(m_HasVideoData);
			IntPtr functionPointerForDelegate3 = Marshal.GetFunctionPointerForDelegate(m_GetVideoDataTime);
			delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, NmGenericByteData.CmGenericByteData**, byte> delegate_002A = (delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, NmGenericByteData.CmGenericByteData**, byte>)functionPointerForDelegate.ToPointer();
			delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, byte> delegate_002A2 = (delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, byte>)functionPointerForDelegate2.ToPointer();
			delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, NmTimeStamp.CmTimeStamp*, byte> delegate_002A3 = (delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, NmTimeStamp.CmTimeStamp*, byte>)functionPointerForDelegate3.ToPointer();
			if (delegate_002A != (delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, NmGenericByteData.CmGenericByteData**, byte>)null && delegate_002A2 != (delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, byte>)null && delegate_002A3 != (delegate* unmanaged[Cdecl, Cdecl]<NmTimeStamp.CmTimeStamp*, int, int, NmTimeStamp.CmTimeStamp*, byte>)null)
			{
				*(long*)((ulong)(nint)m_pUnmanagedProxy + 8uL) = (nint)delegate_002A;
				*(long*)((ulong)(nint)m_pUnmanagedProxy + 16uL) = (nint)delegate_002A2;
				*(long*)((ulong)(nint)m_pUnmanagedProxy + 24uL) = (nint)delegate_002A3;
				GC.KeepAlive(this);
				return;
			}
			throw new ArgumentNullException("Unable to get native function pointers for delegates");
		}
		throw new ArgumentNullException("Video data access has not been set");
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetVideoData(IntPtr ipTimeStamp, int timeMatch, int bSyncPointOnly, IntPtr ippGenericByteData)
	{
		//IL_000b: Expected I8, but got I
		//IL_0196: Expected I, but got I8
		//IL_015c: Expected I, but got I8
		//IL_0084: Expected I, but got I8
		//IL_00fb: Expected I, but got I8
		//IL_011f: Expected I, but got I8
		//IL_0132: Expected I8, but got I
		GenericByteData genericByteData = null;
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pException != null)
		{
			return false;
		}
		if (m_pAccess != null && m_pUnmanagedProxy != null)
		{
			if (global::_003CModule_003E.IsBadReadPtr(ipTimeStamp.ToPointer(), 8uL) != 0)
			{
				throw new ApplicationException("Invalid pointer in callback");
			}
			NmTimeStamp.CmTimeStamp* stamp = (NmTimeStamp.CmTimeStamp*)ipTimeStamp.ToPointer();
			if (global::_003CModule_003E.IsBadReadPtr(ippGenericByteData.ToPointer(), 8uL) != 0)
			{
				throw new ApplicationException("Invalid pointer in callback");
			}
			NmGenericByteData.CmGenericByteData** ptr = (NmGenericByteData.CmGenericByteData**)ippGenericByteData.ToPointer();
			ulong num2 = *(ulong*)ptr;
			if (num2 != 0L && global::_003CModule_003E.IsBadReadPtr((void*)num2, 96uL) != 0)
			{
				throw new ApplicationException("Invalid pointer reference in callback");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				genericByteData = null;
				IVideoDataAccess pAccess = m_pAccess;
				byte bSyncPointOnly2 = (byte)((bSyncPointOnly != 0) ? 1 : 0);
				DateTime dateTime = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(stamp);
				DateTime dateTime2 = dateTime;
				if (!pAccess.GetVideoData(dateTime, (IVideoDataAccess.matchCriterion)timeMatch, bSyncPointOnly2 != 0, ref genericByteData))
				{
					GC.KeepAlive(this);
					return false;
				}
				if (genericByteData == null)
				{
					throw new ApplicationException("IVideoDataAccess::GetVideoData has provided no video data");
				}
				m_pByteDataKeepAlive = genericByteData;
				if (*(long*)ptr != 0L)
				{
					NmGenericByteData.CmGenericByteData* native = genericByteData.GetNative();
					global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002E_003D((NmGenericByteData.CmGenericByteData*)(*(ulong*)ptr), native);
				}
				else
				{
					NmGenericByteData.CmGenericByteData* ptr2 = (NmGenericByteData.CmGenericByteData*)global::_003CModule_003E.@new(96uL);
					NmGenericByteData.CmGenericByteData* ptr3;
					try
					{
						ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002E_007Bctor_007D(ptr2, genericByteData.GetNative()));
						NmGenericByteData.CmGenericByteData* ptr4 = ptr3;
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.delete(ptr2, 96uL);
						throw;
					}
					*(long*)ptr = (nint)ptr3;
				}
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num3 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						m_pException = new Error((exception*)error);
						GC.KeepAlive(this);
						return false;
					}
					catch when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						num3 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num3 != 0;
					}).Invoke())
					{
					}
					if (num3 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num3);
				}
			}
			catch (Exception pException)
			{
				m_pException = pException;
				GC.KeepAlive(this);
				return false;
			}
			return true;
		}
		throw new InvalidOperationException();
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool HasVideoData(IntPtr ipTimeStamp, int timeMatch, int bSyncPointOnly)
	{
		//IL_0008: Expected I8, but got I
		//IL_00de: Expected I, but got I8
		//IL_00a5: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pException != null)
		{
			return false;
		}
		if (m_pAccess != null && m_pUnmanagedProxy != null)
		{
			if (global::_003CModule_003E.IsBadReadPtr(ipTimeStamp.ToPointer(), 8uL) != 0)
			{
				throw new ApplicationException("Invalid pointer in callback");
			}
			NmTimeStamp.CmTimeStamp* stamp = (NmTimeStamp.CmTimeStamp*)ipTimeStamp.ToPointer();
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				IVideoDataAccess pAccess = m_pAccess;
				byte bSyncPointOnly2 = (byte)((bSyncPointOnly != 0) ? 1 : 0);
				DateTime dateTime = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(stamp);
				DateTime dateTime2 = dateTime;
				bool result = pAccess.HasVideoData(dateTime, (IVideoDataAccess.matchCriterion)timeMatch, bSyncPointOnly2 != 0);
				GC.KeepAlive(this);
				return result;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						m_pException = new Error((exception*)error);
						GC.KeepAlive(this);
						return false;
					}
					catch when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num2 != 0;
					}).Invoke())
					{
					}
					if (num2 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
				}
			}
			catch (Exception pException)
			{
				m_pException = pException;
				GC.KeepAlive(this);
				return false;
			}
			return true;
		}
		throw new InvalidOperationException();
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetVideoDataTime(IntPtr ipTimeStamp, int timeMatch, int bSyncPointOnly, IntPtr ipTimeStampOut)
	{
		//IL_0008: Expected I8, but got I
		//IL_013e: Expected I, but got I8
		//IL_0105: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pException != null)
		{
			return false;
		}
		if (m_pAccess != null && m_pUnmanagedProxy != null)
		{
			if (global::_003CModule_003E.IsBadReadPtr(ipTimeStamp.ToPointer(), 8uL) != 0)
			{
				throw new ApplicationException("Invalid pointer in callback");
			}
			NmTimeStamp.CmTimeStamp* stamp = (NmTimeStamp.CmTimeStamp*)ipTimeStamp.ToPointer();
			if (global::_003CModule_003E.IsBadWritePtr(ipTimeStampOut.ToPointer(), 8uL) != 0)
			{
				throw new ApplicationException("Invalid pointer in callback");
			}
			NmTimeStamp.CmTimeStamp* ptr = (NmTimeStamp.CmTimeStamp*)ipTimeStampOut.ToPointer();
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				DateTime rTimeStamp = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(ptr);
				IVideoDataAccess pAccess = m_pAccess;
				byte bSyncPointOnly2 = (byte)((bSyncPointOnly != 0) ? 1 : 0);
				DateTime dateTime = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(stamp);
				DateTime dateTime2 = dateTime;
				if (!pAccess.GetVideoDataTime(dateTime, (IVideoDataAccess.matchCriterion)timeMatch, bSyncPointOnly2 != 0, ref rTimeStamp))
				{
					GC.KeepAlive(this);
					return false;
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				NmTimeStamp.CmTimeStamp* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &rTimeStamp);
				try
				{
					// IL cpblk instruction
					System.Runtime.CompilerServices.Unsafe.CopyBlockUnaligned(ptr, ptr2, 8);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						m_pException = new Error((exception*)error);
						GC.KeepAlive(this);
						return false;
					}
					catch when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num2 != 0;
					}).Invoke())
					{
					}
					if (num2 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
				}
			}
			catch (Exception pException)
			{
				m_pException = pException;
				GC.KeepAlive(this);
				return false;
			}
			return true;
		}
		throw new InvalidOperationException();
	}

	public void ThrowPendingExceptionIfAny()
	{
		Exception pException = m_pException;
		if (pException != null)
		{
			m_pException = null;
			throw pException;
		}
	}
}
