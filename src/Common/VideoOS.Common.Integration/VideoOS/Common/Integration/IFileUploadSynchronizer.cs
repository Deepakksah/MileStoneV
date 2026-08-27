using System;

namespace VideoOS.Common.Integration;

public interface IFileUploadSynchronizer : IDisposable
{
	bool Upload(Guid recorderId, string fileId, TimeSpan timeout, FileExistsFunction existsFunc, FileUploadFunction uploadFunc);
}
