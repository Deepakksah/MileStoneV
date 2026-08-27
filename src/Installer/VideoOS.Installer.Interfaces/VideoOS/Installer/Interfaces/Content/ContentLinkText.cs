using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace VideoOS.Installer.Interfaces.Content;

public class ContentLinkText
{
	public string Text { get; }

	public IEnumerable<ContentLink> ContentLinks { get; }

	public ContentLinkText(string text)
	{
		Text = text;
		ContentLinks = Enumerable.Empty<ContentLink>();
	}

	public ContentLinkText(string format, params Uri[] uris)
		: this(format, Array.ConvertAll(uris, (Uri item) => item.ToString()), uris)
	{
	}

	public ContentLinkText(string format, params (string, Uri)[] links)
		: this(format, Array.ConvertAll(links, ((string, Uri) link) => link.Item1), Array.ConvertAll(links, ((string, Uri) link) => link.Item2))
	{
	}

	public ContentLinkText(string format, params (string, Action)[] links)
	{
		string[] array = Array.ConvertAll(links, ((string, Action) link) => link.Item1);
		Action[] urisOrActions = Array.ConvertAll(links, ((string, Action) link) => link.Item2);
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		object[] args = array;
		Text = string.Format(invariantCulture, format, args);
		ContentLinks = GenerateContentLinks(format, array, urisOrActions);
	}

	public ContentLinkText(string format, string[] linkTexts, Uri[] uris)
	{
		if (linkTexts == null)
		{
			throw new ArgumentNullException("linkTexts");
		}
		if (uris == null)
		{
			throw new ArgumentNullException("uris");
		}
		if (linkTexts.Length != uris.Length)
		{
			throw new ArgumentException("The specified arrays must contain the same number of elements.", "uris");
		}
		Text = string.Format(CultureInfo.InvariantCulture, format, linkTexts);
		ContentLinks = GenerateContentLinks(format, linkTexts, uris);
	}

	private static List<ContentLink> GenerateContentLinks<T>(string format, string[] linkText, T[] urisOrActions)
	{
		List<ContentLink> list = new List<ContentLink>();
		int startIndex = 0;
		int num = 0;
		int num2 = 0;
		foreach (Match item2 in Regex.Matches(format, "\\{(?<index>\\d+)\\}"))
		{
			int num3 = format.IndexOf(item2.Value, startIndex, StringComparison.OrdinalIgnoreCase);
			startIndex = num3 + 1;
			num = num3 + num2;
			int num4 = int.Parse(item2.Groups["index"].Value, CultureInfo.InvariantCulture);
			ContentLink item = new ContentLink(num, linkText[num4].Length, (dynamic)urisOrActions[num4]);
			list.Add(item);
			num2 += linkText[num4].Length - item2.Value.Length;
		}
		return list;
	}
}
