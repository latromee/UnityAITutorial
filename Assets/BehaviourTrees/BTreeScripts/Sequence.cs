using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    public class Sequence : Node
    {
        public Sequence(string n)
        {
            name = n;
        }
        public override Status Process()
        {
            Status childStatus = children[currentChild].Process();

            Debug.Log(children[currentChild].name + "  " + childStatus);

            if(childStatus == Status.RUNNING) return childStatus;
            if (childStatus == Status.FAILURE) return childStatus;
            currentChild++;
            if (currentChild >= children.Count)
            {
                currentChild = 0;
                return Status.SUCCESS;
            }
            return Status.RUNNING   ;
        }
    }
}