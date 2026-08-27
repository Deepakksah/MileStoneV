using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class DatabaseResponseDiskUsage : DatabaseResponse
{
	[DataMember]
	public Guid Id { get; private set; }

	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public bool IsOnline { get; private set; }

	[DataMember]
	public Guid StorageId { get; private set; }

	[DataMember]
	public ulong UsedSpace { get; private set; }

	[DataMember]
	public ulong AvailableSpace { get; private set; }

	public DatabaseResponseDiskUsage(DatabaseOperation databaseOperation, Guid id, ulong usedSpace, ulong availableSpace)
		: base(databaseOperation)
	{
		Id = id;
		StorageId = Guid.Empty;
		UsedSpace = usedSpace;
		AvailableSpace = availableSpace;
	}

	public DatabaseResponseDiskUsage(DatabaseOperation databaseOperation, Guid id, ulong usedSpace, ulong availableSpace, Guid storageId, Guid recorderId, bool isOnline)
		: base(databaseOperation)
	{
		Id = id;
		StorageId = storageId;
		UsedSpace = usedSpace;
		AvailableSpace = availableSpace;
		RecorderId = recorderId;
		IsOnline = isOnline;
	}

	public DatabaseResponseDiskUsage(DatabaseOperation databaseOperation, Guid id, DatabaseResponseStatus status, string message)
		: base(databaseOperation, status, message)
	{
		Id = id;
		StorageId = Guid.Empty;
	}

	public DatabaseResponseDiskUsage(DatabaseOperation databaseOperation, Guid id, DatabaseResponseStatus status, string message, Guid storageId)
		: base(databaseOperation, status, message)
	{
		Id = id;
		StorageId = storageId;
	}

	public DatabaseResponseDiskUsage(DatabaseOperation databaseOperation, Guid id, ulong usedSpace, ulong availableSpace, DatabaseResponseStatus status, string message, Guid storageId, Guid recorderId, bool isOnline)
		: base(databaseOperation, status, message)
	{
		Id = id;
		RecorderId = recorderId;
		StorageId = storageId;
		UsedSpace = usedSpace;
		AvailableSpace = availableSpace;
		IsOnline = isOnline;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
