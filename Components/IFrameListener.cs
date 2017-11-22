using UnityEngine;

namespace Core
{

    //// <summary>
    /// This behavior attached to dummy object on the scene. 
    /// All iframe events on web platform send in object with "iframeListener" name.
    /// (Dont Destroyed On Load)
    /// </summary>
    [DisallowMultipleComponent]
    public class IFrameListener : MonoBehaviour
    {

        void Awake()
        {

        }

        //// <summary>
        /// Detect web platfrom from iframe
        /// </summary>
        public void OnGetWebPlatform(string _data)
        {
            Debug.Log("IFrameListener.OnGetWebPlatform " + _data);
            PlatformManager.OnGetWebPlatform(_data);
        }


        //// <summary>
        /// We just get config of web platform from iframe
        /// </summary>
        public void OnGetPlatformConfig(string _data)
        {
            Debug.Log("IFrameListener.OnGetPlatformConfig " + _data);
            PlatformManager.OnGetPlatformConfig(_data);
        }


        //// <summary>
        /// We just get all friends from iframe
        /// </summary>
        public void OnGetFriendsInfo(string _data)
        {
            Debug.Log("IFrameListener.OnGetFriendsInfo ");
            PlatformManager.OnGetFriendsInfo(_data);
        }

        //// <summary>
        /// We just get callback from web about payment succesfull
        /// </summary>
        public void OnGetPaymentSuccessfull()
        {
            Debug.Log("IFrameListener.OnGetPaymentSuccessfull");
            PlatformManager.OnGetPaymentSuccessfull();
        }


        //// <summary>
        /// We just get callback from web about payment fail
        /// </summary>
        public void OnGetPaymentFail()
        {
            Debug.Log("IFrameListener.OnGetPaymentFail");
            PlatformManager.OnGetPaymentFail();
        }


        //// <summary>
        /// We just get callback from web about friend invite succesfull
        /// </summary>
        public void OnGetInviteSuccessfull()
        {
            Debug.Log("IFrameListener.OnGetInviteSuccessfull");
            PlatformManager.OnGetInviteSuccessfull();
        }


        //// <summary>
        /// We just get callback from web about friend invite fail
        /// </summary>
        public void OnGetInviteFail()
        {
            Debug.Log("IFrameListener.OnGetInviteFail");
            PlatformManager.OnGetInviteFail();
        }


        //// <summary>
        /// We just get callback from web about friend request succesfull
        /// </summary>
        public void OnGetRequestSuccessfull()
        {
            Debug.Log("IFrameListener.OnGetRequestSuccessfull");
            PlatformManager.OnGetRequestSuccessfull();
        }


        //// <summary>
        /// We just get callback from web about friend request fail
        /// </summary>
        public void OnGetRequestFail()
        {
            Debug.Log("IFrameListener.OnGetRequestFail");
            PlatformManager.OnGetRequestFail();
        }


        //// <summary>
        /// We just get callback from web about sharing succesfull
        /// </summary>
        public void OnShareSuccessfull()
        {
            Debug.Log("IFrameListener.OnShareSuccessfull");
            PlatformManager.OnShareSuccessfull();
        }


        //// <summary>
        /// We just get callback from web about sharing fail
        /// </summary>
        public void OnShareFail()
        {
            Debug.Log("IFrameListener.OnShareFail");
            PlatformManager.OnShareFail();
        }


    }
}