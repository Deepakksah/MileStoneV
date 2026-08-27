using System;
using System.Collections.Generic;
using VideoOS.Reporting.Web.Plugin.Alarm;
using VideoOS.Reporting.Web.Plugin.Business.Alarm;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Plugin.Business;

public static class AlarmProvider
{
	public static void ClearAlarmCache(Guid sessionId)
	{
		AlarmDataLayer.ClearReportCache(sessionId);
	}

	public static List<ServerData> GetServers(Guid sessionId, AlarmReportType reportType, AlarmTimescale alarmTimescale, string eventServerAddress, TimeZoneInfo timeZoneInfo, string token, string culture)
	{
		return AlarmDataLayer.GetServers(new AlarmDataParameters(sessionId, TranslateReportType(reportType), TranslateAlarmTimeScale(alarmTimescale), eventServerAddress, token, culture, timeZoneInfo, Guid.Empty), new IntercommunicationCommandAgentWrapper());
	}

	public static List<AlarmSubReport> GetSubReports(Guid sessionId, AlarmReportType reportType, AlarmTimescale alarmTimescale, string eventServerAddress, TimeZoneInfo timeZoneInfo, string token, string culture)
	{
		return AlarmDataLayer.GetAlarmSubReports(new AlarmDataParameters(sessionId, TranslateReportType(reportType), TranslateAlarmTimeScale(alarmTimescale), eventServerAddress, token, culture, timeZoneInfo, Guid.Empty), new IntercommunicationCommandAgentWrapper());
	}

	public static List<HistoryData> GetHistoryData(Guid sessionId, Guid reportId, AlarmReportType reportType, AlarmTimescale alarmTimescale, string eventServerAddress, TimeZoneInfo timeZoneInfo, string token, string culture)
	{
		AlarmDataParameters alarmDataParameters = new AlarmDataParameters(sessionId, TranslateReportType(reportType), TranslateAlarmTimeScale(alarmTimescale), eventServerAddress, token, culture, timeZoneInfo, Guid.Empty);
		return AlarmDataLayer.GetHistoryData(reportId, alarmDataParameters, new IntercommunicationCommandAgentWrapper());
	}

	public static List<SummaryData> GetSummaryData(Guid sessionId, Guid reportId, AlarmReportType reportType, AlarmTimescale alarmTimescale, ResponseTime responseTimes, string eventServerAddress, TimeZoneInfo timeZoneInfo, string token, string culture)
	{
		AlarmDataParameters alarmDataParameters = new AlarmDataParameters(sessionId, TranslateReportType(reportType), TranslateAlarmTimeScale(alarmTimescale), eventServerAddress, token, culture, timeZoneInfo, Guid.Empty);
		List<SummaryData> summaryData = AlarmDataLayer.GetSummaryData(reportId, TranslateResponseTimesToSummaryTimeIntervalInMinutes(responseTimes), alarmDataParameters, new IntercommunicationCommandAgentWrapper());
		if (summaryData[summaryData.Count - 2].SummaryStep == 0.0)
		{
			summaryData.RemoveRange(summaryData.Count - 3, 3);
			summaryData.RemoveAt(0);
			for (int i = 0; i < summaryData.Count; i++)
			{
				summaryData[i].SummaryStep = i;
			}
		}
		return summaryData.FindAll((SummaryData item) => item.SummaryStep >= 0.0 && item.SummaryStep <= (double)TranslateResponseTimesToCutoffTime(responseTimes));
	}

	private static int TranslateAlarmTimeScale(AlarmTimescale alarmTimescale)
	{
		return alarmTimescale switch
		{
			AlarmTimescale.Day => 1440, 
			AlarmTimescale.Month => 44640, 
			AlarmTimescale.SixMonth => 267840, 
			AlarmTimescale.Week => 10080, 
			AlarmTimescale.Year => 525600, 
			_ => throw new NotSupportedException(alarmTimescale.ToString()), 
		};
	}

	private static string TranslateReportType(AlarmReportType reportType)
	{
		return reportType switch
		{
			AlarmReportType.Empty => throw new NotSupportedException(reportType.ToString()), 
			AlarmReportType.Category => "Category", 
			AlarmReportType.State => "State", 
			AlarmReportType.Priority => "Priority", 
			AlarmReportType.ReasonCode => "ReasonCode", 
			AlarmReportType.Site => "Site", 
			AlarmReportType.ResponseTime => "ResponseTime", 
			_ => throw new NotSupportedException(reportType.ToString()), 
		};
	}

	private static int TranslateResponseTimesToSummaryTimeIntervalInMinutes(ResponseTime responseTime)
	{
		return responseTime switch
		{
			ResponseTime.ThirtyMinutes => 1, 
			ResponseTime.OneHour => 1, 
			ResponseTime.TwoHours => 1, 
			ResponseTime.EightHours => 60, 
			ResponseTime.OneDay => 60, 
			ResponseTime.TwoDays => 60, 
			ResponseTime.OneWeek => 1440, 
			_ => throw new NotSupportedException(responseTime.ToString()), 
		};
	}

	private static int TranslateResponseTimesToCutoffTime(ResponseTime responseTime)
	{
		return responseTime switch
		{
			ResponseTime.ThirtyMinutes => 30, 
			ResponseTime.OneHour => 60, 
			ResponseTime.TwoHours => 120, 
			ResponseTime.EightHours => 8, 
			ResponseTime.OneDay => 24, 
			ResponseTime.TwoDays => 48, 
			ResponseTime.OneWeek => 7, 
			_ => throw new NotSupportedException(responseTime.ToString()), 
		};
	}
}
