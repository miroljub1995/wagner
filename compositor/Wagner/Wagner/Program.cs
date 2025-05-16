using System.Net.Sockets;
using System.Text;
using Wagner;

var display = new WlGlobalDisplay();

await display.RunAsync(CancellationToken.None);

// var socketPath = Path.Join(Path.GetTempPath(), "wayland-0");
// var lockPath = socketPath + ".lock";
//
// Console.WriteLine($"Socket {socketPath}");
//
// try
// {
//     await using var lockStream = new FileStream(lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
//
//     var endPoint = new UnixDomainSocketEndPoint(socketPath);
//     var listener = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
//
//     listener.Bind(endPoint);
//     listener.Listen(5);
//
//     Console.WriteLine($"Listening on {socketPath}...");
//
//     AppDomain.CurrentDomain.ProcessExit += (s, e) =>
//     {
//         listener.Dispose();
//         File.Delete(socketPath);
//         File.Delete(lockPath);
//     };
//
//     while (true)
//     {
//         Socket handler = await listener.AcceptAsync();
//
//         _ = Task.Run(async () =>
//         {
//             var buffer = new byte[1024];
//             int bytesReceived = await handler.ReceiveAsync(buffer, SocketFlags.None);
//             string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
//
//             Console.WriteLine($"Received: {receivedText}");
//
//             string response = "Echo: " + receivedText;
//             byte[] responseBytes = Encoding.UTF8.GetBytes(response);
//             await handler.SendAsync(responseBytes, SocketFlags.None);
//
//             handler.Shutdown(SocketShutdown.Both);
//             handler.Close();
//         });
//     }
// }
// finally
// {
// }