public class HeartbeatManager
{
    private readonly Node _node;
    private readonly int _totalNodes;
    private bool _isRunning = true;

    public HeartbeatManager(Node node, int totalNodes)
    {
        _node = node;
        _totalNodes = totalNodes;
    }

    //send heartbeat messages when leader
    public void StartSendingHeartbeats()
    {
        Console.WriteLine($"Node {_node.NodeId}: Sending heartbeats as leader...");
        Task.Run(() => // on separate thread
        {
            while (_isRunning)
            {
                for (int i = 0; i < _totalNodes; i++)
                {
                    if (i == _node.NodeId) continue; // skip sending to itself
                    _node.SendMessage("HEARTBEAT", 10000 + i); // send heartbeat to the current node
                }
                Thread.Sleep(1000); // send heartbeat every second
            }
        });
    }

    public void Stop()
    {
        _isRunning = false;
    }

    //check if the hearbeat is received
    public void MonitorHeartbeats(Action onTimeout)
    {
        Task.Run(() =>
        {
            while (_isRunning)
            {
                if ((DateTime.Now - _node.LastHeartbeatReceived).TotalMilliseconds > 3000)
                {
                    Console.WriteLine($"Node {_node.NodeId}: Heartbeat timeout! Starting election...");
                    onTimeout?.Invoke(); // new election if triggered
                }
                Thread.Sleep(500); // check hearbeat every 0.5 seconds
            }
        });
    }
}