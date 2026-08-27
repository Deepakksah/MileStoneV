using System;
using System.Collections.Generic;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class Message
{
	public int ID { get; set; }

	public string MessageTypeName { get; set; }

	public DateTime Created { get; set; }

	public Dictionary<string, MessageData> Data { get; set; }

	public Message(int id, string messageTypeName, DateTime created, Dictionary<string, MessageData> data)
	{
		ID = id;
		MessageTypeName = messageTypeName;
		Created = created;
		Data = new Dictionary<string, MessageData>();
		if (data == null)
		{
			return;
		}
		foreach (MessageData value in data.Values)
		{
			Data.Add(value.Key, value);
		}
	}
}
