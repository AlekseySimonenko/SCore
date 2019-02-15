namespace SCore.Music
{
    public interface IMusicManager
    {
        void Pause();
        void Play();
        void RestartPlaylist();
        void StartPlaylist(int _ID);
        void StartPlaylistInstance(int _ID);
    }
}