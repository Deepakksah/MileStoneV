using System;

namespace VideoOS.Installer.Interfaces.Content;

public class ContentLink
{
	public int StartIndex { get; }

	public int Length { get; }

	public Uri Uri { get; }

	public Action ClickAction { get; }

	public ContentLink(int startIndex, int length, Uri uri)
		: this(startIndex, length, uri, null)
	{
	}

	public ContentLink(int startIndex, int length, Action clickAction)
		: this(startIndex, length, null, clickAction)
	{
	}

	private ContentLink(int startIndex, int length, Uri uri, Action clickAction)
	{
		StartIndex = startIndex;
		Length = length;
		Uri = uri;
		ClickAction = clickAction;
	}
}
