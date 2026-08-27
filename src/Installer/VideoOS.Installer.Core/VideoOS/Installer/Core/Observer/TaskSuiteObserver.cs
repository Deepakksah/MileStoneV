namespace VideoOS.Installer.Core.Observer;

public class TaskSuiteObserver : Observer
{
	public event UpdatedEventHandler UpdatedEvent;

	public override void Update(Observable subject)
	{
		if (this.UpdatedEvent != null)
		{
			this.UpdatedEvent(subject);
		}
	}
}
