using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSTableColumnBase
{
	private static readonly Dictionary<string, string> _templates = new Dictionary<string, string>();

	public virtual bool IsSortable { get; set; } = true;

	public virtual DataGridLength Width { get; set; } = new DataGridLength(1.0, DataGridLengthUnitType.Star);

	public virtual double MinWidth { get; set; } = 60.0;

	public virtual double MaxWidth { get; set; } = -1.0;

	public virtual bool CanUserResize { get; set; } = true;

	public virtual string ItemSortPropertyName { get; set; } = string.Empty;

	public virtual HorizontalAlignment HorizontalHeaderAlignment { get; set; } = HorizontalAlignment.Stretch;

	public virtual HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;

	public virtual VideoOSTable.SortDirections PrimarySortDirection { get; set; }

	public abstract string FallbackItemSortPropertyName { get; }

	public abstract List<KeyValuePair<string, string>> GetHeaderTemplateReplacePairs();

	public abstract List<KeyValuePair<string, string>> GetCellTemplateReplacePairs();

	public DataTemplate GetHeaderTemplate()
	{
		return GetTemplate(GetType().Name + "HeaderTemplate", GetHeaderTemplateReplacePairs());
	}

	public DataTemplate GetCellTemplate()
	{
		return GetTemplate(GetType().Name + "CellTemplate", GetCellTemplateReplacePairs());
	}

	private static DataTemplate GetTemplate(string name, List<KeyValuePair<string, string>> templateReplacePairs)
	{
		if (!_templates.TryGetValue(name, out var value))
		{
			Stream stream = null;
			try
			{
				stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".UI.Controls.Tables.Internal.ColumnTemplates." + name + ".xaml");
				using StreamReader streamReader = new StreamReader(stream);
				stream = null;
				value = streamReader.ReadToEnd();
				_templates.Add(name, value);
			}
			finally
			{
				stream?.Dispose();
			}
		}
		foreach (KeyValuePair<string, string> templateReplacePair in templateReplacePairs)
		{
			value = value.Replace(templateReplacePair.Key, templateReplacePair.Value);
		}
		StringReader stringReader = null;
		try
		{
			stringReader = new StringReader(value);
			using XmlReader reader = XmlReader.Create(stringReader);
			stringReader = null;
			return (DataTemplate)XamlReader.Load(reader);
		}
		finally
		{
			stringReader?.Dispose();
		}
	}

	public VideoOSTableColumnBase()
	{
	}
}
