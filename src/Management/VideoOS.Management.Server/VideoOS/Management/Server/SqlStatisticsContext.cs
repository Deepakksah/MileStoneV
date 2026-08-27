using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public static class SqlStatisticsContext
{
	[ThreadStatic]
	public static bool CollectingStatistics;

	[ThreadStatic]
	public static int SqlCalls;

	[ThreadStatic]
	public static List<MethodTime> WorstSqlCalls;

	[ThreadStatic]
	public static TimeSpan SqlExecuteTime;

	private static TimeSpan _minTime = TimeSpan.FromMilliseconds(100.0);

	private static string _area = "Profiling";

	private static string _layer = "SqlStatistics";

	public static void Enter(string methodName)
	{
	}

	public static void Leave(string methodName)
	{
	}

	public static void AddSqlExecute(string name, TimeSpan time)
	{
		try
		{
			SqlCalls++;
			SqlExecuteTime += time;
			if (time > _minTime)
			{
				MethodTime item = new MethodTime
				{
					Name = ((name.Length > 140) ? name.Substring(0, 139) : name),
					Time = time
				};
				WorstSqlCalls.Add(item);
			}
		}
		catch (Exception ex)
		{
			HandleException(ex);
		}
	}

	private static void HandleException(Exception ex)
	{
		GeminiTracingService.TraceExceptionEtw(0, _area, _layer, ex);
	}
}
