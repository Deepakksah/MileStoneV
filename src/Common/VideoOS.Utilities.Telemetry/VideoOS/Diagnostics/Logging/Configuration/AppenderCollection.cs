using System.Collections;

namespace VideoOS.Diagnostics.Logging.Configuration;

public class AppenderCollection : CollectionBase
{
	public Appender this[int index] => (Appender)base.List[index];

	public AppenderCollection()
	{
	}

	public int Add(Appender value)
	{
		return base.List.Add(value);
	}
}
