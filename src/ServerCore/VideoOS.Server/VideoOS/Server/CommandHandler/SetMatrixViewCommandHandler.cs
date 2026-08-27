using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.CommandHandler;

public sealed class SetMatrixViewCommandHandler : ICommandHandler
{
	private Server _server;

	public SetMatrixViewCommandHandler(Server server)
	{
		_server = server;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is SetMatrixViewCommand setMatrixViewCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		try
		{
			string matrixName = string.Empty;
			string address = string.Empty;
			int port = 0;
			string password = string.Empty;
			using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_server.Data)))
			{
				MatrixView matrixView = configurationManager.MatrixViews[setMatrixViewCommand.MatrixId];
				if (matrixView != null)
				{
					matrixName = matrixView.Name;
					address = matrixView.Address;
					port = matrixView.Port;
					password = matrixView.Password;
				}
			}
			SetMatrixSmartClient(setMatrixViewCommand.DeviceIds, matrixName, address, port, password);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogWarning("Failed to execute SetMatrixView command. Error: {ExceptionMessage}", ex2.Message);
			ServerLogManager.Instance.AddLogEntry(LogEntryId.MatrixLog, OemResources.Instance.Manager.OemClientName, OemResources.Instance.Manager.MatrixApplicationName);
			responseSink.AddResponse(new FailResponse());
		}
	}

	private static void SetMatrixSmartClient(Guid[] deviceIds, string matrixName, string address, int port, string password)
	{
		if (!SetMatrixSmartClientMultipleCameras(deviceIds, matrixName, address, port, password))
		{
			foreach (Guid deviceId in deviceIds)
			{
				SetMatrixSmartClientSingleCamera(deviceId, matrixName, address, port, password);
			}
		}
	}

	private static bool SetMatrixSmartClientMultipleCameras(Guid[] deviceIds, string matrixName, string address, int port, string password)
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		stringBuilder.Append("GET /NetMatrixMonitorBegin+0+");
		stringBuilder.Append(password);
		if (deviceIds.Length == 0 || (deviceIds.Length == 1 && deviceIds[0] == Guid.Empty))
		{
			stringBuilder.Append("+DISCONNECT+");
		}
		else
		{
			stringBuilder.Append("+CONNECTGUIDS+");
			stringBuilder.Append(deviceIds.Length.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append("+");
			foreach (Guid guid in deviceIds)
			{
				stringBuilder.Append(guid);
				stringBuilder.Append("+");
			}
			stringBuilder.Append("0");
			stringBuilder.Append("+");
			stringBuilder.Append("1");
		}
		stringBuilder.Append("+");
		stringBuilder.Append(matrixName);
		stringBuilder.Append("+NetMatrixMonitorEnd+");
		return SendMatrixCommand(address, port, stringBuilder.ToString());
	}

	private static bool SetMatrixSmartClientSingleCamera(Guid deviceId, string matrixName, string address, int port, string password)
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		stringBuilder.Append("GET /NetMatrixMonitorBegin+0+");
		stringBuilder.Append(password);
		stringBuilder.Append("+CONNECTGUID+");
		stringBuilder.Append(deviceId);
		stringBuilder.Append("+");
		stringBuilder.Append("0");
		stringBuilder.Append("+");
		stringBuilder.Append("1");
		stringBuilder.Append("+");
		stringBuilder.Append(matrixName);
		stringBuilder.Append("+NetMatrixMonitorEnd+");
		return SendMatrixCommand(address, port, stringBuilder.ToString());
	}

	private static bool SendMatrixCommand(string address, int port, string command)
	{
		bool result = false;
		byte[] bytes = Encoding.UTF8.GetBytes(command);
		TcpClient tcpClient = null;
		try
		{
			tcpClient = new TcpClient(address, port);
			using NetworkStream networkStream = tcpClient.GetStream();
			networkStream.Write(bytes, 0, bytes.Length);
			networkStream.Flush();
			StreamReader streamReader = new StreamReader(networkStream);
			string text = streamReader.ReadToEnd();
			string[] array = text.Split('+');
			if (array.Length >= 2 && string.Equals(array[1], command.Split('+')[3], StringComparison.Ordinal))
			{
				result = true;
			}
		}
		catch (SocketException)
		{
			return false;
		}
		finally
		{
			tcpClient?.Close();
		}
		return result;
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
