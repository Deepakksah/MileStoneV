using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceTrackCollection : IEnumerable<DeviceTrack>, IEnumerable
{
	private readonly ConfigurationManager configurationManager;

	private readonly Device device;

	private IEnumerable<DeviceTrack> Tracks => configurationManager.DeviceTrackItems.GetDeviceTracks(device.ID);

	public DeviceTrack this[Guid trackDefinitionId] => Tracks.First((DeviceTrack t) => t.IDTrackDefinition == trackDefinitionId);

	public DeviceTrackCollection(ConfigurationManager configurationManager, Device device)
	{
		this.configurationManager = configurationManager ?? throw new ArgumentNullException("configurationManager");
		this.device = device ?? throw new ArgumentNullException("device");
	}

	public DeviceTrack AddNew(Guid trackDefinitionId, bool defaultPlayback, bool mergeEdge, bool inUse)
	{
		return configurationManager.DeviceTrackItems.AddNew(device.ID, trackDefinitionId, defaultPlayback, mergeEdge, inUse);
	}

	public bool TryGet(Guid trackDefinitionId, out DeviceTrack track)
	{
		track = Tracks.FirstOrDefault((DeviceTrack t) => t.IDTrackDefinition == trackDefinitionId);
		return track != null;
	}

	public IEnumerator<DeviceTrack> GetEnumerator()
	{
		return Tracks.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Tracks.GetEnumerator();
	}
}
