using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class HardwareScanModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid RecorderId { get; set; }

	[RequiredNonemptyGuid]
	public Guid SessionId { get; set; }

	[Required]
	public HardwareScanOperationDto2 HardwareScanOperationDto { get; set; }

	public string CredentialsXml { get; set; }

	[Required]
	public bool UseDefaultCredentials { get; set; }

	public bool UseSecurePort { get; set; }

	public string PrioritizedDriversXml { get; set; }

	public string SelectedDriversXml { get; set; }

	public string AddressIntervalsXml { get; set; }

	public string AddressDriverPairsXml { get; set; }

	public string HostedScanParametersXml { get; set; }
}
