namespace VideoOS.Mobile.Helpers;

public interface IImpersonation
{
	void Impersonate();

	void Revert();
}
