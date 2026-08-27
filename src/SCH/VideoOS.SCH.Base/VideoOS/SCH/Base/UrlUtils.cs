using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace VideoOS.SCH.Base;

public class UrlUtils
{
	public class UrlParts
	{
		public string Scheme;

		public string Domain;

		public string Port;

		public string Path;

		public string FileName;

		public string Extension;

		public string QueryString;

		public string FragmentID;

		public UrlParts(string scheme, string domain, string port, string path, string fileName, string extension, string queryString, string fragmentID)
		{
			Scheme = scheme;
			Domain = domain;
			Port = port;
			Path = path;
			FileName = fileName;
			Extension = extension;
			QueryString = queryString;
			FragmentID = fragmentID;
		}
	}

	public const string UrlRegexPattern = "# Regex for splitting an url into parts.# Full example: http://www.sub-domain.com:80/path1/path-2/path_3/file_name.htm?id=17#myFragment# Minimal example: http://localhost/#^                                     # Beginning of string\n(?<scheme>[^:/]+)://                  # Scheme; any character except ':' or '/'. Must end with '://'\n(?<domain>[A-Za-z0-9\\-]+(\\.[A-Za-z0-9\\-]+)*) # Domain; required, alphanum chars + '-' and '.'\n(:(?<port>[0-9]+))?/                  # Port; optional ':' and port number followed by a required '/'\n(?<path>(?<lastPath>[\\w\\-]+/)*)       # Path; optional, one or more alphanum + '-' chars always ends with '/'\n((?<file>[A-Za-z0-9_\\-]+)             # File; alphanum chars\n(\\.(?<ext>[A-Za-z0-9_\\-]+))?)?        # File extension; optional\n(\\?(?<query>[^\\#\\./\\s]+))?            # Query, any character (except whitespace, '.' and '/') ending with '#'\n(\\#(?<fragment>[^/\\.\\s]+))?           # Fragment, any character (except whitespace, '.' and '/')\n$                                     # End of string";

	public static bool ParseUrl(string url, out UrlParts parts)
	{
		if (url == null)
		{
			parts = null;
			return false;
		}
		Match match = null;
		match = new Regex("# Regex for splitting an url into parts.# Full example: http://www.sub-domain.com:80/path1/path-2/path_3/file_name.htm?id=17#myFragment# Minimal example: http://localhost/#^                                     # Beginning of string\n(?<scheme>[^:/]+)://                  # Scheme; any character except ':' or '/'. Must end with '://'\n(?<domain>[A-Za-z0-9\\-]+(\\.[A-Za-z0-9\\-]+)*) # Domain; required, alphanum chars + '-' and '.'\n(:(?<port>[0-9]+))?/                  # Port; optional ':' and port number followed by a required '/'\n(?<path>(?<lastPath>[\\w\\-]+/)*)       # Path; optional, one or more alphanum + '-' chars always ends with '/'\n((?<file>[A-Za-z0-9_\\-]+)             # File; alphanum chars\n(\\.(?<ext>[A-Za-z0-9_\\-]+))?)?        # File extension; optional\n(\\?(?<query>[^\\#\\./\\s]+))?            # Query, any character (except whitespace, '.' and '/') ending with '#'\n(\\#(?<fragment>[^/\\.\\s]+))?           # Fragment, any character (except whitespace, '.' and '/')\n$                                     # End of string", RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace).Match(url.Trim());
		if (match.Success)
		{
			string[] array = match.Groups["domain"].Value.Split('.');
			foreach (string text in array)
			{
				if (text.StartsWith("-", ignoreCase: true, CultureInfo.InvariantCulture) || text.EndsWith("-", ignoreCase: true, CultureInfo.InvariantCulture))
				{
					parts = null;
					return false;
				}
			}
			parts = new UrlParts(match.Groups["scheme"].Value, match.Groups["domain"].Value, match.Groups["port"].Value, match.Groups["path"].Value, match.Groups["file"].Value, match.Groups["ext"].Value, match.Groups["query"].Value, match.Groups["fragment"].Value);
			return true;
		}
		if (Uri.TryCreate(url, UriKind.Absolute, out var result) && result.HostNameType == UriHostNameType.IPv6)
		{
			ConvertSegments(result.Segments, out var path, out var file, out var ext);
			parts = new UrlParts(result.Scheme, result.Host, result.Port.ToString(), path, file, ext, RemoveFirstChar(result.Query), RemoveFirstChar(result.Fragment));
			return true;
		}
		parts = null;
		return false;
	}

	private static string RemoveFirstChar(string stringInput)
	{
		if (stringInput != null && stringInput.Length > 0)
		{
			return stringInput.Substring(1);
		}
		return string.Empty;
	}

	private static void ConvertSegments(string[] segments, out string path, out string file, out string ext)
	{
		if (segments == null || ((segments.Length == 1) & (segments[0] == "/")))
		{
			path = string.Empty;
			file = string.Empty;
			ext = string.Empty;
			return;
		}
		string text = string.Empty;
		if (segments.Length != 0)
		{
			text = Path.GetFileName(segments[segments.Length - 1]);
		}
		if (text.Length > 0)
		{
			file = Path.GetFileNameWithoutExtension(text);
			ext = RemoveFirstChar(Path.GetExtension(text));
			path = string.Empty;
			for (int i = 0; i < segments.Length - 1; i++)
			{
				path += segments[i];
			}
		}
		else
		{
			file = string.Empty;
			ext = string.Empty;
			path = string.Join(string.Empty, segments);
		}
		if (path.Length > 0 && string.Equals(path.Substring(0, 1), "/", StringComparison.InvariantCulture))
		{
			path = RemoveFirstChar(path);
		}
	}
}
