using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.Common.Integration.WebApi;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class RequiredNonemptyGuid : ValidationAttribute
{
	public override bool IsValid(object value)
	{
		if ((Guid)value == Guid.Empty)
		{
			return false;
		}
		return true;
	}

	public static string ReferenceInvoker()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((object)(DateFormatHandling)0/*cast due to constrained. prefix*/).ToString();
	}
}
