using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Security;
using VideoOS.Server.CommandHandler;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public sealed class TaskHandling : ITaskHandling
{
	public sealed class RawServerTask
	{
		public long SequenceNumber { get; set; }

		public string Type { get; set; }

		public string DataA { get; set; }

		public string DataB { get; set; }

		public DateTime StartTime { get; set; }

		public string StatusDetails { get; set; }

		public int TaskStatus { get; set; }
	}

	public enum DataSetType
	{
		Configuration,
		Security
	}

	private class TaskCommandAndRowInfo<T> where T : DatabaseTaskCommandBase
	{
		public RawServerTask RawServerTask { get; set; }

		public T DatabaseTaskCommand { get; set; }

		public TaskCommandAndRowInfo(RawServerTask rawServerTask, T databaseTaskCommand)
		{
			RawServerTask = rawServerTask;
			DatabaseTaskCommand = databaseTaskCommand;
		}
	}

	private class ArchivingInfo
	{
		public Guid StorageId { get; set; }

		public Guid StorageSourceId { get; set; }

		public Guid StorageTargetId { get; set; }

		public int Progress { get; set; }

		public DateTime BeginTime { get; set; }
	}

	private class RepairingInfo
	{
		public bool IsRepairing { get; set; }

		public int Progress { get; set; }

		public DateTime BeginTime { get; set; }
	}

	private static object _updateRepairInfoSync = new object();

	public static ITaskHandling Instance { get; } = new TaskHandling();

	ServerTaskDto[] ITaskHandling.GetPendingTasks(Guid serviceId, long sequenceNumber)
	{
		return GetPendingTasks(serviceId, sequenceNumber);
	}

	void ITaskHandling.RemoveTask(long taskId)
	{
		RemoveTask(taskId);
	}

	public static ServerTaskDto[] GetPendingTasks(Guid serviceId, long sequenceNumber)
	{
		RawServerTask[] array = null;
		using (TaskComponent taskComponent = new TaskComponent(Server.Instance.CorporateEncryptionSymmetric))
		{
			array = taskComponent.GetPendingTasks(serviceId, sequenceNumber);
		}
		List<ServerTaskDto> list = new List<ServerTaskDto>(array.Length);
		RawServerTask[] array2 = array;
		foreach (RawServerTask rawServerTask in array2)
		{
			ServerTaskDto serverTaskDto = new ServerTaskDto();
			serverTaskDto.SequenceNumber = rawServerTask.SequenceNumber;
			switch (rawServerTask.Type)
			{
			case "diffgram_insert":
				serverTaskDto.TaskXml = ProcessInsertDiffgram(rawServerTask.DataA, DataSetType.Configuration);
				serverTaskDto.TaskType = ServerTaskTypeDto.ConfigurationDiffGram;
				serverTaskDto.TaskDiffGramType = ServerTaskDiffGramTypeDto.Insert;
				break;
			case "diffgram_update":
				serverTaskDto.TaskXml = ProcessUpdateDiffgram(rawServerTask.DataA, rawServerTask.DataB, DataSetType.Configuration);
				serverTaskDto.TaskType = ServerTaskTypeDto.ConfigurationDiffGram;
				serverTaskDto.TaskDiffGramType = ServerTaskDiffGramTypeDto.Update;
				break;
			case "diffgram_delete":
				serverTaskDto.TaskXml = ProcessDeleteDiffgram(rawServerTask.DataA, DataSetType.Configuration);
				serverTaskDto.TaskType = ServerTaskTypeDto.ConfigurationDiffGram;
				serverTaskDto.TaskDiffGramType = ServerTaskDiffGramTypeDto.Delete;
				break;
			case "diffgram_security_insert":
				serverTaskDto.TaskXml = ProcessInsertDiffgram(rawServerTask.DataA, DataSetType.Security);
				serverTaskDto.TaskType = ServerTaskTypeDto.SecurityDiffGram;
				serverTaskDto.TaskDiffGramType = ServerTaskDiffGramTypeDto.Insert;
				break;
			case "diffgram_security_update":
				serverTaskDto.TaskXml = ProcessUpdateDiffgram(rawServerTask.DataA, rawServerTask.DataB, DataSetType.Security);
				serverTaskDto.TaskType = ServerTaskTypeDto.SecurityDiffGram;
				serverTaskDto.TaskDiffGramType = ServerTaskDiffGramTypeDto.Update;
				break;
			case "diffgram_security_delete":
				serverTaskDto.TaskXml = ProcessDeleteDiffgram(rawServerTask.DataA, DataSetType.Security);
				serverTaskDto.TaskType = ServerTaskTypeDto.SecurityDiffGram;
				serverTaskDto.TaskDiffGramType = ServerTaskDiffGramTypeDto.Delete;
				break;
			default:
				serverTaskDto.TaskType = ServerTaskTypeDto.Task;
				serverTaskDto.TaskXml = rawServerTask.DataA;
				serverTaskDto.Status = rawServerTask.TaskStatus.ToString(CultureInfo.InvariantCulture);
				serverTaskDto.TaskDiffGramType = ServerTaskDiffGramTypeDto.DoesNotApply;
				break;
			}
			list.Add(serverTaskDto);
		}
		return list.ToArray();
	}

	public static ServerTaskDto[] GetTasksWithPendingStop(Guid serviceId)
	{
		RawServerTask[] array = null;
		using (TaskComponent taskComponent = new TaskComponent(Server.Instance.CorporateEncryptionSymmetric))
		{
			array = taskComponent.GetTasksWithPendingStop(serviceId);
		}
		List<ServerTaskDto> list = new List<ServerTaskDto>(array.Length);
		RawServerTask[] array2 = array;
		foreach (RawServerTask rawServerTask in array2)
		{
			ServerTaskDto serverTaskDto = new ServerTaskDto();
			serverTaskDto.SequenceNumber = rawServerTask.SequenceNumber;
			switch (rawServerTask.Type)
			{
			case "diffgram_insert":
			case "diffgram_update":
			case "diffgram_delete":
			case "diffgram_security_insert":
			case "diffgram_security_update":
			case "diffgram_security_delete":
				continue;
			}
			serverTaskDto.TaskType = ServerTaskTypeDto.Task;
			serverTaskDto.TaskXml = rawServerTask.DataA;
			serverTaskDto.Status = rawServerTask.TaskStatus.ToString(CultureInfo.InvariantCulture);
			list.Add(serverTaskDto);
		}
		return list.ToArray();
	}

	public static void RemoveTask(long taskId)
	{
		using TaskComponent taskComponent = new TaskComponent(Server.Instance.CorporateEncryptionSymmetric);
		taskComponent.RemoveTask(taskId);
	}

	public static string UpdateTaskProgress(long taskId, int percentComplete, string details, int status, DateTime endTime)
	{
		using TaskComponent taskComponent = new TaskComponent(Server.Instance.CorporateEncryptionSymmetric);
		return taskComponent.UpdateTaskProgress(taskId, percentComplete, details, status, endTime);
	}

	public static string StopTask(long taskId)
	{
		using TaskComponent taskComponent = new TaskComponent(Server.Instance.CorporateEncryptionSymmetric);
		return taskComponent.StopTask(taskId);
	}

	public static void UpdateArchivingTasks(Guid recorderId, Guid serviceId, string archiveInfoXml)
	{
		using TaskComponent taskComponent = new TaskComponent(Server.Instance.CorporateEncryptionSymmetric);
		RawServerTask[] tasksByType = taskComponent.GetTasksByType(serviceId, "Database.Archiving");
		List<TaskCommandAndRowInfo<DatabaseArchivingCommand>> list = new List<TaskCommandAndRowInfo<DatabaseArchivingCommand>>();
		RawServerTask[] array = tasksByType;
		foreach (RawServerTask rawServerTask in array)
		{
			DatabaseArchivingCommand databaseTaskCommand = (DatabaseArchivingCommand)DatabaseTaskCommandBase.CreateCommandFromXml(rawServerTask.DataA);
			list.Add(new TaskCommandAndRowInfo<DatabaseArchivingCommand>(rawServerTask, databaseTaskCommand));
		}
		IEnumerable<ArchivingInfo> source = ParseArchivingInfo(archiveInfoXml);
		HashSet<Guid> currentTasksByStorage = new HashSet<Guid>();
		foreach (TaskCommandAndRowInfo<DatabaseArchivingCommand> currentTask in list)
		{
			ArchivingInfo archivingInfo = source.FirstOrDefault((ArchivingInfo a) => a.StorageSourceId.Equals(currentTask.DatabaseTaskCommand.SourceStorageId) && a.StorageTargetId.Equals(currentTask.DatabaseTaskCommand.TargetStorageId) && a.BeginTime.Equals(currentTask.RawServerTask.StartTime));
			if (archivingInfo != null)
			{
				ArchiveTaskStatusDetails archiveTaskStatusDetails;
				if (string.IsNullOrEmpty(currentTask.RawServerTask.StatusDetails))
				{
					archiveTaskStatusDetails = new ArchiveTaskStatusDetails();
					archiveTaskStatusDetails.AdjustedStartTime = archivingInfo.BeginTime.ToUniversalTime();
				}
				else
				{
					archiveTaskStatusDetails = ArchiveTaskStatusDetails.FromXml(currentTask.RawServerTask.StatusDetails);
				}
				TaskStatus taskStatus = ((archivingInfo.Progress != 0) ? TaskStatus.InProgress : TaskStatus.Inactive);
				if (taskStatus == TaskStatus.Inactive)
				{
					archiveTaskStatusDetails.AdjustedStartTime = DateTime.UtcNow;
				}
				DateTime endTime = TaskCommandBase.CalculateEstimatedEndTime(archiveTaskStatusDetails.AdjustedStartTime, archivingInfo.Progress);
				string details = archiveTaskStatusDetails.ToXml();
				taskComponent.UpdateTaskProgress(currentTask.DatabaseTaskCommand.TaskId, archivingInfo.Progress, details, (int)taskStatus, endTime);
				currentTasksByStorage.Add(currentTask.DatabaseTaskCommand.StorageId);
			}
			else
			{
				taskComponent.RemoveTask(currentTask.DatabaseTaskCommand.TaskId);
			}
		}
		IEnumerable<ArchivingInfo> enumerable = source.Where((ArchivingInfo a) => !currentTasksByStorage.Contains(a.StorageId));
		foreach (ArchivingInfo item in enumerable)
		{
			XDocument xDocument = new XDocument(new XElement("task", new XAttribute("type", "Database.Archiving"), new XAttribute("id", string.Empty), new XElement("target", serviceId.ToString()), new XElement("idRecorder", recorderId.ToString()), new XElement("idStorage", item.StorageId.ToString()), new XElement("idStorageSource", item.StorageSourceId.ToString()), new XElement("idStorageTarget", item.StorageTargetId.ToString())));
			string data_a = xDocument.ToString();
			taskComponent.CreateTask("Database.Archiving", serviceId, data_a, item.Progress, item.BeginTime);
		}
	}

	private static IEnumerable<ArchivingInfo> ParseArchivingInfo(string archiveInfoXml)
	{
		List<ArchivingInfo> list = new List<ArchivingInfo>();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(archiveInfoXml);
		XmlNode xmlNode = xmlDocument["archive_info"];
		if (xmlNode != null && xmlNode.HasChildNodes)
		{
			foreach (XmlNode item2 in xmlNode)
			{
				Guid storageSourceId = ReadNodeGuid(item2["source_bank"], "alternate_id");
				Guid guid = ReadNodeGuid(item2["destination_bank"], "alternate_id");
				if (!storageSourceId.Equals(guid))
				{
					bool flag = false;
					int num = 0;
					DateTime dateTime = DateTime.MinValue;
					XmlNode xmlNode3 = item2["low_priority"];
					if (xmlNode3 != null)
					{
						num = ReadNodeInt(xmlNode3, "progress");
						dateTime = ReadNodeDateTime(xmlNode3, "begin_time");
						flag = true;
					}
					bool flag2 = false;
					int num2 = 0;
					DateTime dateTime2 = DateTime.MinValue;
					xmlNode3 = item2["high_priority"];
					if (xmlNode3 != null)
					{
						num2 = ReadNodeInt(xmlNode3, "progress");
						dateTime2 = ReadNodeDateTime(xmlNode3, "begin_time");
						flag2 = true;
					}
					int progress;
					DateTime beginTime;
					if (flag && flag2)
					{
						progress = (num + num2) / 2;
						beginTime = ((dateTime < dateTime2) ? dateTime : dateTime2);
					}
					else if (flag)
					{
						progress = num;
						beginTime = dateTime;
					}
					else
					{
						progress = num2;
						beginTime = dateTime2;
					}
					ArchivingInfo item = new ArchivingInfo
					{
						StorageId = guid,
						StorageSourceId = storageSourceId,
						StorageTargetId = guid,
						Progress = progress,
						BeginTime = beginTime
					};
					list.Add(item);
				}
			}
		}
		return list.AsEnumerable();
	}

	public static void UpdateReparingTasks(Guid recorderId, Guid serviceId, string repairInfoXml)
	{
		UpdateReparingTasksInternal(recorderId, serviceId, repairInfoXml, new DataComponentFactory());
	}

	public static void UpdateReparingTasksInternal(Guid recorderId, Guid serviceId, string repairInfoXml, IDataComponentFactory dataComponentFactory)
	{
		using ITaskComponent taskComponent = dataComponentFactory.CreateTaskComponent();
		lock (_updateRepairInfoSync)
		{
			RawServerTask[] tasksByType = taskComponent.GetTasksByType(serviceId, "Database.Repairing2");
			RawServerTask rawServerTask = tasksByType.SingleOrDefault();
			RepairingInfo repairingInfo = ParseRepairInfo(repairInfoXml);
			if (repairingInfo.IsRepairing)
			{
				if (rawServerTask != null && !rawServerTask.StartTime.Equals(repairingInfo.BeginTime))
				{
					taskComponent.RemoveTask(rawServerTask.SequenceNumber);
					rawServerTask = null;
				}
				if (rawServerTask != null)
				{
					DatabaseRepairingCommand databaseRepairingCommand = (DatabaseRepairingCommand)DatabaseTaskCommandBase.CreateCommandFromXml(rawServerTask.DataA);
					DateTime endTime = TaskCommandBase.CalculateEstimatedEndTime(repairingInfo.BeginTime, repairingInfo.Progress);
					taskComponent.UpdateTaskProgress(databaseRepairingCommand.TaskId, repairingInfo.Progress, string.Empty, 1, endTime);
					return;
				}
				string content = QueryRecorderName(recorderId, dataComponentFactory);
				XDocument xDocument = new XDocument(new XElement("task", new XAttribute("type", "Database.Repairing2"), new XAttribute("id", string.Empty), new XElement("target", serviceId.ToString()), new XElement("idRecorder", recorderId.ToString()), new XElement("recordingServerName", content), new XElement("idStorage", Guid.Empty)));
				string data_a = xDocument.ToString();
				taskComponent.CreateTask("Database.Repairing2", serviceId, data_a, repairingInfo.Progress, repairingInfo.BeginTime);
			}
			else if (rawServerTask != null)
			{
				taskComponent.RemoveTask(rawServerTask.SequenceNumber);
				rawServerTask = null;
			}
		}
	}

	private static string QueryRecorderName(Guid recorderId, IDataComponentFactory dataComponentFactory)
	{
		string result = string.Empty;
		using (IRecorderConfigDataComponent recorderConfigDataComponent = dataComponentFactory.CreateRecorderConfigDataComponent())
		{
			RecorderConfigData recorderConfigData = null;
			try
			{
				recorderConfigData = recorderConfigDataComponent.QueryRecorderData(recorderId);
				if (recorderConfigData != null)
				{
					result = recorderConfigData.Name;
				}
			}
			catch (DatabaseException)
			{
			}
		}
		return result;
	}

	private static RepairingInfo ParseRepairInfo(string repairInfoXml)
	{
		RepairingInfo result = new RepairingInfo
		{
			IsRepairing = false
		};
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(repairInfoXml);
		XmlNode xmlNode = xmlDocument["RepairInfo"];
		if (xmlNode != null && xmlNode.HasChildNodes)
		{
			bool isRepairing = ReadNodeBoolean(xmlNode, "IsRepairing");
			int progress = ReadNodeInt(xmlNode, "Progress");
			DateTime beginTime = ReadNodeDateTime(xmlNode, "BeginTime");
			result = new RepairingInfo
			{
				IsRepairing = isRepairing,
				Progress = progress,
				BeginTime = beginTime
			};
		}
		return result;
	}

	private static string EncodeXmlColumns(string dataSet)
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(dataSet));
		xmlTextReader.Read();
		xmlDocument.Load(xmlTextReader);
		XmlNodeList childNodes = xmlDocument.FirstChild.ChildNodes;
		if (childNodes != null)
		{
			foreach (XmlNode item in childNodes)
			{
				XmlNodeList childNodes2 = item.ChildNodes;
				if (childNodes2 == null)
				{
					continue;
				}
				foreach (XmlNode item2 in childNodes2)
				{
					if (item2.HasChildNodes && item2.FirstChild.NodeType != XmlNodeType.Text)
					{
						string innerXml = item2.InnerXml;
						item2.RemoveAll();
						XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Text, "#text", null);
						xmlNode3.Value = innerXml;
						item2.AppendChild(xmlNode3);
					}
				}
			}
		}
		return xmlDocument.FirstChild.OuterXml;
	}

	public static string ProcessInsertDiffgram(string dataA, DataSetType type)
	{
		DataSet dataSetInstance = GetDataSetInstance(type);
		try
		{
			dataA = EncodeXmlColumns(dataA);
			dataA = ReplaceNamespace(dataSetInstance.Namespace, dataA);
			using (StringReader reader = new StringReader(dataA))
			{
				dataSetInstance.ReadXml(reader);
			}
			return WriteDataSet(dataSetInstance);
		}
		finally
		{
			dataSetInstance.Dispose();
		}
	}

	public static string ProcessUpdateDiffgram(string dataA, string dataB, DataSetType type)
	{
		DataSet dataSetInstance = GetDataSetInstance(type);
		try
		{
			dataA = EncodeXmlColumns(dataA);
			dataA = ReplaceNamespace(dataSetInstance.Namespace, dataA);
			dataB = EncodeXmlColumns(dataB);
			dataB = ReplaceNamespace(dataSetInstance.Namespace, dataB);
			using (StringReader reader = new StringReader(dataA))
			{
				dataSetInstance.ReadXml(reader);
				dataSetInstance.AcceptChanges();
			}
			DataSet dataSetInstance2 = GetDataSetInstance(type);
			try
			{
				using (StringReader reader2 = new StringReader(dataB))
				{
					dataSetInstance2.ReadXml(reader2);
					dataSetInstance2.AcceptChanges();
				}
				foreach (DataTable table in dataSetInstance2.Tables)
				{
					foreach (DataRow row in table.Rows)
					{
						DataColumn[] primaryKey = table.PrimaryKey;
						object[] array = new object[primaryKey.Length];
						int num = 0;
						DataColumn[] array2 = primaryKey;
						foreach (DataColumn column in array2)
						{
							array[num] = row[column];
							num++;
						}
						DataTable dataTable2 = dataSetInstance.Tables[table.TableName];
						DataRow dataRow2 = dataTable2.Rows.Find(array);
						foreach (DataColumn item in dataTable2.Columns.Cast<DataColumn>())
						{
							item.ReadOnly = false;
						}
						dataRow2.ItemArray = row.ItemArray;
					}
				}
				return WriteDataSet(dataSetInstance);
			}
			finally
			{
				dataSetInstance2.Dispose();
			}
		}
		finally
		{
			dataSetInstance.Dispose();
		}
	}

	public static string ProcessDeleteDiffgram(string dataA, DataSetType type)
	{
		DataSet dataSetInstance = GetDataSetInstance(type);
		try
		{
			dataA = EncodeXmlColumns(dataA);
			dataA = ReplaceNamespace(dataSetInstance.Namespace, dataA);
			using (StringReader reader = new StringReader(dataA))
			{
				dataSetInstance.ReadXml(reader);
				dataSetInstance.AcceptChanges();
			}
			foreach (DataTable table in dataSetInstance.Tables)
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					table.Rows[i].Delete();
				}
			}
			return WriteDataSet(dataSetInstance);
		}
		finally
		{
			dataSetInstance.Dispose();
		}
	}

	private static string WriteDataSet(DataSet dataSet)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter writer = XmlWriter.Create(stringBuilder))
		{
			dataSet.WriteXml(writer, XmlWriteMode.DiffGram);
		}
		return stringBuilder.ToString();
	}

	private static DataSet GetDataSetInstance(DataSetType type)
	{
		DataSet result = null;
		switch (type)
		{
		case DataSetType.Configuration:
			result = new ConfigurationDataSet();
			break;
		case DataSetType.Security:
			result = new SecurityDataSet();
			break;
		}
		return result;
	}

	private static string ReadNodeText(XmlNode node, string name)
	{
		if (node != null && node.HasChildNodes)
		{
			foreach (XmlNode item in node)
			{
				if (item != null && item.HasChildNodes && item.Name == name)
				{
					return item.InnerText;
				}
			}
		}
		return null;
	}

	private static Guid ReadNodeGuid(XmlNode node, string name)
	{
		string text = ReadNodeText(node, name);
		if (text != null)
		{
			return Guid.Parse(text);
		}
		return Guid.Empty;
	}

	private static bool ReadNodeBoolean(XmlNode node, string name)
	{
		string text = ReadNodeText(node, name);
		if (text == null)
		{
			return false;
		}
		if (!bool.TryParse(text, out var result))
		{
			return false;
		}
		return result;
	}

	private static int ReadNodeInt(XmlNode node, string name)
	{
		string text = ReadNodeText(node, name);
		if (text == null)
		{
			return 0;
		}
		if (!int.TryParse(text, out var result))
		{
			return 0;
		}
		return result;
	}

	private static DateTime ReadNodeDateTime(XmlNode node, string name)
	{
		string text = ReadNodeText(node, name);
		if (text == null)
		{
			return DateTime.MinValue;
		}
		if (!DateTime.TryParse(text, out var result))
		{
			return DateTime.MinValue;
		}
		return result.ToUniversalTime();
	}

	private static string ReplaceNamespace(string newNamespace, string xml)
	{
		if (newNamespace == null)
		{
			return xml;
		}
		if (string.IsNullOrEmpty(xml))
		{
			return xml;
		}
		return xml.Replace("{XMLNS}", newNamespace);
	}
}
