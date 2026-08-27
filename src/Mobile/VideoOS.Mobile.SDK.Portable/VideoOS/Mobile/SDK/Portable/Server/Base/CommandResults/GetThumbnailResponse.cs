using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetThumbnailResponse : BaseCommandResponse
{
	public byte[] Thumbnail { get; private set; }

	public GetThumbnailResponse(Command command)
		: base(command)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (command.Thumbnail != null)
		{
			Thumbnail = command.Thumbnail.ToByteArray();
		}
		else if (dictionary.ContainsKey("Thumbnail"))
		{
			Thumbnail = VideoOS.Mobile.Portable.MetaChannel.CommEntries.Thumbnail.ToByteArray(dictionary["Thumbnail"]);
		}
	}

	public GetThumbnailResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
