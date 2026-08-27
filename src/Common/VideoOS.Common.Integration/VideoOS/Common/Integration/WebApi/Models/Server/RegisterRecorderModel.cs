using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class RegisterRecorderModel
{
	[RequiredNonemptyGuid]
	public Guid RecorderId { get; set; }

	public int RecorderServiceType { get; set; }

	public RecorderVersion Version { get; set; }

	[Required]
	public Uri ServerCommunicationWebApiUri { get; set; }

	public string RecorderName { get; set; }

	public string HostName { get; set; }

	public Uri WebServiceUri { get; set; }

	public ushort FailoverPort { get; set; }
}
