using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration;

public class StreamsConfiguration : IEnumerable<StreamInfo>, IEnumerable
{
	private const string STREAMS = "streams";

	private const string STREAM = "stream";

	private const string REFERENCEID = "referenceid";

	private const string HWID = "hwid";

	private const string NAME = "name";

	private const string LIVEFEED = "livefeed";

	private const string LIVEDEFAULT = "livedefault";

	private const string LIVERUNALWAYS = "liverunalways";

	private const string TRACKDEFINITION = "trackdefinition";

	private List<StreamInfo> list = new List<StreamInfo>();

	public int Count => list.Count;

	public StreamsConfiguration()
	{
	}

	public StreamsConfiguration(StreamsConfiguration streams)
	{
		if (streams == null)
		{
			throw new ArgumentNullException("streams");
		}
		foreach (StreamInfo stream in streams)
		{
			Add(new StreamInfo(stream));
		}
	}

	public static StreamsConfiguration FromXml(string streamsConfigurationXml)
	{
		StreamsConfiguration streamsConfiguration = new StreamsConfiguration();
		streamsConfiguration.LoadXml(streamsConfigurationXml);
		return streamsConfiguration;
	}

	public void AddRange(IEnumerable<StreamInfo> streams)
	{
		list.AddRange(streams);
	}

	public void Add(StreamInfo stream)
	{
		list.Add(stream);
	}

	public bool Remove(StreamInfo stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		return list.Remove(stream);
	}

	public bool Remove(Guid streamId)
	{
		StreamInfo streamInfo = list.FirstOrDefault((StreamInfo s) => s.StreamReferenceId == streamId);
		if (streamInfo == null)
		{
			return false;
		}
		return list.Remove(streamInfo);
	}

	public void Clear()
	{
		list.Clear();
	}

	public bool TryGet(StreamHwid hwid, out StreamInfo stream)
	{
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		stream = this.FirstOrDefault((StreamInfo si) => si.Hwid == hwid);
		return stream != null;
	}

	public bool TryGetByReferenceId(Guid referenceId, out StreamInfo stream)
	{
		stream = this.FirstOrDefault((StreamInfo si) => si.StreamReferenceId == referenceId);
		return stream != null;
	}

