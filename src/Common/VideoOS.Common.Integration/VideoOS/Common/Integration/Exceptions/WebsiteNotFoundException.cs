using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class WebsiteNotFoundException : CorporateApplicationException
{
	public WebsiteNotFoundException()
	{
	}

	public WebsiteNotFoundException(string message)
		: base(message)
	{
	}

	public WebsiteNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public WebsiteNotFoundException(long websiteId)
		: base(string.Format(CultureInfo.InvariantCulture, "Web site with the id: {0} not found", websiteId))
	{
	}

	protected WebsiteNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
