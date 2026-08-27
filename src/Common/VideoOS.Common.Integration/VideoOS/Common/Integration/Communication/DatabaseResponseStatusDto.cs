using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public enum DatabaseResponseStatusDto
{
	Success = 1,
	Error,
	Warning
}
