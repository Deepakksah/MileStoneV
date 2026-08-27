using System;

namespace VideoOS.EventServer.Util;

[Serializable]
public class ConfigFilterDetail
{
	public bool eventEnable;

	public Guid ID;

	public Guid GroupID;

	public string sources;

	public string Message { get; private set; }

	public string MessageClose { get; private set; }

	public Guid CloseId { get; private set; }

	public static string XPCO_ProductName { get; set; }

	public static string XPE_ProductName { get; set; }

	public ConfigFilterDetail(bool eventEnable, string message, Guid id, Guid groupId, string messageClose, Guid closeId, string sources)
	{
		this.eventEnable = eventEnable;
		ID = id;
		GroupID = groupId;
		Message = message;
		MessageClose = messageClose;
		CloseId = closeId;
		this.sources = sources;
	}
}
