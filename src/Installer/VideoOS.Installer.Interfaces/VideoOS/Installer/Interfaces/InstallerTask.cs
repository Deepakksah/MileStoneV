using System;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces.Msi;

namespace VideoOS.Installer.Interfaces;

public class InstallerTask : Task
{
	public string Name { get; set; }

	public int CurrentProgress { get; set; }

	public int MaximumProgress { get; set; }

	public string Message { get; set; }

	public string Action { get; set; }

	public bool NeedsReboot { get; set; }

	public string DisplayName { get; set; }

	public new string Id { get; set; }

	public string TargetDir { get; set; }

	public bool Canceled { get; set; }

	public bool RunOnlyPlugin { get; set; }

	public IClMsiHandler ClMsiHandler { get; set; }

	public virtual bool RunInHardwareManufacturerMode => false;

	public virtual bool RunInFinalizeMode => true;

	protected override void DoRun()
	{
		throw new NotImplementedException();
	}

	protected override void PostRun()
	{
	}

	protected override void PreRun()
	{
	}

	public override TaskResult Stop()
	{
		return base.Stop();
	}

	public virtual void CancelInstalation()
	{
		Canceled = true;
	}
}
