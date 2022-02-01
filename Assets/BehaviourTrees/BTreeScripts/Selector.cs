using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    public class Selector : Node
    {
        public Selector(string n)
        {
            name = n;
        }
        public override Status Process()
        {
            Status childStatus = children[currentChild].Process();

            Debug.Log(children[currentChild].name + "  " + childStatus);

            if (childStatus == Status.RUNNING) return childStatus;
            if (childStatus == Status.SUCCESS)
            {
                currentChild = 0;
                return Status.SUCCESS;
            }

            currentChild++;
            if (currentChild >= children.Count)
            {
                currentChild = 0;
                return Status.FAILURE;
            }
            return Status.RUNNING;
        }
    }
}