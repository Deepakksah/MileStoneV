using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class DriverConfigData
{
	public Guid Id { get; set; }

	public Guid RecordingServerServiceId { get; set; }

	public string Name { get; set; }

	public int Number { get; set; }

	public string Version { get; set; }

	public string XmlVersion { get; set; }

	public DateTime LastModified { get; set; }

	public static DriverConfigData QueryByRecordingServerServiceAndNumber(IDataComponentFactory componentFactory, Guid recordingServerServiceId, int driverNumber)
	{
		using IDriverConfigDataComponent driverConfigDataComponent = componentFactory.CreateDriverConfigDataComponent();
		return driverConfigDataComponent.QueryByRecordingServerServiceAndNumber(recordingServerServiceId, driverNumber);
	}
}
