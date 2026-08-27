namespace VideoOS.LPR.Common.Platestyles;

public interface IPlateStyleAliasProvider
{
	string GetAliasOrStyle(string plateStyleCode);

	int GetAliasIdentifier(string plateStyleCode);
}
