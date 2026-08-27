using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Management.Server;

public class TraceWatch : IDisposable
{
	private const string _stringFormat = "TimeElapsed: {0}ms Query: {1}";

	private IGeminiRequestContext RequestContext { get; set; }

	private int Tracepoint { get; set; }

	private TraceLevel TraceLevel { get; set; }

	private Stopwatch Watch { get; set; }

	private TimeSpan Timeout { get; set; }

	private string Area { get; set; }

	private string Layer { get; set; }

	private string Format { get; set; }

	private object[] Args { get; set; }

	public TraceWatch(IGeminiRequestContext requestContext, int tracepoint, TraceLevel traceLevel, TimeSpan timeout, string area, string layer, string format, params object[] args)
	{
		RequestContext = requestContext;
		Tracepoint = tracepoint;
		TraceLevel = traceLevel;
		Timeout = timeout;
		Area = area;
		Layer = layer;
		Format = format;
		Args = args;
		Watch = new Stopwatch();
		Watch.Start();
	}

	public void Dispose()
	{
		Watch.Stop();
		if (Watch.Elapsed > Timeout)
		{
			string text = string.Format(CultureInfo.InvariantCulture, Format, Args);
			GeminiTracingService.TraceEtw(Tracepoint, TraceLevel, Area, Layer, "TimeElapsed: {0}ms Query: {1}", Watch.ElapsedMilliseconds, text);
		}
	}
}
