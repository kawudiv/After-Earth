using Core;
using Player.Components;
using Player.States.Combat;
using Player.States.Combat.Common;
using Player.States.Movement;
using UnityEngine;
using Weapons.Base;

namespace Player.Base
{
    public abstract class PlayerBase : MonoBehaviour
    {
        [Header("General Settings")]
        public float walkSpeed = 2f;
        public float runSpeed = 4f;
        public float sprintSpeed = 6f;
        public float rollDuration = 1f; // Duration of the roll
        public float rollSpeed = 6f; // Speed of the roll
        public float rotationSpeed = 5f;

        [Header("Stats")]
        public float maxHealth = 100f;
        public float armor = 5f;
        public float attackDamage = 15f;
        public float attackCooldown = 1.5f;
        public float criticalChance = 0.5f;
        public float criticalDamage = 2f;

        [Header("Combat")]
        public bool IsWeaponDrawn { get; set; }

        [Header("Components")]
        public Animator animator;
        public CharacterController characterController;
        public PlayerHealth PlayerHealth { get; private set; }
        public PlayerRagdoll PlayerRagdoll { get; private set; }
        public PlayerAnimation PlayerAnimation { get; private set; }
        public PlayerSound PlayerSound { get; private set; }
        public PlayerInputHandler PlayerInputHandler { get; private set; }
        public WeaponBase EquippedWeapon { get; private set; }
        public PlayerInventory PlayerInventory { get; private set; }
        public PlayerCombat PlayerCombat { get; private set; }

        public StateMachine StateMachine { get; private set; }

        // Movement States
        public IdleState IdleState { get; private set; }
        public WalkState WalkState { get; private set; }
        public RunState RunState { get; private set; }
        public SprintState SprintState { get; private set; }
        public RollState RollState { get; private set; }

        // Combat States
        public ToggleWeaponState ToggleWeaponState { get; private set; }
        public AttackWeaponState AttackWeaponState { get; private set; }

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            PlayerHealth = GetComponent<PlayerHealth>();
            PlayerRagdoll = GetComponent<PlayerRagdoll>();
            PlayerAnimation = GetComponent<PlayerAnimation>();
            PlayerSound = GetComponent<PlayerSound>();
            PlayerInventory = GetComponent<PlayerInventory>();
            PlayerCombat = GetComponent<PlayerCombat>();

            StateMachine = new StateMachine();

            // Initialize Movement States
            IdleState = new IdleState(this, StateMachine);
            WalkState = new WalkState(this, StateMachine);
            RunState = new RunState(this, StateMachine);
            SprintState = new SprintState(this, StateMachine);
            RollState = new RollState(this, StateMachine);

            // Initialize Combat States
            ToggleWeaponState = new ToggleWeaponState(this, StateMachine);
            AttackWeaponState = new AttackWeaponState(this, StateMachine);
        }

        protected virtual void Start()
        {
            StateMachine.Initialize(IdleState);
        }

        protected virtual void Update()
        {
            StateMachine.CurrentState?.HandleInput();
            StateMachine.CurrentState?.LogicUpdate();

            if (
                PlayerInputHandler.IsMeleeDraw
                && !(StateMachine.CurrentState is ToggleWeaponState)
                && PlayerInventory.EquippedWeapon != null
            )
            {
                StateMachine.ChangeState(ToggleWeaponState);
            }

            if (
                PlayerInputHandler.IsAttack
                && !(StateMachine.CurrentState is AttackWeaponState)
                && PlayerInventory.EquippedWeapon != null
            )
            {
                PlayerInputHandler.SetIsAttack(false);
                StateMachine.ChangeState(AttackWeaponState);
            }
        }

        protected virtual void FixedUpdate()
        {
            StateMachine.CurrentState?.PhysicsUpdate();
        }

        public void EquipWeapon(WeaponBase newWeapon)
        {
            EquippedWeapon = newWeapon;
            Debug.Log($"Equipped new melee weapon: {newWeapon.name}");
        }
    }
}
