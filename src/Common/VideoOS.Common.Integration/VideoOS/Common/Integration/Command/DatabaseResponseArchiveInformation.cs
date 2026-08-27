using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class DatabaseResponseArchiveInformation : DatabaseResponse
{
	[DataMember]
	public Guid ID { get; private set; }

	[DataMember]
	public byte[] ArchiveInfo { get; private set; }

	public DatabaseResponseArchiveInformation(DatabaseOperation databaseOperation, Guid id, byte[] archiveInfo)
		: base(databaseOperation)
	{
		ID = id;
		ArchiveInfo = archiveInfo;
	}

	public DatabaseResponseArchiveInformation(DatabaseOperation databaseOperation, Guid id, DatabaseResponseStatus status, string message)
		: base(databaseOperation, status, message)
	{
		ID = id;
	}

	public DatabaseResponseArchiveInformation(DatabaseOperation databaseOperation, Guid id, byte[] archiveInfo, DatabaseResponseStatus status, string message)
		: base(databaseOperation, status, message)
	{
		ID = id;
		ArchiveInfo = archiveInfo;
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
