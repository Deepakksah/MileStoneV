using System;

namespace VideoOS.Management.Server;

public interface IHostedHardwareComponent : IDisposable
{
	int CreateHostedHardware(HostedHardware hostedHardwareToCreate, Guid sessionId, string hostedHardwareType);

	string[] QueryHostedHardwareIds(int hostingServerId);

	HostedHardware QueryHostedHardware(int hostedHardwareId);

	HostedHardware[] QueryHostedHardwaresByIds(int[] hostedHardwareIds);

	void UpdateHostedHardware(HostedHardware hostedHardware, Guid sessionId);

	void DeleteHostedHardware(int hostedHardwareId, Guid sessionId);
}
