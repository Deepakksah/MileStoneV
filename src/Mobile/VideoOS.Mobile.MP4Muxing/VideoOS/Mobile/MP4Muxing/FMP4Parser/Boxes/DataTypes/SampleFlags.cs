using System;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.DataTypes;

public class SampleFlags
{
	public enum FieldsSizes
	{
		Reserved = 4,
		IsLeading = 2,
		SampleDependsOn = 2,
		SampleIsDependedOn = 2,
		SampleHasRedundancy = 2,
		SamplePadding = 3,
		IsNotSyncSample = 1,
		SampleDegradationPriority = 16
	}

	public enum FieldsOffsets
	{
		WholeInt = 32,
		Reserved = 28,
		IsLeading = 26,
		SampleDependsOn = 24,
		SampleIsDependedOn = 22,
		SampleHasRedundancy = 20,
		SamplePadding = 17,
		IsNotSyncSample = 16,
		SampleDegradationPriority = 0
	}

	public enum FieldsBaseMasks
	{
		Reserved = 15,
		IsLeading = 3,
		SampleDependsOn = 3,
		SampleIsDependedOn = 3,
		SampleHasRedundancy = 3,
		SamplePadding = 7,
		IsNotSyncSample = 1,
		SampleDegradationPriority = 255
	}

	public enum FieldsMasks
	{
		Reserved = -268435456,
		IsLeading = 201326592,
		SampleDependsOn = 50331648,
		SampleIsDependedOn = 12582912,
		SampleHasRedundancy = 3145728,
		SamplePadding = 917504,
		IsNotSyncSample = 65536,
		SampleDegradationPriority = 255
	}

	public enum IsLeading
	{
		Unknown,
		LeadingWithDependency,
		NotLeading,
		Leading
	}

	public enum SampleDependsOn
	{
		Unknown,
		DependOnOthers,
		Independent,
		Reserved
	}

	public enum SampleIsDependedOn
	{
		Unknown,
		OthersDependOnThis,
		NoOtherDependsOnThis,
		Reserved
	}

	public enum SampleHasRedundancy
	{
		Unknown,
		RedundantCoding,
		NoRedundantCoding,
		Reserved
	}

	public uint Value;

	public IsLeading IsLeadingFlag
	{
		get
		{
			return (IsLeading)GetValue(FieldsOffsets.IsLeading, FieldsBaseMasks.IsLeading);
		}
		set
		{
			SetValue((uint)value, FieldsOffsets.IsLeading, FieldsMasks.IsLeading);
		}
	}

	public SampleDependsOn SampleDependsOnFlag
	{
		get
		{
			return (SampleDependsOn)GetValue(FieldsOffsets.SampleDependsOn, FieldsBaseMasks.IsLeading);
		}
		set
		{
			SetValue((uint)value, FieldsOffsets.SampleDependsOn, FieldsMasks.SampleDependsOn);
		}
	}

	public SampleIsDependedOn SampleIsDependedOnFlag
	{
		get
		{
			return (SampleIsDependedOn)GetValue(FieldsOffsets.SampleIsDependedOn, FieldsBaseMasks.IsLeading);
		}
		set
		{
			SetValue((uint)value, FieldsOffsets.SampleIsDependedOn, FieldsMasks.SampleIsDependedOn);
		}
	}

	public SampleHasRedundancy SampleHasRedundancyFlag
	{
		get
		{
			return (SampleHasRedundancy)GetValue(FieldsOffsets.SampleHasRedundancy, FieldsBaseMasks.IsLeading);
		}
		set
		{
			SetValue((uint)value, FieldsOffsets.SampleHasRedundancy, FieldsMasks.SampleHasRedundancy);
		}
	}

	public bool IsNotSyncSample
	{
		get
		{
			return Convert.ToBoolean(GetValue(FieldsOffsets.IsNotSyncSample, FieldsBaseMasks.IsNotSyncSample));
		}
		set
		{
			SetValue(Convert.ToUInt32(value), FieldsOffsets.IsNotSyncSample, FieldsMasks.IsNotSyncSample);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private uint GetValue(FieldsOffsets offset, FieldsBaseMasks baseMask)
	{
		return (Value >> (int)offset) & (uint)baseMask;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetValue(uint value, FieldsOffsets offset, FieldsMasks mask)
	{
		Value &= (uint)(~mask);
		Value |= value << (int)offset;
	}

	public static SampleFlags FromNullableUint(uint? value)
	{
		if (!value.HasValue)
		{
			return null;
		}
		return new SampleFlags
		{
			Value = value.Value
		};
	}
}
