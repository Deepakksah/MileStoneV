using System;
using System.Runtime.Serialization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.Proxy.ConfigApi;

[Serializable]
public class ValidateResultException : MIPException
{
	public ValidateResult ValidateResult { get; set; }

	public ValidateResultException()
	{
	}

	public ValidateResultException(ValidateResult validateResult)
	{
		ValidateResult = validateResult;
	}

	protected ValidateResultException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		ValidateResult = (ValidateResult)info.GetValue("ValidateResult", typeof(ValidateResult));
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info?.AddValue("ValidateResult", ValidateResult);
		base.GetObjectData(info, context);
	}
}
