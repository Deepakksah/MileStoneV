namespace VideoOS.Common.Integration;

public interface IEventTrace
{
	void Trace(string message);

	void Trace(string format, params object[] args);
}
