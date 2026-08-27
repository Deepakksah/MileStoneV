using System.Collections;

namespace VideoOS.Platform.Util;

public interface IStringSplitter
{
	string JoinString(IEnumerable enumerable, char[] separators);

	IList SplitString(string input, char[] separators);

	string Escape(string input, char[] separators);

	string Unescape(string input, char[] separators);
}
