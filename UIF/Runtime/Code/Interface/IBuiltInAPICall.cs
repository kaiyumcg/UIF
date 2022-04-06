using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public interface IBuiltInAPICall
    {
        public Vector3 InitPosition { get; }
        public Vector3 InitLocalPosition { get; }
        public Quaternion InitRotation { get; }
        public Quaternion InitLocalRotation { get; }
        public Vector3 InitAngle { get; }
        public Vector3 InitLocalAngle { get; }
        public Vector3 InitScale { get; }
        public Vector3 InitLocalScale { get; }
        public UIFStateInfo StateInfo { get; }
        public Transform _Transform { get; }
        public GameObject _GameObject { get; }
    }
}