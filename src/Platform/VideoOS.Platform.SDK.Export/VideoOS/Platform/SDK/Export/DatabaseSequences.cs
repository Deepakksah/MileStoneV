using System;
using System.Globalization;
using VideoOS.DatabaseUtility;
using VideoOS.DatabaseUtility.MediaStorage;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Export;

public class DatabaseSequences : IDisposable
{
	private enum State
	{
		Unknown,
		Connected,
		Error,
		Closed
	}

	private enum Errors
	{
		ConnectionClosed,
		ErrorConnecting
	}

	public Guid Media = new Guid("7ea9b53b-749e-420b-891b-43b238f6144e");

	public Guid Recording = new Guid("8b742df9-5ba1-49df-ace1-66e54f5dc295");

	public Guid Motion = new Guid("908d3e34-3510-4372-9e3f-20c53b92490a");

	public Guid Signature = new Guid("f3f39289-0372-410d-8936-802b1e627567");

	private ISystemInfo _systemInfo;

	private IDatabaseMetaData _database;

	private State _state;

	private object _connectionLock = new object();

	public FQID FQID { get; }

	public static DatabaseSequences FindOrCreate(FQID fqid, ISystemInfo systemInfo)
	{
		foreach (IDisposable item in systemInfo.DisposableObjects())
		{
			if (item is DatabaseSequences databaseSequences && databaseSequences.FQID.ObjectId == fqid.ObjectId)
			{
				return databaseSequences;
			}
		}
		DatabaseSequences databaseSequences2 = new DatabaseSequences(fqid, systemInfo);
		systemInfo.AddToDisposableObjects(databaseSequences2);
		return databaseSequences2;
	}

	public DatabaseSequences(FQID fqid, ISystemInfo systemInfo)
	{
		_systemInfo = systemInfo;
		FQID = fqid;
	}

	public TimelineResponse[] GetTimeLineInformation(MediaType mediaType, TimeLineInformationType activityType, DateTime beginTime, TimeSpan resolution, int count)
	{
		lock (_connectionLock)
		{
			try
			{
				if (_state == State.Closed)
				{
					throw new MIPException(Errors.ConnectionClosed.ToString());
				}
				if (_state != State.Connected)
				{
					InternalOpenConnection();
				}
				if (_state != State.Connected)
				{
					throw new MIPException(Errors.ErrorConnecting.ToString());
				}
				return _database.GetTimeLineInformation(mediaType, activityType, beginTime, resolution, count);
			}
			catch (Exception)
			{
				EnvironmentManager.Instance.Log(error: true, "DatabaseSequences", string.Format("Error in GetTimeLineInformation. MediaType {0}, ActivityType {1}, BeginTime {2}, Resolution {3}, Count {4}", mediaType.ToString(), activityType.ToString(), beginTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture), resolution.ToString(), count.ToString(CultureInfo.InvariantCulture)));
				_state = State.Error;
				InternalCloseConnection();
				throw;
			}
		}
	}

	public SequencesResponse[] SequencesGetAroundWithSpan(SequenceType sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		lock (_connectionLock)
		{
			try
			{
				if (_state == State.Closed)
				{
					throw new MIPException(Errors.ConnectionClosed.ToString());
				}
				if (_state != State.Connected)
				{
					InternalOpenConnection();
				}
				if (_state != State.Connected)
				{
					throw new MIPException(Errors.ErrorConnecting.ToString());
				}
				return _database.SequencesGetAroundWithSpan(sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
			}
			catch (Exception)
			{
				EnvironmentManager.Instance.Log(error: true, "DatabaseSequences", string.Format("Error in SequencesGetAroundWithSpan. SequenceType {0}, CenterTime {1}, MaxTimeBefore {2}, MaxCountBefore {3}, MaxTimeAfter {4}, MaxCountAfter {5}", sequenceType.ToString(), centerTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture), maxTimeBefore.ToString(), maxCountBefore.ToString(CultureInfo.InvariantCulture), maxTimeAfter.ToString(), maxCountAfter.ToString(CultureInfo.InvariantCulture)));
				_state = State.Error;
				InternalCloseConnection();
				throw;
			}
		}
	}

	private void InternalOpenConnection()
	{
		InternalCloseConnection();
		if (_systemInfo.IsConnected)
		{
			if (FQID.ServerId.ServerType == "PQZ")
			{
				throw new NotSupportedMIPException("Attempting to open unsupported export format.");
			}
			IDeviceInFile deviceInFile = _systemInfo.GetDevice(FQID) as IDeviceInFile;
			string bankPath = FQID.ServerId.ServerHostname;
			if (deviceInFile != null && !string.IsNullOrEmpty(deviceInFile.DatabasePath))
			{
				bankPath = deviceInFile.DatabasePath;
			}
			using (BankInfo bankInfo = new BankInfo(bankPath))
			{
				TableInfo tableInfo = FindTable(bankInfo, FQID.ObjectId.ToString());
				if (tableInfo == null)
				{
					tableInfo = FindTable(bankInfo, FQID.ObjectIdString);
				}
				if (tableInfo == null)
				{
					tableInfo = FindTable(bankInfo, "{0}");
					if (tableInfo != null)
					{
						FQID.ObjectIdString = "{0}";
					}
				}
				if (tableInfo != null)
				{
					string masterPassword = string.Empty;
					if (bankInfo.Encrypted)
					{
						LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(FQID);
						if (loginSettings != null)
						{
							masterPassword = loginSettings.NetworkCredential.Password;
						}
					}
					_database = new MediaStorageDatabase(new TableInfo[1] { tableInfo }, includeSiblingArchives: true, masterPassword);
				}
			}
			if (_database != null)
			{
				bool flag = _database.Open();
				_state = (flag ? State.Connected : State.Error);
			}
			else
			{
				_state = State.Error;
			}
		}
		else
		{
			_state = State.Error;
		}
	}

	public void Dispose()
	{
		InternalCloseConnection();
	}

	private void InternalCloseConnection()
	{
		if (_database != null && _database.IsOpen())
		{
			_database.Close();
		}
		_database = null;
	}

	private static TableInfo FindTable(BankInfo bank, string tableName)
	{
		foreach (TableInfo key in bank.ArchiveMap.Keys)
		{
			if (key.Name == tableName)
			{
				return key;
			}
			foreach (TableInfo item in bank.ArchiveMap[key])
			{
				if (item.Name == tableName)
				{
					return key;
				}
			}
		}
		return null;
	}
}
