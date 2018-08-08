using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace SCore.Music
{
    /// <summary>
    /// Music mixer and control
    /// </summary>
    public class MusicController : MonoBehaviour
    {
        //PUBLIC STATIC
        static public MusicController instance;

        //PUBLIC EVENTS

        //PUBLIC VARIABLES
        public MusicPlaylist[] playlists;
        public AudioMixer audioMixer;
        public int PlayOnAwakeID = -1;

        //PRIVATE STATIC
        static private AudioSource audioSource;
        // Playing now track in playlist.
        static private int playtrack = 0;
        static private int playlistID = -1;
        static private bool pause = false;

        //PRIVATE VARIABLES




        void Start()
        {
            instance = this;
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

        static public void RestartPlaylist()
        {
            playtrack = 0;
            PlayTrack();
        }

        static public void StartPlaylist(int _ID)
        {
            if (instance.playlists.Length > _ID)
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

        static private void PlayTrack()
        {
            if (playlistID > -1)
            {
                audioSource.clip = instance.playlists[playlistID].tracks[instance.playlists[playlistID].playlist[playtrack]];
                Play();
            }
        }

        static public void Play()
        {
            pause = false;
            audioSource.Play();
        }

        static public void Pause()
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