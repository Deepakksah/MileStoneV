using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.Tasks;

public static class ServerTaskBatchHandler
{
	public static IEnumerable<ConfigurationDataSet> EnumerateConfigurationChangeTaskBatches(IEnumerable<ServerTaskDto> allServerTasks)
	{
		var diffGramTasks = (from t in allServerTasks
			where t.TaskType == ServerTaskTypeDto.ConfigurationDiffGram
			select new
			{
				Task = t,
				BatchUpdateEntityId = ReadBatchUpdateEntityId(t)
			} into t
			orderby t.Task.SequenceNumber
			select t).ToList();
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		HashSet<string> batchupdateEntityIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		for (int i = 0; i < diffGramTasks.Count(); i++)
		{
			if (configurationDataSet == null)
			{
				configurationDataSet = new ConfigurationDataSet();
			}
			var anon = diffGramTasks.ElementAt(i);
			ReadDataSet(configurationDataSet, anon.Task.TaskXml);
			bool flag = false;
			var anon2 = diffGramTasks.ElementAtOrDefault(i + 1);
			if (anon2 != null)
			{
				switch (anon.Task.TaskDiffGramType)
				{
				case ServerTaskDiffGramTypeDto.Insert:
				case ServerTaskDiffGramTypeDto.Delete:
					if (anon.Task.TaskDiffGramType == anon2.Task.TaskDiffGramType)
					{
						flag = true;
					}
					break;
				case ServerTaskDiffGramTypeDto.Update:
					if (anon2.Task.TaskDiffGramType == ServerTaskDiffGramTypeDto.Update && anon.BatchUpdateEntityId != null)
					{
						batchupdateEntityIds.Add(anon.BatchUpdateEntityId);
						if (anon2.BatchUpdateEntityId != null && !batchupdateEntityIds.Contains(anon2.BatchUpdateEntityId))
						{
							flag = true;
						}
					}
					break;
				}
			}
			if (!flag)
			{
				yield return configurationDataSet;
				configurationDataSet = null;
				batchupdateEntityIds.Clear();
			}
		}
	}

	private static string ReadBatchUpdateEntityId(ServerTaskDto task)
	{
		if (task.TaskDiffGramType != ServerTaskDiffGramTypeDto.Update)
		{
			return null;
		}
		XDocument xDocument = XDocument.Parse(task.TaskXml);
		XElement xElement = xDocument.Root.Elements().First();
		if (xElement.Elements().Count() != 1)
		{
			return null;
		}
		XElement xElement2 = xElement.Elements().First();
		if (string.Equals(xElement2.Name.LocalName, ConfigurationDataSet.ReadOnlyInstance.Devices.TableName, StringComparison.Ordinal))
		{
			XElement xElement3 = xElement2.Elements().First((XElement e) => string.Equals(e.Name.LocalName, ConfigurationDataSet.ReadOnlyInstance.Devices.IDDeviceColumn.ColumnName, StringComparison.Ordinal));
			return xElement3.Value;
		}
		if (string.Equals(xElement2.Name.LocalName, ConfigurationDataSet.ReadOnlyInstance.Hardware.TableName, StringComparison.Ordinal))
		{
			XElement xElement4 = xElement2.Elements().First((XElement e) => string.Equals(e.Name.LocalName, ConfigurationDataSet.ReadOnlyInstance.Hardware.IDHardwareColumn.ColumnName, StringComparison.Ordinal));
			return xElement4.Value;
		}
		return null;
	}

	private static void ReadDataSet(DataSet dataSet, string dataSetXml)
	{
		using StringReader input = new StringReader(dataSetXml);
		using XmlReader reader = XmlReader.Create(input);
		dataSet.ReadXml(reader, XmlReadMode.DiffGram);
	}
}
