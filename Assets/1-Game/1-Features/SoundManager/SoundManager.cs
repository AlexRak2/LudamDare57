using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        private List<AudioSource> _audioSources = new List<AudioSource>();

        private void Awake()
        {
            Instance = this;
        }

        public static AudioSource PlayWorld(AudioClip clip, Vector3 worldPos, float volume = 1f, float pitch = 1f, bool randomPitch = true, float randomPitchAmount = 0.1f, float maxDistance = 100f)
        {
            if (!Instance) return null;
            AudioSource audioSource = CreateWorldAudioSource(worldPos, maxDistance);
            audioSource.pitch = randomPitch ? pitch - randomPitchAmount + Random.Range(-randomPitchAmount, randomPitchAmount) : pitch;
            audioSource.PlayOneShot(clip, volume);
            Instance._audioSources.Add(audioSource);
            Instance.StartCoroutine(Instance.WaitForSound(audioSource));
            return audioSource;
        }
        
        private static AudioSource CreateWorldAudioSource(Vector3 worldPos, float maxDistance)
        {
            GameObject go = new GameObject();
            AudioSource source = go.AddComponent<AudioSource>();
            source.spatialBlend = 1f;
            source.transform.position = worldPos;
            source.maxDistance = maxDistance;
            return source;
        }
        
        private IEnumerator WaitForSound(AudioSource source)
        {
            yield return new WaitUntil(() => !source || !source.gameObject || source.isPlaying == false);
            if (source && source.gameObject)
            {
                Instance._audioSources.Remove(source);
                Destroy(source.gameObject);
            }
        }
    }
}