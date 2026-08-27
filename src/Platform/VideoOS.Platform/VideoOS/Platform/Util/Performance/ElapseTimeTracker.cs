using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace VideoOS.Platform.Util.Performance;

public class ElapseTimeTracker
{
	public Dictionary<string, TimeInfoClass> ElapseTime = new Dictionary<string, TimeInfoClass>();

	public string Area { get; set; } = string.Empty;

	public ElapseTimeTracker()
	{
	}

	public ElapseTimeTracker(string area)
	{
		Area = area;
	}

	public void StoreElapseTime(string where, long elapseTime)
	{
		lock (ElapseTime)
		{
			TimeInfoClass timeInfoClass;
			if (!ElapseTime.ContainsKey(where))
			{
				timeInfoClass = new TimeInfoClass
				{
					Where = where,
					Total = elapseTime,
					Min = elapseTime,
					Max = elapseTime,
					cnt = 1L
				};
				ElapseTime.Add(where, timeInfoClass);
			}
			else
			{
				timeInfoClass = ElapseTime[where];
				timeInfoClass.cnt++;
				timeInfoClass.Total += elapseTime;
				if (elapseTime < timeInfoClass.Min)
				{
					timeInfoClass.Min = elapseTime;
				}
				if (elapseTime > timeInfoClass.Max)
				{
					timeInfoClass.Max = elapseTime;
				}
			}
			long num = elapseTime / 10000;
			if (num < 50)
			{
				timeInfoClass.Cnt50++;
			}
			else if (num < 100)
			{
				timeInfoClass.Cnt100++;
			}
			else if (num < 250)
			{
				timeInfoClass.Cnt250++;
			}
			else if (num < 500)
			{
				timeInfoClass.Cnt500++;
			}
			else
			{
				timeInfoClass.CntOver500++;
			}
			if (num < 50)
			{
				timeInfoClass.Total50 += elapseTime;
			}
			else if (num < 100)
			{
				timeInfoClass.Total100 += elapseTime;
			}
			else if (num < 250)
			{
				timeInfoClass.Total250 += elapseTime;
			}
			else if (num < 500)
			{
				timeInfoClass.Total500 += elapseTime;
			}
			else
			{
				timeInfoClass.TotalOver500 += elapseTime;
			}
		}
	}

	public void PrintTable()
	{
		lock (ElapseTime)
		{
			if (ElapseTime == null || !ElapseTime.Any())
			{
				return;
			}
			TextWriter textWriter = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Milestone\\MIPSDK\\ElaptimeCounters" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm", CultureInfo.InvariantCulture) + ".txt"));
			List<TimeInfoClass> list = ElapseTime.Values.ToList();
			list.Sort((TimeInfoClass i1, TimeInfoClass i2) => Sort.LongValueCompare(i1.Max, i2.Max));
			foreach (TimeInfoClass item in list)
			{
				textWriter.WriteLine("------ Data for :" + item.Where);
				textWriter.WriteLine("--- Total=" + (item.Total / 10000).ToString(CultureInfo.InvariantCulture) + "ms");
				textWriter.WriteLine("--- Min=" + (item.Min / 10000).ToString(CultureInfo.InvariantCulture) + "ms");
				textWriter.WriteLine("--- Max=" + (item.Max / 10000).ToString(CultureInfo.InvariantCulture) + "ms");
				textWriter.WriteLine("--- Cnt Total=" + item.cnt.ToString(CultureInfo.InvariantCulture));
				textWriter.WriteLine("--- Cnt under  50ms=" + item.Cnt50.ToString(CultureInfo.InvariantCulture) + " (" + (item.Total50 / 10000).ToString(CultureInfo.InvariantCulture) + ")");
				textWriter.WriteLine("--- Cnt under 100ms=" + item.Cnt100.ToString(CultureInfo.InvariantCulture) + " (" + (item.Total100 / 10000).ToString(CultureInfo.InvariantCulture) + ")");
				textWriter.WriteLine("--- Cnt under 250ms=" + item.Cnt250.ToString(CultureInfo.InvariantCulture) + " (" + (item.Total250 / 10000).ToString(CultureInfo.InvariantCulture) + ")");
				textWriter.WriteLine("--- Cnt under 500ms=" + item.Cnt500.ToString(CultureInfo.InvariantCulture) + " (" + (item.Total500 / 10000).ToString(CultureInfo.InvariantCulture) + ")");
				textWriter.WriteLine("--- Cnt over  500ms=" + item.CntOver500.ToString(CultureInfo.InvariantCulture) + " (" + (item.TotalOver500 / 10000).ToString(CultureInfo.InvariantCulture) + ")");
				textWriter.WriteLine("--- Avg=" + (item.Total / item.cnt / 10000).ToString(CultureInfo.InvariantCulture) + "ms");
			}
			textWriter.Flush();
			textWriter.Close();
			ElapseTime.Clear();
		}
	}

	public void ToStringBuilder(StringBuilder sb)
	{
		if (sb == null)
		{
			throw new ArgumentNullException("sb");
		}
		lock (ElapseTime)
		{
			sb.AppendLine("Elapse times for: " + Area + "--- Sorted with highest Max execution sorted at the top");
			List<TimeInfoClass> list = ElapseTime.Values.ToList();
			list.Sort((TimeInfoClass i1, TimeInfoClass i2) => Sort.LongValueCompare(i2.Max, i1.Max));
			foreach (TimeInfoClass item in list)
			{
				sb.AppendLine("--- Data for :" + item.Where);
				sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "...... Execution Time     : Total = {0}ms, Min={1}ms,   Max={2}ms, Avg={3}ms", item.Total / 10000, item.Min / 10000, item.Max / 10000, item.Total / item.cnt / 10000));
				sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "...... Exceution Counters : Total={0},   0..50ms={1},   50..100ms={2},   100..250ms={3},   250..500ms={4},   over 500ms={5} ( total execution time for these {6})", item.cnt, item.Cnt50, item.Cnt100, item.Cnt250, item.Cnt500, item.CntOver500, item.TotalOver500 / 10000));
			}
		}
		sb.AppendLine(string.Empty);
	}

	public void Close()
	{
		PrintTable();
	}
}
