using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class GeminiTraceSettings
{
	private TraceSwitch _traceGeneral;

	private TraceSwitch _traceDatabase;

	private TraceSwitch _traceSecurity;

	private TraceSwitch _traceConfiguration;

	public GeminiTraceSettings(List<TraceSwitch> traceSwitches)
	{
		foreach (TraceSwitch traceSwitch in traceSwitches)
		{
			TraceType traceType;
			try
			{
				traceType = (TraceType)Enum.Parse(typeof(TraceType), traceSwitch.DisplayName, ignoreCase: true);
			}
			catch (ArgumentException)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63007, traceSwitch.DisplayName));
			}
			switch (traceType)
			{
			case TraceType.General:
				_traceGeneral = traceSwitch;
				break;
			case TraceType.Database:
				_traceDatabase = traceSwitch;
				break;
			case TraceType.Security:
				_traceSecurity = traceSwitch;
				break;
			case TraceType.Configuration:
				_traceConfiguration = traceSwitch;
				break;
			default:
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63007, traceSwitch.DisplayName));
			}
		}
	}

	public TraceSwitch GetTraceCategory(TraceType traceCategory)
	{
		return traceCategory switch
		{
			TraceType.General => _traceGeneral, 
			TraceType.Database => _traceDatabase, 
			TraceType.Security => _traceSecurity, 
			TraceType.Configuration => _traceConfiguration, 
			_ => throw new ArgumentOutOfRangeException("traceCategory"), 
		};
	}
}
