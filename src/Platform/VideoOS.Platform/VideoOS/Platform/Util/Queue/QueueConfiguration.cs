using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Util.Queue;

public class QueueConfiguration
{
	public static LevelConfiguration RootLevelConfiguration = new LevelConfiguration(int.MaxValue);

	public int MaxConcurrentRequests { get; }

	public List<LevelConfiguration> LevelConfigurations { get; } = new List<LevelConfiguration>();

	public Func<LevelConfiguration, object, object> LevelIdentifierFunc { get; }

	public int MaxResourceProcessorsPerLevel { get; }

	public int MaxIdleTimeMillisBeforeResourceProcessorCleanup { get; }

	public QueueConfiguration(int maxConcurrentRequests, int maxResourceProcessorsPerLevel, int maxIdleTimeMillisBeforeResourceProcessorCleanup, Func<LevelConfiguration, object, object> levelIdentifierFunc)
	{
		MaxConcurrentRequests = maxConcurrentRequests;
		MaxResourceProcessorsPerLevel = maxResourceProcessorsPerLevel;
		MaxIdleTimeMillisBeforeResourceProcessorCleanup = maxIdleTimeMillisBeforeResourceProcessorCleanup;
		LevelIdentifierFunc = levelIdentifierFunc;
	}
}
