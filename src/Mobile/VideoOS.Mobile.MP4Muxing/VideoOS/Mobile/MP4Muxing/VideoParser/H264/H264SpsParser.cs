using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public static class H264SpsParser
{
	public static H264SpsRbsp Parse(byte[] pStart)
	{
		H264SpsRbsp h264SpsRbsp = new H264SpsRbsp();
		h264SpsRbsp.Data = pStart;
		try
		{
			H264RbspParser h264RbspParser = new H264RbspParser(pStart, 0, EmulationPrevention.Remove);
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			h264RbspParser.ReadNalUnit(h264SpsRbsp);
			h264SpsRbsp.ProfileIdc = (byte)h264RbspParser.ReadBits(8);
			h264SpsRbsp.Constrains = (byte)h264RbspParser.ReadBits(8);
			h264SpsRbsp.LevelIdc = (byte)h264RbspParser.ReadBits(8);
			h264RbspParser.ReadUe();
			if (h264SpsRbsp.ProfileIdc == 100 || h264SpsRbsp.ProfileIdc == 110 || h264SpsRbsp.ProfileIdc == 122 || h264SpsRbsp.ProfileIdc == 244 || h264SpsRbsp.ProfileIdc == 44 || h264SpsRbsp.ProfileIdc == 83 || h264SpsRbsp.ProfileIdc == 86 || h264SpsRbsp.ProfileIdc == 118)
			{
				if (h264RbspParser.ReadUe() == 3)
				{
					h264RbspParser.ReadBit();
				}
				h264RbspParser.ReadUe();
				h264RbspParser.ReadUe();
				h264RbspParser.ReadBit();
				if (h264RbspParser.ReadBit() != 0)
				{
					int num5 = 0;
					for (num5 = 0; num5 < 8; num5++)
					{
						if (h264RbspParser.ReadBit() == 0)
						{
							continue;
						}
						int num6 = ((num5 < 6) ? 16 : 64);
						int num7 = 8;
						int num8 = 8;
						int num9 = 0;
						for (num9 = 0; num9 < num6; num9++)
						{
							if (num8 != 0)
							{
								int num10 = h264RbspParser.ReadSe();
								num8 = (num7 + num10 + 256) % 256;
							}
							num7 = ((num8 == 0) ? num7 : num8);
						}
					}
				}
			}
			h264RbspParser.ReadUe();
			switch ((int)h264RbspParser.ReadUe())
			{
			case 0:
				h264RbspParser.ReadUe();
				break;
			case 1:
			{
				h264RbspParser.ReadBit();
				h264RbspParser.ReadSe();
				h264RbspParser.ReadSe();
				uint num11 = h264RbspParser.ReadUe();
				for (int i = 0; i < num11; i++)
				{
					h264RbspParser.ReadSe();
				}
				break;
			}
			}
			h264RbspParser.ReadUe();
			h264RbspParser.ReadBit();
			uint num12 = h264RbspParser.ReadUe();
			uint num13 = h264RbspParser.ReadUe();
			uint num14 = h264RbspParser.ReadBit();
			if (num14 == 0)
			{
				h264RbspParser.ReadBit();
			}
			h264RbspParser.ReadBit();
			if (h264RbspParser.ReadBit() != 0)
			{
				num = h264RbspParser.ReadUe();
				num2 = h264RbspParser.ReadUe();
				num3 = h264RbspParser.ReadUe();
				num4 = h264RbspParser.ReadUe();
			}
			h264RbspParser.ReadBit();
			h264SpsRbsp.Width = (ushort)((num12 + 1) * 16 - num2 * 2 - num * 2);
			h264SpsRbsp.Height = (ushort)((2 - num14) * (num13 + 1) * 16 - num4 * 2 - num3 * 2);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "H264SpsParser.Parse()", ex.ToString());
		}
		return h264SpsRbsp;
	}
}
