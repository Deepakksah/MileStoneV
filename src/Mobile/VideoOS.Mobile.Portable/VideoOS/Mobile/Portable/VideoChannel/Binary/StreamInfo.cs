namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public class StreamInfo
{
	private delegate TOut GetValue<TOut, TIn>(TIn source);

	private delegate void SetMask(BinaryFrameHeaderHelper.StreamInfoFieldsFlags flags);

	public uint? TimeBetweenFrames { get; set; }

	public RawDataType? DataType { get; set; }

	public uint? Flags { get; set; }

	public uint? Profile { get; set; }

	public uint? Level { get; set; }

	public ulong? Constrains { get; set; }

	public uint? Compatibility { get; set; }

	public uint? FramesCount { get; set; }

	private void InitDefaultvalues()
	{
		TimeBetweenFrames = 0u;
		DataType = RawDataType.Unsupported;
		Flags = 0u;
		Profile = 0u;
		Level = 0u;
		Constrains = 0uL;
		Compatibility = 0u;
		FramesCount = 0u;
	}

	public void InitFromStreamInfoHeader(HeaderExtensionStreamInfo streamInfo)
	{
		InitDefaultvalues();
		if (HeaderExtensionStreamInfoExtensions.CheckForMask(ref streamInfo, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.TimeBetweenFrames))
		{
			TimeBetweenFrames = streamInfo.TimeBetweenFrames;
		}
		if (HeaderExtensionStreamInfoExtensions.CheckForMask(ref streamInfo, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.SourceEncodingType))
		{
			DataType = (RawDataType)streamInfo.DataType;
		}
		if (HeaderExtensionStreamInfoExtensions.CheckForMask(ref streamInfo, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Flags))
		{
			Flags = streamInfo.Flags;
		}
		if (HeaderExtensionStreamInfoExtensions.CheckForMask(ref streamInfo, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Profile))
		{
			Profile = streamInfo.Profile;
		}
		if (HeaderExtensionStreamInfoExtensions.CheckForMask(ref streamInfo, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Level))
		{
			Level = streamInfo.Level;
		}
		if (HeaderExtensionStreamInfoExtensions.CheckForMask(ref streamInfo, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Constraints))
		{
			Constrains = streamInfo.Constrains;
		}
		if (HeaderExtensionStreamInfoExtensions.CheckForMask(ref streamInfo, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Compatibility))
		{
			Compatibility = streamInfo.Compatibility;
		}
		if (HeaderExtensionStreamInfoExtensions.CheckForMask(ref streamInfo, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.FramesCount))
		{
			FramesCount = streamInfo.FramesCount;
		}
	}

	public HeaderExtensionStreamInfo ConvertTo()
	{
		HeaderExtensionStreamInfo result = new HeaderExtensionStreamInfo
		{
			HeaderSize = 52u,
			Version = 3u,
			Reserved = 0u
		};
		SetMask setMask = delegate(BinaryFrameHeaderHelper.StreamInfoFieldsFlags flags)
		{
			HeaderExtensionStreamInfoExtensions.SetMask(ref result, flags);
		};
		CheckSet(ref result.TimeBetweenFrames, TimeBetweenFrames, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.TimeBetweenFrames, (uint value) => value, setMask);
		CheckSet(ref result.DataType, DataType, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.SourceEncodingType, (RawDataType value) => (uint)value, setMask);
		CheckSet(ref result.Flags, Flags, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Flags, (uint value) => value, setMask);
		CheckSet(ref result.Profile, Profile, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Profile, (uint value) => value, setMask);
		CheckSet(ref result.Level, Level, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Level, (uint value) => value, setMask);
		CheckSet(ref result.Constrains, Constrains, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Constraints, (ulong value) => value, setMask);
		CheckSet(ref result.Compatibility, Compatibility, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.Compatibility, (uint value) => value, setMask);
		CheckSet(ref result.FramesCount, FramesCount, BinaryFrameHeaderHelper.StreamInfoFieldsFlags.FramesCount, (uint value) => value, setMask);
		return result;
	}

	private void CheckSet<TF, TP>(ref TF fieldToSet, TP? property, BinaryFrameHeaderHelper.StreamInfoFieldsFlags flag, GetValue<TF, TP> getValue, SetMask setMask) where TP : struct
	{
		if (property.HasValue)
		{
			fieldToSet = getValue(property.Value);
			setMask(flag);
		}
	}
}
