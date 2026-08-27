using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public abstract class ExportManager
{
	public abstract int Progress { get; }

	public abstract string LastErrorMessage { get; }

	public virtual bool IncludePluginFilesInExport => true;

	public virtual bool ExportPerformed => false;

	protected ExportManager()
	{
		new TrackPageViewData(typeof(ExportManager)).Send();
	}

	protected ExportManager(ExportParameters exportParameters)
	{
	}

	public abstract void ExportStarting();

	public abstract void ExportFailed();

	public abstract void ExportCancelled();

	public abstract void ExportComplete();

	public virtual void ExportComplete(int result)
	{
		ExportComplete();
	}

	public virtual ulong? EstimateSizeOfExport()
	{
		return null;
	}

	public virtual void EstimationCancelled()
	{
	}
}
