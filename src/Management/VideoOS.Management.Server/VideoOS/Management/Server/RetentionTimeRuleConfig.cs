using System;

namespace VideoOS.Management.Server;

public sealed class RetentionTimeRuleConfig
{
	private int _counterId;

	private Guid _storageId;

	private int _datacollectorId;

	private string _path;

	private long _storageSizeMb;

	public int CounterId
	{
		get
		{
			return _counterId;
		}
		set
		{
			_counterId = value;
		}
	}

	public Guid StorageId
	{
		get
		{
			return _storageId;
		}
		set
		{
			_storageId = value;
		}
	}

	public long StorageSizeMb
	{
		get
		{
			return _storageSizeMb;
		}
		set
		{
			_storageSizeMb = value;
		}
	}

	public int DatacollectorId
	{
		get
		{
			return _datacollectorId;
		}
		set
		{
			_datacollectorId = value;
		}
	}

	public string Path
	{
		get
		{
			return _path;
		}
		set
		{
			_path = value;
		}
	}

	public static RetentionTimeRuleConfig[] QueryStorageCounters(GeminiRequestContext requestContext)
	{
		RetentionTimeRuleConfig[] array = null;
		using PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>();
		return performanceStateRuleConfigComponent.QueryStorageCounters();
	}
}
