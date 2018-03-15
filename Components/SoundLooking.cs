using UnityEngine;

namespace SCore
{
    /// <summary>
    /// Sound volume fallof when main camera not looking on sound
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundLooking : MonoBehaviour
    {
        private AudioSource audioSource;
        private float startVolume = 0.0F;
        private bool isLookingOn = false;
        private const float speedVolumeChange = 1.0f;

        //How othen we check looking on sound
        private float timer = 0.0f;
        private const float timerDelay = 0.5f;

        void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            startVolume = audioSource.volume;
            audioSource.volume = 0.0F;
        }

        void Update()
        {
            //Looking timer
            timer -= Time.unscaledDeltaTime;
            if (timer <= 0.0f)
            {
                timer = timerDelay;
                CheckLooking();
            }

            //Soft volume chnaging
            if (isLookingOn)
            {
                if (audioSource.volume < startVolume)
                    audioSource.volume += Time.unscaledDeltaTime * speedVolumeChange;
            }
            else
            {
                if (audioSource.volume > 0.0F)
                    audioSource.volume -= Time.unscaledDeltaTime * speedVolumeChange;
                else
                    audioSource.volume = 0.0F;
            }
        }

        private void CheckLooking()
        {
            if (Vector3.Distance(gameObject.transform.position, Camera.main.transform.position) < audioSource.maxDistance)
            {
                Vector3 screenPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
                if (screenPos.x > 0.0F && screenPos.x < 1.0F && screenPos.y > 0.0F && screenPos.y < 1.0F && screenPos.z > 0.0F)
                    isLookingOn = true;
                else
                    isLookingOn = false;
            }else
            {
                isLookingOn = false;
            }
        }

    }
}
