using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public abstract class ObjectBinder<T> : IEnumerable<T>, IEnumerable, IDisposable
{
	private class ObjectEnumerator : IEnumerator<T>, IDisposable, IEnumerator
	{
		private ObjectBinder<T> _binder;

		private IDataReader _reader;

		private T _current;

		private bool _endOfReader;

		private ResultCollection _resultCollection;

		private List<T> _items;

		public T Current => _current;

		object IEnumerator.Current => _current;

		public IDataReader Reader => _reader;

		public List<T> Items
		{
			get
			{
				if (_items == null)
				{
					_items = new List<T>();
					while (MoveNext())
					{
						_items.Add(Current);
						if (_resultCollection != null)
						{
							_resultCollection.IncrementRowCounter();
						}
					}
				}
				return _items;
			}
		}

		public ObjectEnumerator(ObjectBinder<T> binder, IDataReader reader, ResultCollection resultCollection)
		{
			_binder = binder;
			_reader = reader;
			_resultCollection = resultCollection;
		}

		public bool MoveNext()
		{
			if (!_endOfReader)
			{
				try
				{
					_endOfReader = !_reader.Read();
				}
				catch (DatabaseException ex)
				{
					if (_resultCollection != null && !_resultCollection.MapException(ex))
					{
						throw;
					}
				}
				if (!_endOfReader)
				{
					_current = _binder.Bind();
				}
			}
			else
			{
				_current = default(T);
			}
			return !_endOfReader;
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
		}
	}

	private ObjectEnumerator _enumerator;

	private string _procedureName;

	protected DatabaseDataReader Reader => _enumerator.Reader as DatabaseDataReader;

	public T Current => _enumerator.Current;

	public List<T> Items => _enumerator.Items;

	protected string StoredProcedureName => _procedureName;

	protected ObjectBinder()
	{
	}

	protected ObjectBinder(DatabaseDataReader reader, string procedureName)
		: this()
	{
		_enumerator = new ObjectEnumerator(this, reader, null);
		_procedureName = procedureName;
	}

	protected abstract T Bind();

	public void Initialize(IDataReader reader, string procedureName, ResultCollection resultCollection)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (resultCollection == null)
		{
			throw new ArgumentNullException("resultCollection");
		}
		_procedureName = procedureName;
		_enumerator = new ObjectEnumerator(this, reader, resultCollection);
	}

	public bool MoveNext()
	{
		return _enumerator.MoveNext();
	}

	public IEnumerator<T> GetEnumerator()
	{
		return _enumerator;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _enumerator;
	}

	public void Dispose()
	{
	}
}
