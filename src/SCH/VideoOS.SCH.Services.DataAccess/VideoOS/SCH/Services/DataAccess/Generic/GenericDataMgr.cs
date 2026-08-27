using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.DataAccess.Memory;
using VideoOS.SCH.Services.DataAccess.SQLServer;

namespace VideoOS.SCH.Services.DataAccess.Generic;

public class GenericDataMgr
{
	public static ISessionDataMgr GetSessionDataMgr()
	{
		return ConfigSettings.AppDataStorageType switch
		{
			AppDataStorageType.InMemory => AppData.Current, 
			AppDataStorageType.SQLServer => new SessionData(), 
			_ => throw new ServiceChannelException("Unknown AppDataStorageType in GetSessionDataMgr()."), 
		};
	}

	public static IMessageDataMgr GetMessageDataMgr()
	{
		return ConfigSettings.AppDataStorageType switch
		{
			AppDataStorageType.InMemory => AppData.Current, 
			AppDataStorageType.SQLServer => new VideoOS.SCH.Services.DataAccess.SQLServer.MessageData(), 
			_ => throw new ServiceChannelException("Unknown AppDataStorageType in GetMessageDataMgr()."), 
		};
	}

	public static IMessageWatchDataMgr GetMessageWatchDataMgr()
	{
		return ConfigSettings.AppDataStorageType switch
		{
			AppDataStorageType.InMemory => AppData.Current, 
			AppDataStorageType.SQLServer => new MessageWatchData(), 
			_ => throw new ServiceChannelException("Unknown AppDataStorageType in GetMessageWatchDataMgr()."), 
		};
	}

	public static IBatchJobDataMgr GetBatchJobDataMgr()
	{
		return ConfigSettings.AppDataStorageType switch
		{
			AppDataStorageType.InMemory => AppData.Current, 
			AppDataStorageType.SQLServer => new BatchJobData(), 
			_ => throw new ServiceChannelException("Unknown AppDataStorageType in GetBatchJobDataMgr()."), 
		};
	}
}
