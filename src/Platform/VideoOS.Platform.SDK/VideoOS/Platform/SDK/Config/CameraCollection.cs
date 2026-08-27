using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class CameraCollection
{
	private Dictionary<Guid, ICamera> _cameras = new Dictionary<Guid, ICamera>();

	public ICamera this[Guid id] => _cameras[id];

	public CameraCollection()
	{
	}

	public CameraCollection(ISystemInfo systemInfo, IRecorder recorder, IRecorderInfo recorderInfo)
	{
		ICameraInfo[] cameras = recorderInfo.Cameras;
		foreach (ICameraInfo cameraInfo in cameras)
		{
			ICamera camera = new CameraOnServer(systemInfo, recorder, cameraInfo);
			_cameras[camera.Guid] = camera;
		}
	}

	public void Remove(ICamera item)
	{
		_cameras.Remove(item.Guid);
	}

	public void Add(ICamera camera)
	{
		_cameras[camera.Guid] = camera;
	}

	public void AddRange(CameraCollection collection)
	{
		if (collection == null)
		{
			return;
		}
		foreach (ICamera item in collection)
		{
			_cameras[item.Guid] = item;
		}
	}

	public bool ContainsKey(Guid guid)
	{
		return _cameras.ContainsKey(guid);
	}

	public IEnumerator<ICamera> GetEnumerator()
	{
		return _cameras.Values.GetEnumerator();
	}
}
