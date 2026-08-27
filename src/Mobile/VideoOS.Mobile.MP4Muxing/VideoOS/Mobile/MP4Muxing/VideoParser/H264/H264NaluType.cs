namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public enum H264NaluType
{
	Unspecified,
	NonIdr,
	DataPortionA,
	DataPortionB,
	DataPortionC,
	Idr,
	Sei,
	Sps,
	Pps,
	AccessUd,
	EoSeq,
	EoStream,
	FilterData
}
