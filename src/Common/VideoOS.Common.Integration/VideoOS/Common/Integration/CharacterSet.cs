using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration;

public class CharacterSet
{
	public IList<char> CharactersList { get; private set; }

	public CharacterSet(IEnumerable<char> values)
	{
		CharactersList = new List<char>(values);
	}

	public CharacterSet()
	{
		CharactersList = new List<char>();
	}

	public IEnumerable<char> Union(IEnumerable<char> other)
	{
		return CharactersList.Union(other);
	}
}
