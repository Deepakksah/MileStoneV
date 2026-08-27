namespace VideoOS.Common.Integration;

public interface IProtectionProvider
{
	string Encode(string clearText);

	string Decode(string encodedText);
}
