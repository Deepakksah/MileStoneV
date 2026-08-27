using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.TrayController.Communication;

public class StatusMessageState
{
	private int nextSequenceNumber = 1;

	private const int MaxSize = 100;

	private int SequenceNumber;

	private object lockObject = new object();

	public Queue<LogMessage> MessageQueue { get; private set; }

	public StatusMessageState()
	{
		MessageQueue = new Queue<LogMessage>();
		SequenceNumber = 0;
	}

	public void AddMessage(DateTime timestamp, string message)
	{
		lock (lockObject)
		{
			LogMessage logMessage = new LogMessage();
			logMessage.SequenceNumber = nextSequenceNumber++;
			logMessage.Timestamp = timestamp;
			logMessage.Message = message;
			MessageQueue.Enqueue(logMessage);
			while (MessageQueue.Count > 100)
			{
				MessageQueue.Dequeue();
			}
			SequenceNumber++;
		}
	}

	public IEnumerable<LogMessage> GetMessages(int sequenceNumber)
	{
		lock (lockObject)
		{
			List<LogMessage> list = new List<LogMessage>();
			if (sequenceNumber < SequenceNumber)
			{
				IEnumerator enumerator = MessageQueue.GetEnumerator();
				for (int i = SequenceNumber - MessageQueue.Count; i < sequenceNumber; i++)
				{
					enumerator.MoveNext();
				}
				while (enumerator.MoveNext())
				{
					LogMessage item = (LogMessage)enumerator.Current;
					list.Add(item);
				}
			}
			return list;
		}
	}
}
