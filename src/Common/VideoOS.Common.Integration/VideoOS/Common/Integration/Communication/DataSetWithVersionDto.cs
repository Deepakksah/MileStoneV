using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
[JsonObject(/*Could not decode attribute arguments.*/)]
public abstract class DataSetWithVersionDto
{
	[DataMember]
	private string dataSetXml;

	private long lastUpdate;

	private long nextUpdate;

	[DataMember]
	public long LastUpdate
	{
		[DebuggerStepThrough]
		get
		{
			return lastUpdate;
		}
		[DebuggerStepThrough]
		set
		{
			lastUpdate = value;
		}
	}

	[DataMember]
	public long NextUpdate
	{
		[DebuggerStepThrough]
		get
		{
			return nextUpdate;
		}
		[DebuggerStepThrough]
		set
		{
			nextUpdate = value;
		}
	}

	public DataSetWithVersionDto()
	{
		lastUpdate = 0L;
		nextUpdate = 0L;
	}

	protected void WriteDataSet(DataSet dataSet)
	{
		if (dataSet == null)
		{
			throw new ArgumentNullException("dataSet");
		}
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter writer = XmlWriter.Create(stringBuilder))
		{
			dataSet.WriteXml(writer, XmlWriteMode.DiffGram);
		}
		dataSetXml = stringBuilder.ToString();
	}

	protected void ReadDataSet(DataSet dataSet)
	{
		if (dataSet == null)
		{
			throw new ArgumentNullException("dataSet");
		}
		using TextReader input = new StringReader(dataSetXml);
		using XmlReader reader = XmlReader.Create(input);
		dataSet.ReadXml(reader, XmlReadMode.DiffGram);
	}
}
