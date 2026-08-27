using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Management.Common.Tasks;

public class ArgumentCollection : Collection<Argument>
{
	public void Add(int index, string value)
	{
		Add(new Argument(index, value));
	}

	public void Add(int index, string value, string resourceId)
	{
		Add(new Argument(index, value, resourceId));
	}

	public string[] ValuesAsArray()
	{
		return (from e in this
			orderby e.Index
			select e.Value).ToArray();
	}
}
