namespace VideoOS.Vms.Utilities.Oem;

public interface IOemResources
{
	IAppResources App { get; }

	IServerResources Server { get; }

	IRecorderResources Recorder { get; }

	IManagerResources Manager { get; }
}
