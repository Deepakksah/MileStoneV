namespace VideoOS.Common.Integration.Threading;

public delegate void AsyncTask(TaskState taskState, ReportProgress reportProgress, AbortRequested abortRequested);
