using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class DatabaseResponseDiskUsages : DatabaseResponse
{
	[DataMember]
	public DatabaseResponseDiskUsage[] DatabaseResponseDiskUsageList { get; private set; }

	public DatabaseResponseDiskUsages(DatabaseOperation databaseOperation, DatabaseResponseDiskUsage[] diskUsage)
		: base(databaseOperation)
	{
		DatabaseResponseDiskUsageList = diskUsage;
	}

	public DatabaseResponseDiskUsages(DatabaseOperation databaseOperation, DatabaseResponseStatus status, string message)
		: base(databaseOperation, status, message)
	{
	}

	public DatabaseResponseDiskUsages(DatabaseOperation databaseOperation, DatabaseResponseDiskUsage[] diskUsage, DatabaseResponseStatus status, string message)
		: base(databaseOperation, status, message)
	{
		DatabaseResponseDiskUsageList = diskUsage;
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
