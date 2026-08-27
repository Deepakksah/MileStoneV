namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public enum H265NaluType
{
	SliceTrailN = 0,
	SliceTrailR = 1,
	SliceTsaN = 2,
	SliceTsaR = 3,
	SliceStsaN = 4,
	SliceStsaR = 5,
	SliceRadlN = 6,
	SliceRadlR = 7,
	SliceDlp = 7,
	SliceRaslN = 8,
	SliceRaslR = 9,
	SliceTfd = 9,
	SliceBla = 16,
	SliceBlaNt = 17,
	SliceBlaNlp = 18,
	SliceIdr = 19,
	SliceIdrNlp = 20,
	SliceCra = 21,
	Vps = 32,
	Sps = 33,
	Pps = 34,
	AccessUd = 35,
	Eos = 36,
	Eob = 37,
	FilterData = 38,
	Sei = 39,
	SeiSuffix = 40
}
