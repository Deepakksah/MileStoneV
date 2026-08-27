using System;
using System.Linq;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public static class BoxParseExtensions
{
	public static void Traverse(this IBoxParse box, Action<IBoxParse> onBox)
	{
		onBox(box);
		foreach (IBoxParse child in box.Children)
		{
			child.Traverse(onBox);
		}
	}

	public static void RemoveChildren(this IBoxParse box, Func<IBoxParse, bool> match)
	{
		box.Children.Select((IBoxParse bp) => bp).Where(match).ToArray()
			.Foreach(delegate(IBoxParse rem)
			{
				box.Children.Remove(rem);
			});
	}
}
