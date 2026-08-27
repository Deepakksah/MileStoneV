using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.XPath;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Drivers;

namespace VideoOS.Common.Integration;

public class ImmerVisionWrapper
{
	public const string GenericLensValue = "Generic";

	private bool _profilesLoaded;

	private ExternalPlugins _externalPlugins;

	private string _settings;

	private List<LensProfile> _lensProfiles = new List<LensProfile>();

	private static readonly Lazy<IReadOnlyList<ImmerVisionLensDescription>> _lenses = new Lazy<IReadOnlyList<ImmerVisionLensDescription>>(() => ImmerVisionLenses.Lenses);

	public ImmerVisionWrapper(ExternalPlugins externalPlugIns)
	{
		_externalPlugins = externalPlugIns;
	}

	public ImmerVisionWrapper(string plugInData)
	{
		_settings = plugInData;
	}

	public ReadOnlyCollection<LensProfile> LoadLensProfiles()
	{
		ReadOnlyCollection<LensProfile> result = new ReadOnlyCollection<LensProfile>(new List<LensProfile>());
		_lensProfiles.Clear();
		string empty = string.Empty;
		if (_externalPlugins != null)
		{
			if (!_externalPlugins.Contains(GlobalIDs.ImmerVisionPlugInGuid))
			{
				return result;
			}
			empty = _externalPlugins[GlobalIDs.ImmerVisionPlugInGuid].Data;
		}
		else
		{
			empty = _settings;
		}
		if (string.IsNullOrEmpty(empty))
		{
			return result;
		}
		using (StringReader textReader = new StringReader(empty))
		{
			XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator().SelectSingleNode("pluginconfiguration");
			if (xPathNavigator == null)
			{
				return result;
			}
			XPathNodeIterator xPathNodeIterator = xPathNavigator.SelectChildren("definition", string.Empty);
			if (xPathNodeIterator == null)
			{
				return result;
			}
			foreach (XPathNavigator item in xPathNodeIterator)
			{
				XPathNavigator xPathNavigator3 = item.SelectSingleNode("rpl");
				if (xPathNavigator3 != null)
				{
					string name = string.Empty;
					XPathNavigator xPathNavigator4 = item.SelectSingleNode("name");
					if (xPathNavigator4 != null)
					{
						name = xPathNavigator4.Value;
					}
					double result2 = -1.0;
					XPathNavigator xPathNavigator5 = item.SelectSingleNode("fovangle");
					if (xPathNavigator5 != null && !double.TryParse(xPathNavigator5.Value, out result2))
					{
						result2 = 180.0;
					}
					_lensProfiles.Add(new LensProfile(xPathNavigator3.Value, name, result2));
				}
			}
		}
		IReadOnlyCollection<LensProfile> readOnlyCollection = ImmerVisionLenses.MergeProfiles(_lenses.Value, _lensProfiles);
		_lensProfiles.Clear();
		foreach (LensProfile item2 in readOnlyCollection)
		{
			_lensProfiles.Add(item2);
		}
		_profilesLoaded = true;
		return new ReadOnlyCollection<LensProfile>(_lensProfiles);
	}

	public bool TryGetLensProfile(string registeredPanomorphLens, out LensProfile lensProfile)
	{
		if (!_profilesLoaded)
		{
			LoadLensProfiles();
		}
		lensProfile = _lensProfiles.Find((LensProfile l) => l.RegisteredPanomorphLensNumber.Equals(registeredPanomorphLens));
		if (lensProfile != null)
		{
			return true;
		}
		return false;
	}
}
