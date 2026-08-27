namespace VideoOS.Mobile.Helpers;

public interface ISanitizerConfiguration
{
	string InvalidCharacters { get; set; }

	char ReplacingCharacter { get; set; }

	uint MaxAllowedLength { get; set; }
}
