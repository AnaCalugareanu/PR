int totalNodes = 3;
var nodes = new Node[totalNodes];
var elections = new LeaderElection[totalNodes];
var heartbeats = new HeartbeatManager[totalNodes];

// Initialize nodes
for (int i = 0; i < totalNodes; i++)
{
    nodes[i] = new Node(i, 10000 + i);
}

for (int i = 0; i < totalNodes; i++)
{
    int currentNodeIndex = i;

    elections[i] = new LeaderElection(nodes[i], totalNodes);
    heartbeats[i] = new HeartbeatManager(nodes[i], totalNodes);

    nodes[i].Listen((message, endpoint) =>
    {
        if (message == "HEARTBEAT")
        {
            Console.WriteLine($"Node {currentNodeIndex}: Heartbeat received.");
            nodes[currentNodeIndex].LastHeartbeatReceived = DateTime.Now;
        }
        else if (message == "VOTE_REQUEST")
        {
            Console.WriteLine($"Node {currentNodeIndex}: Vote granted to {endpoint.Port - 10000}.");
            nodes[currentNodeIndex].SendMessage("VOTE_GRANTED", endpoint.Port);
        }
    });

    // Start heartbeat monitoring and leader election logic for each node
    heartbeats[i].MonitorHeartbeats(() =>
    {
        elections[currentNodeIndex].StartElection(() =>
        {
            heartbeats[currentNodeIndex].StartSendingHeartbeats();
        });
    });
}

Console.WriteLine("Press Enter to stop the simulation...");
Console.ReadLine();