#define TRACE
using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.UI.Common.WPF.Utils;

public class TraceTime : IDisposable
{
	private Stopwatch _stopwatch = new Stopwatch();

	private string _label;

	private DateTime _startTime;

	public TraceTime(string label)
	{
		_label = label;
		_startTime = DateTime.Now;
		_stopwatch.Start();
	}

	public void Dispose()
	{
		_stopwatch.Stop();
		Trace.WriteLine(string.Format("TraceTime ({0}) - {1} - {2} ms", _startTime.ToString("hh:mm:ss.fff", CultureInfo.CurrentUICulture), _label, _stopwatch.ElapsedMilliseconds));
	}
}
