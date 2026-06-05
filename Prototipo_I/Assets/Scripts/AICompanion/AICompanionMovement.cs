using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace AICompanion
{
    public class AICompanionMovement : TweenMovement
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform target;
        [SerializeField, Range(0, 5)] private float arrivalDistance;
        [SerializeField, Range(0, 5)] private float chaseCooldown;
        private bool hasArrived => Vector3.Distance(this.transform.position, target.position) < arrivalDistance;
        private bool previousHasArrived;
        private float yPosition;
        private float currentChaseCooldown = 0;

        [Header("Leaning")]
        [SerializeField] private Tween<float> leanAngle;
        [SerializeField] private GameObject pivot;

        private NavMeshAgent nma;
        private NavMeshPath path;

        override protected void Start()
        {
            xAxis.startValue = this.transform.position.x;
            yPosition = this.transform.position.y;
            zAxis.startValue = this.transform.position.z;

            previousHasArrived = hasArrived;

            nma = GetComponent<NavMeshAgent>();
            path = new NavMeshPath();
        }

        void Update()
        {
            

            nma.CalculatePath(target.position, path);
            nma.SetPath(path);

            
            
        }
    }
}