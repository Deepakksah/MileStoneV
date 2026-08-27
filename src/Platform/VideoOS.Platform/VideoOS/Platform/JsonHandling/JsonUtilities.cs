using System;
using System.Globalization;
using System.Text;

namespace VideoOS.Platform.JsonHandling;

public static class JsonUtilities
{
	public static string JsonEscape(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in value)
		{
			if (c == '"' || c == '\\')
			{
				stringBuilder.Append('\\');
				stringBuilder.Append(c);
			}
			else if (c < ' ')
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\\u{0:x4}", (int)c));
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	public static string Compress(string json)
	{
		if (json == null)
		{
			throw new ArgumentNullException("json");
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < json.Length; i++)
		{
			char c = json[i];
			if (flag2)
			{
				if (flag3)
				{
					flag3 = false;
				}
				else
				{
					switch (c)
					{
					case '"':
						flag2 = false;
						break;
					case '\\':
						flag3 = true;
						break;
					}
				}
			}
			else
			{
				bool flag4 = c == ' ' || c == '\n' || c == '\r' || c == '\t';
				if (flag && flag4)
				{
					continue;
				}
				if (flag4)
				{
					c = ' ';
				}
				flag = flag4;
				if (c == '"')
				{
					flag2 = true;
				}
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}
}
