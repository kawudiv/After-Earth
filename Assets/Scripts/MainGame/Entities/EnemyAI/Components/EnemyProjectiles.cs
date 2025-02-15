using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemyProjectile : MonoBehaviour
    {
        public float speed = 10f;
        public float damage = 10f;
        public float lifetime = 5f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // var playerHealth = other.GetComponent<PlayerHealth>();
                // if (playerHealth)
                // {
                //     playerHealth.TakeDamage(damage);
                // }
                Destroy(gameObject);
            }
        }
    }
}
