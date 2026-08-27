using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmMediaProcessing;
using NmTimeStamp;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class MotionDetectionResult
{
	public struct MotionDetectionEvent
	{
		public enum eventType_t
		{
			motionOn,
			motionOff,
			motionLevel
		}

		public eventType_t m_eType;

		public DateTime m_timeStamp;

		public double m_motionLevel;
	}

	public enum decoderImplementation_t
	{
		none,
		onlyIntel,
		hardwareIntel,
		autoNvidia,
		onlyNvidia,
		hardwareNvidia,
		softwareIntel,
		ipp
	}

	private List<MotionDetectionEvent> m_pEvents;

	private List<MotionHistogramGrid> m_pMotionHistogramGrids;

	private List<MotionLevelGrid> m_pMotionLevelGrids;

	private decoderImplementation_t m_decoderImplementation;

	public decoderImplementation_t DecoderImplementation => m_decoderImplementation;

	public List<MotionLevelGrid> MotionLevelGrids => m_pMotionLevelGrids;

	public List<MotionHistogramGrid> MotionHistogramGrids => m_pMotionHistogramGrids;

	public List<MotionDetectionEvent> Events => m_pEvents;

	private unsafe void SetDecoderImplementation(CmMotionDetectionResult* result)
	{
		switch (global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002EGetDecoderImplementation(result))
		{
		case (CmMotionDetectionResult.decoderImplementation_t)1:
			m_decoderImplementation = decoderImplementation_t.onlyIntel;
			break;
		case (CmMotionDetectionResult.decoderImplementation_t)2:
			m_decoderImplementation = decoderImplementation_t.hardwareIntel;
			break;
		case (CmMotionDetectionResult.decoderImplementation_t)3:
			m_decoderImplementation = decoderImplementation_t.autoNvidia;
			break;
		case (CmMotionDetectionResult.decoderImplementation_t)4:
			m_decoderImplementation = decoderImplementation_t.onlyNvidia;
			break;
		case (CmMotionDetectionResult.decoderImplementation_t)5:
			m_decoderImplementation = decoderImplementation_t.hardwareNvidia;
			break;
		case (CmMotionDetectionResult.decoderImplementation_t)6:
			m_decoderImplementation = decoderImplementation_t.softwareIntel;
			break;
		case (CmMotionDetectionResult.decoderImplementation_t)7:
			m_decoderImplementation = decoderImplementation_t.ipp;
			break;
		case (CmMotionDetectionResult.decoderImplementation_t)0:
			m_decoderImplementation = decoderImplementation_t.none;
			break;
		}
	}

	public unsafe MotionDetectionResult(CmMotionDetectionResult* result, CmStreamMotionDetection* smd)
	{
		//IL_0008: Expected I8, but got I
		//IL_01c1: Expected I, but got I8
		//IL_018b: Expected I, but got I8
		//IL_019b: Expected I, but got I8
		//IL_028d: Expected I, but got I8
		//IL_0257: Expected I, but got I8
		//IL_0267: Expected I, but got I8
		//IL_00ab: Expected I, but got I8
		//IL_01da: Expected I, but got I8
		//IL_0120: Expected I, but got I8
		//IL_0124: Expected I, but got I8
		//IL_020a: Expected I, but got I8
		//IL_020e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pEvents = new List<MotionDetectionEvent>();
		m_pMotionHistogramGrids = new List<MotionHistogramGrid>();
		m_pMotionLevelGrids = new List<MotionLevelGrid>();
		m_decoderImplementation = decoderImplementation_t.none;
		base._002Ector();
		List<MotionDetectionEvent> pEvents = m_pEvents;
		int capacity = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002Esize(result);
		pEvents.Capacity = capacity;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out _Vector_const_iterator_003Cstd_003A_003A_Vector_val_003Cstd_003A_003A_Simple_types_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E_0020_003E obj);
		global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002Ebegin(result, &obj);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out _Vector_const_iterator_003Cstd_003A_003A_Vector_val_003Cstd_003A_003A_Simple_types_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E_0020_003E obj2);
		while (true)
		{
			_Vector_const_iterator_003Cstd_003A_003A_Vector_val_003Cstd_003A_003A_Simple_types_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E_0020_003E* ptr = global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002Eend(result, &obj2);
			if (*(long*)(&obj) == *(long*)ptr)
			{
				break;
			}
			MotionDetectionEvent item = default(MotionDetectionEvent);
			MotionDetectionEvent.eventType_t eType;
			int num2;
			switch (*(int*)(*(ulong*)(&obj)))
			{
			case 0:
				eType = MotionDetectionEvent.eventType_t.motionOn;
				goto IL_0094;
			default:
				num2 = 2;
				break;
			case 1:
				num2 = 1;
				break;
			}
			MotionDetectionEvent.eventType_t eventType_t = (MotionDetectionEvent.eventType_t)num2;
			eType = eventType_t;
			goto IL_0094;
			IL_0094:
			item.m_eType = eType;
			DateTime dateTime = new DateTime((global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002EGetMillisecondsSinceTheEpoch((NmTimeStamp.CmTimeStamp*)(*(long*)(&obj) + 4)) + 62135596800000L) * 10000, DateTimeKind.Utc);
			DateTime timeStamp = dateTime;
			item.m_timeStamp = timeStamp;
			item.m_motionLevel = *(double*)(*(long*)(&obj) + 16);
			m_pEvents.Add(item);
			*(long*)(&obj) += 24L;
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out unique_ptr_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_003E_0020_003E obj3);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out uint num3);
		while (true)
		{
			try
			{
				*(long*)(&obj3) = 0L;
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002EGetMotionHistogramGrid(smd, &obj3);
					if (*(long*)(&obj3) != 0L)
					{
						byte b = 1;
						byte b2 = b;
						List<MotionHistogramGrid> pMotionHistogramGrids = m_pMotionHistogramGrids;
						void* ptr2 = null;
						CmMotionHistogramGrid* ptr3 = (CmMotionHistogramGrid*)(*(ulong*)(&obj3));
						*(long*)(&obj3) = 0L;
						CmMotionHistogramGrid* ptr4 = ptr3;
						CmMotionHistogramGrid* ptr5 = ptr3;
						MotionHistogramGrid item2 = new MotionHistogramGrid(ptr3);
						pMotionHistogramGrids.Add(item2);
						goto IL_014e;
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_003E_0020_003E_002E_007Bdtor_007D), &obj3);
					throw;
				}
				try
				{
					byte b2 = 0;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_003E_0020_003E_002E_007Bdtor_007D), &obj3);
					throw;
				}
				global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_003E_0020_003E_002E_007Bdtor_007D(&obj3);
				break;
				IL_014e:
				global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionHistogramGrid_003E_0020_003E_002E_007Bdtor_007D(&obj3);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, null) != 0)
			{
				num3 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						goto end_IL_019c;
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
					end_IL_019c:;
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num3);
				}
			}
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out unique_ptr_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_003E_0020_003E obj6);
		while (true)
		{
			try
			{
				*(long*)(&obj6) = 0L;
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002EGetMotionLevelGrid(smd, &obj6);
					CmMotionLevelGrid* ptr6 = (CmMotionLevelGrid*)(*(ulong*)(&obj6));
					if (*(long*)(&obj6) != 0L)
					{
						goto IL_01fe;
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_003E_0020_003E_002E_007Bdtor_007D), &obj6);
					throw;
				}
				global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_003E_0020_003E_002E_007Bdtor_007D(&obj6);
				goto end_IL_01c8;
				IL_01fe:
				try
				{
					List<MotionLevelGrid> pMotionLevelGrids = m_pMotionLevelGrids;
					void* ptr7 = null;
					CmMotionLevelGrid* ptr8 = (CmMotionLevelGrid*)(*(ulong*)(&obj6));
					*(long*)(&obj6) = 0L;
					CmMotionLevelGrid* ptr9 = ptr8;
					CmMotionLevelGrid* ptr10 = ptr8;
					MotionLevelGrid item3 = new MotionLevelGrid(ptr8);
					pMotionLevelGrids.Add(item3);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_003E_0020_003E_002E_007Bdtor_007D), &obj6);
					throw;
				}
				global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003ACmMotionLevelGrid_003E_0020_003E_002E_007Bdtor_007D(&obj6);
				continue;
				end_IL_01c8:;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, null) != 0)
			{
				num3 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
					}
					catch when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						num3 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num3 != 0;
					}).Invoke())
					{
						goto IL_027f;
					}
					goto end_IL_0268;
					IL_027f:
					if (num3 != 0)
					{
						throw;
					}
					end_IL_0268:;
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num3);
				}
				continue;
			}
			break;
		}
		SetDecoderImplementation(result);
	}

	public unsafe MotionDetectionResult(CmMotionDetectionResult* result)
	{
		//IL_009e: Expected I, but got I8
		m_pEvents = new List<MotionDetectionEvent>();
		m_pMotionHistogramGrids = new List<MotionHistogramGrid>();
		m_pMotionLevelGrids = new List<MotionLevelGrid>();
		m_decoderImplementation = decoderImplementation_t.none;
		base._002Ector();
		List<MotionDetectionEvent> pEvents = m_pEvents;
		int capacity = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002Esize(result);
		pEvents.Capacity = capacity;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out _Vector_const_iterator_003Cstd_003A_003A_Vector_val_003Cstd_003A_003A_Simple_types_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E_0020_003E obj);
		global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002Ebegin(result, &obj);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out _Vector_const_iterator_003Cstd_003A_003A_Vector_val_003Cstd_003A_003A_Simple_types_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E_0020_003E obj2);
		while (true)
		{
			_Vector_const_iterator_003Cstd_003A_003A_Vector_val_003Cstd_003A_003A_Simple_types_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E_0020_003E* ptr = global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002Eend(result, &obj2);
			if (*(long*)(&obj) == *(long*)ptr)
			{
				break;
			}
			MotionDetectionEvent item = default(MotionDetectionEvent);
			MotionDetectionEvent.eventType_t eType;
			int num;
			switch (*(int*)(*(ulong*)(&obj)))
			{
			case 0:
				eType = MotionDetectionEvent.eventType_t.motionOn;
				goto IL_0087;
			default:
				num = 2;
				break;
			case 1:
				num = 1;
				break;
			}
			MotionDetectionEvent.eventType_t eventType_t = (MotionDetectionEvent.eventType_t)num;
			eType = eventType_t;
			goto IL_0087;
			IL_0087:
			item.m_eType = eType;
			DateTime dateTime = new DateTime((global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002EGetMillisecondsSinceTheEpoch((NmTimeStamp.CmTimeStamp*)(*(long*)(&obj) + 4)) + 62135596800000L) * 10000, DateTimeKind.Utc);
			DateTime timeStamp = dateTime;
			item.m_timeStamp = timeStamp;
			item.m_motionLevel = *(double*)(*(long*)(&obj) + 16);
			m_pEvents.Add(item);
			*(long*)(&obj) += 24L;
		}
		SetDecoderImplementation(result);
	}

	public MotionDetectionResult()
	{
		m_pEvents = new List<MotionDetectionEvent>();
		m_decoderImplementation = decoderImplementation_t.none;
		base._002Ector();
	}
}
