using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.SharedComponent.Data.ViewLayout;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Common.SharedComponent;

public static class ViewHandling
{
	public enum SetViewGroupDataResultType
	{
		UnspecifiedError,
		Success,
		ReadOnlyError
	}

	public static readonly Guid _privateViewGroupXPCOId = new Guid("4BF8A8B0-54AB-4E82-994F-98D8E9584176");

	public static readonly int _maxNumberOfNestedGroupLevels = 12;

	public static Guid _smartClientApplicationId = Guid.Empty;

	public const string SmartClientIdAttributeName = "smartClientId";

	public static ViewData CreateViewDataFromViewLayout(string viewLayoutXml, string viewName)
	{
		if (string.IsNullOrEmpty(viewLayoutXml))
		{
			throw new SharedComponentException("The ViewLayoutXml cannot be null or empty");
		}
		if (string.IsNullOrEmpty(viewName))
		{
			throw new SharedComponentException("The ViewName cannot be null or empty");
		}
		try
		{
			XmlReader xmlReader = XmlReader.Create(new StringReader(viewLayoutXml));
			ViewLayout viewLayout = new XmlSerializer(typeof(ViewLayout)).Deserialize(xmlReader) as ViewLayout;
			ViewData viewData;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Encoding = Encoding.UTF8;
				xmlWriterSettings.OmitXmlDeclaration = true;
				using XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
				xmlWriter.WriteStartElement("view");
				xmlWriter.WriteAttributeString("id", Guid.NewGuid().ToString());
				xmlWriter.WriteAttributeString("displayname", viewName);
				xmlWriter.WriteAttributeString("shortcut", "");
				xmlWriter.WriteAttributeString("viewlayouttype", viewLayout.ViewLayoutType);
				xmlWriter.WriteStartElement("properties");
				xmlWriter.WriteStartElement("property");
				xmlWriter.WriteAttributeString("name", "LayoutProperty");
				xmlWriter.WriteStartAttribute("value");
				xmlWriter.WriteRaw(viewLayout.GetViewLayout());
				xmlWriter.WriteEndAttribute();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				viewData = ViewData.CreateFromXMLDefinition(new StreamReader(memoryStream, Encoding.UTF8).ReadToEnd());
			}
			int index = 0;
			foreach (ViewItem viewItem in viewLayout.ViewItems)
			{
				_ = viewItem;
				ViewItemData viewItemData = CreateViewItem(ViewItemType.Empty, Guid.NewGuid(), "Empty ViewItem", Guid.Empty, "");
				viewData.SetViewItemData(index, viewItemData.PartialXML);
			}
			return viewData;
		}
		catch (SharedComponentException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("Error while reading the viewLayoutxml", innerException);
		}
	}

	public static Bitmap GetViewLayoutIcon(string xml)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ViewLayout));
			StringReader stringReader = new StringReader(xml);
			XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
			ViewLayout obj = xmlSerializer.Deserialize(xmlTextReader) as ViewLayout;
			xmlTextReader.Close();
			stringReader.Close();
			return obj.Icon;
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("Error in GetViewLayoutIcon", innerException);
		}
	}

	public static ViewItemData CreateViewItem(ViewItemType viewItemType, Guid id, string name, Guid cameraId, string cameraName, string smartClientId = null)
	{
		return CreateViewItem(viewItemType, id, name, cameraId, cameraName, new ViewItemSettings(), smartClientId);
	}

	public static ViewItemData CreateViewItem(ViewItemType viewItemType, Guid id, string name, Guid cameraId, string cameraName, ViewItemSettings settings, string smartClientId = null)
	{
		ViewItemData viewItemData = null;
		if (viewItemType != ViewItemType.Camera && viewItemType != ViewItemType.Empty)
		{
			throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The {0} ViewItemType is not supported. Only use Camera or Empty ViewItemType", viewItemType.ToString()));
		}
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name", "The name has to be a valid string");
		}
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Encoding = Encoding.UTF8;
				xmlWriterSettings.OmitXmlDeclaration = true;
				using XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
				switch (viewItemType)
				{
				case ViewItemType.Camera:
					if (string.IsNullOrEmpty(cameraName))
					{
						throw new ArgumentException("CameraName cannot be null or empty when a CameraViewItem is created");
					}
					if (cameraId == Guid.Empty)
					{
						throw new ArgumentException("CameraId cannot be " + Guid.Empty.ToString() + ".");
					}
					xmlWriter.WriteStartElement("viewitem");
					xmlWriter.WriteAttributeString("id", id.ToString());
					xmlWriter.WriteAttributeString("displayname", name);
					xmlWriter.WriteAttributeString("shortcut", "");
					xmlWriter.WriteAttributeString("type", "VideoOS.RemoteClient.Application.Data.ContentTypes.CameraContentType.CameraViewItem, VideoOS.RemoteClient.Application");
					if (!string.IsNullOrEmpty(smartClientId))
					{
						xmlWriter.WriteAttributeString("smartClientId", smartClientId);
					}
					xmlWriter.WriteStartElement("iteminfo");
					xmlWriter.WriteAttributeString("cameraid", cameraId.ToString());
					xmlWriter.WriteAttributeString("lastknowncameradisplayname", cameraName);
					xmlWriter.WriteAttributeString("imagequality", settings.ImageQuality.ToString(CultureInfo.InvariantCulture));
					xmlWriter.WriteAttributeString("framerate", settings.FrameRate.ToString(CultureInfo.InvariantCulture));
					xmlWriter.WriteAttributeString("ipixsplitmode", settings.IpixSplitMode.ToString(CultureInfo.InvariantCulture));
					xmlWriter.WriteAttributeString("maintainimageaspectratio", settings.MaintainImageAspectRatio.ToString());
					xmlWriter.WriteAttributeString("keepimagequalitywhenmaximized", settings.KeepImageQualityWhenMaximized.ToString());
					xmlWriter.WriteAttributeString("updateonmotiononly", settings.UpdateOnMotionOnly.ToString());
					xmlWriter.WriteAttributeString("soundonmotion", settings.SoundOnMotion.ToString(CultureInfo.InvariantCulture));
					xmlWriter.WriteAttributeString("soundonevent", settings.SoundOnEvent.ToString(CultureInfo.InvariantCulture));
					xmlWriter.WriteAttributeString("smartsearchgridwidth", settings.SmartSearchGridWidth.ToString(CultureInfo.InvariantCulture));
					xmlWriter.WriteAttributeString("smartsearchgridheight", settings.SmartSearchGridHeight.ToString(CultureInfo.InvariantCulture));
					xmlWriter.WriteAttributeString("smartsearchgridmask", settings.SmartSearchGridMask);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("properties");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					break;
				case ViewItemType.Empty:
					xmlWriter.WriteStartElement("viewitem");
					xmlWriter.WriteAttributeString("id", id.ToString());
					xmlWriter.WriteAttributeString("displayname", name);
					xmlWriter.WriteAttributeString("shortcut", "");
					xmlWriter.WriteAttributeString("type", "VideoOS.RemoteClient.Application.Data.Configuration.EmptyViewItem, VideoOS.RemoteClient.Application");
					if (!string.IsNullOrEmpty(smartClientId))
					{
						xmlWriter.WriteAttributeString("smartClientId", smartClientId);
					}
					xmlWriter.WriteStartElement("properties");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
					break;
				default:
					throw new SharedComponentException("The ViewItem " + viewItemType.ToString() + " is not known by the system.");
				}
				xmlWriter.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				viewItemData = ViewItemData.CreateFromXMLDefinition(new StreamReader(memoryStream, Encoding.UTF8).ReadToEnd());
			}
			viewItemData.Id = id;
			return viewItemData;
		}
		catch (SharedComponentException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("Error while creating a viewitem", innerException);
		}
	}

	public static bool GetViewItemInformation(string partialXML, out ViewItemType viewItemType, out Guid id, out Guid cameraId)
	{
		viewItemType = ViewItemType.Unknown;
		id = Guid.Empty;
		cameraId = Guid.Empty;
		try
		{
			ViewItemData viewItemData = ViewItemData.CreateFromXMLDefinition(partialXML);
			if (viewItemData != null && viewItemData.Id != Guid.Empty)
			{
				id = viewItemData.Id;
				viewItemType = viewItemData.ViewItemType;
				switch (viewItemType)
				{
				case ViewItemType.Camera:
					if (viewItemData.ViewItemInformation is CameraViewItemInformation cameraViewItemInformation)
					{
						cameraId = cameraViewItemInformation.CameraId;
						return true;
					}
					break;
				case ViewItemType.Unknown:
				case ViewItemType.Empty:
					return true;
				}
				return false;
			}
		}
		catch (XmlException)
		{
			viewItemType = ViewItemType.Unknown;
			id = Guid.Empty;
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("Could not get ViewItemInformation", innerException);
		}
		return false;
	}

	private static ViewItemInfo GetFirstEmptyViewItem(ViewData defaultView)
	{
		foreach (ViewItemInfo item in defaultView.ViewItemsInfo)
		{
			if (item.ViewItemData.ViewItemType == ViewItemType.Empty)
			{
				return item;
			}
		}
		return null;
	}

	private static int GetNumberOfEmptyItems(ViewData defaultView)
	{
		int num = 0;
		foreach (ViewItemInfo item in defaultView.ViewItemsInfo)
		{
			if (item.ViewItemData.ViewItemType == ViewItemType.Empty)
			{
				num++;
			}
		}
		return num;
	}

	public static string GetViewGroupData(IViewGroup serverViewGroup)
	{
		return GetViewGroupData(serverViewGroup, new List<Guid>());
	}

	public static string GetViewGroupData(IViewGroup serverViewGroup, List<Guid> idOnObjectsToUpdate)
	{
		try
		{
			string text = GetCustomProperty(serverViewGroup.CustomProperties);
			if (string.IsNullOrEmpty(text))
			{
				text = "<viewgroup xmlns=\"\"><properties /></viewgroup>";
			}
			ViewGroupData viewGroupData = ViewGroupData.CreateFromXMLDefinition(text);
			ReadViewGroups(serverViewGroup.ViewGroups, viewGroupData, idOnObjectsToUpdate);
			return viewGroupData.GetFullXML();
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("An error occured in GetViewGroupData", innerException);
		}
	}

	private static void ReadViewGroups(IViewGroups viewGroups, ViewGroupData viewGroupData, List<Guid> idOnObjectsToUpdate)
	{
		foreach (IViewGroup viewGroup in viewGroups)
		{
			string text = GetCustomProperty(viewGroup.CustomProperties);
			if (string.IsNullOrWhiteSpace(text))
			{
				text = CreateGroupXml(viewGroup.Id, viewGroup.Name);
			}
			GroupData groupData = viewGroupData.AddGroupData(text);
			groupData.Id = viewGroup.Id;
			ReadGroups(viewGroup.ViewGroups, groupData, idOnObjectsToUpdate);
			ReadViews(viewGroup.Views, groupData, idOnObjectsToUpdate);
		}
	}

	private static string CreateGroupXml(Guid viewGroupId, string viewGroupName)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Encoding = Encoding.UTF8;
		xmlWriterSettings.OmitXmlDeclaration = true;
		using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
		{
			xmlWriter.WriteStartElement("group");
			xmlWriter.WriteAttributeString("id", viewGroupId.ToString());
			xmlWriter.WriteAttributeString("displayname", viewGroupName);
			xmlWriter.WriteAttributeString("shortcut", "");
			xmlWriter.WriteStartElement("properties");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
		}
		using StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8);
		return streamReader.ReadToEnd();
	}

	private static void ReadGroups(IViewGroups viewGroups, GroupData viewGroupData, List<Guid> idOnObjectsToUpdate)
	{
		Parallel.ForEach(viewGroups, delegate(IViewGroup viewGroup)
		{
			string text = GetCustomProperty(viewGroup.CustomProperties);
			if (string.IsNullOrWhiteSpace(text))
			{
				text = string.Format(CultureInfo.InvariantCulture, "<group id=\"{0}\" displayname=\"{1}\" shortcut=\"\"><properties /></group>", viewGroup.Id, viewGroup.Name);
			}
			GroupData groupData = viewGroupData.AddGroupData(text);
			groupData.Id = viewGroup.Id;
			ReadGroups(viewGroup.ViewGroups, groupData, idOnObjectsToUpdate);
			ReadViews(viewGroup.Views, groupData, idOnObjectsToUpdate);
		});
	}

	private static void ReadViews(IViews views, GroupData group, List<Guid> idOnObjectsToUpdate)
	{
		foreach (IView view in views)
		{
			ViewData viewData = group.AddViewData(view.DefinitionXml);
			viewData.Id = view.Id;
			int num = 0;
			foreach (IViewItem viewItem in view.ViewItems)
			{
				viewData.SetViewItemData(num++, viewItem.DefinitionXml).Id = viewItem.Id;
			}
		}
	}

	public static string GetUniqueViewGroupName(string name, IViewGroups viewGroups)
	{
		int num = 2;
		string text = name;
		while (viewGroups.Contains(text))
		{
			if (num == int.MaxValue)
			{
				throw new SharedComponentException("Cannot create a unique name for " + name + ". InstallViewGroupOnVMO failed.");
			}
			text = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", name, num++);
		}
		return text;
	}

	private static string GetUniqueViewName(string name, IViews views)
	{
		int num = 2;
		string text = name;
		while (views.Contains(text))
		{
			if (num == int.MaxValue)
			{
				throw new SharedComponentException("Cannot create a unique name for " + name + ". InstallViewGroupOnVMO failed.");
			}
			text = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", name, num++);
		}
		return text;
	}

	public static SetViewGroupDataResultType SetViewGroupData(Guid viewGroupId, ViewGroupData newViewGroupdata, IViewGroups viewGroups)
	{
		try
		{
			int num = 1;
			IViewGroup viewGroup = null;
			try
			{
				viewGroup = viewGroups[viewGroupId];
			}
			catch
			{
			}
			if (viewGroup == null)
			{
				viewGroup = viewGroups[_privateViewGroupXPCOId];
				if (viewGroup == null)
				{
					return SetViewGroupDataResultType.UnspecifiedError;
				}
				viewGroup = viewGroup.ViewGroups[viewGroupId];
				if (viewGroup == null)
				{
					return SetViewGroupDataResultType.UnspecifiedError;
				}
				num++;
			}
			if (viewGroup.CustomProperties.Contains(VMOConstants.CustomPropertyXmlDefinition))
			{
				if (viewGroup.CustomProperties[VMOConstants.CustomPropertyXmlDefinition].Value != newViewGroupdata.PartialXML)
				{
					viewGroup.CustomProperties[VMOConstants.CustomPropertyXmlDefinition].Value = newViewGroupdata.PartialXML;
					viewGroup.CustomProperties[VMOConstants.CustomPropertyXmlDefinition].Update();
				}
			}
			else
			{
				viewGroup.NewCustomProperty(VMOConstants.CustomPropertyXmlDefinition, newViewGroupdata.PartialXML).Create();
			}
			Queue<IViewGroup> groupsToBeDeleted = new Queue<IViewGroup>();
			List<IView> viewsToBeDeleted = new List<IView>();
			List<ICustomProperty> customPropertiesToBeDeleted = new List<ICustomProperty>();
			CreateListOfItemsToBeDeleted(newViewGroupdata.Groups, viewGroup, ref groupsToBeDeleted, ref viewsToBeDeleted, ref customPropertiesToBeDeleted);
			DeleteItems(groupsToBeDeleted, viewsToBeDeleted, customPropertiesToBeDeleted);
			SaveOrUpdateGroups(newViewGroupdata.Groups, viewGroup, uniqueNames: false, num);
			return SetViewGroupDataResultType.Success;
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("An error occured in SetViewGroupData", innerException);
		}
	}

	private static void DeleteItems(Queue<IViewGroup> groupsToBeDeleted, List<IView> viewsToBeDeleted, List<ICustomProperty> customPropertiesToBeDeleted)
	{
		foreach (ICustomProperty item in customPropertiesToBeDeleted)
		{
			item.Delete();
		}
		foreach (IView item2 in viewsToBeDeleted)
		{
			item2.Delete();
		}
		foreach (IViewGroup item3 in groupsToBeDeleted)
		{
			item3.Delete();
		}
	}

	private static void CreateListOfItemsToBeDeleted(IList<GroupData> groupsData, IViewGroup viewGroup, ref Queue<IViewGroup> groupsToBeDeleted, ref List<IView> viewsToBeDeleted, ref List<ICustomProperty> customPropertiesToBeDeleted)
	{
		foreach (IViewGroup viewGroup2 in viewGroup.ViewGroups)
		{
			GroupData groupData = GetGroupData(groupsData, viewGroup2.Id);
			if (groupData == null)
			{
				foreach (ICustomProperty customProperty in viewGroup2.CustomProperties)
				{
					customPropertiesToBeDeleted.Add(customProperty);
				}
				DeleteAllSubItemsInViewGroup(viewGroup2, ref groupsToBeDeleted, ref viewsToBeDeleted);
			}
			else
			{
				CreateListOfItemsToBeDeleted(groupData.Groups, viewGroup2, ref groupsToBeDeleted, ref viewsToBeDeleted, ref customPropertiesToBeDeleted);
				AddViewsToDeletedItems(groupData.Views, viewGroup2, ref viewsToBeDeleted);
			}
		}
	}

	private static void DeleteAllSubItemsInViewGroup(IViewGroup viewGroupToBeDeleted, ref Queue<IViewGroup> groupsToBedeleted, ref List<IView> viewsToBeDeleted)
	{
		foreach (IViewGroup viewGroup in viewGroupToBeDeleted.ViewGroups)
		{
			DeleteAllSubItemsInViewGroup(viewGroup, ref groupsToBedeleted, ref viewsToBeDeleted);
		}
		foreach (IView view in viewGroupToBeDeleted.Views)
		{
			viewsToBeDeleted.Add(view);
		}
		groupsToBedeleted.Enqueue(viewGroupToBeDeleted);
	}

	private static void AddViewsToDeletedItems(IList<ViewData> viewsData, IViewGroup group, ref List<IView> viewsToBeDeleted)
	{
		foreach (IView view in group.Views)
		{
			if (GetViewData(viewsData, view.Id) == null)
			{
				viewsToBeDeleted.Add(view);
			}
		}
	}

	private static ViewData GetViewData(IList<ViewData> viewsData, Guid viewId)
	{
		ViewData result = null;
		foreach (ViewData viewsDatum in viewsData)
		{
			if (viewsDatum.Id == viewId)
			{
				result = viewsDatum;
				break;
			}
		}
		return result;
	}

	private static GroupData GetGroupData(IList<GroupData> data, Guid viewGroupId)
	{
		GroupData result = null;
		foreach (GroupData datum in data)
		{
			if (datum.Id == viewGroupId)
			{
				result = datum;
				break;
			}
		}
		return result;
	}

	public static void SaveOrUpdateGroups(IList<GroupData> groupsData, IViewGroup viewGroup, bool uniqueNames, int level)
	{
		if (groupsData == null)
		{
			return;
		}
		foreach (GroupData groupsDatum in groupsData)
		{
			groupsDatum.SmartClientId = _smartClientApplicationId;
			IViewGroup viewGroup2 = null;
			if (level >= _maxNumberOfNestedGroupLevels - 1)
			{
				viewGroup2 = viewGroup;
			}
			else
			{
				try
				{
					viewGroup2 = viewGroup.ViewGroups[groupsDatum.Id];
				}
				catch
				{
				}
			}
			if (viewGroup2 == null)
			{
				string name = groupsDatum.Name;
				if (uniqueNames)
				{
					name = GetUniqueViewGroupName(name, viewGroup.ViewGroups);
				}
				groupsDatum.Name = name;
				viewGroup2 = viewGroup.NewViewGroup(name, groupsDatum.Id);
				viewGroup2.Create();
				viewGroup2.NewCustomProperty(VMOConstants.CustomPropertyXmlDefinition, groupsDatum.PartialXML).Create();
				SaveOrUpdateViews(groupsDatum.Views, viewGroup2, uniqueNames);
			}
			else
			{
				bool flag = false;
				if (level < _maxNumberOfNestedGroupLevels - 1)
				{
					if (viewGroup2.CustomProperties.Contains(VMOConstants.CustomPropertyXmlDefinition))
					{
						if (viewGroup2.CustomProperties[VMOConstants.CustomPropertyXmlDefinition].Value != groupsDatum.PartialXML)
						{
							viewGroup2.CustomProperties[VMOConstants.CustomPropertyXmlDefinition].Value = groupsDatum.PartialXML;
							viewGroup2.CustomProperties[VMOConstants.CustomPropertyXmlDefinition].Update();
						}
					}
					else
					{
						viewGroup2.NewCustomProperty(VMOConstants.CustomPropertyXmlDefinition, groupsDatum.PartialXML).Create();
					}
					if (viewGroup2.Name != groupsDatum.Name)
					{
						viewGroup2.Name = groupsDatum.Name;
						flag = true;
					}
				}
				SaveOrUpdateViews(groupsDatum.Views, viewGroup2, uniqueNames);
				if (flag)
				{
					viewGroup2.Update();
				}
			}
			if (groupsDatum.Groups != null)
			{
				SaveOrUpdateGroups(groupsDatum.Groups, viewGroup2, uniqueNames, level + 1);
			}
		}
	}

	private static void SaveOrUpdateViews(IList<ViewData> viewsData, IViewGroup group, bool uniqueNames)
	{
		if (viewsData == null)
		{
			return;
		}
		foreach (ViewData viewsDatum in viewsData)
		{
			IView view = null;
			try
			{
				view = group.Views[viewsDatum.Id];
			}
			catch
			{
			}
			if (view == null)
			{
				string name = viewsDatum.Name;
				if (uniqueNames)
				{
					name = GetUniqueViewName(name, group.Views);
				}
				viewsDatum.Name = name;
				IView view2 = group.NewView(name, viewsDatum.Id);
				view2.DefinitionXml = viewsDatum.PartialXML;
				int num = 0;
				foreach (ViewItemInfo item in viewsDatum.ViewItemsInfo)
				{
					view2.AddViewItem(num++, item.ViewItemData.Id, item.ViewItemData.PartialXML);
				}
				view2.Create();
			}
			else
			{
				bool flag = false;
				if (view.DefinitionXml != viewsDatum.PartialXML)
				{
					view.DefinitionXml = viewsDatum.PartialXML;
					flag = true;
				}
				if (view.Name != viewsDatum.Name)
				{
					view.Name = viewsDatum.Name;
					flag = true;
				}
				UpdateViewItems(viewsDatum.ViewItemsInfo, view);
				if (flag)
				{
					view.Update();
				}
			}
		}
	}

	private static void UpdateViewItems(IList<ViewItemInfo> viewItemsInfo, IView existingView)
	{
		if (viewItemsInfo == null)
		{
			return;
		}
		foreach (IViewItem viewItem in existingView.ViewItems)
		{
			int position = viewItem.Position;
			if (position >= viewItemsInfo.Count)
			{
				throw new SharedComponentException("The position number of a viewitem in VMO is greather than the viewitem count in the view.");
			}
			ViewItemInfo viewItemInfo = viewItemsInfo[position];
			if (viewItemInfo == null)
			{
				continue;
			}
			ViewItemData viewItemData = viewItemInfo.ViewItemData;
			if (viewItemData == null)
			{
				continue;
			}
			if (viewItemData.Id != viewItem.Id)
			{
				viewItem.ChangeId(viewItemData.Id);
				viewItemData.SmartClientId = _smartClientApplicationId;
				viewItem.DefinitionXml = viewItemData.PartialXML;
				viewItem.Update();
				continue;
			}
			string text = viewItem.DefinitionXml;
			if (!string.IsNullOrEmpty(text))
			{
				int num = text.IndexOf("smartClientId");
				if (num > 0)
				{
					text = text.Remove(num - 1, 53);
				}
			}
			if (viewItemData.PartialXML != text)
			{
				viewItemData.SmartClientId = _smartClientApplicationId;
				viewItem.DefinitionXml = viewItemData.PartialXML;
				viewItem.Update();
			}
		}
	}

	private static string GetCustomProperty(ICustomProperties properties)
	{
		if (properties.Contains(VMOConstants.CustomPropertyXmlDefinition))
		{
			return properties[VMOConstants.CustomPropertyXmlDefinition].Value;
		}
		return "";
	}
}
