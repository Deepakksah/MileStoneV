using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data.MediaProviderInternal;

public static class Protocol
{
	private static DateTime _lastDateReceived = DateTime.MinValue;

	private static DateTime _lastActualTime = DateTime.Now;

	private static readonly IDictionary<HttpStatusCode, string> HttpStatusCodes = CreateStatusCodes();

	public static RequestHeader ProcessRequest(Socket sock, string request, SecureString securePassword, IDictionary<string, string> sessionParms)
	{
		RequestHeader requestHeader = new RequestHeader();
		string text = "MIP";
		if (sessionParms.ContainsKey("DriverType"))
		{
			text = sessionParms["DriverType"];
		}
		string[] array = Regex.Split(request, "\r\n");
		string[] array2 = array[0].Split(' ');
		if (array2.Length < 2)
		{
			SendStreamResponse(sock, HttpStatusCode.BadRequest, "Invalid HTTP command");
			requestHeader.ResponseSent = true;
			requestHeader.Error = true;
			return requestHeader;
		}
		string[] array3 = array2[1].Split('?');
		if (array3.Length < 2)
		{
			SendStreamResponse(sock, HttpStatusCode.BadRequest, "Invalid or missing URL");
			requestHeader.ResponseSent = true;
			requestHeader.Error = true;
			return requestHeader;
		}
		requestHeader.ParameterList = array3[1].Split('&');
		string[] array4 = array3[0].Split('/');
		if (array4.Length < 3 || !string.IsNullOrEmpty(array4[0]) || array4[1] != text)
		{
			SendStreamResponse(sock, HttpStatusCode.BadRequest, "Invalid or missing URL");
			requestHeader.ResponseSent = true;
			requestHeader.Error = true;
			return requestHeader;
		}
		switch (array4[2])
		{
		case "Param.cgi":
			requestHeader.Command = "param";
			break;
		case "Stream.cgi":
			requestHeader.Command = "stream";
			break;
		case "Command.cgi":
			requestHeader.Command = "command";
			break;
		case "Ptz.cgi":
			requestHeader.Command = "ptz";
			break;
		default:
			SendStreamResponse(sock, HttpStatusCode.BadRequest, "Invalid or missing URL");
			requestHeader.ResponseSent = true;
			requestHeader.Error = true;
			return requestHeader;
		}
		string[] array5 = requestHeader.ParameterList[0].Split('=');
		if (array5[0] != "Action")
		{
			SendStreamResponse(sock, HttpStatusCode.BadRequest, "Invalid or missing Action");
			requestHeader.ResponseSent = true;
			requestHeader.Error = true;
			return requestHeader;
		}
		string text2 = array5[1];
		bool flag = false;
		string text3 = null;
		string text4 = null;
		string[] parameterList = requestHeader.ParameterList;
		for (int i = 0; i < parameterList.Length; i++)
		{
			string[] array6 = parameterList[i].Split('=');
			if (array6.Length < 2)
			{
				requestHeader.ParameterIds.Add(array6[0]);
				continue;
			}
			requestHeader.Parameters[array6[0]] = array6[1];
			if (array6[0] == "Token")
			{
				text3 = array6[1];
			}
			if (array6[0] == "Date")
			{
				text4 = array6[1];
			}
			if (array6[0] == "DeviceNum" && !int.TryParse(array6[1].Trim(), out requestHeader.Channel))
			{
				requestHeader.Channel = 0;
			}
		}
		if (text3 != null && text4 != null)
		{
			flag = ValidateToken(text3, text4, securePassword);
		}
		if (!flag)
		{
			SendStreamResponse(sock, HttpStatusCode.Unauthorized, "Invalid or missing token");
			requestHeader.ResponseSent = true;
			requestHeader.Error = true;
			return requestHeader;
		}
		if (requestHeader.Command == "param")
		{
			string s = "";
			requestHeader.Action = text2;
			switch (text2)
			{
			case "Get":
				requestHeader.ParameterIds.Select((string key) => key + "=" + (sessionParms.ContainsKey(key) ? sessionParms[key] : "NotSupported") + "\r\n").Aggregate((string text6, string text7) => text6 + text7);
				SendString(sock, s, HttpStatusCode.OK);
				requestHeader.ResponseSent = true;
				requestHeader.Error = false;
				return requestHeader;
			case "GetAll":
				s = sessionParms.Select((KeyValuePair<string, string> kvp) => kvp.Key + "=" + kvp.Value + "\r\n").Aggregate((string text6, string text7) => text6 + text7);
				SendString(sock, s, HttpStatusCode.OK);
				requestHeader.ResponseSent = true;
				requestHeader.Error = false;
				return requestHeader;
			case "Set":
				foreach (string key in requestHeader.Parameters.Keys)
				{
					requestHeader.SetParameters[key] = requestHeader.Parameters[key];
					if (key.StartsWith("MDStream.", StringComparison.Ordinal))
					{
						if (!int.TryParse(key.Substring("MDStream.".Length, 1), out var result))
						{
							result = 0;
						}
						requestHeader.Channel = result + 1;
					}
				}
				SendString(sock, "", HttpStatusCode.OK);
				requestHeader.ResponseSent = true;
				requestHeader.Error = false;
				return requestHeader;
			default:
				SendStreamResponse(sock, HttpStatusCode.BadRequest, "Invalid or missing URL");
				requestHeader.ResponseSent = true;
				requestHeader.Error = true;
				return requestHeader;
			}
		}
		if (requestHeader.Command == "stream")
		{
			requestHeader.MediaProviderSession = new MediaProviderSession
			{
				Channel = requestHeader.Channel,
				FPS = 10,
				MediaContent = MediaContent.Jpeg,
				ImageWidth = 320,
				ImageHeight = 240,
				Quality = 75,
				GopLength = 10
			};
			foreach (string key2 in requestHeader.Parameters.Keys)
			{
				string text5 = requestHeader.Parameters[key2];
				requestHeader.SetParameters[key2] = text5;
				switch (key2)
				{
				case "Fps":
				{
					if (!int.TryParse(text5, out var result3))
					{
						EnvironmentManager.Instance.Log(error: true, "MediaProvider", "Error parsing fps:" + array[0], null);
						result3 = 5;
					}
					requestHeader.MediaProviderSession.FPS = result3;
					break;
				}
				case "Quality":
				{
					if (!int.TryParse(text5, out var result6))
					{
						EnvironmentManager.Instance.Log(error: true, "MediaProvider", "Error parsing quality:" + array[0], null);
						result6 = 75;
					}
					if (result6 < 10)
					{
						result6 = 10;
					}
					if (result6 > 100)
					{
						result6 = 100;
					}
					requestHeader.MediaProviderSession.Quality = result6;
					break;
				}
				case "Resolution":
				{
					string[] array7 = text5.Split('x');
					if (array7.Length == 2)
					{
						if (int.TryParse(array7[0], out var result4) && int.TryParse(array7[1], out var result5))
						{
							requestHeader.MediaProviderSession.ImageWidth = result4;
							requestHeader.MediaProviderSession.ImageHeight = result5;
						}
					}
					else if (text5 != "Auto")
					{
						EnvironmentManager.Instance.Log(error: true, "MediaProvider", "Error parsing Resolution:" + array[0]);
					}
					break;
				}
				case "Codec":
					switch (text5)
					{
					case "Jpeg":
						requestHeader.MediaProviderSession.MediaContent = MediaContent.Jpeg;
						break;
					case "H264":
					case "H.264":
						requestHeader.MediaProviderSession.MediaContent = MediaContent.H264;
						break;
					case "H265":
					case "H.265":
						requestHeader.MediaProviderSession.MediaContent = MediaContent.H265;
						break;
					case "Audio":
						requestHeader.MediaProviderSession.MediaContent = MediaContent.Audio;
						break;
					case "Metadata":
						requestHeader.MediaProviderSession.MediaContent = MediaContent.MetaData;
						break;
					case "Pcm":
						requestHeader.MediaProviderSession.MediaContent = MediaContent.Pcm;
						break;
					default:
						EnvironmentManager.Instance.Log(error: true, "MediaProvider", "Error parsing codec:" + array[0]);
						requestHeader.MediaProviderSession.MediaContent = MediaContent.Jpeg;
						break;
					}
					break;
				case "Gop":
				{
					if (int.TryParse(text5, out var result2))
					{
						requestHeader.MediaProviderSession.GopLength = result2;
					}
					break;
				}
				}
			}
			requestHeader.ResponseSent = false;
			requestHeader.Error = false;
			return requestHeader;
		}
		if (requestHeader.Command == "ptz")
		{
			double num = GetDouble(requestHeader, "Pan");
			double num2 = GetDouble(requestHeader, "Tilt");
			double num3 = GetDouble(requestHeader, "Zoom");
			switch (text2)
			{
			case "GetAbsPos":
				requestHeader.CommandMessageId = "Control.PTZGetAbsoluteRequest";
				requestHeader.CommandData = null;
				break;
			case "SetAbsPos":
				requestHeader.CommandMessageId = "Control.PTZMoveAbsoluteCommand";
				requestHeader.CommandData = new PTZMoveAbsoluteCommandData
				{
					AllowRepeats = true,
					Pan = num,
					Tilt = num2,
					Zoom = num3,
					Speed = 1.0
				};
				break;
			case "PTZMove":
				if (num == 0.0 && num2 == 0.0 && num3 == 0.0)
				{
					requestHeader.CommandMessageId = "Control.PTZMoveStopCommand";
					requestHeader.CommandData = null;
					break;
				}
				requestHeader.CommandMessageId = "Control.PTZMoveStartCommand";
				requestHeader.CommandData = new PTZMoveStartCommandData
				{
					Pan = num,
					Tilt = num2,
					Zoom = num3,
					Speed = 1.0
				};
				break;
			case "PTZMoveOnce":
				requestHeader.CommandMessageId = "Control.PTZMoveCommand";
				if (num > 0.0)
				{
					if (num2 == 0.0)
					{
						requestHeader.CommandData = "Right";
					}
					else if (num2 < 0.0)
					{
						requestHeader.CommandData = "DownRight";
					}
					else
					{
						requestHeader.CommandData = "UpRight";
					}
				}
				else if (num < 0.0)
				{
					if (num2 == 0.0)
					{
						requestHeader.CommandData = "Left";
					}
					else if (num2 < 0.0)
					{
						requestHeader.CommandData = "DownLeft";
					}
					else
					{
						requestHeader.CommandData = "UpLeft";
					}
				}
				else if (num2 == 0.0)
				{
					if (num3 < 0.0)
					{
						requestHeader.CommandData = "ZoomOut";
					}
					else if (num3 > 0.0)
					{
						requestHeader.CommandData = "ZoomIn";
					}
					else
					{
						requestHeader.CommandData = "Home";
					}
				}
				else if (num2 < 0.0)
				{
					requestHeader.CommandData = "Down";
				}
				else
				{
					requestHeader.CommandData = "Up";
				}
				break;
			case "PTZCenter":
				requestHeader.CommandMessageId = "Control.PTZCenterCommand";
				requestHeader.CommandData = new PTZCenterCommandData
				{
					RefHeight = 1000.0,
					RefWidth = 1000.0,
					CenterX = 5.0 * num,
					CenterY = 5.0 * num2
				};
				break;
			case "PTZCommand":
				foreach (string parameterId in requestHeader.ParameterIds)
				{
					if (parameterId.StartsWith("AUX", StringComparison.Ordinal))
					{
						if (!int.TryParse(parameterId.Substring(3, 1), out var result7))
						{
							result7 = 0;
						}
						bool flag2 = parameterId.Substring(4, 2) == "On";
						requestHeader.CommandMessageId = "Control.PTZAUXCommand";
						requestHeader.CommandData = new PTZAUXCommandData
						{
							AuxNumber = result7,
							On = flag2
						};
					}
					else
					{
						requestHeader.CommandMessageId = "Control.LensCommand";
						requestHeader.CommandData = ConvertLensCommand(parameterId);
					}
				}
				break;
			default:
				requestHeader.CommandMessageId = "UnknownCommand:" + text2;
				break;
			}
			requestHeader.ResponseSent = false;
			requestHeader.Error = false;
			return requestHeader;
		}
		SendStreamResponse(sock, HttpStatusCode.NotFound);
		requestHeader.ResponseSent = true;
		requestHeader.Error = true;
		return requestHeader;
	}

