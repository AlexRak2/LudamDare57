using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Player
{
    public class PlayerFootsteps : MonoBehaviour
    {
        public AudioClip[] footstepClips;
        public float stepInterval = 0.5f;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float stepTimer;
        [SerializeField] private bool _isWalking;
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void Update()
        { 
            _isWalking = PlayerMovement.Instance.IsGrounded && PlayerMovement.Instance.MoveInput.magnitude > 0.2f &&  !PlayerMovement.IsFrozen;
            if (_isWalking)
            {
                stepTimer += Time.deltaTime;

                if (stepTimer >= stepInterval)
                {
                    PlayFootstep();
                    stepTimer = 0f;
                }
            }
            else
            {
                stepTimer = stepInterval;
            }
        }

        void PlayFootstep()
        {
            if (footstepClips.Length == 0) return;

            int index = Random.Range(0, footstepClips.Length);
            _audioSource.PlayOneShot(footstepClips[index], 0.25f);
        }
    }
}