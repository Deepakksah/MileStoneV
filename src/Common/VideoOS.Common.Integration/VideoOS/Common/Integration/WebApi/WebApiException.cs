using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class WebApiException : Exception
{
	public string Route { get; set; }

	public WebApiException()
	{
	}

	public WebApiException(string message)
		: base(message)
	{
	}

	public WebApiException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected WebApiException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		Route = info.GetString("Route");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("Route", Route);
		base.GetObjectData(info, context);
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "Route: {0} {1} {2}", Route, Environment.NewLine, base.ToString());
	}
}
