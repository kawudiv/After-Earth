using EnemyAI.States.Common; // Include the Common States namespace
using EnemyAI.Components; // Include Components
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI.Base
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("General Settings")]
        public float patrolSpeed = 2f;
        public float chaseSpeed = 4f;
        public float attackRange = 2f;
        public float detectionRange = 10f;

        [Header("Stats")]
        public float maxHealth = 100f;
        public float health;
        public float armor = 5f;
        public float attackDamage = 15f;
        public float attackCooldown = 1.5f;
        public float criticalChance = 0.1f;
        public float criticalDamage = 2f;

        [Header("Components")]
        public Transform target;
        public NavMeshAgent agent;
        public Animator animator;
        public EnemySensor enemySensor;
        protected EnemyHealth enemyHealth;
        protected EnemyAnimation enemyAnimation;
        protected EnemySound enemySound;

        public StateMachine stateMachine { get; private set; }

        // States
        public IdleState IdleState { get; private set; }
        public PatrolState PatrolState { get; private set; }

        // Abstract States
        public abstract State ChaseState { get; }
        public abstract State AttackState { get; }

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            enemyHealth = GetComponent<EnemyHealth>();
            enemyAnimation = GetComponent<EnemyAnimation>();
            enemySound = GetComponent<EnemySound>();
            enemySensor = GetComponent<EnemySensor>();

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

            // Set up health events
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += HandleDeath;
                enemyHealth.OnHealthChanged += HandleHealthChange;
            }

            stateMachine = new StateMachine();

            // Common states
            IdleState = new IdleState(this, stateMachine);
            PatrolState = new PatrolState(this, stateMachine);
        }

        protected virtual void Start()
        {
            stateMachine.Initialize(IdleState);
            health = maxHealth;
        }

        protected virtual void Update()
        {
            stateMachine.currentState?.LogicUpdate();

            if (!CanSeePlayer() && stateMachine.currentState == ChaseState)
            {
                Debug.Log("Lost sight of player. Returning to PatrolState.");
                stateMachine.ChangeState(PatrolState);
            }
        }

        protected virtual void FixedUpdate()
        {
            stateMachine.currentState?.PhysicsUpdate();
        }

        protected virtual void HandleDeath()
        {
            Debug.Log($"{name} has died.");
            enemyAnimation?.PlayDeath();
            enemySound?.PlayDeathSound();
            Destroy(gameObject, 2f);
        }

        protected virtual void HandleHealthChange(float current, float max)
        {
            Debug.Log($"{name} health changed: {current}/{max}");
            if (current <= 0)
            {
                HandleDeath();
            }
        }
        private bool CanSeePlayer()
        {
            if (target == null) return false;
            return Vector3.Distance(transform.position, target.position) <= detectionRange;
        }
        
    }
}
