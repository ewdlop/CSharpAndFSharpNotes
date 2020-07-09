<Query Kind="Statements" />

System.Net.IPAddress address = System.Net.IPAddress.Any;
int port = 54321;
System.Net.Sockets.TcpListener server = new System.Net.Sockets.TcpListener(address, port);
byte[] bytes = new byte[256];
            
using (var client = await server.AcceptTcpClientAsync()) 
{
	using (var tcpStream = client.GetStream()) 
	{
		await tcpStream.ReadAsync(bytes, 0, bytes.Length);
		var requestMessage = Encoding.UTF8.GetString(bytes);
		Console.WriteLine(requestMessage);
	}
}
server.Stop();