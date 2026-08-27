using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class SimpleMotionRegion : IDisposable
{
	protected unsafe CmSimpleImageRegion* m_pRegion;

	public unsafe Position Right
	{
		get
		{
			Position result = new Position(global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002EGetRight(GetNative()));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0075: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmSimpleImageRegion* native = GetNative();
				CmSimpleImageRegion.CmPosition* native2 = value.GetNative();
				global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ESetRight(native, native2);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error);
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
			GC.KeepAlive(value);
			GC.KeepAlive(this);
		}
	}

	public unsafe Position Left
	{
		get
		{
			Position result = new Position(global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002EGetLeft(GetNative()));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0075: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmSimpleImageRegion* native = GetNative();
				CmSimpleImageRegion.CmPosition* native2 = value.GetNative();
				global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ESetLeft(native, native2);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error);
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
			GC.KeepAlive(value);
			GC.KeepAlive(this);
		}
	}

	public unsafe Position Bottom
	{
		get
		{
			Position result = new Position(global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002EGetBottom(GetNative()));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0075: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmSimpleImageRegion* native = GetNative();
				CmSimpleImageRegion.CmPosition* native2 = value.GetNative();
				global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ESetBottom(native, native2);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error);
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
			GC.KeepAlive(value);
			GC.KeepAlive(this);
		}
	}

	public unsafe Position Top
	{
		get
		{
			Position result = new Position(global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002EGetTop(GetNative()));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0075: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmSimpleImageRegion* native = GetNative();
				CmSimpleImageRegion.CmPosition* native2 = value.GetNative();
				global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ESetTop(native, native2);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error);
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
			GC.KeepAlive(value);
			GC.KeepAlive(this);
		}
	}

	public unsafe SimpleMotionRegion(CmSimpleImageRegion* region)
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0087: Expected I, but got I8
		//IL_005c: Expected I, but got I8
		//IL_002f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pRegion = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleImageRegion* ptr = (CmSimpleImageRegion*)global::_003CModule_003E.@new(64uL);
			CmSimpleImageRegion* pRegion;
			if (ptr != null)
			{
				// IL cpblk instruction
				System.Runtime.CompilerServices.Unsafe.CopyBlock(ptr, region, 64);
				pRegion = ptr;
			}
			else
			{
				pRegion = null;
			}
			m_pRegion = pRegion;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
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
	}

	public unsafe SimpleMotionRegion()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0095: Expected I, but got I8
		//IL_006a: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pRegion = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleImageRegion* ptr = (CmSimpleImageRegion*)global::_003CModule_003E.@new(64uL);
			CmSimpleImageRegion* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002E_007Bctor_007D(ptr));
				CmSimpleImageRegion* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 64uL);
				throw;
			}
			m_pRegion = ptr2;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
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
	}

	private void _007ESimpleMotionRegion()
	{
		_0021SimpleMotionRegion();
	}

	private unsafe void _0021SimpleMotionRegion()
	{
		//IL_0016: Expected I, but got I8
		global::_003CModule_003E.delete(m_pRegion, 64uL);
		m_pRegion = null;
	}

	public unsafe uint GetTop(Image image)
	{
		//IL_0008: Expected I8, but got I
		//IL_007a: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (image == null)
		{
			throw new ArgumentNullException();
		}
		uint result = 0u;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleImageRegion* native = GetNative();
			CmImage* native2 = image.GetNative();
			result = (uint)global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002EGetTop(native, native2);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
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
		GC.KeepAlive(image);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe uint GetBottom(Image image)
	{
		//IL_0008: Expected I8, but got I
		//IL_007a: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (image == null)
		{
			throw new ArgumentNullException();
		}
		uint result = 0u;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleImageRegion* native = GetNative();
			CmImage* native2 = image.GetNative();
			result = (uint)global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002EGetBottom(native, native2);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
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
		GC.KeepAlive(image);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe uint GetLeft(Image image)
	{
		//IL_0008: Expected I8, but got I
		//IL_007a: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (image == null)
		{
			throw new ArgumentNullException();
		}
		uint result = 0u;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleImageRegion* native = GetNative();
			CmImage* native2 = image.GetNative();
			result = (uint)global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002EGetLeft(native, native2);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
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
		GC.KeepAlive(image);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe uint GetRight(Image image)
	{
		//IL_0008: Expected I8, but got I
		//IL_007a: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (image == null)
		{
			throw new ArgumentNullException();
		}
		uint result = 0u;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleImageRegion* native = GetNative();
			CmImage* native2 = image.GetNative();
			result = (uint)global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002EGetRight(native, native2);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
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
		GC.KeepAlive(image);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe CmSimpleImageRegion* GetNative()
	{
		CmSimpleImageRegion* pRegion = m_pRegion;
		if (pRegion == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.SimpleMotionRegion");
		}
		return pRegion;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007ESimpleMotionRegion();
			return;
		}
		try
		{
			_0021SimpleMotionRegion();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(A_0: true);
		GC.SuppressFinalize(this);
		GC.KeepAlive(this);
	}

	~SimpleMotionRegion()
	{
		Dispose(A_0: false);
	}
}
