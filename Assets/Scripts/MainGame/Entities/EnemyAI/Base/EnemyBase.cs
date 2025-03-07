using EnemyAI.Components; // Include Components
using EnemyAI.States.Common; // Include the Common States namespace
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI.Base
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("General Settings")]
        public float patrolSpeed;
        public float chaseSpeed;
        public float attackRange;
        public float detectionRange;

        [Header("Stats")]
        public float health;
        public float armor;
        public float attackDamage;
        public float attackCooldown;
        public float criticalChance;
        public float criticalDamage;

        [Header("Components")]
        public Transform target;
        public NavMeshAgent agent;
        public Animator animator;
        public EnemySensor enemySensor;
        protected EnemyHealth enemyHealth;
        protected EnemyAnimation enemyAnimation;
        protected EnemySound enemySound;
        protected EnemyRagdoll enemyRagdoll;

        public StateMachine stateMachine { get; private set; }

        // States
        public IdleState IdleState { get; private set; }
        public PatrolState PatrolState { get; private set; }

        // Abstract States
        public abstract State ChaseState { get; }
        public abstract State AttackState { get; }

        // Enemy General Settings
        public float PatrolSpeed => patrolSpeed;
        public float ChaseSpeed => chaseSpeed;
        public float AttackRange => attackRange;
        public float DetectionRange => detectionRange;

        // Enemy Stats Settings
        public float Health => health;
        public float Armor => armor;
        public float AttackDamage => attackDamage;
        public float AttackCooldown => attackCooldown;
        public float CriticalChance => criticalChance;
        public float CriticalDamage => criticalDamage;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            enemyHealth = GetComponent<EnemyHealth>();
            enemyAnimation = GetComponent<EnemyAnimation>();
            enemySound = GetComponent<EnemySound>();
            enemySensor = GetComponent<EnemySensor>();
            enemyRagdoll = GetComponent<EnemyRagdoll>();

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("No GameObject with tag 'Player' found in the scene!");
                target = null; // Handle this case gracefully in states
            }

            stateMachine = new StateMachine();

            // Common states
            IdleState = new IdleState(this, stateMachine);
            PatrolState = new PatrolState(this, stateMachine);
        }

        protected virtual void Start()
        {
            stateMachine.Initialize(IdleState);
        }

        protected virtual void Update()
        {
            stateMachine.currentState?.LogicUpdate();

            if (!enemySensor.CanSeePlayer() && stateMachine.currentState == ChaseState)
            {
                Debug.Log(
                    "Lost sight of player. Returning to PatrolState.{enemySensor.CanSeePlayer}"
                );
                stateMachine.ChangeState(PatrolState);
            }
        }

        protected virtual void FixedUpdate()
        {
            stateMachine.currentState?.PhysicsUpdate();
        }
    }
}
