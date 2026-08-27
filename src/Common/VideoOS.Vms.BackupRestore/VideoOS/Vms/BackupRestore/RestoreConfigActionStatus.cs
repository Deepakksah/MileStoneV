namespace VideoOS.Vms.BackupRestore;

public enum RestoreConfigActionStatus
{
	Undefined,
	PreparingForRestore,
	Validating,
	Restoring,
	WaitingForZeroActiveActions,
	CleaningUp,
	StartingServices,
	StoppingServices,
	ManagementServerCheck
}
