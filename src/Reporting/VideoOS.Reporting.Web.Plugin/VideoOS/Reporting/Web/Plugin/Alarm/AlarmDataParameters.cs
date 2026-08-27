using System;

namespace VideoOS.Reporting.Web.Plugin.Alarm;

public class AlarmDataParameters : IEquatable<AlarmDataParameters>
{
	public string EventServerAddress { get; set; }

	public string Culture { get; set; }

	public string Token { get; set; }

	public int Interval { get; set; }

	public Guid SessionId { get; set; }

	public string ReportType { get; set; }

	public TimeZoneInfo TimeZone { get; set; }

	public Guid WcfSessionId { get; set; }

	public AlarmDataParameters()
	{
	}

	public AlarmDataParameters(Guid sessionId, string reportType, int interval, string eventServerAddress, string token, string culture, TimeZoneInfo timeZoneInfo, Guid wcfSessionId)
	{
		SessionId = sessionId;
		ReportType = reportType;
		Interval = interval;
		EventServerAddress = eventServerAddress;
		Token = token;
		Culture = culture;
		WcfSessionId = wcfSessionId;
		TimeZone = timeZoneInfo;
	}

	public void ValidateParameters()
	{
		if (string.IsNullOrEmpty(EventServerAddress))
		{
			throw new InvalidParameterException("EventServerAddress");
		}
		if (string.IsNullOrEmpty(ReportType))
		{
			throw new InvalidParameterException("ReportType");
		}
		if (string.IsNullOrEmpty(Culture))
		{
			throw new InvalidParameterException("Culture");
		}
		if (string.IsNullOrEmpty(Token))
		{
			throw new InvalidParameterException("Token");
		}
		if (TimeZone == null)
		{
			throw new InvalidParameterException("TimeZone");
		}
		_ = SessionId;
		_ = WcfSessionId;
	}

	public bool Equals(AlarmDataParameters other)
	{
		if (other == null)
		{
			return false;
		}
		if (EventServerAddress.Equals(other.EventServerAddress) && Culture.Equals(other.Culture) && Token.Equals(other.Token) && Interval.Equals(other.Interval) && SessionId.Equals(other.SessionId) && ReportType.Equals(other.ReportType) && TimeZone.Equals(other.TimeZone))
		{
			return WcfSessionId.Equals(other.WcfSessionId);
		}
		return false;
	}
}
