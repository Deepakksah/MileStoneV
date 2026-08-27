using System;
using System.Collections.Generic;

namespace VideoOS.Rules.Adapters.XmlHelper;

public class SourcesHelper
{
	public string[] SourceList { get; set; }

	public string[] GroupList { get; set; }

	public string[] SiteSourceList { get; set; }

	public IEnumerable<Guid> SourceListAsGuids
	{
		get
		{
			Guid[] array = new Guid[SourceList.Length];
			int num = 0;
			string[] sourceList = SourceList;
			foreach (string g in sourceList)
			{
				array[num] = new Guid(g);
				num++;
			}
			return array;
		}
	}

	public IEnumerable<Guid> GroupListAsGuids
	{
		get
		{
			Guid[] array = new Guid[GroupList.Length];
			int num = 0;
			string[] groupList = GroupList;
			foreach (string g in groupList)
			{
				array[num] = new Guid(g);
				num++;
			}
			return array;
		}
	}

	public static bool CheckIdentical(SourcesHelper sourcesHelper1, SourcesHelper sourcesHelper2)
	{
		if (sourcesHelper1 == null)
		{
			throw new ArgumentNullException("sourcesHelper1");
		}
		if (sourcesHelper2 == null)
		{
			throw new ArgumentNullException("sourcesHelper2");
		}
		bool flag = sourcesHelper1.SourceList.Length == sourcesHelper2.SourceList.Length && sourcesHelper1.GroupList.Length == sourcesHelper2.GroupList.Length;
		if (flag)
		{
			bool flag2 = true;
			string[] sourceList = sourcesHelper1.SourceList;
			foreach (string text in sourceList)
			{
				flag2 = false;
				string[] sourceList2 = sourcesHelper2.SourceList;
				foreach (string text2 in sourceList2)
				{
					flag2 = flag2 || text == text2;
				}
			}
			flag = flag2;
		}
		if (flag)
		{
			bool flag3 = true;
			string[] sourceList = sourcesHelper1.GroupList;
			foreach (string text3 in sourceList)
			{
				flag3 = false;
				string[] sourceList2 = sourcesHelper2.GroupList;
				foreach (string text4 in sourceList2)
				{
					flag3 = flag3 || text3 == text4;
				}
			}
			flag = flag3;
		}
		return flag;
	}

	public static bool CheckAllDifferent(SourcesHelper sourcesHelper1, SourcesHelper sourcesHelper2)
	{
		if (sourcesHelper1 == null)
		{
			throw new ArgumentNullException("sourcesHelper1");
		}
		if (sourcesHelper2 == null)
		{
			throw new ArgumentNullException("sourcesHelper2");
		}
		bool flag = false;
		string[] sourceList = sourcesHelper1.SourceList;
		foreach (string text in sourceList)
		{
			string[] sourceList2 = sourcesHelper2.SourceList;
			foreach (string text2 in sourceList2)
			{
				if (text == text2)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			sourceList = sourcesHelper1.GroupList;
			foreach (string text3 in sourceList)
			{
				string[] sourceList2 = sourcesHelper2.GroupList;
				foreach (string text4 in sourceList2)
				{
					if (text3 == text4)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		return !flag;
	}

	public SourcesHelper()
	{
		SourceList = Array.Empty<string>();
		GroupList = Array.Empty<string>();
		SiteSourceList = Array.Empty<string>();
	}

	public SourcesHelper(string[] sourceList, string[] groupList)
	{
		SourceList = sourceList;
		GroupList = groupList;
		SiteSourceList = Array.Empty<string>();
	}

	public SourcesHelper(string[] sourceList, string[] groupList, string[] siteSourceList)
	{
		SourceList = sourceList;
		GroupList = groupList;
		SiteSourceList = siteSourceList;
	}
}
