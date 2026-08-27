using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using VideoOS.Reporting.Web.Plugin.Business.Alarm;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

namespace VideoOS.Reporting.Web.Plugin.Alarm;

public class AlarmDataLayer
{
	private static Dictionary<Guid, LocalReportData> _lastReportData = new Dictionary<Guid, LocalReportData>();

	private static object _lastReportDataLock = new object();

	private static IntercommunicationReportRequest GenerateReportRequest(AlarmDataParameters parameters)
	{
		return new IntercommunicationReportRequest
		{
			ReportType = parameters.ReportType,
			Interval = parameters.Interval,
			Data = "<stepSizes><stepSize>1</stepSize><stepSize>60</stepSize><stepSize>1440</stepSize></stepSizes>"
		};
	}

	private static void GenerateReport(AlarmDataParameters alarmDataParameters, IIntercommunicationCommandAgent agent)
	{
		try
		{
			if (alarmDataParameters == null)
			{
				throw new InvalidParameterException("alarmDataParameters");
			}
			alarmDataParameters.ValidateParameters();
			lock (_lastReportDataLock)
			{
				if (_lastReportData.TryGetValue(alarmDataParameters.SessionId, out var value))
				{
					if (value.Parameters.Equals(alarmDataParameters))
					{
						return;
					}
					_lastReportData.Remove(alarmDataParameters.SessionId);
				}
			}
			IntercommunicationReportRequest request = GenerateReportRequest(alarmDataParameters);
			IntercommunicationReport intercommunicationReport = agent.GenerateReport(alarmDataParameters.EventServerAddress, alarmDataParameters.Token, alarmDataParameters.Culture, alarmDataParameters.TimeZone, alarmDataParameters.WcfSessionId, request);
			if (intercommunicationReport != null)
			{
				lock (_lastReportDataLock)
				{
					LocalReportData value2 = new LocalReportData(intercommunicationReport, alarmDataParameters);
					_lastReportData[alarmDataParameters.SessionId] = value2;
					return;
				}
			}
		}
		catch (EndpointNotFoundException)
		{
			throw new CommunicationException();
		}
		catch (FaultException<IntercommunicationServiceFault> ex2)
		{
			switch (ex2.Detail.ErrorReason)
			{
			case IntercommunicationFaultType.Unauthorized:
				throw new UnauthorizedAccessException();
			case IntercommunicationFaultType.InvalidTimeout:
				throw new InvalidTimeoutException();
			case IntercommunicationFaultType.InvalidToken:
				throw new InvalidTokenException();
			default:
				throw new UnknownErrorException();
			}
		}
		catch (InvalidParameterException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new UnknownErrorException();
		}
	}

	public static void ClearReportCache(Guid sessionId)
	{
		lock (_lastReportDataLock)
		{
			if (_lastReportData.ContainsKey(sessionId))
			{
				_lastReportData.Remove(sessionId);
			}
		}
	}

	public static List<ServerData> GetServers(AlarmDataParameters alarmDataParameters, IIntercommunicationCommandAgent agent)
	{
		lock (_lastReportDataLock)
		{
			return ConvertToServerData(GetReport(alarmDataParameters, agent));
		}
	}

	public static List<AlarmSubReport> GetAlarmSubReports(AlarmDataParameters alarmDataParameters, IIntercommunicationCommandAgent agent)
	{
		lock (_lastReportDataLock)
		{
			return ConvertToSubReports(GetReport(alarmDataParameters, agent));
		}
	}

	public static List<HistoryData> GetHistoryData(Guid reportId, AlarmDataParameters alarmDataParameters, IIntercommunicationCommandAgent agent)
	{
		lock (_lastReportDataLock)
		{
			return ConvertToHistoryData(GetReport(alarmDataParameters, agent), reportId);
		}
	}

	public static List<SummaryData> GetSummaryData(Guid reportId, int SummaryIntervalInMinutes, AlarmDataParameters alarmDataParameters, IIntercommunicationCommandAgent agent)
	{
		lock (_lastReportDataLock)
		{
			return ConvertToSummaryData(GetReport(alarmDataParameters, agent), reportId, SummaryIntervalInMinutes);
		}
	}

	private static LocalReportData GetReport(AlarmDataParameters alarmDataParameters, IIntercommunicationCommandAgent agent)
	{
		lock (_lastReportDataLock)
		{
			GenerateReport(alarmDataParameters, agent);
			if (_lastReportData.TryGetValue(alarmDataParameters.SessionId, out var value))
			{
				return value;
			}
			throw new UnknownErrorException("Report was not generated");
		}
	}

	private static List<ServerData> ConvertToServerData(LocalReportData tempReport)
	{
		List<ServerData> list = new List<ServerData>();
		IntercommunicationReportServer[] array = tempReport.Servers();
		foreach (IntercommunicationReportServer intercommunicationReportServer in array)
		{
			list.Add(new ServerData
			{
				Name = intercommunicationReportServer.Name,
				Included = intercommunicationReportServer.Included,
				StateField = intercommunicationReportServer.State
			});
		}
		return list;
	}

	private static List<SummaryData> ConvertToSummaryData(LocalReportData tempReport, Guid reportId, int summaryIntervalInMinutes)
	{
		IntercommunicationReportDoubleResult intercommunicationReportDoubleResult = tempReport.DoubleResult(reportId, summaryIntervalInMinutes);
		if (intercommunicationReportDoubleResult == null)
		{
			throw new InvalidParameterException("Unknown reportId for summary results");
		}
		List<SummaryData> list = new List<SummaryData>(intercommunicationReportDoubleResult.Data.Length);
		for (int i = 0; i < intercommunicationReportDoubleResult.Data.Length; i++)
		{
			list.Add(new SummaryData
			{
				SummaryStep = intercommunicationReportDoubleResult.Data[i].Key,
				Value = intercommunicationReportDoubleResult.Data[i].Value
			});
		}
		return list;
	}

	private static List<HistoryData> ConvertToHistoryData(LocalReportData reportData, Guid reportId)
	{
		IntercommunicationReportDateTimeResult intercommunicationReportDateTimeResult = reportData.DateTimeResult(reportId);
		if (intercommunicationReportDateTimeResult == null)
		{
			throw new InvalidParameterException("Unknown reportId for datetime results");
		}
		List<HistoryData> list = new List<HistoryData>(intercommunicationReportDateTimeResult.Data.Length);
		for (int i = 0; i < intercommunicationReportDateTimeResult.Data.Length; i++)
		{
			list.Add(new HistoryData
			{
				Time = intercommunicationReportDateTimeResult.Data[i].Key,
				Value = intercommunicationReportDateTimeResult.Data[i].Value
			});
		}
		return list;
	}

	private static List<AlarmSubReport> ConvertToSubReports(LocalReportData reportData)
	{
		List<AlarmSubReport> list = new List<AlarmSubReport>();
		IntercommunicationReportDateTimeResult[] array = reportData.DateTimeResults();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(new AlarmSubReport
				{
					Name = array[i].Name,
					ReportId = array[i].Id
				});
			}
		}
		IntercommunicationReportDoubleResult[] doubleResults = reportData.DoubleResults();
		if (doubleResults.Length != 0)
		{
			int j;
			for (j = 0; j < doubleResults.Length; j++)
			{
				if (!list.Any((AlarmSubReport item) => item.ReportId == doubleResults[j].Id))
				{
					list.Add(new AlarmSubReport
					{
						Name = doubleResults[j].Name,
						ReportId = doubleResults[j].Id
					});
				}
			}
		}
		return list;
	}
}
