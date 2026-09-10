using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MilestoneManagementClient.Core
{
    public class LocalWebcamCaptureEngine
    {
        private const int WM_CAP_START = 0x400;
        private const int WM_CAP_DRIVER_CONNECT = WM_CAP_START + 10;
        private const int WM_CAP_DRIVER_DISCONNECT = WM_CAP_START + 11;
        private const int WM_CAP_EDIT_COPY = WM_CAP_START + 30;
        private const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;
        private const int WM_CAP_SET_PREVIEWRATE = WM_CAP_START + 52;
        private const int WM_CAP_SET_SCALE = WM_CAP_START + 53;
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;

        [DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        private static extern IntPtr capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWnd, int nID);

        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private IntPtr _hCapWnd = IntPtr.Zero;
        private volatile bool _isConnected = false;
        private static LocalWebcamCaptureEngine _instance;
        public static LocalWebcamCaptureEngine Instance
        {
            get { return _instance ?? (_instance = new LocalWebcamCaptureEngine()); }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
        }

        public bool StartCapture(IntPtr parentHandle, int width, int height)
        {
            try
            {
                if (_isConnected && _hCapWnd != IntPtr.Zero)
                {
                    ResizeCapture(width, height);
                    return true;
                }

                _hCapWnd = capCreateCaptureWindowA("MilestoneOpticalFeed", WS_CHILD | WS_VISIBLE, 0, 0, Math.Max(320, width), Math.Max(240, height), parentHandle, 1);
                if (_hCapWnd != IntPtr.Zero)
                {
                    // Connect to driver 0 (Integrated Webcam / Primary USB video capture)
                    if (SendMessage(_hCapWnd, WM_CAP_DRIVER_CONNECT, 0, 0))
                    {
                        SendMessage(_hCapWnd, WM_CAP_SET_SCALE, 1, 0);
                        SendMessage(_hCapWnd, WM_CAP_SET_PREVIEWRATE, 33, 0); // 30 FPS
                        SendMessage(_hCapWnd, WM_CAP_SET_PREVIEW, 1, 0);
                        _isConnected = true;
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public void ResizeCapture(int width, int height)
        {
            if (_hCapWnd != IntPtr.Zero && width > 0 && height > 0)
            {
                try
                {
                    SetWindowPos(_hCapWnd, IntPtr.Zero, 0, 0, width, height, 0x0014); // SWP_NOZORDER | SWP_NOACTIVATE
                }
                catch { }
            }
        }

        public void Stop()
        {
            if (_hCapWnd != IntPtr.Zero)
            {
                try
                {
                    SendMessage(_hCapWnd, WM_CAP_DRIVER_DISCONNECT, 0, 0);
                    DestroyWindow(_hCapWnd);
                }
                catch { }
                _hCapWnd = IntPtr.Zero;
                _isConnected = false;
            }
        }
    }
}
