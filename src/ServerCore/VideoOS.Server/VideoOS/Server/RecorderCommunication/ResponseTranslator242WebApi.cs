using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.WebApi;
using VideoOS.Common.Integration.WebApi.Models.Server;

namespace VideoOS.Server.RecorderCommunication;

public sealed class ResponseTranslator242WebApi : CommonResponseVisitorBaseAsync
{
	private ICommunicator _communicator;

	public ResponseTranslator242WebApi(ICommunicator communicator, Func<ResponseBase, Task> onResponseNotSupported)
		: base(onResponseNotSupported)
	{
		_communicator = communicator;
	}

	public static async Task<bool> TranslateAsync(ResponseBase response, ICommunicator communicator)
	{
		bool translationFailed = false;
		if (response is CommonResponseBase commonResponseBase)
		{
			await commonResponseBase.AcceptAsync(new ResponseTranslator242WebApi(communicator, async delegate
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
		VideoOS.Common.Integration.WebApi.ResponseModel obj = new VideoOS.Common.Integration.WebApi.ResponseModel
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
		VideoOS.Common.Integration.WebApi.ResponseModel obj = new VideoOS.Common.Integration.WebApi.ResponseModel
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
		VideoOS.Common.Integration.WebApi.BoolResponseModel obj = new VideoOS.Common.Integration.WebApi.BoolResponseModel
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
		VideoOS.Common.Integration.WebApi.ErrorResponseModel obj = new VideoOS.Common.Integration.WebApi.ErrorResponseModel
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
		VideoOS.Common.Integration.WebApi.WarningResponseModel obj = new VideoOS.Common.Integration.WebApi.WarningResponseModel
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
		VideoOS.Common.Integration.WebApi.ExceptionResponseModel obj = new VideoOS.Common.Integration.WebApi.ExceptionResponseModel
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
		VideoOS.Common.Integration.WebApi.ResponseModel obj = new VideoOS.Common.Integration.WebApi.ResponseModel
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
		VideoOS.Common.Integration.WebApi.ResponseModel obj = new VideoOS.Common.Integration.WebApi.ResponseModel
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

	public override async Task Visit(ProgressResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		ProgressResponseModel obj = new ProgressResponseModel
		{
			Token = response.Token,
			Progress = response.Progress
		};
		await _communicator.SendAsync("api/response/progress", obj);
	}

	public override async Task Visit(FileUploadResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		FileUploadResponseModel obj = new FileUploadResponseModel
		{
			Token = response.Token,
			StorageId = response.StorageId
		};
		await _communicator.SendAsync("api/response/fileupload", obj);
	}

	public override async Task Visit(FirmwareUpgradeSuccessResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		FirmwareUpgradeSuccessResponseModel obj = new FirmwareUpgradeSuccessResponseModel
		{
			Token = response.Token,
			NewFirmwareVersion = response.NewFirmwareVersion
		};
		await _communicator.SendAsync("api/response/firmwareupgradesuccess", obj);
	}
}
