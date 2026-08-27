using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;

namespace VideoOS.Platform.Search.FilterValues;

public class RegionSelectionFilterValue : FilterValueBase
{
	public ICollection<RegionSelection> Selections { get; }

	public RegionSelectionFilterValue()
	{
		ObservableCollection<RegionSelection> observableCollection = new ObservableCollection<RegionSelection>();
		observableCollection.CollectionChanged += SelectionsOnCollectionChanged;
		Selections = observableCollection;
	}

	private void SelectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.Action == NotifyCollectionChangedAction.Add)
		{
			foreach (RegionSelection newItem in e.NewItems)
			{
				newItem.Changed += RegionSelectionOnChanged;
			}
		}
		if (e.Action == NotifyCollectionChangedAction.Remove)
		{
			foreach (RegionSelection oldItem in e.OldItems)
			{
				oldItem.Changed -= RegionSelectionOnChanged;
			}
		}
		FireChangedEvent();
	}

	private void RegionSelectionOnChanged(object sender, EventArgs e)
	{
		FireChangedEvent();
	}

	public override string FallbackDisplayValue()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0}", Selections.Count);
	}

	public override string Serialize()
	{
		XmlWriterSettings settings = new XmlWriterSettings
		{
			Indent = true,
			OmitXmlDeclaration = true
		};
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
		xmlWriter.WriteStartElement("RegionSelectionFilterValue");
		xmlWriter.WriteStartElement("RegionSelections");
		foreach (RegionSelection selection in Selections)
		{
			if (!(selection.Item?.FQID == null))
			{
				xmlWriter.WriteStartElement("RegionSelection");
				selection.Item.FQID.Serialize(xmlWriter);
				xmlWriter.WriteStartElement("Size");
				xmlWriter.WriteAttributeString("Width", selection.Size.Width.ToString(CultureInfo.InvariantCulture));
				xmlWriter.WriteAttributeString("Height", selection.Size.Height.ToString(CultureInfo.InvariantCulture));
				xmlWriter.WriteEndElement();
				xmlWriter.WriteElementString("Mask", selection.Mask);
				xmlWriter.WriteEndElement();
			}
		}
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.Flush();
		return stringWriter.ToString();
	}

	public override bool Deserialize(string data)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(data);
		foreach (XmlElement item in xmlDocument.SelectNodes("RegionSelectionFilterValue/RegionSelections/RegionSelection"))
		{
			try
			{
				RegionSelection regionSelection = new RegionSelection();
				regionSelection.Item = Configuration.Instance.GetItem(new FQID(item["FQID"].OuterXml));
				if (regionSelection.Item == null)
				{
					return false;
				}
				regionSelection.Mask = item["Mask"].InnerText;
				XmlAttributeCollection attributes = item["Size"].Attributes;
				int width = int.Parse(attributes["Width"].Value, CultureInfo.InvariantCulture);
				int height = int.Parse(attributes["Height"].Value, CultureInfo.InvariantCulture);
				regionSelection.Size = new Size(width, height);
				Selections.Add(regionSelection);
			}
			catch
			{
				return false;
			}
		}
		return true;
	}
}
