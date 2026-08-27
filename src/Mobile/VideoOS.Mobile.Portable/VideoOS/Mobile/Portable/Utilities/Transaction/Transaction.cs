using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities.Transaction;

public class Transaction : ITransaction
{
	private readonly TransactionFailed _failedExecution = new TransactionFailed();

	private readonly TransactionFailed _failedRollback = new TransactionFailed();

	public List<ITransactionStep> Steps { get; } = new List<ITransactionStep>();

	public ITransactionFailed FailedExecution => _failedExecution;

	public ITransactionFailed FailedRollback => _failedRollback;

	ICollection<ITransactionStep> ITransaction.Steps => Steps;

	public TransactionResult Execute()
	{
		List<TransactionResult> list = new List<TransactionResult>();
		List<Exception> list2 = new List<Exception>();
		for (int i = 0; i < Steps.Count; i++)
		{
			ITransactionStep transactionStep = Steps[i];
			try
			{
				list.Add(transactionStep.Action());
			}
			catch (Exception ex)
			{
				list.Add((!transactionStep.StopActionOnFail) ? TransactionResult.OkWithWarning : TransactionResult.Failed);
				Logger.Instance.Log(LogContent.Error, "Transaction.Execute()", ex.ToString());
				list2.Add(ex);
			}
			if (TransactionResult.Failed == list.Last())
			{
				_failedExecution.Step = transactionStep;
				Rollback(i - 1);
				break;
			}
		}
		if (list2.Any())
		{
			_failedExecution.Exception = new AggregateException(list2);
		}
		return list.Max();
	}

	private void Rollback(int startIndex)
	{
		for (int num = startIndex; num >= 0; num--)
		{
			ITransactionStep transactionStep = Steps[num];
			TransactionResult transactionResult;
			try
			{
				transactionResult = transactionStep.Rollback();
			}
			catch (Exception ex)
			{
				transactionResult = ((!transactionStep.StopActionOnFail) ? TransactionResult.OkWithWarning : TransactionResult.Failed);
				Logger.Instance.Log(LogContent.Error, "Transaction.Rollback()", ex.Message);
				_failedRollback.Exception = new AggregateException(ex);
			}
			if (TransactionResult.Failed == transactionResult)
			{
				_failedRollback.Step = transactionStep;
				break;
			}
		}
	}
}
