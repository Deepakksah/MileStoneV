using System;
using System.Collections.Generic;

namespace VideoOS.Platform.EventsData;

public class AccessControlData
{
	public Guid? System { get; set; }

	public string Id { get; set; }

	public Guid? EventType { get; set; }

	public Guid? Source { get; set; }

	public Guid? SourceType { get; set; }

	public string[] CredentialHolders { get; set; }

	public Guid[] Elements { get; set; }

	public Guid[] Categories { get; set; }

	public string[] SurveillanceItems { get; set; }

	public string Reason { get; set; }

	public Dictionary<string, string> Properties { get; set; }
}
