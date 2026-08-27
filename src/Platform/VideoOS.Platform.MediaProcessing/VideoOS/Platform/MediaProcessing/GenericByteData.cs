using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmBuffers;
using NmError;
using NmGenericByteData;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class GenericByteData : IDisposable
{
	private ReadOnlySharedBuffer m_data;

	private ReadOnlySharedBuffer m_dataBody;

	private unsafe NmGenericByteData.CmGenericByteData* m_pByteData;

	private unsafe CmRawVideoByteDataAccess* m_pAccess;

	public int PicHeight
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public int PicWidth
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public unsafe int FieldFormat
	{
		get
		{
			_GenericVideoData_CodedFieldFormat_e result = global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EGetFieldFormat(GetAccess());
			GC.KeepAlive(this);
			return (int)result;
		}
	}

	public unsafe int CodecType
	{
		get
		{
			NmGenericByteData._GenericVideoStreamPacket_CodecTypes_e result = global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EGetCodecType(GetAccess());
			GC.KeepAlive(this);
			return (int)result;
		}
	}

	public unsafe bool TopFieldFirst
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EIsTopFieldFirst(GetAccess());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe bool Interlaced
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EIsInterlaced(GetAccess());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe bool Rotated90Right
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool flag = false;
			bool flag2 = false;
			bool result = false;
			if (!global::_003CModule_003E.NmGenericByteData_002EConvertCodedPictureOrientationEnumToOperations(global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EGetOrientation(GetAccess()), &flag2, &flag, &result))
			{
				throw new InvalidOperationException();
			}
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe bool FlippedHorizontally
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = false;
			bool flag = false;
			bool flag2 = false;
			if (!global::_003CModule_003E.NmGenericByteData_002EConvertCodedPictureOrientationEnumToOperations(global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EGetOrientation(GetAccess()), &flag, &result, &flag2))
			{
				throw new InvalidOperationException();
			}
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe bool FlippedVertically
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool flag = false;
			bool result = false;
			bool flag2 = false;
			if (!global::_003CModule_003E.NmGenericByteData_002EConvertCodedPictureOrientationEnumToOperations(global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EGetOrientation(GetAccess()), &result, &flag, &flag2))
			{
				throw new InvalidOperationException();
			}
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe bool SyncPoint
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EIsSyncPoint(GetAccess());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe virtual ReadOnlySharedBuffer BodyData
	{
		get
		{
			ReadOnlySharedBuffer dataBody = m_dataBody;
			if (null != dataBody)
			{
				return dataBody;
			}
			ReadOnlySharedBuffer data = GetData();
			NmGenericByteData.CmGenericByteData* native = GetNative();
			m_dataBody = new ReadOnlySharedBuffer(data, global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetBodyDataPtr(native), global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetBodyLength(native));
			GC.KeepAlive(this);
			return m_dataBody;
		}
	}

	public virtual ReadOnlySharedBuffer Data
	{
		get
		{
			ReadOnlySharedBuffer data = GetData();
			GC.KeepAlive(this);
			return data;
		}
	}

	public virtual ulong Length
	{
		get
		{
			ulong length = GetData().Length;
			GC.KeepAlive(this);
			return length;
		}
	}

	public unsafe GenericByteData(CmUniqueBuffer* buffer)
	{
		//IL_001b: Expected I, but got I8
		//IL_0023: Expected I, but got I8
		m_data = new ReadOnlySharedBuffer((CmUniqueBuffer*)buffer);
		m_dataBody = null;
		m_pByteData = null;
		m_pAccess = null;
		base._002Ector();
		if (0L != (nint)m_data.Data && m_data.Length != 0)
		{
			GC.KeepAlive(this);
			return;
		}
		throw new ArgumentException("Buffer without data passed as argument.", "buffer");
	}

	public unsafe GenericByteData(ReadOnlySharedBuffer data)
	{
		//IL_001b: Expected I, but got I8
		//IL_0023: Expected I, but got I8
		m_data = new ReadOnlySharedBuffer(data);
		m_dataBody = null;
		m_pByteData = null;
		m_pAccess = null;
		base._002Ector();
		if (null == data)
		{
			throw new ArgumentNullException("data");
		}
		if (0L != (nint)data.Data && data.Length != 0)
		{
			GC.KeepAlive(this);
			return;
		}
		throw new ArgumentException("Buffer without data passed as argument.", "data");
	}

	private void _007EGenericByteData()
	{
		((IDisposable)m_dataBody)?.Dispose();
		m_dataBody = null;
		((IDisposable)m_data)?.Dispose();
		m_data = null;
		_0021GenericByteData();
		GC.KeepAlive(this);
	}

	private unsafe void _0021GenericByteData()
	{
		//IL_0021: Expected I, but got I8
		//IL_0040: Expected I, but got I8
		//IL_0037: Expected I, but got I8
		CmRawVideoByteDataAccess* pAccess = m_pAccess;
		if (pAccess != null)
		{
			global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002E_007Bdtor_007D(pAccess);
			global::_003CModule_003E.delete(pAccess, 72uL);
		}
		m_pAccess = null;
		NmGenericByteData.CmGenericByteData* pByteData = m_pByteData;
		if (pByteData != null)
		{
			long num = *(long*)(*(ulong*)pByteData);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pByteData, 1u);
		}
		m_pByteData = null;
	}

	public unsafe NmGenericByteData.CmGenericByteData* GetNative()
	{
		//IL_0009: Expected I8, but got I
		//IL_0464: Expected I, but got I8
		//IL_0438: Expected I, but got I8
		//IL_006e: Expected I, but got I8
		//IL_02a6: Expected I, but got I8
		//IL_03cd: Expected I, but got I8
		//IL_03d1: Expected I, but got I8
		//IL_0289: Expected I, but got I8
		//IL_00e9: Expected I, but got I8
		//IL_0179: Expected I, but got I8
		//IL_01ce: Expected I, but got I8
		//IL_0222: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pByteData == null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ReadOnlySharedBuffer data = GetData();
				ulong length = data.Length;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmGenericByteData.CmGenericByteData cmGenericByteData);
				global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002E_007Bctor_007D(&cmGenericByteData, data.Data, &length);
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out unique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E obj);
					*(long*)(&obj) = 0L;
					try
					{
						if (global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetIsLegacyDataType(&cmGenericByteData))
						{
							NmGenericByteData.CmGenericByteData* ptr = (NmGenericByteData.CmGenericByteData*)global::_003CModule_003E.@new(96uL);
							NmGenericByteData.CmGenericByteData* ptr2;
							try
							{
								if (ptr != null)
								{
									ulong length2 = data.Length;
									ptr2 = global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002E_007Bctor_007D(ptr, data.Data, &length2);
								}
								else
								{
									ptr2 = null;
								}
								NmGenericByteData.CmGenericByteData* ptr3 = ptr2;
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.delete(ptr, 96uL);
								throw;
							}
							global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002Ereset(&obj, ptr2);
						}
						else if (global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetIsGenericByteDataBlockType(&cmGenericByteData))
						{
							ulong length3 = data.Length;
							System.Runtime.CompilerServices.Unsafe.SkipInit(out CmGenericByteDataBlock cmGenericByteDataBlock);
							global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataBlock_002E_007Bctor_007D(&cmGenericByteDataBlock, data.Data, &length3);
							try
							{
								if (global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataBlock_002EGetIsVideoDataType(&cmGenericByteDataBlock))
								{
									CmGenericVideoByteDataBlock* ptr4 = (CmGenericVideoByteDataBlock*)global::_003CModule_003E.@new(96uL);
									CmGenericVideoByteDataBlock* ptr5;
									try
									{
										if (ptr4 != null)
										{
											ulong length4 = data.Length;
											ptr5 = global::_003CModule_003E.NmGenericByteData_002ECmGenericVideoByteDataBlock_002E_007Bctor_007D(ptr4, data.Data, &length4);
										}
										else
										{
											ptr5 = null;
										}
										CmGenericVideoByteDataBlock* ptr6 = ptr5;
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.delete(ptr4, 96uL);
										throw;
									}
									global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002Ereset(&obj, (NmGenericByteData.CmGenericByteData*)ptr5);
								}
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmGenericByteDataBlock*, void>)(&global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataBlock_002E_007Bdtor_007D), &cmGenericByteDataBlock);
								throw;
							}
							global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataBlock_002E_007Bdtor_007D(&cmGenericByteDataBlock);
						}
						else if (global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetIsGenericSingleDataPacketType(&cmGenericByteData))
						{
							ulong length5 = data.Length;
							System.Runtime.CompilerServices.Unsafe.SkipInit(out CmGenericByteDataSinglePacket cmGenericByteDataSinglePacket);
							global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataSinglePacket_002E_007Bctor_007D(&cmGenericByteDataSinglePacket, data.Data, &length5);
							try
							{
								if (global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataSinglePacket_002EGetIsGenericVideoStreamType(&cmGenericByteDataSinglePacket))
								{
									NmGenericByteData.CmGenericVideoStreamDataPacket* ptr7 = (NmGenericByteData.CmGenericVideoStreamDataPacket*)global::_003CModule_003E.@new(96uL);
									NmGenericByteData.CmGenericVideoStreamDataPacket* ptr8;
									try
									{
										if (ptr7 != null)
										{
											ulong length6 = data.Length;
											ptr8 = global::_003CModule_003E.NmGenericByteData_002ECmGenericVideoStreamDataPacket_002E_007Bctor_007D(ptr7, data.Data, &length6);
										}
										else
										{
											ptr8 = null;
										}
										NmGenericByteData.CmGenericVideoStreamDataPacket* ptr9 = ptr8;
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.delete(ptr7, 96uL);
										throw;
									}
									global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002Ereset(&obj, (NmGenericByteData.CmGenericByteData*)ptr8);
								}
								else if (global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataSinglePacket_002EGetIsGenericAudioStreamType(&cmGenericByteDataSinglePacket))
								{
									CmGenericAudioStreamDataPacket* ptr10 = (CmGenericAudioStreamDataPacket*)global::_003CModule_003E.@new(96uL);
									CmGenericAudioStreamDataPacket* ptr11;
									try
									{
										if (ptr10 != null)
										{
											ulong length7 = data.Length;
											ptr11 = global::_003CModule_003E.NmGenericByteData_002ECmGenericAudioStreamDataPacket_002E_007Bctor_007D(ptr10, data.Data, &length7);
										}
										else
										{
											ptr11 = null;
										}
										CmGenericAudioStreamDataPacket* ptr12 = ptr11;
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.delete(ptr10, 96uL);
										throw;
									}
									global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002Ereset(&obj, (NmGenericByteData.CmGenericByteData*)ptr11);
								}
								else if (global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataSinglePacket_002EGetIsGenericMetaDataStreamType(&cmGenericByteDataSinglePacket))
								{
									CmGenericMetaDataStreamPacket* ptr13 = (CmGenericMetaDataStreamPacket*)global::_003CModule_003E.@new(96uL);
									CmGenericMetaDataStreamPacket* ptr14;
									try
									{
										if (ptr13 != null)
										{
											ulong length8 = data.Length;
											ptr14 = global::_003CModule_003E.NmGenericByteData_002ECmGenericMetaDataStreamPacket_002E_007Bctor_007D(ptr13, data.Data, &length8);
										}
										else
										{
											ptr14 = null;
										}
										CmGenericMetaDataStreamPacket* ptr15 = ptr14;
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.delete(ptr13, 96uL);
										throw;
									}
									global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002Ereset(&obj, (NmGenericByteData.CmGenericByteData*)ptr14);
								}
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmGenericByteDataSinglePacket*, void>)(&global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataSinglePacket_002E_007Bdtor_007D), &cmGenericByteDataSinglePacket);
								throw;
							}
							global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataSinglePacket_002E_007Bdtor_007D(&cmGenericByteDataSinglePacket);
						}
						else if (global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetIsMultiDataPacketType(&cmGenericByteData))
						{
							CmGenericByteDataMultiPacket* ptr16 = (CmGenericByteDataMultiPacket*)global::_003CModule_003E.@new(96uL);
							CmGenericByteDataMultiPacket* ptr17;
							try
							{
								if (ptr16 != null)
								{
									ulong length9 = data.Length;
									ptr17 = global::_003CModule_003E.NmGenericByteData_002ECmGenericByteDataMultiPacket_002E_007Bctor_007D(ptr16, data.Data, &length9);
								}
								else
								{
									ptr17 = null;
								}
								CmGenericByteDataMultiPacket* ptr18 = ptr17;
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.delete(ptr16, 96uL);
								throw;
							}
							global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002Ereset(&obj, (NmGenericByteData.CmGenericByteData*)ptr17);
						}
						NmGenericByteData.CmGenericByteData* ptr19 = (NmGenericByteData.CmGenericByteData*)(*(ulong*)(&obj));
						if (*(long*)(&obj) == 0L)
						{
							System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj2);
							global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D(&obj2, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_0CD_0040BOMPFHAH_0040Unable_003F5to_003F5determine_003F5byte_003F5data_003F5t_0040));
							try
							{
								System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj3);
								std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* right = global::_003CModule_003E.std_002Eto_string(&obj3, 313);
								try
								{
									System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj4);
									std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left = global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D(&obj4, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_0DP_0040FPNHKMHC_0040VideoOS_003F3_003F3Platform_003F3_003F3MediaProcess_0040));
									try
									{
										System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj5);
										std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left2 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj5, left, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_07HEIMECHP_0040_003F5_003F_0024CIline_003F5_0040));
										try
										{
											System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj6);
											std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left3 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj6, left2, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_0HP_0040OFEHDGHJ_0040C_003F3_003F2actions_003F9runner_003F2_work_003F2XPCO_003F2XP_0040));
											try
											{
												System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj7);
												std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left4 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj7, left3, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_02KEGNLNML_0040_003F0_003F5_0040));
												try
												{
													System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj8);
													std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left5 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj8, left4, right);
													try
													{
														System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj9);
														std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr20 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj9, left5, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_01PKGAHCOL_0040_003F_0024CJ_0040));
														try
														{
															System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaProcessingError cmMediaProcessingError);
															CmMediaProcessingError* error = global::_003CModule_003E.NmMediaProcessing_002ECmMediaProcessingError_002E_007Bctor_007D(&cmMediaProcessingError, ptr20, &obj2);
															try
															{
																throw new Error((CmError*)error);
															}
															catch
															{
																//try-fault
																global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmMediaProcessingError*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmMediaProcessingError_002E_007Bdtor_007D), &cmMediaProcessingError);
																throw;
															}
														}
														catch
														{
															//try-fault
															global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj9);
															throw;
														}
													}
													catch
													{
														//try-fault
														global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj8);
														throw;
													}
												}
												catch
												{
													//try-fault
													global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj7);
													throw;
												}
											}
											catch
											{
												//try-fault
												global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj6);
												throw;
											}
										}
										catch
										{
											//try-fault
											global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj5);
											throw;
										}
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj4);
										throw;
									}
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj3);
									throw;
								}
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj2);
								throw;
							}
						}
						void* ptr21 = null;
						NmGenericByteData.CmGenericByteData* ptr22 = (NmGenericByteData.CmGenericByteData*)(*(ulong*)(&obj));
						*(long*)(&obj) = 0L;
						NmGenericByteData.CmGenericByteData* ptr23 = ptr22;
						NmGenericByteData.CmGenericByteData* ptr24 = ptr22;
						m_pByteData = ptr22;
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002E_007Bdtor_007D), &obj);
						throw;
					}
					global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002E_007Bdtor_007D(&obj);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmGenericByteData.CmGenericByteData*, void>)(&global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002E_007Bdtor_007D), &cmGenericByteData);
					throw;
				}
				global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002E_007Bdtor_007D(&cmGenericByteData);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error2);
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
			GC.KeepAlive(this);
		}
		return m_pByteData;
	}

	public unsafe CmRawVideoByteDataAccess* GetAccess()
	{
		//IL_0009: Expected I8, but got I
		//IL_01e5: Expected I, but got I8
		//IL_01b9: Expected I, but got I8
		//IL_0035: Expected I, but got I8
		//IL_002f: Expected I, but got I8
		//IL_006f: Expected I, but got I8
		//IL_0077: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pAccess == null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				CmRawVideoByteDataAccess* ptr = (CmRawVideoByteDataAccess*)global::_003CModule_003E.@new(72uL);
				CmRawVideoByteDataAccess* ptr2;
				try
				{
					ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002E_007Bctor_007D(ptr, GetNative(), false, null));
					CmRawVideoByteDataAccess* ptr3 = ptr2;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr, 72uL);
					throw;
				}
				m_pAccess = ptr2;
				if (!global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002EIsValid(ptr2))
				{
					CmRawVideoByteDataAccess* pAccess = m_pAccess;
					if (pAccess != null)
					{
						void* ptr4 = global::_003CModule_003E.NmGenericByteData_002ECmRawVideoByteDataAccess_002E__delDtor(pAccess, 1u);
					}
					else
					{
						void* ptr4 = null;
					}
					m_pAccess = null;
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
					global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D(&obj, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_0CM_0040CHNDCBJB_0040Generic_003F5byte_003F5data_003F5is_003F5not_003F5a_003F5know_0040));
					try
					{
						System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj2);
						std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* right = global::_003CModule_003E.std_002Eto_string(&obj2, 342);
						try
						{
							System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj3);
							std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left = global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D(&obj3, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_0DP_0040GGBCPOIG_0040VideoOS_003F3_003F3Platform_003F3_003F3MediaProcess_0040));
							try
							{
								System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj4);
								std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left2 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj4, left, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_07HEIMECHP_0040_003F5_003F_0024CIline_003F5_0040));
								try
								{
									System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj5);
									std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left3 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj5, left2, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_0HP_0040OFEHDGHJ_0040C_003F3_003F2actions_003F9runner_003F2_work_003F2XPCO_003F2XP_0040));
									try
									{
										System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj6);
										std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left4 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj6, left3, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_02KEGNLNML_0040_003F0_003F5_0040));
										try
										{
											System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj7);
											std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* left5 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj7, left4, right);
											try
											{
												System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj8);
												std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr5 = global::_003CModule_003E.std_002Eoperator_002B_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E(&obj8, left5, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_01PKGAHCOL_0040_003F_0024CJ_0040));
												try
												{
													System.Runtime.CompilerServices.Unsafe.SkipInit(out CmError cmError);
													CmError* error = global::_003CModule_003E.NmError_002ECmError_002E_007Bctor_007D(&cmError, ptr5, &obj);
													try
													{
														throw new Error((CmError*)error);
													}
													catch
													{
														//try-fault
														global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmError*, void>)(&global::_003CModule_003E.NmError_002ECmError_002E_007Bdtor_007D), &cmError);
														throw;
													}
												}
												catch
												{
													//try-fault
													global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj8);
													throw;
												}
											}
											catch
											{
												//try-fault
												global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj7);
												throw;
											}
										}
										catch
										{
											//try-fault
											global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj6);
											throw;
										}
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj5);
										throw;
									}
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj4);
									throw;
								}
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj3);
								throw;
							}
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj2);
							throw;
						}
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj);
						throw;
					}
				}
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error2);
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
			GC.KeepAlive(this);
		}
		return m_pAccess;
	}

	private unsafe ReadOnlySharedBuffer GetData()
	{
		ReadOnlySharedBuffer data = m_data;
		if (null == data)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.GenericByteData");
		}
		if (data.Data != null && m_data.Length != 0)
		{
			GC.KeepAlive(this);
			return m_data;
		}
		throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.GenericByteData contents");
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EGenericByteData();
			return;
		}
		try
		{
			_0021GenericByteData();
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

	~GenericByteData()
	{
		Dispose(A_0: false);
	}
}
