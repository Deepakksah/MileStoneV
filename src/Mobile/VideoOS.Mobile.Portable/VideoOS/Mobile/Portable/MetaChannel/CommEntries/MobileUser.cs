using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class MobileUser
{
	public const string UserIdXmlName = "Id";

	public const string UserNameXmlName = "Name";

	public const string UserStateXmlName = "State";

	public const string ConnectionsXmlName = "Connections";

	public const string SingleConnectionXmlName = "Connection";

	public const string LocalEndPointName = "LocalEndPoint";

	public const string RemoteEndPointName = "RemoteEndPoint";

	private LoginState _loginState;

	[JsonProperty("Id")]
	[XmlAttribute(AttributeName = "Id")]
	public Guid UserId { get; set; }

	[JsonProperty("Name")]
	[XmlAttribute(AttributeName = "Name")]
	public string UserName { get; set; }

	[JsonProperty("State")]
	[XmlAttribute(AttributeName = "State")]
	public string UserState
	{
		get
		{
			return EnumsHelper.GetDescription(_loginState);
		}
		set
		{
			_loginState = EnumsHelper.EnumConverter<LoginState>(value);
		}
	}

	[JsonProperty("Connections")]
	[XmlArray(ElementName = "Connections")]
	[XmlArrayItem(ElementName = "Connection")]
	public List<UserConnection> UserConnections { get; set; }

	[JsonProperty("LocalEndPoint")]
	[XmlElement(ElementName = "LocalEndPoint")]
	public EndPoint LocalEndPoint { get; set; }

	[JsonProperty("RemoteEndPoint")]
	[XmlElement(ElementName = "RemoteEndPoint")]
	public EndPoint RemoteEndPoint { get; set; }

	public void SetUserState(LoginState state)
	{
		_loginState = state;
	}

	public LoginState GetUserState()
	{
		return _loginState;
	}

	public MobileUser()
	{
		UserConnections = new List<UserConnection>();
	}
}
