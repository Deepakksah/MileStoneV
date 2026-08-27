using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class VmoServerServiceException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 0;

	[SqlError(60303)]
	[SqlError(60304)]
	[SqlError(60305)]
	[SqlError(60306)]
	[SqlError(60313)]
	[SqlError(60314)]
	[SqlError(60315)]
	[SqlError(60321)]
	[SqlError(60325)]
	[SqlError(60339)]
	[SqlError(60340)]
	[SqlError(60341)]
	[SqlError(60342)]
	[SqlError(60348)]
	[SqlError(60349)]
	[SqlError(60366)]
	[SqlError(60460)]
	[SqlError(60370)]
	[SqlError(60373)]
	[SqlError(60376)]
	[SqlError(60378)]
	[SqlError(60379)]
	[SqlError(60380)]
	[SqlError(60381)]
	[SqlError(60382)]
	[SqlError(60383)]
	[SqlError(60384)]
	[SqlError(60385)]
	[SqlError(60396)]
	[SqlError(60397)]
	[SqlError(60398)]
	[SqlError(60399)]
	[SqlError(60407)]
	[SqlError(60408)]
	[SqlError(60409)]
	[SqlError(60410)]
	[SqlError(60660)]
	[SqlError(60661)]
	[SqlError(60413)]
	[SqlError(60415)]
	[SqlError(60416)]
	[SqlError(60417)]
	public VmoServerServiceException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		databaseError.FormatMessage(out IEnumerable<(int, string, string)> parameters);
		foreach (var item in parameters)
		{
			base.MessageParameters.Add(item.Item1, new MessageParameter(item.Item1, item.Item2, item.Item3));
		}
		base.ErrorCode = databaseError.Number;
	}

	public VmoServerServiceException()
	{
	}

	public VmoServerServiceException(string message)
		: base(message)
	{
	}

	public VmoServerServiceException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected VmoServerServiceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public static VmoServerServiceException CreateFromVmoErrorMessage(string vmoErrorMessage)
	{
		if (vmoErrorMessage == null)
		{
			throw new ArgumentNullException("vmoErrorMessage");
		}
		Match match = Regex.Match(vmoErrorMessage, "^VMO(?<Number>\\d+):\\s(?<Message>.*)", RegexOptions.Compiled | RegexOptions.Singleline);
		if (!match.Success)
		{
			throw new ArgumentException("The message must be a VMO error message formatted like 'VMOxxxxx: message'", "vmoErrorMessage");
		}
		return new VmoServerServiceException(vmoErrorMessage)
		{
			ErrorCode = Convert.ToInt32(match.Groups["Number"].Value, CultureInfo.InvariantCulture)
		};
	}
}
