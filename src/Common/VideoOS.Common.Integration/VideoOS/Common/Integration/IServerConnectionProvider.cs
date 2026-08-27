namespace VideoOS.Common.Integration;

public interface IServerConnectionProvider
{
	bool IsOnline { get; }
}
