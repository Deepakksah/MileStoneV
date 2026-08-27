namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public enum MFVideoInterlaceMode : uint
{
	MFVideoInterlace_Unknown = 0u,
	MFVideoInterlace_Progressive = 2u,
	MFVideoInterlace_FieldInterleavedUpperFirst = 3u,
	MFVideoInterlace_FieldInterleavedLowerFirst = 4u,
	MFVideoInterlace_FieldSingleUpper = 5u,
	MFVideoInterlace_FieldSingleLower = 6u,
	MFVideoInterlace_MixedInterlaceOrProgressive = 7u,
	MFVideoInterlace_Last = 8u,
	MFVideoInterlace_ForceDWORD = 2147483647u
}
