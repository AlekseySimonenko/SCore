using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace Core
{
    /// <summary>
    /// Music mixer and control
    /// </summary>
    public class MusicController : MonoBehaviour
    {

        #region Public variables
        public MusicPlaylist[] playlists;
        public AudioMixer audioMixer;
        public int PlayOnAwakeID = -1;
        public bool DontDestroy;
        #endregion

        #region Public constants
        #endregion

        #region Private constants
        #endregion

        #region Private variables
        static public MusicController instance;
        static private AudioSource audioSource;
        // Playing now track in playlist.
        static private int playtrack = 0;
        static private int playlistID = -1;
        static private bool pause = false;
        #endregion


        void Awake()
        {
            if (DontDestroy)
                DontDestroyOnLoad(this.gameObject);
        }

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