#define TRACE
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class StatusDialogThread
{
	private Thread statusThread;

	private StatusDialogForm statusForm;

	private string statusMessage;

	private Screen parentScreen;

	public StatusDialogThread()
	{
		statusThread = null;
		statusForm = null;
		statusMessage = string.Empty;
	}

	public void StartStatusThread(string message, Screen screen)
	{
		try
		{
			statusMessage = message;
			parentScreen = screen;
			statusThread = new Thread(StatusThreadFunc);
			statusThread.Name = "Status Thread";
			statusThread.IsBackground = true;
			statusThread.Start();
			Thread.Sleep(200);
		}
		catch (ThreadStateException)
		{
		}
	}

	public void StopStatusThread()
	{
		try
		{
			if (statusForm != null)
			{
				statusForm.CloseForm();
			}
			if (statusThread != null)
			{
				statusThread.Join(5000);
			}
			statusThread = null;
		}
		catch (Exception)
		{
		}
	}

	public void UpdateStatusMessage(string statusMessage)
	{
		if (statusForm != null && !statusForm.IsDisposed)
		{
			statusForm.SetStatusMessage(statusMessage);
		}
	}

	private void StatusThreadFunc()
	{
		try
		{
			Screen screen = Screen.AllScreens.FirstOrDefault((Screen f) => f.DeviceName == parentScreen.DeviceName) ?? Screen.AllScreens.ElementAtOrDefault(0);
			statusForm = new StatusDialogForm();
			statusForm.Left = screen.WorkingArea.Left + screen.Bounds.Size.Width / 2 - statusForm.Size.Width / 2;
			statusForm.Top = screen.WorkingArea.Top + screen.Bounds.Size.Height / 2 - statusForm.Size.Height / 2;
			statusForm.StartPosition = FormStartPosition.Manual;
			statusForm.SetStatusMessage(statusMessage);
			statusForm.SetSpeed(3);
			statusForm.ShowDialog();
			statusForm = null;
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.ToString());
		}
	}
}
