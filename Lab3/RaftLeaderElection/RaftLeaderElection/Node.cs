using System.Net;
using System.Net.Sockets;
using System.Text;

public class Node
{
    public int NodeId { get; }
    public int Port { get; }
    public DateTime LastHeartbeatReceived { get; set; }

    public Node(int nodeId, int port)
    {
        NodeId = nodeId;
        Port = port;
        LastHeartbeatReceived = DateTime.Now;
    }

    //processes incoming messages(starts a UDP listener)
    public void Listen(Action<string, IPEndPoint> onMessageReceived)
    {
        Task.Run(() => // separate thread
        {
            using (var udpClient = new UdpClient(Port))//binf udp socket to node
            {
                Console.WriteLine($"Node {NodeId} listening on port {Port}...");
                while (true)
                {
                    var remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    var data = udpClient.Receive(ref remoteEndpoint);
                    var message = Encoding.UTF8.GetString(data);
                    onMessageReceived?.Invoke(message, remoteEndpoint); // call the handler to proccess the message
                }
            }
        });
    }

    //sends a UDP message to another node
    public void SendMessage(string message, int targetPort)
    {
        using (var udpClient = new UdpClient())
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, targetPort); // define the reciever address and port
            var data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, endpoint);//send the message
        }
    }
}