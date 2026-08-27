namespace VideoOS.Mobile.MP4Muxing.VideoParser;

public class NaluInfo
{
	public byte NaluType;

	public int Start;

	public int End;

	public int Length;

	public int StartCodeLength;

	public int StrippedCodeStart;

	public int StrippedCodeLength;

	public int StrippedNalStart;

	public int StrippedNalLength;

	private void Initialize(byte naluType, int start = 0, int end = 0, int startCodeLen = 0)
	{
		NaluType = naluType;
		Start = start;
		End = end;
		Length = end - start;
		StartCodeLength = startCodeLen;
		StrippedCodeStart = Start + startCodeLen;
		StrippedCodeLength = Length - startCodeLen;
		StrippedNalStart = StrippedCodeStart + 1;
		StrippedNalLength = StrippedCodeLength - 1;
	}

	public NaluInfo(byte nalType, int start, int end, int startCodeLen)
	{
		Initialize(nalType, start, end, startCodeLen);
	}
}
