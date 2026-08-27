using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public enum RecorderShutdownState
{
	Running,
	Shutdown
}
