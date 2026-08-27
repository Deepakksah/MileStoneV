namespace VideoOS.Mobile.Portable.Utilities.Transaction;

public interface ITransactionStep
{
	bool StopActionOnFail { get; }

	TransactionResult Action();

	TransactionResult Rollback();
}
