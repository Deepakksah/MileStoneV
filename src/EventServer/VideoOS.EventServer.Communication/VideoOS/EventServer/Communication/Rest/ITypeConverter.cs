using System;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Rest;

public interface ITypeConverter
{
	FQID GetFQID(Guid id);
}
