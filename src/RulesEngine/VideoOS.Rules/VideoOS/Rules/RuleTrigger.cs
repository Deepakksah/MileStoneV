using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class RuleTrigger : IRuleTrigger
{
	private Guid _eventTypeId;

	private List<SourceCondition> _sourceConditions;

	public Guid EventTypeId => _eventTypeId;

	public List<SourceCondition> SourceConditions
	{
		get
		{
			return _sourceConditions;
		}
		set
		{
			_sourceConditions = value;
		}
	}

	public RuleTrigger(Guid eventTypeId)
	{
		_eventTypeId = eventTypeId;
	}

	public void InitializeSources(XmlNode sourcesNode, IGroupConfiguration groupConfiguration)
	{
		if (sourcesNode == null)
		{
			SourceConditions = new List<SourceCondition>();
			return;
		}
		List<SourceCondition> list = new List<SourceCondition>();
		foreach (XmlNode childNode in sourcesNode.ChildNodes)
		{
			string name = childNode.Name;
			if (!(name == "SourceList"))
			{
				if (name == "GroupList")
				{
					List<Guid> list2 = new List<Guid>();
					foreach (XmlNode childNode2 in childNode.ChildNodes)
					{
						if (childNode2.NodeType == XmlNodeType.Element && !(childNode2.Name != "GroupId"))
						{
							list2.Add(new Guid(childNode2.InnerText));
						}
					}
					if (list2.Count > 0)
					{
						list.Add(new GroupSourceCondition(list2, groupConfiguration));
					}
					continue;
				}
				throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "Unexpected condition node name in rule source xml. Node name: '{0}'", childNode.Name));
			}
			List<Guid> list3 = new List<Guid>();
			foreach (XmlNode childNode3 in childNode.ChildNodes)
			{
				if (childNode3.NodeType == XmlNodeType.Element && !(childNode3.Name != "SourceId"))
				{
					list3.Add(new Guid(childNode3.InnerText));
				}
			}
			if (list3.Count > 0)
			{
				list.Add(new ListSourceCondition(list3));
			}
		}
		SourceConditions = list;
	}

	public List<Guid> GetSources()
	{
		HashSet<Guid> hashSet = new HashSet<Guid>();
		foreach (SourceCondition sourceCondition in _sourceConditions)
		{
			foreach (Guid source in sourceCondition.GetSources())
			{
				hashSet.Add(source);
			}
		}
		return hashSet.ToList();
	}

	public List<Guid> GetSourceTargetGroupIds()
	{
		List<Guid> list = new List<Guid>();
		foreach (SourceCondition sourceCondition in _sourceConditions)
		{
			if (sourceCondition is GroupSourceCondition groupSourceCondition)
			{
				list.AddRange(groupSourceCondition.GroupIds);
			}
		}
		return list;
	}
}
