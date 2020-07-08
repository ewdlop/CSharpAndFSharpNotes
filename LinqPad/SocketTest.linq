<Query Kind="Statements" />

string GetRequestMessage(string server, int port, string path) {
	var message = $"GET {path} HTTP/1.1\r\n";
	message += $"Host: {server}:{port}\r\n";
	message += "cache-control: no-cache\r\n";
	message += "\r\n";
	return message;
}



string server = "localhost";
int port = 5000;
string path = "/api/values";
System.Net.Sockets.Socket socket = null;
System.Net.IPEndPoint endpoint = null;
var host = System.Net.Dns.GetHostEntry(server);
foreach (var address in host.AddressList) 
{
	try
	{
		socket = new System.Net.Sockets.Socket(address.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
		endpoint = new System.Net.IPEndPoint(address, port);
		//System.Net.Sockets.SocketAsyncEventArgs socketEventArg = new System.Net.Sockets.SocketAsyncEventArgs();
		//socketEventArg.RemoteEndPoint  = endpoint;
		//await socket.ConnectAsync();
		socket.Connect(endpoint);
		if (socket.Connected) {
			var message = GetRequestMessage(server, port, path);
			var messageBytes = Encoding.ASCII.GetBytes(message);
			var segment = new ArraySegment<byte>(messageBytes);
			//await socket.SendAsync();
			socket.Send(segment, System.Net.Sockets.SocketFlags.None);
			break;
		}
		
	}
	catch(System.Net.Sockets.SocketException e)
	{
		string.Format("Soruce: {0}", e.Source).Dump();
		string.Format("Message: {0}", e.Message).Dump();
	}
	catch(Exception e)
	{
		string.Format("Soruce: {0}", e.Source).Dump();
		string.Format("Message: {0}", e.Message).Dump();
	}
	socket.Close();
}

