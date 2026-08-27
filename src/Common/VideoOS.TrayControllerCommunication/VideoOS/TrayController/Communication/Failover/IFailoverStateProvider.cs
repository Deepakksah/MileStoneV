namespace VideoOS.TrayController.Communication.Failover;

public interface IFailoverStateProvider
{
	FailoverState GetState();
}
