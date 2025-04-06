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
        [SerializeField] private List<SFXItem> sFXItems = new();
        private Dictionary<string, AudioClip[]> audioClipsPool = new();
        private Dictionary<string, float> volumeOverrides = new();
        [SerializeField] private AudioSource sFXAudioSource;
        private void Awake()
        {
            Instance = this;
            foreach(SFXItem item in sFXItems) {
                audioClipsPool.Add(item.name, item.clips);
                volumeOverrides.Add(item.name, item.volumeOverride);
            }
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
        public void PlayUISFX(string clipName)
        {
            AudioClip[] clipRequested;
            clipRequested = AudioClipExists(clipName);

            if(clipRequested == null) {
                #if UNITY_EDITOR
                    Debug.Log($"Clip was not found");
                #endif
                return;
            }

            AudioClip ac = clipRequested[Random.Range(0, clipRequested.Length)];
            sFXAudioSource.PlayOneShot(ac, volumeOverrides[clipName]);
        }
        public AudioClip[] AudioClipExists(string clipName)
        {
            if (audioClipsPool.TryGetValue(clipName, out AudioClip[] clipRequested))
                return clipRequested;
            else
                return null;
        }
    }

    [Serializable] public struct SFXItem 
    {
        public string name;
        public float volumeOverride;
        public AudioClip[] clips;
        public SFXItem(string name, AudioClip[] clips, float volumeOverride)
        {
            this.name = name;
            this.clips = clips;
            this.volumeOverride = volumeOverride;
        }
    }
    public static class SFXData
    {
        public static readonly string ButtonClick = "button-click";
        public static readonly string ButtonHover = "button-hover";
    }
}