namespace VideoOS.Mobile.Portable.Utilities;

public interface ISanitizerConfiguration
{
	string InvalidCharacters { get; set; }

	char ReplacingCharacter { get; set; }

	uint MaxAllowedLength { get; set; }
}
