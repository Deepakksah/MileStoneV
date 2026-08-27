using System;

namespace VideoOS.Mobile.Portable.Utilities.Transaction;

public class TransactionFailed : ITransactionFailed
{
	public AggregateException Exception { get; set; }

	public ITransactionStep Step { get; set; }
}
