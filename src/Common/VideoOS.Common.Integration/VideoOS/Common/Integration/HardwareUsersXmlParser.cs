using System.Collections.Generic;
using System.Xml;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration;

public static class HardwareUsersXmlParser
{
	public static IList<HardwareUser> ParseXmlToList(string userListXml)
	{
		List<HardwareUser> list = new List<HardwareUser>();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(userListXml);
		foreach (XmlNode item in xmlDocument.SelectNodes("/UserList/User"))
		{
			if (item.Attributes["Username"] != null && item.Attributes["UserLevel"] != null && item.Attributes["UserLevelString"] != null)
			{
				int result;
				int? num = ((!int.TryParse(item.Attributes["UserLevel"].Value, out result)) ? ((int?)null) : new int?(result));
				string value = item.Attributes["Username"].Value;
				string value2 = item.Attributes["UserLevelString"].Value;
				list.Add(new HardwareUser
				{
					UserId = value,
					UserName = value,
					UserLevel = (HardwareUserLevel)(num ?? (-1)),
					UserLevelString = value2
				});
			}
		}
		return list;
	}
}
