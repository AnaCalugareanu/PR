public class LeaderElection
{
    private readonly Node _node;
    private readonly int _totalNodes;
    private bool _isLeader = false;
    private readonly Random _random = new Random();

    public LeaderElection(Node node, int totalNodes)
    {
        _node = node;
        _totalNodes = totalNodes;
    }

    //election of the leader
    public void StartElection(Action onElectedLeader)
    {
        Console.WriteLine($"Node {_node.NodeId}: Starting election...");
        int votes = 1; // vote for itself
        for (int i = 0; i < _totalNodes; i++)
        {
            if (i == _node.NodeId) continue; // skip vote for itself
            _node.SendMessage("VOTE_REQUEST", 10000 + i); // vote for another one
        }

        Thread.Sleep(1000); // simulate wait for votes
        votes += _random.Next(0, _totalNodes - 1); // random votes received
        if (votes > _totalNodes / 2) // check if the node received the majority
        {
            Console.WriteLine($"Node {_node.NodeId}: Elected as leader with {votes} votes!");
            _isLeader = true;//node is the leader
            onElectedLeader?.Invoke();// notify leader
        }
    }

    public bool IsLeader => _isLeader;
}