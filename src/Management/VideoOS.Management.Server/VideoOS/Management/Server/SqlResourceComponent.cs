using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using Microsoft.Win32;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public abstract class SqlResourceComponent : VideoOS.Utilities.Database.SqlResourceComponent
{
	private IGeminiRequestContext _requestContext;

	private static Dictionary<int, ConstructorInfo> _exceptionConstructors;

	private DatabaseTransaction _sqlTransaction;

	private const string LoggingOptionsRegKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoOS\\Server";

	protected virtual IGeminiRequestContext RequestContext => _requestContext;

	protected GeminiDatabaseLoggingOptions LoggingOptions { get; set; }

	private bool IsInitialized => base.Connection != null;

	private static void RegisterExceptions(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			ConstructorInfo[] constructors = types[i].GetConstructors();
			foreach (ConstructorInfo constructorInfo in constructors)
			{
				object[] customAttributes = constructorInfo.GetCustomAttributes(typeof(SqlErrorAttribute), inherit: false);
				for (int k = 0; k < customAttributes.Length; k++)
				{
					SqlErrorAttribute sqlErrorAttribute = (SqlErrorAttribute)customAttributes[k];
					try
					{
						_exceptionConstructors.Add(sqlErrorAttribute.ErrorCode, constructorInfo);
					}
					catch
					{
						GeminiTrace.Error("VMO-Sql: RegisterExceptions duplicate id: " + sqlErrorAttribute.ErrorCode.ToString(CultureInfo.InvariantCulture));
						throw;
					}
				}
			}
		}
	}

	static SqlResourceComponent()
	{
		_exceptionConstructors = new Dictionary<int, ConstructorInfo>();
		RegisterExceptions(Assembly.GetCallingAssembly());
	}

	protected SqlResourceComponent()
	{
	}

	protected SqlResourceComponent(IGeminiRequestContext requestContext)
	{
		Initialize(requestContext);
	}

	protected SqlResourceComponent(ISqlConnectionManager sqlConnectinManager)
	{
		_requestContext = null;
		InitializeSettings(sqlConnectinManager, GeminiDatabaseLoggingOptions.None);
	}

	public void Initialize(IGeminiRequestContext requestContext)
	{
		GeminiValidation.CheckForNull(requestContext, "requestContext");
		if (IsInitialized)
		{
			throw new InvalidOperationException("Object already initialized");
		}
		_requestContext = requestContext;
		DatabaseSettings databaseSettings = _requestContext.GetDatabaseSettings() as DatabaseSettings;
		ISqlConnectionManager sqlConnectionManager = databaseSettings.GetSqlConnectionManager();
		InitializeSettings(sqlConnectionManager, databaseSettings.LoggingOptions);
	}

	private void InitializeSettings(ISqlConnectionManager sqlConnectionManager, GeminiDatabaseLoggingOptions loggingOptions)
	{
		base.Connection = sqlConnectionManager.GetSqlConnection();
		GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.ActiveSqlConnections).Increment();
		if (!Enum.TryParse<GeminiDatabaseLoggingOptions>((string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoOS\\Server", "LoggingOptions", "None"), out loggingOptions))
		{
			loggingOptions = GeminiDatabaseLoggingOptions.None;
		}
		if (loggingOptions == GeminiDatabaseLoggingOptions.Statistics || loggingOptions == GeminiDatabaseLoggingOptions.All)
		{
			PrepareSqlBatch(64);
			AddStatement("SET STATISTICS TIME ON\r\nSET STATISTICS IO ON");
			ExecuteNonQuery();
		}
		LoggingOptions = loggingOptions;
	}

	protected virtual void VerifyInitialized()
	{
		if (!IsInitialized)
		{
			throw new InvalidOperationException("Connection is not initialized");
		}
	}

	protected void BeginTransaction(IsolationLevel isolationLevel)
	{
		VerifyInitialized();
		_sqlTransaction = base.Connection.BeginTransaction(isolationLevel);
	}

	protected void CommitTransaction()
	{
		VerifyInitialized();
		CloseReader();
		if (_sqlTransaction != null)
		{
			_sqlTransaction.Commit();
			_sqlTransaction.Dispose();
			_sqlTransaction = null;
		}
	}

	protected void RollbackTransaction()
	{
		VerifyInitialized();
		CloseReader();
		if (_sqlTransaction == null)
		{
			return;
		}
		if (_sqlTransaction.Connection != null)
		{
			try
			{
				_sqlTransaction.Rollback();
			}
			catch (InvalidOperationException)
			{
			}
		}
		_sqlTransaction.Dispose();
		_sqlTransaction = null;
	}

	private void CloseReader()
	{
		if (base.DataReader == null)
		{
			return;
		}
		if (LoggingOptions != GeminiDatabaseLoggingOptions.None && !base.DataReader.IsClosed)
		{
			try
			{
				while (base.DataReader.NextResult())
				{
				}
			}
			catch (DatabaseException)
			{
			}
			catch (Exception)
			{
			}
		}
		base.DataReader.Dispose();
		base.DataReader = null;
	}

	protected override void MapException(DatabaseException ex)
	{
		VmoServerException ex2 = TranslateException(ex);
		if (ex2 != null)
		{
			throw ex2;
		}
		throw ex;
	}

	private static VmoServerException TranslateException(DatabaseException ex)
	{
		DatabaseError[] errors = ex.Errors;
		foreach (DatabaseError databaseError in errors)
		{
			if (_exceptionConstructors.TryGetValue(databaseError.Number, out var value))
			{
				return (VmoServerException)value.Invoke(new object[2] { ex, databaseError });
			}
		}
		return null;
	}

	public override void Dispose()
	{
		if (_sqlTransaction != null)
		{
			RollbackTransaction();
		}
		GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.ActiveSqlConnections).Decrement();
		base.Dispose();
	}
}
