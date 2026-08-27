using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class UploadFileChunkCommand : CommonCommandBase
{
	[DataMember]
	private byte[] _chunkBuffer;

	[DataMember]
	private byte[] _fileChecksum;

	[DataMember]
	public Guid TransferId { get; private set; }

	public ReadOnlyCollection<byte> ChunkBuffer => new ReadOnlyCollection<byte>(_chunkBuffer);

	public string PermanentId => UploadCommandsCommon.GetPermanentIdFromHash(_fileChecksum);

	[DataMember]
	public int OffsetInBytes { get; private set; }

	[DataMember]
	public long TotalSizeInBytes { get; private set; }

	public ReadOnlyCollection<byte> FileChecksum => new ReadOnlyCollection<byte>(_fileChecksum);

	public UploadFileChunkCommand(Guid transferId, byte[] chunkBuffer, int offsetInBytes, long totalSizeInBytes, byte[] fileChecksum)
	{
		TransferId = transferId;
		_chunkBuffer = chunkBuffer;
		OffsetInBytes = offsetInBytes;
		TotalSizeInBytes = totalSizeInBytes;
		_fileChecksum = fileChecksum;
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
