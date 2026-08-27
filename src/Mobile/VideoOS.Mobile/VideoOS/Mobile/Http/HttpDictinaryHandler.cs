using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using VideoOS.Mobile.Portable.Http;

namespace VideoOS.Mobile.Http;

public class HttpDictinaryHandler
{
	public HttpStatuses Process(HttpListenerContext httpContext, Dictionary<string, string> values)
	{
		return Process(httpContext, (IEnumerable<KeyValuePair<string, string>>)values);
	}

	public HttpStatuses Process(HttpListenerContext httpContext, NameValueCollection values)
	{
		return Process(httpContext, values.AsEnumerable().AsKeyValuePairs());
	}

	public HttpStatuses Process(HttpListenerContext httpContext, IEnumerable<KeyValuePair<string, string>> kvps)
	{
		return HttpJsBaseHandler.Process(httpContext, delegate(StringBuilder builder)
		{
			builder.Append("{");
			bool flag = true;
			foreach (KeyValuePair<string, string> kvp in kvps)
			{
				string element = ReplaceIllegalCharacters(kvp.Key);
				string element2 = ReplaceIllegalCharacters(kvp.Value);
				if (!flag)
				{
					builder.Append(",");
				}
				builder.AppendJsonElement(element);
				builder.Append(":");
				builder.AppendJsonElement(element2);
				flag = false;
			}
			builder.Append("}");
		});
	}

	private string ReplaceIllegalCharacters(string input)
	{
		input = input.Replace("<", "&lt;");
		return input.Replace(">", "&gt;");
	}
}
