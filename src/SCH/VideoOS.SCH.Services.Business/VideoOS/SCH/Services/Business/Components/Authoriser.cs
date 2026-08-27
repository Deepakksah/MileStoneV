namespace VideoOS.SCH.Services.Business.Components;

public class Authoriser
{
	public static bool IsAuthorised(bool isAnonymous, string userName, string featureID)
	{
		if (featureID == "DiagnosisPing")
		{
			return true;
		}
		if (isAnonymous)
		{
			return false;
		}
		return true;
	}
}
