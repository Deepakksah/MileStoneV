using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class FileUploadToRecorderCommand : CommonCommandBase
{
	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public string FileId { get; private set; }

	public FileUploadToRecorderCommand(Guid recorderId, string fileId)
	{
		RecorderId = recorderId;
		FileId = fileId;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
