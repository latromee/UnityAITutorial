using UnityEngine;
using UnityEngine.AI;

namespace AutonomouslyAgents
{
    public class Bot : MonoBehaviour
    {
        NavMeshAgent agent;
        GameObject target;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            target = World.cop;
        }

        void Seek(Vector3 location)
        {
            Debug.DrawLine(transform.position, location);
            agent.SetDestination(location);
        }

        void Pursue()
        {
            Vector3 targetDir = target.transform.position - transform.position;

            float targetSpeed = target.GetComponent<Move>().currentSpeed;

            float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
            float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));

            if ((toTarget > 90 && relativeHeading < 20) || targetSpeed < 0.01f)
            {
                Seek(target.transform.position);
                return;
            }

            Vector3 targetVelocity = target.transform.forward * targetSpeed;

            Vector3 predictedDir = targetVelocity + targetDir;
            float predictedTime = predictedDir.magnitude / agent.speed;

            Vector3 predictedLocation = target.transform.position + targetVelocity * predictedTime;
            Seek(predictedLocation);
        }

        void Flee(Vector3 location)
        {
            Vector3 direction = location - transform.position;
            Vector3 fleeLocation = transform.position - direction;

            agent.SetDestination(fleeLocation);
        }

        void Evade()
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float targetSpeed = target.GetComponent<Move>().currentSpeed;
            Vector3 targetVelocity = target.transform.forward * targetSpeed;

            Vector3 predictedDir = targetVelocity + targetDir;
            float predictedTime = predictedDir.magnitude / agent.speed;

            Vector3 predictedLocation = target.transform.position + targetVelocity * predictedTime;
            Flee(predictedLocation);
        }

        Vector3 wanderTarget = Vector3.zero;
        void Wander()
        {
            float wanderRadius = 10f;
            float wanderDistance = 10f;
            float wanderJitter = 1f;

            wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter,
                0f,
                Random.Range(-1f, 1f) * wanderJitter);
            wanderTarget.Normalize();
            wanderTarget *= wanderRadius;

            Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
            Vector3 targetWorld = transform.InverseTransformVector(targetLocal);

            Seek(targetWorld);
        }

        void Hide()
        {
            float dist = Mathf.Infinity;
            Vector3 closestSpot = Vector3.zero;

            for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
            {
                Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
                Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10f;

                if (Vector3.Distance(transform.position, hidePos) < dist)
                {
                    closestSpot = hidePos;
                    dist = Vector3.Distance(transform.position, hidePos);
                }
            }

            Seek(closestSpot);
        }

        void CleverHide()
        {
            float dist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;
            Vector3 chosenDir = Vector3.zero;
            GameObject chosenGo = World.Instance.GetHidingSpots()[0];

            for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
            {
                Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
                Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 50f;

                if (Vector3.Distance(transform.position, hidePos) < dist)
                {
                    chosenSpot = hidePos;
                    chosenDir = hideDir;
                    chosenGo = World.Instance.GetHidingSpots()[i];
                    dist = Vector3.Distance(transform.position, hidePos);
                }
            }

            Collider hideCol = chosenGo.GetComponent<Collider>();
            Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
            RaycastHit hit;
            float rayDist = 100f;
            hideCol.Raycast(backRay, out hit, rayDist);


            Seek(hit.point + chosenDir.normalized * 5f);
        }

        bool CanSeeTarget()
        {
            RaycastHit hit;
            Vector3 rayToTarget = target.transform.position - transform.position;

            if (Physics.Raycast(transform.position, rayToTarget, out hit))
            {
                if (hit.transform.tag == "cop")
                {
                    return true;
                }
            }
            return false;
        }

        bool TargetCanSeeMe()
        {
            Vector3 toAgent = transform.position - target.transform.position;
            float lookingAngle = Vector3.Angle(target.transform.forward, toAgent);

            return lookingAngle < 60f;
        }

        bool coolDown = false;

        void BehaviourCoolDown()
        {
            coolDown = false;
        }

        bool TargetInRange()
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            return distance < 20f;
        }

        private void Update()
        {

            if (!TargetInRange()) Wander();
            else if (!coolDown)
            {
                if (CanSeeTarget() && TargetCanSeeMe())
                {
                    CleverHide();
                    coolDown = true;
                    Invoke("BehaviourCoolDown", 5f);
                }
                else Pursue();
            }
        }
    }
}