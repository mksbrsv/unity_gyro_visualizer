using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GyroscopeControl
{
    public class ReadyGyroDataEventArgs : EventArgs
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }    
        public float Roll { get; set; }
    }
}
