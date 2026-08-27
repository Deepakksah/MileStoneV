using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class BoxJoinHelper
{
	public static void Do<T>(RootBox rootBox, string boxType, Action<T, T> onBoxFirstAndNext, Action<T> finalizeFirst = null) where T : Box
	{
		List<IBoxParse> list = (from box in rootBox.Children
			select (box) into box
			where box.BoxType == boxType
			select box).ToList();
		T val = list.First() as T;
		list.Remove(val);
		foreach (IBoxParse item in list)
		{
			rootBox.Children.Remove(item);
			item.Parent = null;
			onBoxFirstAndNext(val, item as T);
		}
		finalizeFirst?.Invoke(val);
	}

	public static void JoinMoofs(RootBox rootBox)
	{
		Do(rootBox, "moof", delegate(MovieFragmentBox firstMoof, MovieFragmentBox nextMoof)
		{
			TrackFragmentRunBox trun = GetTrun(firstMoof);
			foreach (TrackFragmentRunBoxSample sample in GetTrun(nextMoof).Samples)
			{
				trun.Samples.Add(sample);
			}
		}, delegate(MovieFragmentBox firstMoof)
		{
			GetTrun(firstMoof).FinalizeChanges();
		});
	}

	private static TrackFragmentRunBox GetTrun(MovieFragmentBox moofBox)
	{
		return moofBox.Children.First((IBoxParse box) => box.BoxType == "traf").Children.First((IBoxParse box) => box.BoxType == "trun") as TrackFragmentRunBox;
	}

	public static void JoinMdats(RootBox rootBox)
	{
		Do(rootBox, "mdat", delegate(MediaDataBox firstMdat, MediaDataBox nextMdat)
		{
			foreach (GenericMultiDataBoxData data in nextMdat.Datas)
			{
				firstMdat.Datas.Add(data);
			}
		});
	}
}
