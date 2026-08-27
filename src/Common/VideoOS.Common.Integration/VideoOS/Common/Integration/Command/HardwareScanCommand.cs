using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.DeviceDiscovery;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class HardwareScanCommand : CommonCommandBase
{
	public const int DefaultTimeoutInSeconds = 105;

	[DataMember]
	public HardwareScanOperation Operation { get; private set; }

	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public Guid SessionId { get; private set; }

	[DataMember]
	public string CredentialsXml { get; private set; }

	[DataMember]
	public bool UseDefaultCredentials { get; private set; }

	[DataMember]
	public bool UseSecurePort { get; private set; }

	[DataMember]
	public string PrioritizedDriversXml { get; private set; }

	[DataMember]
	public string SelectedDriversXml { get; private set; }

	[DataMember]
	public string AddressIntervalsXml { get; private set; }

	[DataMember]
	public string AddressDriverPairsXml { get; private set; }

	[DataMember]
	public string HostedScanParametersXml { get; private set; }

	public static HardwareScanCommand Create(Guid recorderId, Guid sessionId, HardwareScanOperation hardwareScanOperation, string credentialsXml, bool useDefaultCredentials, bool useSecurePort, string prioritizedDriversXml, string selectedDriversXml, string addressIntervalsXml, string addressDriverPairsXml, string hostedScanParametersXml)
	{
		return new HardwareScanCommand
		{
			RecorderId = recorderId,
			SessionId = sessionId,
			Operation = hardwareScanOperation,
			CredentialsXml = credentialsXml,
			UseDefaultCredentials = useDefaultCredentials,
			UseSecurePort = useSecurePort,
			PrioritizedDriversXml = prioritizedDriversXml,
			SelectedDriversXml = selectedDriversXml,
			AddressIntervalsXml = addressIntervalsXml,
			AddressDriverPairsXml = addressDriverPairsXml,
			HostedScanParametersXml = hostedScanParametersXml
		};
	}

	public HardwareScanCommand()
	{
		SessionId = Guid.NewGuid();
	}

	public void StartExpressScanSession(Guid recorderId, Credentials credentials, bool useDefaultCredentials, bool useSecurePort, ScanPriorities prioritizedDrivers)
	{
		if (credentials == null)
		{
			throw new ArgumentNullException("credentials");
		}
		if (prioritizedDrivers == null)
		{
			throw new ArgumentNullException("prioritizedDrivers");
		}
		RecorderId = recorderId;
		Operation = HardwareScanOperation.ExpressScan;
		CredentialsXml = credentials.ToXml();
		UseDefaultCredentials = useDefaultCredentials;
		UseSecurePort = useSecurePort;
		PrioritizedDriversXml = prioritizedDrivers.ToXml();
	}

	public void StartRangeScanSession(Guid recorderId, Credentials credentials, bool useDefaultCredentials, ScanPriorities prioritizedDrivers, ScanPriorities selectedDrivers, AddressIntervals addressIntervals)
	{
		if (credentials == null)
		{
			throw new ArgumentNullException("credentials");
		}
		if (prioritizedDrivers == null)
		{
			throw new ArgumentNullException("prioritizedDrivers");
		}
		if (selectedDrivers == null)
		{
			throw new ArgumentNullException("selectedDrivers");
		}
		if (addressIntervals == null)
		{
			throw new ArgumentNullException("addressIntervals");
		}
		RecorderId = recorderId;
		Operation = HardwareScanOperation.RangeScan;
		CredentialsXml = credentials.ToXml();
		UseDefaultCredentials = useDefaultCredentials;
		PrioritizedDriversXml = prioritizedDrivers.ToXml();
		SelectedDriversXml = selectedDrivers.ToXml();
		AddressIntervalsXml = addressIntervals.ToXml();
	}

	public void StartManualScanSession(Guid recorderId, Credentials credentials, bool useDefaultCredentials, ScanPriorities prioritizedDrivers, ScanPriorities selectedDrivers, AddressDriverPairs addressDriverPairs)
	{
		if (credentials == null)
		{
			throw new ArgumentNullException("credentials");
		}
		if (prioritizedDrivers == null)
		{
			throw new ArgumentNullException("prioritizedDrivers");
		}
		if (selectedDrivers == null)
		{
			throw new ArgumentNullException("selectedDrivers");
		}
		if (addressDriverPairs == null)
		{
			throw new ArgumentNullException("addressDriverPairs");
		}
		RecorderId = recorderId;
		Operation = HardwareScanOperation.ManualScan;
		CredentialsXml = credentials.ToXml();
		UseDefaultCredentials = useDefaultCredentials;
		PrioritizedDriversXml = prioritizedDrivers.ToXml();
		SelectedDriversXml = selectedDrivers.ToXml();
		AddressDriverPairsXml = addressDriverPairs.ToXml();
	}

	public void StartHostedScanSession(Guid recorderId, HostedScanParameters hostedScanParameters)
	{
		if (hostedScanParameters == null)
		{
			throw new ArgumentNullException("hostedScanParameters");
		}
		RecorderId = recorderId;
		Operation = HardwareScanOperation.HostedScan;
		HostedScanParametersXml = hostedScanParameters.ToXml();
	}

	public void CancelScanSession(Guid recorderId, Guid sessionId)
	{
		RecorderId = recorderId;
		Operation = HardwareScanOperation.CancelScan;
		SessionId = sessionId;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
