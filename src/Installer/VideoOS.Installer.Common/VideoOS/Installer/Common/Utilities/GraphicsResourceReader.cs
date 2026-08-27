using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public class GraphicsResourceReader
{
	public class GraphicsValues
	{
		public string Text { get; public set; }

		public string Caption { get; public set; }

		public string PictureName { get; public set; }
	}

	private const string PictureId = "Picture";

	private const string TextId = "Text";

	private const string CaptionId = "Caption";

	private const string DefaultSku = "DefaultSku";

	private readonly Dictionary<string, Dictionary<string, GraphicsValues>> _graphicsResources;

	private string _defaultSku;

	public int NumberOfSkus => _graphicsResources.Count;

	public GraphicsResourceReader(string resourcesFileName, string enUsGraphics)
	{
		bool flag = false;
		if (!File.Exists(resourcesFileName))
		{
			flag = true;
			resourcesFileName = enUsGraphics;
		}
		if (string.IsNullOrEmpty(resourcesFileName))
		{
			throw new ArgumentNullException("resourcesFileName");
		}
		if (!File.Exists(resourcesFileName))
		{
			throw new FileNotFoundException("GraphicsResourceReader: Specified resourcefile not found.", resourcesFileName);
		}
		if (!File.Exists(enUsGraphics))
		{
			LogManager.Write("GraphicsResourceReader: Specified en-US original version does not exist.", TraceEventType.Warning);
		}
		_graphicsResources = new Dictionary<string, Dictionary<string, GraphicsValues>>();
		ReadResources(enUsGraphics);
		if (!flag)
		{
			ReadResources(resourcesFileName);
		}
	}

	private void ReadResources(string resourcesFileName)
	{
		IDictionaryEnumerator enumerator = new ResXResourceReader(resourcesFileName).GetEnumerator();
		while (enumerator.MoveNext())
		{
			AddGraphics((string)enumerator.Key, (string)enumerator.Value);
		}
	}

	private void AddGraphics(string key, string value)
	{
		if (string.Compare(key, "DefaultSku", StringComparison.InvariantCultureIgnoreCase) == 0)
		{
			_defaultSku = value;
			return;
		}
		string[] array = key.Split('_');
		string key2 = array[0];
		string key3 = array[1];
		string text = array[2];
		if (!_graphicsResources.ContainsKey(key2))
		{
			_graphicsResources[key2] = new Dictionary<string, GraphicsValues>();
		}
		if (!_graphicsResources[key2].ContainsKey(key3))
		{
			_graphicsResources[key2][key3] = new GraphicsValues();
		}
		switch (text)
		{
		case "Picture":
			_graphicsResources[key2][key3].PictureName = value;
			break;
		case "Caption":
			_graphicsResources[key2][key3].Caption = value;
			break;
		case "Text":
			_graphicsResources[key2][key3].Text = value;
			break;
		}
	}

	public Dictionary<string, GraphicsValues> GetAllFromSku(string skuId)
	{
		if (!_graphicsResources.ContainsKey(skuId))
		{
			return GetAllFromDefault();
		}
		return _graphicsResources[skuId];
	}

	public Dictionary<string, GraphicsValues> GetAllFromDefault()
	{
		if (_graphicsResources.Count == 0)
		{
			return new Dictionary<string, GraphicsValues>();
		}
		if (_graphicsResources.ContainsKey(_defaultSku))
		{
			return _graphicsResources[_defaultSku];
		}
		return _graphicsResources.First().Value;
	}
}
