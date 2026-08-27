using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server;

public class VmoResourceComponent : SqlResourceComponent
{
	protected static readonly ILogger Logger = Telemetry.GetILogger("SQLLog");

	private static Dictionary<int, ConstructorInfo> _exceptionConstructors = RegisterExceptions(Assembly.GetCallingAssembly());

	private readonly ISymmetricEncryption _encryption;

	protected ISymmetricEncryption Encryption => _encryption;

	protected override bool IsDebugEnabled => Logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug);

	public VmoResourceComponent()
		: this(Server.Instance.SqlConnectionManager, Server.Instance.CorporateEncryptionSymmetric)
	{
	}

	public VmoResourceComponent(ISqlConnectionManager sqlConnectionManager, ISymmetricEncryption encryption)
		: base(sqlConnectionManager, 0, 0)
	{
		_encryption = encryption;
	}

	protected override void LogDebug(string entry)
	{
		Logger.LogDebug(entry);
	}

	protected override void AddLogEntryDatabaseConnectionLost()
	{
		ServerLogManager.Instance.AddLogEntry(LogEntryId.DatabaseConnectionLost);
	}

	protected override void LogWarning(string entryFormat, params object[] args)
	{
		Logger.LogWarning(entryFormat, args);
	}

	protected override void MapException(DatabaseException ex)
	{
		VmoServerException ex2 = TranslateException(ex);
		if (ex2 != null)
		{
			throw ex2;
		}
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

	private static Dictionary<int, ConstructorInfo> RegisterExceptions(Assembly assembly)
	{
		Dictionary<int, ConstructorInfo> dictionary = new Dictionary<int, ConstructorInfo>();
		try
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				ConstructorInfo[] constructors = type.GetConstructors();
				foreach (ConstructorInfo constructorInfo in constructors)
				{
					object[] customAttributes = constructorInfo.GetCustomAttributes(typeof(SqlErrorAttribute), inherit: false);
					for (int k = 0; k < customAttributes.Length; k++)
					{
						SqlErrorAttribute sqlErrorAttribute = (SqlErrorAttribute)customAttributes[k];
						dictionary.Add(sqlErrorAttribute.ErrorCode, constructorInfo);
					}
				}
			}
		}
		catch (ReflectionTypeLoadException)
		{
			Console.WriteLine("VmoResourceComponent: Unable to GetTypes() from " + assembly.FullName);
			Logger.LogError("VmoResourceComponent: Unable to GetTypes() from {Assembly}", assembly.FullName);
		}
		return dictionary;
	}

	protected override void Trace(Exception ex)
	{
	}

	protected override void Trace(string entry)
	{
	}
}
