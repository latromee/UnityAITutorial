using UnityEngine;

namespace BTree
{
    public class BehaviourTree : Node
    {
        public BehaviourTree()
        {
            name = "Tree";
        }

        public BehaviourTree(string name) { this.name = name; }

        public override Status Process()
        {
            return children[currentChild].Process();
        }

        public struct NodeLevel
        {
            public int level;
            public Node node;

            public NodeLevel(Node n, int l = 0)
            {
                node = n;
                level = l;
            }
        }
        public void PrintTree(NodeLevel root)
        {
            int level = root.level;
            if (root.node.children.Count > 0)
            {
                foreach (var child in root.node.children)
                {
                    PrintTree(new NodeLevel(child, level + 1));
                }
            }
            Debug.Log(new string('-', level) + root.node.name);
        }
    }
}