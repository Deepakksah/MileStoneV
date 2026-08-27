using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("GetMetaDataFunctionToken")]
public class GetMetaDataFunctionToken : FunctionToken, IEvaluate
{
	private static GetMetaDataFunctionToken _instance = new GetMetaDataFunctionToken();

	public static GetMetaDataFunctionToken Instance => _instance;

	private GetMetaDataFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		string value = ((StringValueToken)resultStack.Pop()).Value;
		ValueToken valueToken = null;
		valueToken = ((triggerEvent.MetaData == null || !triggerEvent.MetaData.TryGetValue(value, out var value2)) ? new NullValueToken() : (value2.DataType switch
		{
			EventMetaData.DataTypeEnum.Bool => new BooleanValueToken(value2.GetValueAsBool()), 
			EventMetaData.DataTypeEnum.Int => new IntegerValueToken(value2.GetValueAsInt()), 
			EventMetaData.DataTypeEnum.Double => new DoubleValueToken(value2.GetValueAsDouble()), 
			EventMetaData.DataTypeEnum.String => new StringValueToken(value2.GetValueAsString()), 
			_ => new NullValueToken(), 
		}));
		resultStack.Push(valueToken);
	}
}
