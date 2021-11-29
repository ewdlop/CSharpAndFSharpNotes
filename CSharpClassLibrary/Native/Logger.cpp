typedef void(*LOGFUNC)(const char*);

LOGFUNC Log = nullptr;

extern "C" __declspec(dllexport) void SetLogger(LOGFUNC pfnLog)
{
	if (!Log)
		Log = pfnLog;
}

extern "C" __declspec(dllexport) void RunLog(const char* pszMsg)
{
	if (Log)
		Log(pszMsg);
}