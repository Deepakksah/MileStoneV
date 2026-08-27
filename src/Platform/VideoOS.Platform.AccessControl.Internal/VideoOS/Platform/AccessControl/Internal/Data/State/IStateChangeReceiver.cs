namespace VideoOS.Platform.AccessControl.Internal.Data.State;

public interface IStateChangeReceiver
{
	void StateChanged(AccessControlStateChange accessControlStateChange);
}