	private static string ConvertLensCommand(string cmd)
	{
		return cmd switch
		{
			"FocusAuto" => "FocusAuto", 
			"FocusNear" => "FocusUp", 
			"FocusFar" => "FocusDown", 
			"FocusStop" => "FocusStop", 
			"IrisUp" => "IrisUp", 
			"IrisDown" => "IrisDown", 
			"IrisReset" => "IrisReset", 
			"ShutterUp" => "ShutterUp", 
			"ShutterDown" => "ShutterDown", 
			"ShuttersReset" => "ShutterReset", 
			_ => "", 
		};
	}

	private static double GetDouble(RequestHeader requestHeader, string key)
	{
		if (requestHeader.Parameters.ContainsKey(key) && double.TryParse(requestHeader.Parameters[key], out var result))
		{
			return result;
		}
		return 0.0;
	}

	public static void SendStreamResponseStartMultiPart(Socket sock)
	{
		SendFirstMultiPart(sock);
	}

	public static void SendStreamResponse(Socket sock, HttpStatusCode code, string moreInfo = null)
	{
		SendString(sock, moreInfo ?? string.Empty, code);
	}

	public static bool SendMediaData(Socket sock, MediaData mediaData)
	{
		try
		{
			if (mediaData.Bytes.Length == 0)
			{
				return false;
			}
			string text = TimeConvert.ToGmtString(mediaData.DateTime, includeMilliSeconds: true);
			string text2 = ((mediaData.TimeSpan.Ticks == 0L) ? "" : string.Format(CultureInfo.InvariantCulture, "TimeSpan: {0} ms\r\n", mediaData.TimeSpan.Ticks / 10000));
			string arg = "image/jpeg";
			switch (mediaData.MediaContent)
			{
			case MediaContent.MetaData:
				arg = "text/xml";
				break;
			case MediaContent.Audio:
				arg = "audio/basic";
				break;
			case MediaContent.H264:
				arg = "video/h264";
				break;
			case MediaContent.H265:
				arg = "video/h265";
				break;
			case MediaContent.Pcm:
				arg = "audio/x-wav;codec=pcm";
				break;
			}
			string s = string.Format(CultureInfo.InvariantCulture, "--MIPFrame\r\nContent-Type: {1}\r\nContent-Length: {0}\r\nX-FrameTime: " + text + "\r\n" + text2 + "\r\n", mediaData.Bytes.Length, arg);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			int num = sock.Send(bytes, SocketFlags.Partial);
			int num2 = sock.Send(mediaData.Bytes);
			if (num != bytes.Length || num2 != mediaData.Bytes.Length)
			{
				string.Format(CultureInfo.InvariantCulture, "Send error. {0} bytes sent, {1} expected.", num + num2, bytes.Length + mediaData.Bytes.Length);
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	private static bool ValidateToken(string token, string date, SecureString securePassword)
	{
		Utility.HashType hashType = Utility.GetHashType(token);
		if (hashType == Utility.HashType.NotSupported)
		{
			EnvironmentManager.Instance.Log(error: true, "MediaProvider.ValidateToken", "Token hash type not supported.");
			return false;
		}
		string password = new NetworkCredential(string.Empty, securePassword).Password;
		if (Utility.GetExpectedToken(date, "Copyright_Milestone_Duplication_Not_Allowed", password, hashType) != token)
		{
			EnvironmentManager.Instance.Log(error: true, "MediaProvider.ValidateToken", "Token invalid.");
			return false;
		}
		if (!DateTime.TryParse(date, out var result))
		{
			return false;
		}
		DateTime now = DateTime.Now;
		TimeSpan timeSpan = now - _lastActualTime;
		_lastActualTime = now;
		if (timeSpan > TimeSpan.FromMinutes(1.0) && result == _lastDateReceived)
		{
			return false;
		}
		_lastDateReceived = result;
		if ((DateTime.Now - result).Duration() > TimeSpan.FromHours(3.0))
		{
			return false;
		}
		return true;
	}

	private static void SendString(Socket sock, string s, HttpStatusCode httpStatusCode)
	{
		if (!HttpStatusCodes.TryGetValue(httpStatusCode, out var value))
		{
			value = string.Empty;
		}
		string s2 = string.Format(CultureInfo.InvariantCulture, "HTTP/1.1 {2} {3}\r\nContent-Type: text/ascii; charset=\"UTF-8\"\r\nConnection: Close\r\nContent-Length: {0}\r\n\r\n{1}\r\n\r\n", s.Length, s, (int)httpStatusCode, value);
		byte[] bytes = Encoding.UTF8.GetBytes(s2);
		sock.Send(bytes);
	}

	private static void SendFirstMultiPart(Socket socket)
	{
		try
		{
			string s = string.Format(CultureInfo.InvariantCulture, "HTTP/1.1 200 OK\r\nContent-Type: multipart/x-mixed-replace; boundary=MIPFrame\r\n\r\n");
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			socket.Send(bytes);
		}
		catch (Exception)
		{
		}
	}

	private static IDictionary<HttpStatusCode, string> CreateStatusCodes()
	{
		return Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().Distinct()
			.ToDictionary((HttpStatusCode statusCode) => statusCode, (HttpStatusCode statusCode) => Regex.Replace(Enum.GetName(typeof(HttpStatusCode), statusCode), "(?!^)[A-Z][a-z]", " $0"));
	}
}
