using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCore.ObjectPool
{
    public interface IObjectPoolReusable 
    {
        void ClearForReuse();
    }
}
