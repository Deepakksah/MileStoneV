using System;
using System.Collections.Generic;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class MessageWatch
{
	public Guid SessionID { get; set; }

	public string MessageTypeName { get; set; }

	public List<MatchExpression> Expressions { get; set; }

	public MessageWatch(Guid sessionID, string messageTypeName, List<MatchExpression> expressions)
	{
		SessionID = sessionID;
		MessageTypeName = messageTypeName;
		Expressions = expressions;
	}

	public MessageWatch(Guid sessionID, string messageTypeName)
		: this(sessionID, messageTypeName, new List<MatchExpression>())
	{
	}
}
