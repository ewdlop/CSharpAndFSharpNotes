<Query Kind="Statements" />

async System.Threading.Tasks.Task<string> WebQuestAsync()
{
	string result;
	System.Net.WebRequest request = System.Net.WebRequest.Create("http://test-domain.com");
	request.Method = "POST";
	Stream reqStream = request.GetRequestStream();
	using (StreamWriter sw = new StreamWriter(reqStream)) {
		sw.Write("Our test data query");
	}
	var responseTask = request.GetResponseAsync();
	var webResponse = await responseTask;
	try
	{
		using (StreamReader sr = new StreamReader(webResponse.GetResponseStream())) {
			result = await sr.ReadToEndAsync();
	}
	catch(System.Net.WebException e)
	{
		ProcessException(e);
	}
	return result;
}

void ProcessException(System.Net.WebException ex) {
switch(ex.Status) {
	case System.Net.WebExceptionStatus.ConnectFailure:
	case System.Net.WebExceptionStatus.ConnectionClosed:
	case System.Net.WebExceptionStatus.RequestCanceled:
	case System.Net.WebExceptionStatus.PipelineFailure:
	case System.Net.WebExceptionStatus.SendFailure:
	case System.Net.WebExceptionStatus.KeepAliveFailure:
	case System.Net.WebExceptionStatus.Timeout:
		Console.WriteLine("We should retry connection attempts");
	break;
	case System.Net.WebExceptionStatus.NameResolutionFailure:
	case System.Net.WebExceptionStatus.ProxyNameResolutionFailure:
	case System.Net.WebExceptionStatus.ServerProtocolViolation:
	case System.Net.WebExceptionStatus.ProtocolError:
		"Prevent further attempts and notify consumers tocheck URL configurations".Dump();
	break;
	case System.Net.WebExceptionStatus.SecureChannelFailure:
	case System.Net.WebExceptionStatus.TrustFailure:
		"Authentication or security issue. Prompt forcredentials and perhaps try again".Dump();
	break;
	default:
		"We don't know how to handle this. We should post the error message and terminate our current workflow.".Dump();
	break;
}