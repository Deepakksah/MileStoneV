using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.LoginThread;

[Obsolete("Marked as obsolete as  it seem not used anywhere")]
public class UriChecker
{
	public enum UriState
	{
		Timeout,
		Available
	}

	public class UriData
	{
		public Uri Uri { get; set; }

		public UriState UriState { get; set; }
	}

	public delegate void OnCompletion(IEnumerable<UriData> uriDatas);

	public enum CheckerState
	{
		Started,
		Completed,
		Error,
		Timeout
	}

	public class CheckerData
	{
		public Uri Uri { get; set; }

		public CheckerState State { get; set; }
	}

	private class CheckerDataInternal : CheckerData
	{
		public HttpWebRequest Request { get; set; }
	}

	private HttpClient httpClient = new HttpClient();

	private TimeSpan _timeout;

	private OnCompletion _callback;

	private readonly IEnumerable<CheckerDataInternal> _checkers;

	public UriChecker(IEnumerable<Uri> uris, TimeSpan timeout, OnCompletion callback)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_checkers = uris.Select((Uri uri) => new CheckerDataInternal
		{
			Uri = uri,
			State = CheckerState.Started
		}).ToList();
		Initialize(timeout, callback);
	}

	public UriChecker(IEnumerable<CheckerData> uris, TimeSpan timeout, OnCompletion callback)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_checkers = uris.Select((CheckerData checker) => new CheckerDataInternal
		{
			Uri = checker.Uri,
			State = checker.State
		}).ToList();
		Initialize(timeout, callback);
	}

	private void Initialize(TimeSpan timeout, OnCompletion callback)
	{
		_timeout = timeout;
		_callback = callback;
		Task.Factory.StartNew(CheckerTask);
	}

	private void CheckerTask()
	{
		StartCheckers(_checkers);
		WaitCheckersToComplete(_checkers);
		CleanCheckers(_checkers);
		PrintCheckers(_checkers);
		IEnumerable<UriData> uriDatas = _checkers.Select((CheckerDataInternal checker) => new UriData
		{
			Uri = checker.Uri,
			UriState = ((checker.State == CheckerState.Completed) ? UriState.Available : UriState.Timeout)
		});
		_callback(uriDatas);
	}

	private void OnGetResponse(IAsyncResult asyncResult)
	{
		CheckerDataInternal checkerDataInternal = (CheckerDataInternal)asyncResult.AsyncState;
		try
		{
			HttpWebResponse obj = (HttpWebResponse)checkerDataInternal.Request.EndGetResponse(asyncResult);
			if (obj.StatusCode != HttpStatusCode.GatewayTimeout)
			{
				checkerDataInternal.State = CheckerState.Completed;
			}
			obj.Close();
		}
		catch (Exception)
		{
			checkerDataInternal.State = CheckerState.Error;
		}
	}

	private void StartCheckers(IEnumerable<CheckerData> checkers)
	{
		foreach (CheckerData checker in checkers)
		{
			if (checker.State == CheckerState.Started)
			{
				Task.Factory.StartNew(SingleUriChecker, checker);
			}
		}
	}

	private void SingleUriChecker(object obj)
	{
		CheckerDataInternal checkerDataInternal = obj as CheckerDataInternal;
		try
		{
			checkerDataInternal.Request = (HttpWebRequest)WebRequest.Create(checkerDataInternal.Uri);
			checkerDataInternal.Request.Method = "GET";
			checkerDataInternal.Request.BeginGetResponse(OnGetResponse, checkerDataInternal);
		}
		catch (Exception)
		{
			checkerDataInternal.State = CheckerState.Error;
		}
	}

	private void WaitCheckersToComplete(IEnumerable<CheckerDataInternal> checkers)
	{
		DateTime utcNow = DateTime.UtcNow;
		while (DateTime.UtcNow - utcNow < _timeout && !AreCheckersCompleted(checkers))
		{
			Thread.Sleep(100);
		}
	}

	private bool AreCheckersCompleted(IEnumerable<CheckerDataInternal> checkerDatas)
	{
		return checkerDatas.All((CheckerDataInternal checkerData) => checkerData.State != CheckerState.Started);
	}

	private void CleanCheckers(IEnumerable<CheckerDataInternal> checkers)
	{
		foreach (CheckerDataInternal checker in checkers)
		{
			if (checker.State == CheckerState.Started)
			{
				try
				{
					checker.State = CheckerState.Timeout;
					checker.Request.Abort();
				}
				catch (Exception)
				{
				}
			}
		}
	}

	private void PrintCheckers(IEnumerable<CheckerDataInternal> checkers)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (CheckerDataInternal checker in checkers)
		{
			stringBuilder.Clear();
			stringBuilder.Append(checker.Uri.ToString()).Append(" - ").Append(checker.State.ToString());
			Log.Info("UriChecker", stringBuilder.ToString());
		}
	}
}
