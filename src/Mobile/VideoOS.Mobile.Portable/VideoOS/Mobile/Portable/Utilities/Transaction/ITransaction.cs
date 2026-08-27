using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Utilities.Transaction;

public interface ITransaction
{
	ITransactionFailed FailedExecution { get; }

	ITransactionFailed FailedRollback { get; }

	ICollection<ITransactionStep> Steps { get; }

	TransactionResult Execute();
}
