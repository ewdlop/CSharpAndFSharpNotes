<Query Kind="Statements" />

int port = 54321;
System.Net.IPAddress address = System.Net.IPAddress.Parse("127.0.0.1");
using (System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient()) {
	client.Connect(address, port);
	if (client.Connected) {
		"Connected from the client".Dump();
		var message = "Hello server | Return this payload to sender!";
		var bytes = Encoding.UTF8.GetBytes(message);
		using (var requestStream = client.GetStream()) 
		{
			await requestStream.WriteAsync(bytes, 0, bytes.Length);
			string.Format("Send: {0}", message);
			var responseBytes = new byte[256];
            await requestStream.ReadAsync(responseBytes, 0, responseBytes.Length);
            var responseMessage = Encoding.UTF8.GetString(responseBytes);
		}
	}
}