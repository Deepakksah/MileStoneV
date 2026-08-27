using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using _003FA0x4cdda686;
using NmError;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class CmManagedImageConsumerProxy
{
	protected delegate void OnImageDelegate(IntPtr ipImage);

	protected delegate void OnErrorDelegate();

	protected delegate void OnExceptionDelegate(IntPtr ipException);

	protected IImageConsumer m_pConsumer;

	protected OnImageDelegate m_OnImage;

	protected OnErrorDelegate m_OnError;

	protected OnExceptionDelegate m_OnException;

	protected unsafe CmUnmanagedImageConsumerProxy* m_pUnmanagedProxy;

	protected Exception m_pException;

	public unsafe CmManagedImageConsumerProxy(IImageConsumer pConsumer, CmUnmanagedImageConsumerProxy* pUnmanagedProxy)
	{
		//IL_00ce: Expected I8, but got I
		//IL_00da: Expected I8, but got I
		//IL_00e6: Expected I8, but got I
		m_pConsumer = pConsumer;
		m_OnImage = null;
		m_OnError = null;
		m_OnException = null;
		m_pUnmanagedProxy = pUnmanagedProxy;
		m_pException = null;
		base._002Ector();
		if (m_pConsumer != null && m_pUnmanagedProxy != null)
		{
			m_OnImage = OnImage;
			m_OnError = OnError;
			m_OnException = OnException;
			IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate(m_OnImage);
			IntPtr functionPointerForDelegate2 = Marshal.GetFunctionPointerForDelegate(m_OnError);
			IntPtr functionPointerForDelegate3 = Marshal.GetFunctionPointerForDelegate(m_OnException);
			delegate* unmanaged[Cdecl, Cdecl]<CmImage*, void> delegate_002A = (delegate* unmanaged[Cdecl, Cdecl]<CmImage*, void>)functionPointerForDelegate.ToPointer();
			delegate* unmanaged[Cdecl, Cdecl]<void> delegate_002A2 = (delegate* unmanaged[Cdecl, Cdecl]<void>)functionPointerForDelegate2.ToPointer();
			delegate* unmanaged[Cdecl, Cdecl]<CmError*, void> delegate_002A3 = (delegate* unmanaged[Cdecl, Cdecl]<CmError*, void>)functionPointerForDelegate3.ToPointer();
			if (delegate_002A != (delegate* unmanaged[Cdecl, Cdecl]<CmImage*, void>)null && delegate_002A2 != (delegate* unmanaged[Cdecl, Cdecl]<void>)null && delegate_002A3 != (delegate* unmanaged[Cdecl, Cdecl]<CmError*, void>)null)
			{
				*(long*)((ulong)(nint)m_pUnmanagedProxy + 8uL) = (nint)delegate_002A;
				*(long*)((ulong)(nint)m_pUnmanagedProxy + 16uL) = (nint)delegate_002A2;
				*(long*)((ulong)(nint)m_pUnmanagedProxy + 24uL) = (nint)delegate_002A3;
				GC.KeepAlive(this);
				return;
			}
			throw new ArgumentNullException("Unable to get native function pointers for delegates");
		}
		throw new ArgumentNullException("Consumer has not been set");
	}

	public unsafe void OnImage(IntPtr ipImage)
	{
		//IL_0009: Expected I8, but got I
		//IL_00fc: Expected I, but got I8
		//IL_00cb: Expected I, but got I8
		//IL_0084: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pConsumer != null && m_pUnmanagedProxy != null)
		{
			if (global::_003CModule_003E.IsBadReadPtr(ipImage.ToPointer(), 120uL) != 0)
			{
				throw new ApplicationException("Invalid pointer in callback");
			}
			CmImage* ptr = (CmImage*)global::_003CModule_003E.__RTDynamicCast(ipImage.ToPointer(), 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmBaseImage_0040NmMediaProcessing_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmImage_0040NmMediaProcessing_0040_0040_00408), 0);
			if (ptr == null)
			{
				throw new ApplicationException("Invalid type in callback");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				IImageConsumer pConsumer = m_pConsumer;
				CmImage* ptr2 = (CmImage*)global::_003CModule_003E.@new(120uL);
				CmImage* ptr3;
				try
				{
					ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bctor_007D(ptr2, ptr));
					CmImage* ptr4 = ptr3;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr2, 120uL);
					throw;
				}
				Image image = new Image(ptr3);
				pConsumer.OnImage(image);
				return;
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
						return;
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
					return;
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
				}
			}
			catch (Exception pException)
			{
				m_pException = pException;
				return;
			}
		}
		throw new InvalidOperationException();
	}

	public unsafe void OnError()
	{
		if (m_pConsumer != null && m_pUnmanagedProxy != null)
		{
			try
			{
				m_pConsumer.OnError();
				return;
			}
			catch (Exception pException)
			{
				m_pException = pException;
				return;
			}
		}
		throw new InvalidOperationException();
	}

	public unsafe void OnException(IntPtr ipException)
	{
		//IL_0008: Expected I8, but got I
		//IL_00ae: Expected I, but got I8
		//IL_007f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pConsumer != null && m_pUnmanagedProxy != null)
		{
			if (global::_003CModule_003E.IsBadReadPtr(ipException.ToPointer(), 24uL) != 0)
			{
				throw new ApplicationException("Invalid pointer in callback");
			}
			exception* error = (exception*)ipException.ToPointer();
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				IImageConsumer pConsumer = m_pConsumer;
				Error ex = new Error((exception*)error);
				pConsumer.OnException(ex);
				return;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						m_pException = new Error((exception*)error2);
						return;
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
					return;
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
				}
			}
			catch (Exception pException)
			{
				m_pException = pException;
				return;
			}
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
