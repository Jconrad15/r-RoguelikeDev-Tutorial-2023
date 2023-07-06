public class BinaryTree
{
    public Node rootNode;

    public BinaryTree(Node rootNode)
    {
        this.rootNode = rootNode;
    }

}

public class Node
{
    public Node ParentNode { get; set; }
    public Node LeftNode { get; set; }
    public Node RightNode { get; set; }
    public (int, int) XRange { get; set; }
    public (int, int) YRange { get; set; }

    public Node(Node parent, (int, int) xRange, (int, int) yRange)
    {
        ParentNode = parent;
        LeftNode = null;
        RightNode = null;
        XRange = xRange;
        YRange = yRange;
    }
}