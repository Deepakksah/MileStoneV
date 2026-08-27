using System;
using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Common.Integration.AviConfig;

public sealed class MediaSources
{
	private List<MediaSource> videoSources = new List<MediaSource>();

	private List<MediaSource> audioSources = new List<MediaSource>();

	public MediaSource VideoSource
	{
		get
		{
			if (videoSources.Count > 0)
			{
				return videoSources[0];
			}
			return null;
		}
	}

	public IEnumerable<MediaSource> AudioSources => audioSources;

	public void AddVideoSource(MediaSource mediaSource)
	{
		videoSources.Add(mediaSource);
	}

	public void AddVideoSource(Guid deviceId, string address)
	{
		videoSources.Add(new MediaSource
		{
			DeviceId = deviceId,
			MediaType = "Video",
			Address = address
		});
	}

	public void AddAudioSource(MediaSource mediaSource)
	{
		audioSources.Add(mediaSource);
	}

	public void AddAudioSource(Guid deviceId, string address)
	{
		audioSources.Add(new MediaSource
		{
			DeviceId = deviceId,
			MediaType = "Audio",
			Address = address
		});
	}

	public void ToXml(XmlWriter writer)
	{
		writer.WriteStartElement("MediaSources");
		foreach (MediaSource videoSource in videoSources)
		{
			videoSource.ToXml(writer);
		}
		foreach (MediaSource audioSource in audioSources)
		{
			audioSource.ToXml(writer);
		}
		writer.WriteEndElement();
	}

	public static MediaSources FromXml(XmlReader reader)
	{
		MediaSources mediaSources = new MediaSources();
		bool isEmptyElement = reader.IsEmptyElement;
		reader.Read();
		if (!isEmptyElement)
		{
			while (reader.NodeType == XmlNodeType.Element)
			{
				string name = reader.Name;
				if (name != null && name.Equals("MediaSource", StringComparison.OrdinalIgnoreCase))
				{
					MediaSource mediaSource = MediaSource.FromXml(reader);
					if (mediaSource.MediaType == "Video")
					{
						mediaSources.AddVideoSource(mediaSource);
					}
					else if (mediaSource.MediaType == "Audio")
					{
						mediaSources.AddAudioSource(mediaSource);
					}
				}
				else
				{
					reader.Read();
				}
			}
			reader.ReadEndElement();
		}
		return mediaSources;
	}
}
