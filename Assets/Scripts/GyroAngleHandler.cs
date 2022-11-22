using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GyroscopeControl
{
    public class GyroAngleHandler : IDisposable
    {
        private GyroDataHandler _gyroDataHandler;
        public float Yaw;
        public float Pitch;
        public float Roll;
        private float PreviousYaw;
        private float PreviousPitch;
        private float PreviousRoll;
        public GyroAngleHandler(GyroDataHandler gyroDataHandler)
        {
            _gyroDataHandler = gyroDataHandler;
            _gyroDataHandler.ReadyGyroDataHandler += ReadyDataHandler;

        }

        private float FakeClamp(float v1, float v2)
        {
            if (v1 <= 179)
                return Yaw;
            return 180;
        }
        
        private void ReadyDataHandler(object sender, ReadyGyroDataEventArgs e)
        {
            PreviousYaw = Yaw;
            PreviousPitch = Pitch;
            PreviousRoll = Roll;
            Yaw = e.Yaw;
            Pitch = e.Pitch;
            Roll = e.Roll;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

