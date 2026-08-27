using System;

namespace VideoOS.Management.Common;

public static class ProcessTime
{
	public enum ProcessTimeType
	{
		Ms100 = 100,
		Ms250 = 250,
		Ms500 = 500,
		Ms750 = 750,
		Ms1000 = 1000
	}

	public static int ConvertProcessTimeToInt32(ProcessTimeType processTime)
	{
		return ConvertProcessTimeToInt32(processTime, 250);
	}

	public static int ConvertProcessTimeToInt32(ProcessTimeType processTime, int defaultValue)
	{
		if (Enum.IsDefined(typeof(ProcessTimeType), processTime))
		{
			return (int)processTime;
		}
		return defaultValue;
	}

	public static ProcessTimeType ConvertInt32ToProcessTime(int processTime)
	{
		return ConvertInt32ToProcessTime(processTime, ProcessTimeType.Ms250);
	}

	public static ProcessTimeType ConvertInt32ToProcessTime(int processTime, ProcessTimeType defaultValue)
	{
		if (Enum.IsDefined(typeof(ProcessTimeType), processTime))
		{
			return (ProcessTimeType)processTime;
		}
		return defaultValue;
	}
}
