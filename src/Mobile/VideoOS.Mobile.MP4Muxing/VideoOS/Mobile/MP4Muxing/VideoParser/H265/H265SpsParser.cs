using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public static class H265SpsParser
{
	public static H265Sps Parse(byte[] data)
	{
		H265Sps h265Sps = new H265Sps();
		h265Sps.Data = data;
		try
		{
			H265RbspParser h265RbspParser = new H265RbspParser(data, 0, EmulationPrevention.Remove);
			h265RbspParser.ReadNalUnit(h265Sps);
			h265Sps.VideoParamSetId = (byte)h265RbspParser.ReadBits(4);
			h265Sps.MaxNumberSubLayers = (byte)h265RbspParser.ReadBits(3);
			h265Sps.TempIdNestingFlag = (byte)h265RbspParser.ReadBits(1);
			h265Sps.ProfileTierLevel = ParseProfileTierLevel(h265RbspParser, h265Sps.MaxNumberSubLayers);
			h265Sps.SpsId = (byte)h265RbspParser.ReadUe();
			h265Sps.ChromaFormatIdc = (byte)h265RbspParser.ReadUe();
			if (3 == h265Sps.ChromaFormatIdc)
			{
				h265RbspParser.ReadBit();
			}
			h265Sps.Width = (ushort)h265RbspParser.ReadUe();
			h265Sps.Height = (ushort)h265RbspParser.ReadUe();
			if (h265RbspParser.ReadBit() != 0)
			{
				h265RbspParser.ReadUe();
				h265RbspParser.ReadUe();
				h265RbspParser.ReadUe();
				h265RbspParser.ReadUe();
			}
			h265Sps.BitDepthLuma = (byte)h265RbspParser.ReadUe();
			h265Sps.BitDepthChroma = (byte)h265RbspParser.ReadUe();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "H265SpsParser.Parse()", ex.ToString());
		}
		return h265Sps;
	}

	public static ProfileTierLevel ParseProfileTierLevel(H265RbspParser parser, byte maxNumberSubLayers)
	{
		ProfileTierLevel profileTierLevel = new ProfileTierLevel();
		profileTierLevel.ProfileTierLevelParams = ParseProfileTierLevelParams(parser);
		profileTierLevel.LevelIdc = (byte)parser.ReadBits(8);
		if (maxNumberSubLayers > 0)
		{
			byte[] array = new byte[maxNumberSubLayers];
			byte[] array2 = new byte[maxNumberSubLayers];
			for (int i = 0; i < maxNumberSubLayers; i++)
			{
				array[i] = (byte)parser.ReadBit();
				array2[i] = (byte)parser.ReadBit();
			}
			for (int j = maxNumberSubLayers; j < 8; j++)
			{
				parser.ReadBits(2);
			}
			for (int k = 0; k < maxNumberSubLayers; k++)
			{
				SublayerParams sublayerParams = new SublayerParams();
				if (array[k] != 0)
				{
					sublayerParams.ProfileTierLevelParams = ParseProfileTierLevelParams(parser);
				}
				if (array2[k] != 0)
				{
					sublayerParams.LevelIdc = (byte)parser.ReadBits(8);
				}
				profileTierLevel.SubLayersParams.Add(sublayerParams);
			}
		}
		return profileTierLevel;
	}

	public static ProfileTierLevelParams ParseProfileTierLevelParams(H265RbspParser parser)
	{
		ProfileTierLevelParams profileTierLevelParams = new ProfileTierLevelParams();
		profileTierLevelParams.ProfileSpace = (byte)parser.ReadBits(2);
		profileTierLevelParams.TierFlag = (byte)parser.ReadBits(1);
		profileTierLevelParams.ProfileIdc = (byte)parser.ReadBits(5);
		profileTierLevelParams.ProfileCompatibilityFlags = parser.ReadBits(32);
		byte b = (byte)(profileTierLevelParams.ConstraintsFlags = (byte)parser.ReadBits(8));
		byte b2 = (byte)parser.ReadBits(8);
		if (b2 != 0)
		{
			profileTierLevelParams.ConstraintsFlags = (uint)((b << 8) | b2);
		}
		parser.ReadBits(31);
		parser.ReadBit();
		return profileTierLevelParams;
	}
}
