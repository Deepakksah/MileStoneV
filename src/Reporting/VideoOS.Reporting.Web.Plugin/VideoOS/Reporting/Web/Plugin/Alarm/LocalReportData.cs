using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

namespace VideoOS.Reporting.Web.Plugin.Alarm;

public class LocalReportData
{
	private IntercommunicationReport _report;

	private Dictionary<Guid, IntercommunicationReportDateTimeResult> _reportDateTimeResults;

	private Dictionary<int, Dictionary<Guid, IntercommunicationReportDoubleResult>> _reportDoubleResults;

	private AlarmDataParameters _parameters;

	public AlarmDataParameters Parameters => _parameters;

	public LocalReportData(IntercommunicationReport report, AlarmDataParameters parameters)
	{
		_report = report;
		_reportDateTimeResults = new Dictionary<Guid, IntercommunicationReportDateTimeResult>();
		_reportDoubleResults = new Dictionary<int, Dictionary<Guid, IntercommunicationReportDoubleResult>>();
		if (report != null)
		{
			if (report.DateTimeResults != null)
			{
				_reportDateTimeResults = new List<IntercommunicationReportDateTimeResult>(report.DateTimeResults).ToDictionary((IntercommunicationReportDateTimeResult x) => x.Id, (IntercommunicationReportDateTimeResult x) => x);
			}
			if (report.DoubleResults != null)
			{
				IntercommunicationReportDoubleResult[] doubleResults = _report.DoubleResults;
				foreach (IntercommunicationReportDoubleResult intercommunicationReportDoubleResult in doubleResults)
				{
					if (_reportDoubleResults.TryGetValue(intercommunicationReportDoubleResult.StepSize, out var value))
					{
						value[intercommunicationReportDoubleResult.Id] = intercommunicationReportDoubleResult;
						continue;
					}
					value = new Dictionary<Guid, IntercommunicationReportDoubleResult> { { intercommunicationReportDoubleResult.Id, intercommunicationReportDoubleResult } };
					_reportDoubleResults.Add(intercommunicationReportDoubleResult.StepSize, value);
				}
			}
		}
		_parameters = parameters;
	}

	public IntercommunicationReportDateTimeResult[] DateTimeResults()
	{
		List<IntercommunicationReportDateTimeResult> list = new List<IntercommunicationReportDateTimeResult>();
		if (_report != null)
		{
			list.AddRange(_report.DateTimeResults);
		}
		return list.ToArray();
	}

	public IntercommunicationReportDateTimeResult DateTimeResult(Guid id)
	{
		IntercommunicationReportDateTimeResult value = null;
		_reportDateTimeResults.TryGetValue(id, out value);
		return value;
	}

	public IntercommunicationReportDoubleResult[] DoubleResults()
	{
		List<IntercommunicationReportDoubleResult> list = new List<IntercommunicationReportDoubleResult>();
		if (_report != null)
		{
			list.AddRange(_report.DoubleResults);
		}
		return list.ToArray();
	}

	public IntercommunicationReportDoubleResult DoubleResult(Guid id, int stepSize)
	{
		IntercommunicationReportDoubleResult value = null;
		if (_reportDoubleResults.TryGetValue(stepSize, out var value2))
		{
			value2.TryGetValue(id, out value);
		}
		return value;
	}

	public IntercommunicationReportServer[] Servers()
	{
		List<IntercommunicationReportServer> list = new List<IntercommunicationReportServer>();
		if (_report != null)
		{
			list.AddRange(_report.Servers);
		}
		return list.ToArray();
	}
}
