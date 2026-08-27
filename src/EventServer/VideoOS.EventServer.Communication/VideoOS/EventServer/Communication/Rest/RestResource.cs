using System;
using System.Globalization;

namespace VideoOS.EventServer.Communication.Rest;

public class RestResource
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Name, Id);
	}
}
