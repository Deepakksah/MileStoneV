using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NmMediaProcessing;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class MotionLegacy
{
	private MotionLegacy()
	{
	}

	public static double ConvertEnterpriseNoiseSensitivity(uint threshold)
	{
		return global::_003CModule_003E.NmMediaProcessing_002EConvertEnterpriseNoiseSensitivity(threshold);
	}

	public static double ConvertEnterpriseMotionLevel(uint sensitivity, uint width, uint height)
	{
		return global::_003CModule_003E.NmMediaProcessing_002EConvertEnterpriseMotionLevel(sensitivity, width, height);
	}

	public static double ConvertCorporateNoiseSensitivity(int threshold)
	{
		return global::_003CModule_003E.NmMediaProcessing_002EConvertCorporateNoiseSensitivity(threshold);
	}

	public static double ConvertCorporateMotionLevel(int motionPixCount)
	{
		return global::_003CModule_003E.NmMediaProcessing_002EConvertCorporateMotionLevel(motionPixCount);
	}

	public static double ConvertSmartSearchNoiseSensitivity(int sensitivity)
	{
		return global::_003CModule_003E.NmMediaProcessing_002EConvertSmartSearchNoiseSensitivity(sensitivity);
	}

	public static double ConvertSmartSearchMotionLevel(int sensitivity)
	{
		return global::_003CModule_003E.NmMediaProcessing_002EConvertSmartSearchMotionLevel(sensitivity);
	}

	public unsafe static List<SimpleMotionRegion> ConvertMotionDetectionRows(List<List<bool>> rows)
	{
		//Discarded unreachable code: IL_017d
		//IL_0140: Expected I, but got I8
		//IL_00b6: Expected I, but got I8
		//IL_00e0: Expected I, but got I8
		if (rows == null)
		{
			throw new ArgumentNullException();
		}
		if (rows.Count == 0)
		{
			throw new ArgumentException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E obj);
		*(long*)(&obj) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 16)) = 0L;
		List<SimpleMotionRegion> list;
		try
		{
			global::_003CModule_003E.std_002Evector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E_002Ereserve(&obj, (ulong)rows.Count);
			List<List<bool>>.Enumerator enumerator = rows.GetEnumerator();
			if (enumerator.MoveNext())
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E obj2);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out allocator_003Cbool_003E allocator_003Cbool_003E2);
				do
				{
					List<bool> current = enumerator.Current;
					if (current.Count == rows[0].Count)
					{
						global::_003CModule_003E.std_002E_Vb_val_003Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002E_007Bctor_007D((_Vb_val_003Cstd_003A_003Aallocator_003Cbool_003E_0020_003E*)(&obj2), &allocator_003Cbool_003E2);
						try
						{
							global::_003CModule_003E.std_002Evector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E_002E_Emplace_one_at_back_003Cclass_0020std_003A_003Avector_003Cbool_002Cclass_0020std_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E(&obj, &obj2);
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002E_007Bdtor_007D), &obj2);
							throw;
						}
						global::_003CModule_003E.std_002Evector_003Cunsigned_0020int_002Cstd_003A_003Aallocator_003Cunsigned_0020int_003E_0020_003E_002E_Tidy((vector_003Cunsigned_0020int_002Cstd_003A_003Aallocator_003Cunsigned_0020int_003E_0020_003E*)(&obj2));
						long num = System.Runtime.CompilerServices.Unsafe.As<vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) - 32;
						ulong num2 = (ulong)current.Count;
						global::_003CModule_003E.std_002Evector_003Cunsigned_0020int_002Cstd_003A_003Aallocator_003Cunsigned_0020int_003E_0020_003E_002Ereserve((vector_003Cunsigned_0020int_002Cstd_003A_003Aallocator_003Cunsigned_0020int_003E_0020_003E*)num, num2 + 31 >> 5);
						List<bool>.Enumerator enumerator2 = current.GetEnumerator();
						if (enumerator2.MoveNext())
						{
							do
							{
								bool current2 = enumerator2.Current;
								global::_003CModule_003E.std_002Evector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Epush_back((vector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E*)(System.Runtime.CompilerServices.Unsafe.As<vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) - 32), &current2);
							}
							while (enumerator2.MoveNext());
						}
						continue;
					}
					throw new ArgumentException();
				}
				while (enumerator.MoveNext());
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E obj3);
			vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E* ptr = &obj3;
			vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E* ptr2 = global::_003CModule_003E.std_002Evector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E_002E_007Bctor_007D(&obj3, &obj);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_003E_0020_003E obj4);
			global::_003CModule_003E.NmMediaProcessing_002EConvertMotionDetectionRows(&obj4, ptr2);
			try
			{
				list = new List<SimpleMotionRegion>((int)(System.Runtime.CompilerServices.Unsafe.As<vector_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj4, 8)) - *(long*)(&obj4) >> 6));
				for (ulong num3 = *(ulong*)(&obj4); num3 != (ulong)System.Runtime.CompilerServices.Unsafe.As<vector_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj4, 8)); num3 += 64)
				{
					list.Add(new SimpleMotionRegion((CmSimpleImageRegion*)num3));
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_003E_0020_003E_002E_007Bdtor_007D), &obj4);
				throw;
			}
			global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmSimpleImageRegion_003E_0020_003E_002E_Tidy(&obj4);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		global::_003CModule_003E.std_002Evector_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Avector_003Cbool_002Cstd_003A_003Aallocator_003Cbool_003E_0020_003E_0020_003E_0020_003E_002E_Tidy(&obj);
		return list;
	}
}
