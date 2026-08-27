using System.Collections;

namespace VideoOS.Diagnostics.Logging.Configuration;

public class LoggerCollection : DictionaryBase
{
	public Logger this[string key]
	{
		get
		{
			return (Logger)base.Dictionary[key];
		}
		set
		{
			base.Dictionary[key] = value;
		}
	}

	public void Add(string key, Logger value)
	{
		base.Dictionary.Add(key, value);
	}
}
