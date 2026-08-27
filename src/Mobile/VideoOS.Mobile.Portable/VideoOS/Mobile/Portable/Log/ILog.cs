namespace VideoOS.Mobile.Portable.Log;

public interface ILog
{
	void Log(LogContent content, string origin, string text, object dump = null);
}
