using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration;

public class BulkMessageProcessorAsync<T> : MessageProcessorAsync<T>
{
	private int _maxBulkSize;

	public BulkMessageProcessorAsync(ProcessDelegate processDelegate, IEventTrace eventTracing, int maxBulkSize)
		: base(processDelegate, eventTracing)
	{
		_maxBulkSize = maxBulkSize;
	}

	public override LinkedListNode<T> ProcessMessage(LinkedListNode<T> node)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (!(node.Value is CommandPackage commandPackage))
		{
			throw new ArgumentNullException("node");
		}
		if (!(commandPackage.CommandBase is ICanBulk<ICommandBulk> canBulk))
		{
			base.PendingItems.Remove(node);
			return node;
		}
		Type type = canBulk.GetType();
		ICommandBulk bulkCommand = canBulk.BulkCommand;
		int num = 0;
		base.PendingItems.Remove(node);
		bulkCommand.AddCommand(commandPackage);
		for (num++; num < _maxBulkSize; num++)
		{
			if (base.PendingItems.Count <= 0)
			{
				break;
			}
			node = base.PendingItems.First;
			if (!((node.Value as CommandPackage).CommandBase.GetType() == type))
			{
				break;
			}
			base.PendingItems.Remove(node);
			CommandPackage command = node.Value as CommandPackage;
			bulkCommand.AddCommand(command);
		}
		eventTracingObj.Trace("Bulk size is {0}", num);
		node = new LinkedListNode<T>((T)Convert.ChangeType(new CommandPackage(bulkCommand as CommonCommandBase, new BasicCallContextInfo(null, null, null)), typeof(T), CultureInfo.CurrentCulture));
		return node;
	}
}
