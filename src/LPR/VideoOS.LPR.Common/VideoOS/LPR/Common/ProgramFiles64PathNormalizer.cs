namespace VideoOS.LPR.Common;

public class ProgramFiles64PathNormalizer : IProgramFiles64PathNormalizer
{
	public string Normalize(string path)
	{
		return path.Replace(" (x86)", "");
	}
}
