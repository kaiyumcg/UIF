using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    //todo precomputed data in editor time which can be consumed by runtime
    public interface ILoadReference
    {
        void LoadReference();
        void InitIfRequired();
    }
}