using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemySound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip attackClip;
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip hitClip;
        [SerializeField] private AudioClip[] footstepClips;

        public void PlayAttackSound()
        {
            PlaySound(attackClip);
        }

        public void PlayHitSound()
        {
            PlaySound(hitClip);
        }

        public void PlayDeathSound()
        {
            PlaySound(deathClip);
        }

        public void PlayFootstepSound()
        {
            if (footstepClips.Length > 0)
            {
                PlaySound(footstepClips[Random.Range(0, footstepClips.Length)]);
            }
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip && audioSource)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