	public void LoadXml(string streamInfoXml)
	{
		Clear();
		if (string.IsNullOrEmpty(streamInfoXml))
		{
			return;
		}
		using StringReader input = new StringReader(streamInfoXml);
		using XmlTextReader xmlTextReader = new XmlTextReader(input);
		StreamInfo streamInfo = null;
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element)
			{
				switch (xmlTextReader.Name)
				{
				case "stream":
					streamInfo = new StreamInfo();
					Add(streamInfo);
					break;
				case "referenceid":
					streamInfo.StreamReferenceId = new Guid(xmlTextReader.ReadString());
					break;
				case "hwid":
					streamInfo.Hwid = new StreamHwid(xmlTextReader.ReadString());
					break;
				case "name":
					streamInfo.Name = xmlTextReader.ReadString();
					break;
				case "livefeed":
					streamInfo.LiveFeed = bool.Parse(xmlTextReader.ReadString());
					break;
				case "livedefault":
					streamInfo.LiveDefault = bool.Parse(xmlTextReader.ReadString());
					break;
				case "liverunalways":
					streamInfo.LiveRunAlways = bool.Parse(xmlTextReader.ReadString());
					break;
				case "trackdefinition":
				{
					string text = xmlTextReader.ReadString();
					streamInfo.TrackDefinition = (string.IsNullOrWhiteSpace(text) ? ((Guid?)null) : new Guid?(new Guid(text)));
					break;
				}
				}
			}
		}
		xmlTextReader.Close();
	}

	public string SaveXml()
	{
		return SaveXml(list);
	}

	public string SaveXml(IComparer<StreamInfo> comparer)
	{
		return SaveXml(list.OrderBy((StreamInfo s) => s, comparer));
	}

	private static string SaveXml(IEnumerable<StreamInfo> items)
	{
		string result = string.Empty;
		if (items.Count() > 0)
		{
			using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteStartElement("streams");
			foreach (StreamInfo item in items)
			{
				xmlTextWriter.WriteStartElement("stream");
				xmlTextWriter.WriteElementString("referenceid", item.StreamReferenceId.ToString());
				xmlTextWriter.WriteElementString("hwid", item.Hwid.Text);
				xmlTextWriter.WriteElementString("name", item.Name);
				xmlTextWriter.WriteElementString("livefeed", item.LiveFeed.ToString());
				xmlTextWriter.WriteElementString("livedefault", item.LiveDefault.ToString());
				xmlTextWriter.WriteElementString("liverunalways", item.LiveRunAlways.ToString());
				xmlTextWriter.WriteElementString("trackdefinition", item.TrackDefinition.HasValue ? item.TrackDefinition.Value.ToString() : "");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			result = stringWriter.ToString();
			xmlTextWriter.Close();
		}
		return result;
	}

	public StreamValidationResult ValidateConfiguration(DeviceDefinition deviceDefinition, IEnumerable<TrackConfiguration> tracks)
	{
		if (deviceDefinition == null)
		{
			throw new ArgumentNullException("deviceDefinition");
		}
		StreamValidationResult streamValidationResult = new StreamValidationResult();
		streamValidationResult.EmptyStreamNameError = this.Any((StreamInfo s) => string.IsNullOrEmpty(s.Name));
		HashSet<Guid> hashSet = new HashSet<Guid>();
		HashSet<string> hashSet2 = new HashSet<string>();
		using (IEnumerator<StreamInfo> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				StreamInfo current = enumerator.Current;
				hashSet.Add(current.StreamReferenceId);
				hashSet2.Add(current.Name);
			}
		}
		streamValidationResult.SameStreamsError = hashSet.Count < Count;
		streamValidationResult.SameStreamNamesError = hashSet2.Count < Count;
		streamValidationResult.EmptySelectionError = this.Any((StreamInfo s) => !s.LiveFeed && !s.TrackDefinition.HasValue);
		streamValidationResult.OnlyOneDefaultLiveStreamError = this.Count((StreamInfo s) => s.LiveFeed && s.LiveDefault) > 1;
		streamValidationResult.MustHaveOneDefaultLiveStreamError = this.Count((StreamInfo s) => s.LiveFeed && s.LiveDefault) == 0;
		streamValidationResult.OnlyOneDefaultPlaybackStreamError = tracks.Count((TrackConfiguration t) => t.DefaultPlayback) > 1;
		streamValidationResult.MustHaveOneDefaultPlaybackStreamError = tracks.Count((TrackConfiguration t) => t.DefaultPlayback) == 0;
		streamValidationResult.OnlyOnePrimaryTrackStreamError = this.Count((StreamInfo s) => s.IsPrimaryTrack) > 1 || tracks.Count((TrackConfiguration t) => t.DefinitionId == WellKnownDeviceTrackDefinition.Primary) > 1;
		streamValidationResult.MustHaveOnePrimaryTrackStreamError = this.Count((StreamInfo s) => s.IsPrimaryTrack) == 0 || tracks.Count((TrackConfiguration t) => t.DefinitionId == WellKnownDeviceTrackDefinition.Primary) == 0;
		streamValidationResult.TwoOrMoreStreamsWithSameTrackStreamError = (from s in this
			where s.TrackDefinition.HasValue && !s.IsPrimaryTrack
			group s by s.TrackDefinition.Value).Any((IGrouping<Guid, StreamInfo> g) => g.Count() > 1) || tracks.Select((TrackConfiguration t) => t.DefinitionId).Distinct().Count() != tracks.Count();
		HashSet<Guid> validIdsForTracks = new HashSet<Guid>
		{
			WellKnownDeviceTrackDefinition.Primary,
			WellKnownDeviceTrackDefinition.Secondary,
			WellKnownDeviceTrackDefinition.Edge
		};
		HashSet<Guid> validIdsForStreams = new HashSet<Guid>(validIdsForTracks.Where((Guid d) => d != WellKnownDeviceTrackDefinition.Edge));
		streamValidationResult.UnknownTrackStreamError = this.Any((StreamInfo s) => s.TrackDefinition.HasValue && !validIdsForStreams.Contains(s.TrackDefinition.Value)) || this.Any((StreamInfo s) => s.TrackDefinition.HasValue && !tracks.Any(delegate(TrackConfiguration t)
		{
			Guid definitionId = t.DefinitionId;
			Guid? trackDefinition = s.TrackDefinition;
			return definitionId == trackDefinition;
		})) || tracks.Any((TrackConfiguration t) => !validIdsForTracks.Contains(t.DefinitionId));
		streamValidationResult.InvalidSelectionError = this.Any((StreamInfo s) => !s.LiveFeed && (s.LiveDefault || s.LiveRunAlways));
		if (deviceDefinition.StreamCombinations.Count == 0)
		{
			streamValidationResult.OnlyOneStreamAllowedError = Count > 1;
		}
		else
		{
			streamValidationResult.StreamCombinationError = ValidateConfiguration_HasStreamCombinationError(deviceDefinition);
			streamValidationResult.UnknownStreamError = this.Any((StreamInfo si) => deviceDefinition.Streams.FindByReferenceId(si.StreamReferenceId) == null);
		}
		return streamValidationResult;
	}

	private bool ValidateConfiguration_HasStreamCombinationError(DeviceDefinition deviceDefinition)
	{
		Dictionary<Guid, int> dictionary = new Dictionary<Guid, int>();
		using (IEnumerator<StreamInfo> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				StreamInfo current = enumerator.Current;
				StreamDefinition streamDefinition = deviceDefinition.Streams.FindByReferenceId(current.StreamReferenceId);
				if (streamDefinition != null)
				{
					Guid streamType = streamDefinition.StreamType;
					if (dictionary.ContainsKey(streamType))
					{
						dictionary[streamType]++;
					}
					else
					{
						dictionary.Add(streamType, 1);
					}
				}
			}
		}
		foreach (Guid key in dictionary.Keys)
		{
			StreamCombinations streamCombinations = deviceDefinition.StreamCombinations[key];
			foreach (Guid key2 in dictionary.Keys)
			{
				if (dictionary[key2] > streamCombinations.FindStreamMaxCount(key2))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void EnsureLiveAndRecordSelections()
	{
		EnsureExactlyOneDefaultLiveStream();
		EnsureExactlyOnePrimaryTrack();
		void EnsureExactlyOneDefaultLiveStream()
		{
			if (!this.Any((StreamInfo s) => s.LiveDefault))
			{
				StreamInfo obj = this.FirstOrDefault((StreamInfo s) => s.LiveDefault) ?? this.FirstOrDefault((StreamInfo s) => s.LiveRunAlways) ?? this.FirstOrDefault((StreamInfo s) => s.IsPrimaryTrack) ?? this.First();
				obj.LiveFeed = true;
				obj.LiveDefault = true;
				obj.LiveRunAlways = true;
			}
		}
		void EnsureExactlyOnePrimaryTrack()
		{
			if (!this.Any((StreamInfo s) => s.IsPrimaryTrack))
			{
				(this.FirstOrDefault((StreamInfo s) => !s.TrackDefinition.HasValue && s.LiveDefault) ?? this.FirstOrDefault((StreamInfo s) => !s.TrackDefinition.HasValue && s.LiveRunAlways) ?? this.FirstOrDefault((StreamInfo s) => !s.TrackDefinition.HasValue) ?? this.First((StreamInfo s) => s.LiveDefault)).TrackDefinition = WellKnownDeviceTrackDefinition.Primary;
			}
		}
	}

	public IEnumerator<StreamInfo> GetEnumerator()
	{
		return list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return list.GetEnumerator();
	}
}
