using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BTree
{
    public class RobberBehaviour : MonoBehaviour
    {
        BehaviourTree tree;
        public List<GameObject> valuables;
        public GameObject van;
        public GameObject frontDoor;
        public GameObject backDoor;

        NavMeshAgent agent;
        public enum ActionState { IDLE, WORKING };
        ActionState state = ActionState.IDLE;

        Node.Status treeStatus = Node.Status.RUNNING;

        [Range(0, 3000)]
        public int money = 800;
        public int moneyToStopStealing = 1000;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            valuables = new List<GameObject>(GameObject.FindGameObjectsWithTag("valuable"));
            CreateBehaviourTree();
        }

        void CreateBehaviourTree()
        {
            tree = new BehaviourTree();
            Sequence steal = new Sequence("Steal something");
            Leaf goToFrontDoor = new Leaf("Go to back door", GoToFrontDoor);
            Leaf goToBackDoor = new Leaf("Go to back door", GoToBackDoor);
            Leaf hasGotMoney = new Leaf("Has got money", HasNoMoney);
            Leaf goToValuable = new Leaf("Go to valuable", GoToValuable);
            Leaf goToVan = new Leaf("Go to van", GoToVan);
            Selector openDoor = new Selector("Open door");


            openDoor.AddChild(goToBackDoor);
            openDoor.AddChild(goToFrontDoor);

            steal.AddChild(hasGotMoney);
            steal.AddChild(openDoor);
            steal.AddChild(goToValuable);
            //steal.AddChild(goToBackDoor);
            steal.AddChild(goToVan);
            tree.AddChild(steal);

            tree.PrintTree(new BehaviourTree.NodeLevel(tree));
        }

        public Node.Status HasNoMoney()
        {
            if (money >= moneyToStopStealing)
                return Node.Status.FAILURE;
            return Node.Status.SUCCESS;
        }

        public Node.Status GoToValuable()
        {
            if(valuables.Count == 0) return Node.Status.FAILURE;

            GameObject valuable = valuables[0];
            Node.Status result = GoToLocation(valuable.transform.position);

            if (result == Node.Status.SUCCESS)
            {
                valuable.transform.parent = transform;
                valuable.transform.localPosition = Vector3.up * 2f;
                valuable.transform.localScale *= 0.5f;
            }
            return result;
        }
        public Node.Status GoToVan()
        {
            GameObject valuable = valuables[0];
            Node.Status result = GoToLocation(van.transform.position);

            if (result == Node.Status.SUCCESS && valuable.transform.parent == transform)
            {
                money += valuable.GetComponent<Valuable>().cost;
                valuables.Remove(valuable);
                Destroy(valuable);
            }
            return result;
        }

        public Node.Status GoToBackDoor()
        {
            return GoToDoor(backDoor);
        }
        public Node.Status GoToFrontDoor()
        {
            return GoToDoor(frontDoor);
        }

        public Node.Status GoToDoor(GameObject door)
        {
            Node.Status s = GoToLocation(door.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                if (!door.GetComponent<Lock>().isLocked)
                {
                    door.SetActive(false);
                    return Node.Status.SUCCESS;
                }
                return Node.Status.FAILURE;
            }
            else
                return s;
        }

        Node.Status GoToLocation(Vector3 destination)
        {
            float distance = Vector3.Distance(destination, transform.position);
            if (state == ActionState.IDLE)
            {
                agent.SetDestination(destination);
                state = ActionState.WORKING;
            }
            else if (Vector3.Distance(agent.pathEndPosition, destination) >= 4f)
            {
                state = ActionState.IDLE;
                return Node.Status.FAILURE;
            }
            else if (distance < 4f)
            {
                state = ActionState.IDLE;
                return Node.Status.SUCCESS;
            }
            return Node.Status.RUNNING;
        }

        private void Update()
        {
            if (treeStatus != Node.Status.SUCCESS)
            {
                treeStatus = tree.Process();
            }
            else if (HasNoMoney() == Node.Status.SUCCESS)
            {
                Debug.Log("recreate tree");
                treeStatus = Node.Status.RUNNING;
                CreateBehaviourTree();
            }
        }
    }
}