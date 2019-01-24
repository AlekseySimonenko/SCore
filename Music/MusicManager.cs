using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace SCore.Music
{
    /// <summary>
    /// Music mixer and control
    /// </summary>
    public class MusicManager : MonoBehaviourSingleton<MusicManager>
    {
        //PUBLIC EVENTS

        //PUBLIC VARIABLES
        public MusicPlaylist[] playlists;
        public AudioMixer audioMixer;
        public int PlayOnAwakeID = -1;

        //PRIVATE STATIC
        private AudioSource audioSource;
        // Playing now track in playlist.
        private int playtrack = 0;
        private int playlistID = -1;
        private bool pause = false;

        //PRIVATE VARIABLES

        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            if (PlayOnAwakeID > -1)
                StartPlaylist(PlayOnAwakeID);
        }

        void Update()
        {
            if (playlistID > -1 && pause == false && audioSource.isPlaying == false)
            {
                playtrack++;
                if (playtrack >= playlists[playlistID].playlist.Length)
                {
                    playtrack = 0;
                }
                PlayTrack();
            }
        }

        public void RestartPlaylist()
        {
            playtrack = 0;
            PlayTrack();
        }

        public void StartPlaylist(int _ID)
        {
            if (playlists.Length > _ID)
            {
                if (playlistID != _ID)
                {
                    playlistID = _ID;
                    RestartPlaylist();
                }
            }
            else
            {
                Debug.LogError("MusicController : playlist " + _ID + " not found");
            }
        }

        private void PlayTrack()
        {
            if (playlistID > -1)
            {
                audioSource.clip = playlists[playlistID].tracks[playlists[playlistID].playlist[playtrack]];
                Play();
            }
        }

        public void Play()
        {
            pause = false;
            audioSource.Play();
        }

        public void Pause()
        {
            pause = true;
            audioSource.Pause();
        }

        public void StartPlaylistInstance(int _ID)
        {
            StartPlaylist(_ID);
        }
    }
}