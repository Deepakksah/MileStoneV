using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.WebApi;

namespace VideoOS.Server.RecorderCommunication;

public sealed class ResponseTranslator202WebApi : CommonResponseVisitorBaseAsync
{
	private ICommunicator _communicator;

	public ResponseTranslator202WebApi(ICommunicator communicator, Func<ResponseBase, Task> onResponseNotSupported)
		: base(onResponseNotSupported)
	{
		_communicator = communicator;
	}

	public static async Task<bool> TranslateAsync(ResponseBase response, ICommunicator communicator)
	{
		bool translationFailed = false;
		if (response is CommonResponseBase commonResponseBase)
		{
			await commonResponseBase.AcceptAsync(new ResponseTranslator202WebApi(communicator, async delegate
			{
				translationFailed = true;
				await Task.CompletedTask;
			}));
		}
		else
		{
			translationFailed = true;
		}
		return !translationFailed;
	}

	public override async Task Visit(SuccessResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		ResponseModel obj = new ResponseModel
		{
			Token = response.Token
		};
		await _communicator.SendAsync("api/response/success", obj);
	}

	public override async Task Visit(FailResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		ResponseModel obj = new ResponseModel
		{
			Token = response.Token
		};
		await _communicator.SendAsync("api/response/failure", obj);
	}

	public override async Task Visit(BoolResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		BoolResponseModel obj = new BoolResponseModel
		{
			Token = response.Token,
			Value = response.Value
		};
		await _communicator.SendAsync("api/response/bool", obj);
	}

	public override async Task Visit(ErrorResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		ErrorResponseModel obj = new ErrorResponseModel
		{
			Token = response.Token,
			ErrorCode = response.ErrorCode,
			Message = response.Message
		};
		await _communicator.SendAsync("api/response/error", obj);
	}

	public override async Task Visit(WarningResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		WarningResponseModel obj = new WarningResponseModel
		{
			Token = response.Token,
			Message = response.Message
		};
		await _communicator.SendAsync("api/response/warning", obj);
	}

	public override async Task Visit(ExceptionResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		ExceptionResponseModel obj = new ExceptionResponseModel
		{
			Token = response.Token,
			Exception = response.Exception
		};
		HttpResponseMessage val = await _communicator.SendAsync("api/response/exception", obj);
		if (val != null && val.StatusCode == HttpStatusCode.BadRequest)
		{
			await Visit(new ErrorResponse(-1, response.Exception.ToString())
			{
				Token = response.Token
			});
		}
	}

	public override async Task Visit(TimeoutResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		ResponseModel obj = new ResponseModel
		{
			Token = response.Token
		};
		await _communicator.SendAsync("api/response/timeout", obj);
	}

	public override async Task Visit(HeartBeatResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		ResponseModel obj = new ResponseModel
		{
			Token = response.Token
		};
		await _communicator.SendAsync("api/response/heartbeat", obj);
	}

	public override async Task Visit(QueryDeviceEvidenceLockSizesResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		await Task.Run(delegate
		{
			throw new NotImplementedException();
		});
	}
}
