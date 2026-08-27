using System;
using System.Linq;

namespace VideoOS.EventServer.Communication.Rest.Utils;

public static class MipItemSourceHelper
{
	public static bool TryParseMipItem(string restResourcePath, out MipItemSource mipItemSource)
	{
		mipItemSource = null;
		string[] array = restResourcePath.Split('/');
		if (array.Length < 4)
		{
			return false;
		}
		if (!array[2].Equals("mipItems", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		string input = array[1];
		string input2 = array.Last();
		if (!Guid.TryParse(input, out var result))
		{
			return false;
		}
		if (!Guid.TryParse(input2, out var result2))
		{
			return false;
		}
		ParentType parentType = ((!array.First().Equals("mipKinds", StringComparison.OrdinalIgnoreCase)) ? ParentType.MipItem : ParentType.MipKind);
		mipItemSource = new MipItemSource
		{
			ParentId = result,
			MipItemId = result2,
			ParentType = parentType
		};
		return true;
	}
}
