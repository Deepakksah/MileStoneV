#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi;

namespace VideoOS.Management.Server;

public class ViewLayout
{
	private const string DefaultLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayoutCustom, VideoOS.RemoteClient.Application";

	private static HashSet<Guid> BuiltInLayout = new HashSet<Guid>
	{
		new Guid("e4391ae5-7d96-4a0f-835a-9019a73f269a"),
		new Guid("da344fbc-2607-47aa-9660-c562f536f48a"),
		new Guid("04515830-c0fe-4057-b7d2-da0faa5ac308"),
		new Guid("dada8bbb-a82b-4719-9afd-23397c5795ba"),
		new Guid("864234dc-8bcc-4f4a-8579-7856804f8655"),
		new Guid("1a2d8100-9de2-4789-b758-a3f80732526e"),
		new Guid("4c1c5058-4543-4053-a225-aecfef6a3cc4"),
		new Guid("c807a2c1-ed3a-413b-9a55-f1261f152bdf"),
		new Guid("8f62b5ac-6036-4115-aec2-1de3f9708a5a"),
		new Guid("5de4886b-6bb9-42d9-ac32-8575db337764"),
		new Guid("b7c02ce1-5f66-4886-b0c8-018224a59621"),
		new Guid("1707f761-7c59-42c7-94cd-d5a6dd140f6e"),
		new Guid("00fb21c4-26f5-49a3-922e-0b8608500ce8"),
		new Guid("4d344d3b-3a8e-425e-b72d-711279ba6037"),
		new Guid("095e5cc8-b81b-46ae-8ba2-31565f13cd56"),
		new Guid("9677f4ff-2ccf-450f-8e55-7caaad2a2f04"),
		new Guid("6f499fd2-fddc-4f55-820a-47eaae4f853a"),
		new Guid("b564df2c-9906-4c0c-bbb9-bd6a599a38c0"),
		new Guid("ce64ddc3-d27a-42be-8465-ed780e748e49"),
		new Guid("64ced8ef-4000-4a9c-aee9-f1f8a2b64ed4"),
		new Guid("aaaef1e1-f157-4132-adf7-fba45fac722c"),
		new Guid("6d39cf5d-cb2e-493f-8c2b-3a25857c14e3"),
		new Guid("d6c1bc0e-9319-4cb4-98d7-80d953ccf6b7"),
		new Guid("bdda290a-8413-4b99-b266-b7db2263a3fe"),
		new Guid("a8b0ec85-5f87-489b-adf2-296185bfadd6"),
		new Guid("edb95d6d-a010-4450-b60a-9a0c043c3e9a"),
		new Guid("eb69a2fe-78c0-4d3d-bf02-1c0109fc2265"),
		new Guid("05ecb5e7-564a-4192-92b8-2ba99f89081c"),
		new Guid("240578d7-6a67-46dc-8d88-09aaedd65378"),
		new Guid("85500509-2959-42c2-8559-b31d40b59b2b"),
		new Guid("2ade8e95-31bc-4b3d-b73a-41606b1c2b52"),
		new Guid("8f51eee7-89b0-4180-8662-d5b7484ce756"),
		new Guid("f4f41dc1-f987-4d74-9246-2d70634d25b0"),
		new Guid("9c9559e1-72a6-4cd0-80ce-e966b1d8ba84"),
		new Guid("6d299029-ab1e-4d04-9fd5-0b13fa265b90"),
		new Guid("82a7ece4-9dcf-4ab5-8407-05dc420826d5"),
		new Guid("9d182f63-aaa8-4225-8062-03b6132bd0e0"),
		new Guid("61d83f34-0780-4670-957f-aca4e94d4b29"),
		new Guid("9ceb86ba-84f9-4975-889d-f6691c0b3f14"),
		new Guid("b26008f8-fd36-4952-b769-c90edd034e6c"),
		new Guid("50a202cf-9ce1-4ec0-a09d-036cd8c80511")
	};

	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string DefinitionXml { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public string ViewLayoutGroupId { get; set; }

	public static ViewLayout QueryViewLayoutById(IGeminiRequestContext requestContext, string id)
	{
		ViewLayout viewLayout = null;
		using IViewLayoutComponent viewLayoutComponent = requestContext.CreateViewLayoutComponent();
		return viewLayoutComponent.QueryViewLayout(id);
	}

	public static ViewLayout[] QueryViewLayoutByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		ViewLayout[] array = new ViewLayout[0];
		using IViewLayoutComponent viewLayoutComponent = requestContext.CreateViewLayoutComponent();
		return viewLayoutComponent.QueryViewLayoutsByIds(ids);
	}

	public static string[] QueryViewLayoutIdsByViewGroupId(IGeminiRequestContext requestContext, string viewgroupId)
	{
		string[] array = new string[0];
		using IViewLayoutComponent viewLayoutComponent = requestContext.CreateViewLayoutComponent();
		return viewLayoutComponent.QueryViewLayoutIds(viewgroupId);
	}

