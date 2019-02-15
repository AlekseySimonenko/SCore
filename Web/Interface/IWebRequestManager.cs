using System;
using System.Runtime.CompilerServices;

namespace SCore.Web
{
    public interface IWebRequestManager
    {
        void Request(string _url, Action<object> successCallbackFunction = null, Action<object> failCallbackFunction = null, float timeLimitSeconds = 10, [CallerMemberName] string callerName = "");
    }
}