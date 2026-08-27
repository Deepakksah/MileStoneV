using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security;
using VideoOS.Platform.Data.MediaProviderInternal;
using VideoOS.Platform.Login;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class MediaProviderService
{
	public delegate void SessionDisconnectedDelegate(RequestHeader requestHeader);

	private ServerService _serverService;

	private readonly Dictionary<int, MediaProviderChannel> _mediaProviders = new Dictionary<int, MediaProviderChannel>();

	private readonly Dictionary<int, MetadataProviderChannel> _metadataProviders = new Dictionary<int, MetadataProviderChannel>();

	private string _macAddress = string.Empty;

	private HardwareDefinition _hardwareDefinition;

	public virtual string MacAddress
	{
		get
		{
			if (_macAddress == "")
			{
				string[] mACChoices = Utility.GetMACChoices();
				if (mACChoices.Length != 0)
				{
					_macAddress = mACChoices[0];
				}
			}
			return _macAddress;
		}
		set
		{
			_macAddress = value;
		}
	}

	public ReadOnlyCollection<MediaProviderChannel> MediaProviders => new ReadOnlyCollection<MediaProviderChannel>(_mediaProviders.Values.ToList());

	public MediaProviderService()
	{
		new TrackPageViewData(typeof(MediaProviderService)).Send();
	}

	public virtual void Init(int port, string password, IDictionary<string, string> parameters)
	{
		Init(port, password.ToSecureString(), parameters);
	}

	public virtual void Init(int port, SecureString password, IDictionary<string, string> parameters)
	{
		if (_serverService != null)
		{
			throw new InvalidOperationException("Instance is already initialized");
		}
		_serverService = new ServerService(port, password, parameters);
		_serverService.Init(ValidateMediaRequestReceived, SessionDisconnected, GetMediaData, ValidateMetadataRequestReceived);
		lock (_mediaProviders)
		{
			foreach (MediaProviderChannel value in _mediaProviders.Values)
			{
				value.Init();
			}
		}
	}

	public virtual void Init(int port, string password, HardwareDefinition hardwareDefinition)
	{
		Init(port, password.ToSecureString(), hardwareDefinition);
	}

	public virtual void Init(int port, SecureString password, HardwareDefinition hardwareDefinition)
	{
		if (hardwareDefinition == null)
		{
			throw new ArgumentNullMIPException("hardwareDefinition");
		}
		_hardwareDefinition = hardwareDefinition;
		foreach (MetadataProviderChannel value2 in _metadataProviders.Values)
		{
			MetadataDeviceDefintion metadataDeviceDefintion = null;
			if (value2.Channel > 0 && _hardwareDefinition.MetadataDevices.Count >= value2.Channel)
			{
				metadataDeviceDefintion = _hardwareDefinition.MetadataDevices[value2.Channel - 1];
				foreach (MetadataTypeDefinition metadataType in metadataDeviceDefintion.MetadataTypes)
				{
					foreach (MetadataDeviceCapability capability in metadataType.Capabilities)
					{
						if (!value2.ParameterDictionary.ContainsKey(capability.Name))
						{
							string value = "";
							if (capability is DataCapability)
							{
								value = ((DataCapability)capability).Default.ToString(CultureInfo.InvariantCulture);
							}
							if (capability is EnumCapability)
							{
								value = ((EnumCapability)capability).DefaultValue;
							}
							value2.ParameterDictionary[capability.Name] = value;
						}
					}
				}
			}
			value2.MetadataDeviceDefintion = metadataDeviceDefintion;
		}
		IDictionary<string, string> parameters = hardwareDefinition.GenerateParameters(this);
		Init(port, password, parameters);
	}

	public virtual void Close()
	{
		if (_serverService != null)
		{
			while (_mediaProviders.Count > 0)
			{
				RemoveMediaProvider(_mediaProviders.Values.First());
			}
			while (_metadataProviders.Count > 0)
			{
				RemoveMetadataProvider(_metadataProviders.Values.First());
			}
			_serverService.Close();
			_serverService = null;
		}
	}

	public MediaProviderChannel CreateMediaProvider(int channel)
	{
		return CreateMediaProvider(channel, MediaContent.Jpeg);
	}

	public MediaProviderChannel CreateMediaProvider(int channel, MediaContent content)
	{
		lock (_mediaProviders)
		{
			int channelFromMediaContent = Utility.GetChannelFromMediaContent(channel, content);
			if (_mediaProviders.ContainsKey(channelFromMediaContent))
			{
				throw new MIPException(string.Format(CultureInfo.InvariantCulture, "A MediaProvider is already created for channel {0}", channelFromMediaContent));
			}
			MediaProviderChannel mediaProviderChannel = Utility.CreateMediaProviderFromMediaContent(channel, content);
			_mediaProviders[channelFromMediaContent] = mediaProviderChannel;
			if (_serverService != null)
			{
				mediaProviderChannel.Init();
			}
			return mediaProviderChannel;
		}
	}

	public MetadataProviderChannel CreateMetadataProvider(int channel, int queueLength = 1)
	{
		lock (_metadataProviders)
		{
			if (_metadataProviders.ContainsKey(channel))
			{
				throw new MIPException(string.Format(CultureInfo.InvariantCulture, "A MetadataProvider is already created for channel {0}", channel));
			}
			MetadataProviderChannel metadataProviderChannel = new MetadataProviderChannel(channel, queueLength);
			_metadataProviders[channel] = metadataProviderChannel;
			return metadataProviderChannel;
		}
	}

	public void RemoveMediaProvider(MediaProviderChannel mediaProvider)
	{
		lock (_mediaProviders)
		{
			if (_mediaProviders.ContainsValue(mediaProvider))
			{
				int key = _mediaProviders.FirstOrDefault((KeyValuePair<int, MediaProviderChannel> kvp) => kvp.Value == mediaProvider).Key;
				mediaProvider.Close();
				_mediaProviders.Remove(key);
			}
		}
	}

	public void RemoveMetadataProvider(MetadataProviderChannel metadataProvider)
	{
		lock (_metadataProviders)
		{
			if (_metadataProviders.ContainsValue(metadataProvider))
			{
				_metadataProviders.Remove(metadataProvider.Channel);
			}
		}
	}

	public MetadataProviderChannel GetMetadataProviderChannel(int channel)
	{
		lock (_metadataProviders)
		{
			if (_metadataProviders.ContainsKey(channel))
			{
				return _metadataProviders[channel];
			}
		}
		return null;
	}

	public MediaProviderChannel ValidateMediaRequestReceived(RequestHeader requestHeader)
	{
		MediaProviderChannel value;
		lock (_mediaProviders)
		{
			int channelFromRequestHeader = Utility.GetChannelFromRequestHeader(requestHeader);
			if (!_mediaProviders.TryGetValue(channelFromRequestHeader, out value))
			{
				return null;
			}
		}
		if (requestHeader.Command == "stream")
		{
			try
			{
				value.FireSessionOpeningEvent(requestHeader);
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: false, "MediaProviderService:SessionOpening", ex.Message, null);
				return null;
			}
		}
		return value;
	}

	public MetadataProviderChannel ValidateMetadataRequestReceived(RequestHeader requestHeader)
	{
		MetadataProviderChannel value;
		lock (_metadataProviders)
		{
			if (!_metadataProviders.TryGetValue(requestHeader.Channel, out value))
			{
				return null;
			}
		}
		if (requestHeader.Command == "stream")
		{
			try
			{
				value.FireSessionOpeningEvent(requestHeader);
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: false, "MetadataProviderService:SessionOpening", ex.Message, null);
				return null;
			}
		}
		if (requestHeader.Command == "param")
		{
			value.UpdateRelevantSettings(requestHeader);
			value.FireParametersChanged(requestHeader);
			if (_hardwareDefinition != null)
			{
				_serverService.Parameters = _hardwareDefinition.GenerateParameters(this);
			}
		}
		return value;
	}

	public void SessionDisconnected(RequestHeader requestHeader)
	{
		if (requestHeader.MediaProviderSession == null)
		{
			return;
		}
		try
		{
			if (requestHeader.MediaProviderSession.MediaContent == MediaContent.MetaData)
			{
				lock (_metadataProviders)
				{
					if (_metadataProviders.TryGetValue(requestHeader.MediaProviderSession.Channel, out var value))
					{
						value.FireSessionClosedEvent(requestHeader);
					}
					return;
				}
			}
			lock (_mediaProviders)
			{
				int channelFromRequestHeader = Utility.GetChannelFromRequestHeader(requestHeader);
				if (_mediaProviders.TryGetValue(channelFromRequestHeader, out var value2))
				{
					value2.FireSessionClosedEvent(requestHeader);
				}
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MediaProviderService:SessionDisconnected", ex.Message, null);
		}
	}

	public MediaData GetMediaData(MediaProviderChannel mediaProvider, RequestHeader requestHeader)
	{
		try
		{
			return mediaProvider.ExecGetMediaDataEvent(requestHeader);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MediaProviderService:GetMediaData", ex.Message, null);
			return null;
		}
	}
}