	public static void CreateViewLayout(IGeminiRequestContext requestContext, string id, string viewLayoutGroupId, string name, string description, string definitionXml)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using IViewLayoutComponent viewLayoutComponent = requestContext.CreateViewLayoutComponent();
		viewLayoutComponent.CreateViewLayout(id, viewLayoutGroupId, name, description, definitionXml);
	}

	public static void UpdateViewLayout(IGeminiRequestContext requestContext, string id, string name, string description, string definitionXml)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		if (IsBuiltinLayout(id))
		{
			throw new BuiltInViewLayoutCannotBeChangedException();
		}
		using IViewLayoutComponent viewLayoutComponent = requestContext.CreateViewLayoutComponent();
		viewLayoutComponent.UpdateViewLayout(id, name, description, definitionXml);
	}

	public static void DeleteViewLayout(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using IViewLayoutComponent viewLayoutComponent = requestContext.CreateViewLayoutComponent();
		viewLayoutComponent.DeleteViewLayout(id);
	}

	public static void UpdateLayoutValidate(string name, ref string description, ref string definitionXml, string viewGroupId, string id)
	{
		Validation.CheckName(name, "Name", allowNull: true);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
		CheckDefinitionXml(ref definitionXml, viewGroupId, id);
	}

	public static void CheckLayoutIdForChange(string id)
	{
		if (IsBuiltinLayout(id))
		{
			throw new BuiltInViewLayoutCannotBeChangedException();
		}
	}

	public static bool IsBuiltinLayout(string layoutGuid)
	{
		if (Guid.TryParse(layoutGuid, out var result) && BuiltInLayout.Contains(result))
		{
			return true;
		}
		return false;
	}

	private static void CheckDefinitionXml(ref string definitionXml, string viewGroupId, string id)
	{
		bool flag = false;
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(definitionXml);
			XmlNode xmlNode = xmlDocument["ViewLayout"];
			if (xmlNode != null)
			{
				CheckAttributes(xmlDocument, xmlNode, viewGroupId, id);
				XmlNode xmlNode2 = xmlNode["ViewLayoutIcon"];
				if (xmlNode2 != null)
				{
					try
					{
						Convert.FromBase64String(xmlNode2.InnerText);
					}
					catch (FormatException)
					{
						throw new PropertyInvalidException("Invalid format, ViewLayoutIcon value is not base64", "definitionXml");
					}
				}
				XmlNode xmlNode3 = xmlNode.SelectSingleNode("ViewItems");
				if (xmlNode3 != null)
				{
					foreach (XmlNode item in xmlNode3.SelectNodes("ViewItem"))
					{
						XmlNode xmlNode4 = item["Position"];
						if (xmlNode4 == null)
						{
							throw new PropertyInvalidException("Invalid format, position missing", "definitionXml");
						}
						if (xmlNode4["X"] == null || xmlNode4["Y"] == null)
						{
							throw new PropertyInvalidException("Invalid format, position.X or .Y missing", "definitionXml");
						}
						XmlNode xmlNode5 = item["Size"];
						if (xmlNode5 == null)
						{
							throw new PropertyInvalidException("Invalid format, size missing", "definitionXml");
						}
						if (xmlNode5["Width"] == null || xmlNode5["Height"] == null)
						{
							throw new PropertyInvalidException("Invalid format, size.Width or .Height missing", "definitionXml");
						}
					}
					flag = true;
					definitionXml = xmlDocument.OuterXml;
				}
			}
		}
		catch (XmlException ex2)
		{
			Trace.WriteLine("Layout definitionXml Invalid: " + ex2.Message);
			flag = false;
		}
		if (!flag)
		{
			throw new ArgumentException("Invalid format", "definitionXml");
		}
	}

	private static void CheckAttributes(XmlDocument doc, XmlNode node, string viewGroupId, string id)
	{
		XmlAttributeCollection obj = node.Attributes ?? throw new ArgumentException("Invalid format, no attributes", "definitionXml");
		XmlAttribute xmlAttribute = obj["ViewLayoutType"];
		if (xmlAttribute == null)
		{
			xmlAttribute = doc.CreateAttribute("ViewLayoutType");
			xmlAttribute.Value = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayoutCustom, VideoOS.RemoteClient.Application";
			node.Attributes.Append(xmlAttribute);
		}
		xmlAttribute = obj["Id"];
		if (xmlAttribute == null)
		{
			xmlAttribute = doc.CreateAttribute("Id");
			xmlAttribute.Value = id;
			node.Attributes.Append(xmlAttribute);
		}
		else if (string.Compare(xmlAttribute.Value, id, StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw new PropertyInvalidException("Invalid format, ID has invalid value", "definitionXml");
		}
		xmlAttribute = obj["ViewLayoutGroupId"];
		if (xmlAttribute == null)
		{
			xmlAttribute = doc.CreateAttribute("ViewLayoutGroupId");
			xmlAttribute.Value = viewGroupId;
			node.Attributes.Append(xmlAttribute);
		}
		else if (string.Compare(xmlAttribute.Value, viewGroupId, StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw new PropertyInvalidException("Invalid format, ViewLayoutGroupId has invalid value", "definitionXml");
		}
	}
}
