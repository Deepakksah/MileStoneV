using Microsoft.Extensions.Logging;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces.Msi;

namespace VideoOS.Installer.Sequencer;

public class SequenceManagerWrapper : ISequenceManagerWrapper
{
	public ISequenceManager SequenceManager { get; set; }

	public ITaskSequencer TaskSequencer { get; set; }

	public SequenceManagerWrapper()
	{
	}

	public SequenceManagerWrapper(IClMsiHandler handler, InstallationParams currentInstallation, InstallationParams oldInstallation, ILogger log, PlatformType platform, int websitePort, bool isDebugMode)
	{
		TaskSequencer = new TaskSequencer();
		SequenceManager = new SequenceManager(handler, currentInstallation, oldInstallation, log, platform, websitePort, TaskSequencer, isDebugMode);
	}
}
