using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; 
    public float moveSpeed = 3f; 
    public float detectionRange = 10f; 
    public float fightRange = 2f; 

    private Animator animator; 

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        
        if (distanceToPlayer < detectionRange)
        {
            
            MoveTowardsPlayer();
            animator.SetBool("isMoving", true);

            if (distanceToPlayer < fightRange)
            {
                animator.SetBool("isInRange", true);
                animator.SetBool("isWalking", false); 
            }
            else
            {
                animator.SetBool("isInRange", false);
                animator.SetBool("isWalking", true); 
            }
        }
        else
        {
            
            animator.SetBool("isMoving", false);
            animator.SetBool("isInRange", false);
            animator.SetBool("isWalking", false);
        }
    }

    private void MoveTowardsPlayer()
    {
       
        Vector3 direction = (player.position - transform.position).normalized;

        
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}