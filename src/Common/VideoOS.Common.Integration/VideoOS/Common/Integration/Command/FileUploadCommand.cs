using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Extensions;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class FileUploadCommand : CommonCommandBase
{
	[DataMember]
	public IServerFileStorageEntry Storage { get; private set; }

	public IProgress<int> UploadProgress { get; private set; }

	public FileUploadError UploadError { get; set; }

	public string ErrorMessage { get; set; }

	public FileUploadCommand(IServerFileStorageEntry storage)
	{
		Storage = storage;
	}

	public FileUploadCommand(IServerFileStorageEntry storage, IProgress<int> uploadProgress)
	{
		Storage = storage;
		UploadProgress = uploadProgress;
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

	public string GetPermanentId()
	{
		using Stream stream = Storage.CreateStream();
		return UploadCommandsCommon.GetPermanentIdFromHash(stream.GetSHA256Hash());
	}
}
