using System;

namespace VideoOS.Mobile.Portable.Utilities.Transaction;

public interface ITransactionFailed
{
	ITransactionStep Step { get; }

	AggregateException Exception { get; }
}
