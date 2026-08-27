namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public interface IStateChangeInternal
{
	ConnectionStates State { get; }

	void ChangeState(ConnectionStates state);
}
