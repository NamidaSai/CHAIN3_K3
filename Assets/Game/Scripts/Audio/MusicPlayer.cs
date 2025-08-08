using System;
using System.Collections;
using Game.Dialogue;
using UnityEngine;

namespace Game.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private Sound[] tracks = null;
        [SerializeField] private string startingTrack = null;
        [Tooltip("Time it takes to fade out the current track in seconds")]
        [SerializeField] private float decayRate = 1f;
        
        private bool _isSwitchingTracks = false;
        private Sound _currentTrack = null;
        private Sound _nextTrack = null;

        private void Start()
        {
            foreach (Sound track in tracks)
            {
                track.source = gameObject.AddComponent<AudioSource>();
                track.source.clip = track.clip;
                track.source.pitch = track.pitch;
                track.source.loop = track.loop;
                track.source.volume = track.volume;
            }
            
            DialogueSystem.Instance.onDialogueEnd.AddListener(HandleDialogueEnd);
            HandleDialogueEnd();
        }

        private void OnDestroy()
        {
            DialogueSystem.Instance.onDialogueEnd.RemoveListener(HandleDialogueEnd);
        }

        private void HandleDialogueEnd()
        {
            foreach (Sound track in tracks)
            {
                string flagToCheck = track.name;
                if (!CHAIN_SharedData.DoesFlagExist(flagToCheck)) { continue; }
                Play(track.name);
                return;
            }
            
            Play(startingTrack);
        }

        private void Update()
        {
            if (!_isSwitchingTracks) return;
        
            if (_currentTrack.source.volume > 0f)
            {
                _currentTrack.source.volume -= Time.deltaTime * _currentTrack.volume / decayRate;
            }
            
            if (_nextTrack.source.volume < _nextTrack.volume)
            {
                _nextTrack.source.volume += Time.deltaTime * _currentTrack.volume / decayRate;
            }
        }

        private void Play(string trackName)
        {
            Sound track = Array.Find(tracks, trackClip => trackClip.name == trackName);

            if (track == null)
            {
                Debug.LogWarning("Sound: " + trackName + " not found.");
                return;
            }

            if (track.source == null)
            {
                return;
            }

            if (track == _currentTrack && _currentTrack.source.isPlaying)
            {
                return;
            }

            if (_currentTrack != null && _currentTrack.source.isPlaying)
            {
                StartCoroutine(SwitchTrack(track));
                return;
            }

            track.source.Play();
            track.source.mute = false;
            track.source.volume = track.volume;
            _currentTrack = track;
        }

        private IEnumerator SwitchTrack(Sound newTrack)
        {
            _nextTrack = newTrack;
            _isSwitchingTracks = true;

            if (!_nextTrack.source.isPlaying)
            {
                _nextTrack.source.Play();
            }

            _nextTrack.source.mute = false;
            _nextTrack.source.volume = 0f;
            yield return new WaitWhile(() => _currentTrack.source.volume > 0);
            _isSwitchingTracks = false;
            _currentTrack.source.mute = true;
            _currentTrack = _nextTrack;

        }

        public void SetMusicVolume(float value)
        {
            foreach (Sound track in tracks)
            {
                track.source.volume = track.volume * value;
            }
        }
    }
}