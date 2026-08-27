namespace VideoOS.Mobile.Portable.Log;

public class Logger : ILog
{
	public static Logger Instance = new Logger();

	public ILog Implementer { private get; set; }

	private Logger()
	{
		Implementer = LogImplementer.Instance;
	}

	public void Log(LogContent content, string origin, string text, object dump = null)
	{
		Implementer?.Log(content, origin, text, dump);
	}
}
