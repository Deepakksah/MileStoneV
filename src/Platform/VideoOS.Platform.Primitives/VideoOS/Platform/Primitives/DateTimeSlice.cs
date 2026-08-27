using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VideoOS.Platform.Primitives;

public struct DateTimeSlice(DateTime beginTimeStamp, DateTime endTimeStamp)
{
	public static DateTimeSlice Invalid;

	public DateTime m_BeginTimeStamp = beginTimeStamp;

	public DateTime m_EndTimeStamp = endTimeStamp;

	public bool IsEmpty
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_BeginTimeStamp >= m_EndTimeStamp;
		}
	}

	public bool IsValid
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_BeginTimeStamp <= m_EndTimeStamp;
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool Contains(DateTime timeStamp)
	{
		int num = ((timeStamp >= m_BeginTimeStamp && timeStamp <= m_EndTimeStamp) ? 1 : 0);
		return (byte)num != 0;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool Overlaps(DateTimeSlice other)
	{
		int num = ((IsValid && other.IsValid && m_EndTimeStamp >= other.m_BeginTimeStamp && m_BeginTimeStamp <= other.m_EndTimeStamp) ? 1 : 0);
		return (byte)num != 0;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public static bool operator ==(DateTimeSlice lVal, DateTimeSlice rVal)
	{
		int num = ((lVal.m_BeginTimeStamp == rVal.m_BeginTimeStamp && lVal.m_EndTimeStamp == rVal.m_EndTimeStamp) ? 1 : 0);
		return (byte)num != 0;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public static bool operator !=(DateTimeSlice lVal, DateTimeSlice rVal)
	{
		int num = ((lVal.m_BeginTimeStamp != rVal.m_BeginTimeStamp || lVal.m_EndTimeStamp != rVal.m_EndTimeStamp) ? 1 : 0);
		return (byte)num != 0;
	}

	public DateTimeSlice Union(DateTimeSlice timeSlice)
	{
		DateTimeSlice result = default(DateTimeSlice);
		if (!IsValid)
		{
			if (timeSlice.IsValid)
			{
				return timeSlice;
			}
		}
		else if (!timeSlice.IsValid)
		{
			ref DateTime beginTimeStamp = ref result.m_BeginTimeStamp;
			beginTimeStamp = m_BeginTimeStamp;
			ref DateTime endTimeStamp = ref result.m_EndTimeStamp;
			endTimeStamp = m_EndTimeStamp;
		}
		else
		{
			ref DateTime beginTimeStamp2 = ref result.m_BeginTimeStamp;
			beginTimeStamp2 = m_BeginTimeStamp;
			ref DateTime endTimeStamp2 = ref result.m_EndTimeStamp;
			endTimeStamp2 = m_EndTimeStamp;
			if (m_BeginTimeStamp > timeSlice.m_BeginTimeStamp)
			{
				ref DateTime beginTimeStamp3 = ref result.m_BeginTimeStamp;
				beginTimeStamp3 = timeSlice.m_BeginTimeStamp;
			}
			if (m_EndTimeStamp < timeSlice.m_EndTimeStamp)
			{
				ref DateTime endTimeStamp3 = ref result.m_EndTimeStamp;
				endTimeStamp3 = timeSlice.m_EndTimeStamp;
			}
		}
		return result;
	}

	public DateTimeSlice Intersection(DateTimeSlice timeSlice)
	{
		DateTimeSlice result = default(DateTimeSlice);
		if (Overlaps(timeSlice))
		{
			if (m_BeginTimeStamp == timeSlice.m_EndTimeStamp)
			{
				ref DateTime beginTimeStamp = ref result.m_BeginTimeStamp;
				beginTimeStamp = m_BeginTimeStamp;
				ref DateTime endTimeStamp = ref result.m_EndTimeStamp;
				endTimeStamp = m_BeginTimeStamp;
			}
			else if (m_EndTimeStamp == timeSlice.m_BeginTimeStamp)
			{
				ref DateTime beginTimeStamp2 = ref result.m_BeginTimeStamp;
				beginTimeStamp2 = m_EndTimeStamp;
				ref DateTime endTimeStamp2 = ref result.m_EndTimeStamp;
				endTimeStamp2 = m_EndTimeStamp;
			}
			else if (m_BeginTimeStamp > timeSlice.m_BeginTimeStamp)
			{
				ref DateTime beginTimeStamp3 = ref result.m_BeginTimeStamp;
				beginTimeStamp3 = m_BeginTimeStamp;
			}
			else
			{
				ref DateTime beginTimeStamp4 = ref result.m_BeginTimeStamp;
				beginTimeStamp4 = timeSlice.m_BeginTimeStamp;
			}
			if (m_EndTimeStamp < timeSlice.m_EndTimeStamp)
			{
				ref DateTime endTimeStamp3 = ref result.m_EndTimeStamp;
				endTimeStamp3 = m_EndTimeStamp;
			}
			else
			{
				ref DateTime endTimeStamp4 = ref result.m_EndTimeStamp;
				endTimeStamp4 = timeSlice.m_EndTimeStamp;
			}
		}
		return result;
	}

	static DateTimeSlice()
	{
		DateTime dateTime = new DateTime(0L);
		DateTime dateTime2 = new DateTime(1L);
		DateTime endTimeStamp = dateTime;
		DateTime beginTimeStamp = dateTime2;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out DateTimeSlice invalid);
		invalid.m_BeginTimeStamp = beginTimeStamp;
		invalid.m_EndTimeStamp = endTimeStamp;
		Invalid = invalid;
	}
}
