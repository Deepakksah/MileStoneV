namespace VideoOS.Platform.Util.Queue;

public class LevelConfiguration
{
	public int MaxConcurrentNodes { get; }

	public LevelConfiguration(int maxConcurrentNodes)
	{
		MaxConcurrentNodes = maxConcurrentNodes;
	}
}
