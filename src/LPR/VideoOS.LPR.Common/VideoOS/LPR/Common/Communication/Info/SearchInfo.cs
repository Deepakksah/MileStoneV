#define TRACE
using System;
using System.Diagnostics;
using System.Text;
using VideoOS.EventServer.Communication.AlarmClient;

namespace VideoOS.LPR.Common.Communication.Info;

public class SearchInfo
{
	public SearchGroup SearchGroup;

	public SearchOrders SearchOrders;

	public Guid SearchId;

	public int Page;

	public int PageSize;

	public string GenerateAuditLogString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		SetSearchGroupString(stringBuilder, SearchGroup);
		stringBuilder.Append(' ');
		SetSearchOrderString(stringBuilder, SearchOrders);
		Trace.WriteLine("¤¤¤¤¤¤¤¤¤¤¤¤ : " + stringBuilder.ToString());
		return stringBuilder.ToString();
	}

	private static void SetSearchGroupString(StringBuilder builder, SearchGroup searchGroup)
	{
		builder.Append("( ");
		bool flag = true;
		if (searchGroup.SearchElements != null)
		{
			SearchElement[] searchElements = searchGroup.SearchElements;
			foreach (SearchElement searchElement in searchElements)
			{
				if (!flag)
				{
					builder.Append(GetLogicalOperatorString(searchGroup.GroupOperator));
				}
				flag = false;
				builder.Append(GetTargetString(searchElement.SearchTarget));
				builder.Append(GetOperatorString(searchElement.SearchOperator));
				builder.Append(searchElement.SearchValue);
			}
		}
		flag = true;
		if (searchGroup.SearchGroups != null)
		{
			SearchGroup[] searchGroups = searchGroup.SearchGroups;
			foreach (SearchGroup searchGroup2 in searchGroups)
			{
				if (!flag || (searchGroup.SearchElements != null && searchGroup.SearchElements.Length != 0))
				{
					builder.Append(GetLogicalOperatorString(searchGroup.GroupOperator));
				}
				flag = false;
				SetSearchGroupString(builder, searchGroup2);
			}
		}
		builder.Append(" )");
	}

	private static void SetSearchOrderString(StringBuilder builder, SearchOrders searchOrders)
	{
		if (searchOrders == null || searchOrders.Orders == null)
		{
			return;
		}
		builder.Append(" ORDER BY ");
		for (int i = 0; i < searchOrders.Orders.Length; i++)
		{
			builder.Append(' ');
			builder.Append(GetTargetString(searchOrders.Orders[i].Target));
			builder.Append(' ');
			builder.Append(GetOrderByString(searchOrders.Orders[i].Direction));
			if (i != searchOrders.Orders.Length - 1)
			{
				builder.Append(" , ");
			}
		}
	}

	private static string GetOrderByString(Direction direction)
	{
		if (direction != Direction.Ascending && direction == Direction.Descending)
		{
			return " DESC ";
		}
		return " ASC ";
	}

	private static string GetOperatorString(SearchOperator op)
	{
		switch (op)
		{
		case SearchOperator.Equals:
			return " = ";
		case SearchOperator.NotEquals:
			return " != ";
		case SearchOperator.LessThan:
			return " < ";
		case SearchOperator.GreaterThan:
			return " > ";
		case SearchOperator.Contains:
		case SearchOperator.BeginsWith:
			return " LIKE ";
		default:
			return "";
		}
	}

	private static string GetLogicalOperatorString(LogicalOperator op)
	{
		return op switch
		{
			LogicalOperator.And => " AND ", 
			LogicalOperator.Or => " OR ", 
			_ => "", 
		};
	}

	private static string GetTargetString(string target)
	{
		return target switch
		{
			"ObjectValue" => "Number", 
			"CustomTag" => "Country", 
			"SourceName" => "Source", 
			"ObjectId" => "SourceId", 
			"Timestamp" => "Time", 
			"Message" => "List", 
			"Type" => "Type", 
			_ => "<unknown>", 
		};
	}
}
