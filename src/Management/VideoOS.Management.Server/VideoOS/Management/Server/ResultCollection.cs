using System;
using System.Collections;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class ResultCollection : IDisposable
{
	private ArrayList _binders;

	private IDataReader _reader;

	private IGeminiRequestContext _requestContext;

	private int _currentBinderIndex;

	private bool _endOfResult;

	private string _storedProcedureName;

	private int _rowsReadCounter;

	private int _maxItemsLimit;

	private bool _isDisposed;

	public IGeminiRequestContext RequestContext => _requestContext;

	public event DatabaseExceptionHandler OnDatabaseException;

	public ResultCollection(IDataReader reader, string storedProcedureName, IGeminiRequestContext requestContext)
		: this(reader, storedProcedureName, 0, null, requestContext)
	{
	}

	public ResultCollection(IDataReader reader, string storedProcedureName, int maximumRows, DatabaseExceptionHandler exceptionHandler, IGeminiRequestContext requestContext)
	{
		_binders = new ArrayList();
		if (maximumRows <= 0)
		{
			maximumRows = int.MaxValue;
		}
		_maxItemsLimit = maximumRows;
		_reader = reader;
		_storedProcedureName = storedProcedureName;
		_requestContext = requestContext;
		OnDatabaseException += exceptionHandler;
	}

	public void AddBinder<T>(ObjectBinder<T> binder)
	{
		binder.Initialize(_reader, _storedProcedureName, this);
		_binders.Add(binder);
	}

	public object GetCurrent()
	{
		return _binders[_currentBinderIndex];
	}

	public ObjectBinder<T> GetCurrent<T>()
	{
		return (ObjectBinder<T>)_binders[_currentBinderIndex];
	}

	public void NextResult()
	{
		if (!TryNextResult())
		{
			throw new UnexpectedDatabaseResultException(_storedProcedureName);
		}
	}

	public bool TryNextResult()
	{
		if (!_endOfResult)
		{
			try
			{
				_endOfResult = !_reader.NextResult();
			}
			catch (DatabaseException ex)
			{
				MapException(ex);
			}
			if (!_endOfResult)
			{
				_currentBinderIndex++;
			}
		}
		return !_endOfResult;
	}

	public bool MapException(DatabaseException ex)
	{
		if (this.OnDatabaseException != null)
		{
			this.OnDatabaseException(ex, QueryExecutionState.ReturningResults);
			return true;
		}
		return false;
	}

	public void IncrementRowCounter()
	{
		_rowsReadCounter++;
		if (_rowsReadCounter >= _maxItemsLimit)
		{
			throw new TooManyItemsException(_maxItemsLimit);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing && _reader != null)
		{
			if (!_reader.IsClosed)
			{
				while (TryNextResult())
				{
				}
			}
			_reader.Dispose();
		}
		_isDisposed = true;
	}

	~ResultCollection()
	{
		Dispose(disposing: false);
	}
}
