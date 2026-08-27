using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[Obsolete("We need to keep this class only because it is used in legacy interfaces. It will never really be used for anything anymore.")]
public sealed class DatabaseResponseVerifyStoragePathDto : DatabaseResponseDto
{
	public string FullPath;
}
