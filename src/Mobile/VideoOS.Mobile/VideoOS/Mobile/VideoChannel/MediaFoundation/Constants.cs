using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public static class Constants
{
	public const uint MF_RESOLUTION_MEDIASOURCE = 1u;

	public static Guid MF_PD_DURATION = new Guid(1821969715u, 48014, 18298, 133, 152, 13, 93, 150, 252, 216, 138);

	public static Guid MF_BYTESTREAM_CONTENT_TYPE = new Guid(4231365257u, 15542, 17932, 164, 36, 182, 104, 18, 96, 55, 90);

	public const uint MESessionTopologySet = 101u;

	public const uint MESessionClosed = 106u;

	public const int E_NOTIMPL = -2147467263;

	public static Guid MF_MT_MAJOR_TYPE = new Guid(1223401870u, 63689, 18055, 191, 17, 10, 116, 201, 249, 106, 143);

	public static Guid MF_MT_SUBTYPE = new Guid(4158868634u, 17128, 18196, 183, 75, 203, 41, 215, 44, 53, 229);

	public static Guid MF_MT_AVG_BITRATE = new Guid(540223012u, 64269, 19870, 189, 13, 203, 246, 120, 108, 16, 46);

	public static Guid MF_MT_INTERLACE_MODE = new Guid(3799141304u, 58998, 18438, 180, 178, 168, 214, 239, 180, 76, 205);

	public static Guid MF_MT_FRAME_SIZE = new Guid(374522685u, 54962, 16402, 184, 52, 114, 3, 8, 73, 163, 125);

	public static Guid MF_MT_FRAME_RATE = new Guid(3294208744u, 15660, 20036, 177, 50, 254, 229, 21, 108, 123, 176);

	public static Guid MF_MT_PIXEL_ASPECT_RATIO = new Guid(3325520414u, 36106, 16423, 190, 69, 109, 154, 10, 211, 155, 182);

	public static Guid MF_MT_MPEG_SEQUENCE_HEADER = new Guid(1006857703, 15056, 19614, 146, 22, 238, 109, 106, 194, 28, 179);

	public static Guid MF_TRANSCODE_CONTAINERTYPE = new Guid(353366591, 19132, 18315, 172, 79, 225, 145, 111, 186, 28, 202);

	public static Guid MFTranscodeContainerType_MPEG4 = new Guid(3698118749u, 47568, 16623, 189, 53, 250, 98, 44, 26, 178, 138);

	public static Guid MFTranscodeContainerType_FMPEG4 = new Guid(2611508977u, 16799, 19319, 161, 224, 53, 149, 157, 157, 64, 4);

	public static Guid MF_READWRITE_ENABLE_HARDWARE_TRANSFORMS = new Guid(2788469020u, 33323, 16825, 164, 148, 77, 228, 100, 54, 18, 176);

	public static Guid MF_READWRITE_DISABLE_CONVERTERS = new Guid(2564141157u, 4980, 18503, 141, 93, 49, 82, 15, 238, 113, 86);

	public static Guid MF_LOW_LATENCY = new Guid(2619836698u, 60794, 16609, 136, 232, 178, 39, 39, 160, 36, 238);

	public static Guid MFMediaType_Video = new Guid(1935960438, 0, 16, 128, 0, 0, 170, 0, 56, 155, 113);

	public static Guid MFVideoFormat_H264 = (Guid)new FourCC("H264");

	public static Guid MFVideoFormat_M4S2 = (Guid)new FourCC("M4S2");

	public static Guid MFVideoFormat_RGB32 = new Guid(22, 0, 16, 128, 0, 0, 170, 0, 56, 155, 113);

	public static Guid MFVideoFormat_IYUV = (Guid)new FourCC("IYUV");

	public static Guid MFSampleExtension_CleanPoint = new Guid(2631860696u, 41200, 17338, 176, 119, 234, 160, 108, 189, 114, 138);
}
