using System.Collections.Generic;
using System.IO;
using System.Linq;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.DataTypes;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class TrackFragmentRunBox : FullBox
{
	public const uint DataOffsetPresentFlag = 1u;

	public const uint FirstSampleFlagsPresentFlag = 4u;

	public const uint SampleDurationPresentFlag = 256u;

	public const uint SampleSizePresentFlag = 512u;

	public const uint SampleFlagsPresentFlag = 1024u;

	public const uint SmplComposTimeOffstsPrsntFlag = 2048u;

	public uint? DataOffset;

	public SampleFlags FirstSampleFlags;

	private readonly List<TrackFragmentRunBoxSample> _samples = new List<TrackFragmentRunBoxSample>();

	public override bool CanHaveData => true;

	public override uint DataSize => GetDataSize();

	public uint SampleCount => (uint)_samples.Count;

	public IList<TrackFragmentRunBoxSample> Samples => _samples;

	public TrackFragmentRunBox()
	{
		base.BoxType = "trun";
	}

	public void FinalizeChanges()
	{
		SetResetFlag(DataOffset, 1u);
		SetResetFlag(FirstSampleFlags != null, 4u);
		SetResetFlag(_samples.Any((TrackFragmentRunBoxSample sample) => sample.SampleDuration.HasValue), 256u);
		SetResetFlag(_samples.Any((TrackFragmentRunBoxSample sample) => sample.SampleSize.HasValue), 512u);
		SetResetFlag(_samples.Any((TrackFragmentRunBoxSample sample) => sample.SampleFlags != null), 1024u);
		SetResetFlag(_samples.Any((TrackFragmentRunBoxSample sample) => sample.ComposTimeOffset.HasValue), 2048u);
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		uint parameter = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
		ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref DataOffset, 1u);
		ParseSampleFlags(buffer, startIndex, restInBoxSize, ref readBytes, out FirstSampleFlags, 4u);
		_samples.Clear();
		_samples.Capacity = (int)parameter;
		for (int i = 0; i < parameter; i++)
		{
			TrackFragmentRunBoxSample trackFragmentRunBoxSample = new TrackFragmentRunBoxSample();
			ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref trackFragmentRunBoxSample.SampleDuration, 256u);
			ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref trackFragmentRunBoxSample.SampleSize, 512u);
			ParseSampleFlags(buffer, startIndex, restInBoxSize, ref readBytes, out trackFragmentRunBoxSample.SampleFlags, 1024u);
			ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref trackFragmentRunBoxSample.ComposTimeOffset, 2048u);
			_samples.Insert(i, trackFragmentRunBoxSample);
		}
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt32(SampleCount);
		WriteInt32(stream, DataOffset, 1u);
		WriteInt32(stream, FirstSampleFlags?.Value, 4u);
		foreach (TrackFragmentRunBoxSample sample in _samples)
		{
			WriteInt32(stream, sample.SampleDuration, 256u);
			WriteInt32(stream, sample.SampleSize, 512u);
			WriteInt32(stream, sample.SampleFlags?.Value, 1024u);
			WriteInt32(stream, sample.ComposTimeOffset, 2048u);
		}
	}

	private uint GetDataSize()
	{
		uint num = 4u;
		if (IsSetFlag(1u))
		{
			num += 4;
		}
		if (IsSetFlag(4u))
		{
			num += 4;
		}
		uint num2 = 0u;
		if (IsSetFlag(256u))
		{
			num2 += 4;
		}
		if (IsSetFlag(512u))
		{
			num2 += 4;
		}
		if (IsSetFlag(1024u))
		{
			num2 += 4;
		}
		if (IsSetFlag(2048u))
		{
			num2 += 4;
		}
		uint num3 = SampleCount * num2;
		return num + num3;
	}

	private void ParseSampleFlags(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, out SampleFlags sampleFlags, uint mask)
	{
		uint? parameter = null;
		ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter, mask);
		sampleFlags = SampleFlags.FromNullableUint(parameter);
	}
}
