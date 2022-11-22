using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using System;

namespace GyroscopeControl
{
    public class GyroDataHandler : IDisposable
    {
        private SerialPort _gyroDataPort;
        private bool ReceivedTaskRun {get; set;} = false;
        public EventHandler<ReadyGyroDataEventArgs> ReadyGyroDataHandler;

        private void onReadyGyroData(ReadyGyroDataEventArgs e)
        {
            EventHandler<ReadyGyroDataEventArgs> handler = ReadyGyroDataHandler;
            if (handler != null)
                handler(this, e);
        }

        public GyroDataHandler(SerialPort gyroPort)
        {
            _gyroDataPort = gyroPort;
            ReceivedTaskRun = true;
            DataReceiveTask();
        }

        private byte[] ReadData()
        {
            if (_gyroDataPort != null) 
            { 
                if (_gyroDataPort.BytesToRead > 0)
                {
                    byte[] data = new byte[12];
                    _gyroDataPort.Read(data, 0, data.Length);
                    return data;
                   //byte[] data = new byte[_gyroDataPort.BytesToRead];
                   //_gyroDataPort.Read(data, 0, data.Length);
                   //return data;
                }
            }
            return null;
        }

        private async void DataReceiveTask()
        {
            await Task.Run(async () => 
            {
                while (ReceivedTaskRun)
                {
                    if (_gyroDataPort.BytesToRead > 0)
                    { 
                        Debug.Log(_gyroDataPort.BytesToRead);
                        byte[] data = ReadData();
                        if (data != null)
                        {
                            List<float> gyroData = ParseData(data);
                            //List<float> gyroData = ParseData(new List<byte>(data));
                            onReadyGyroData(new ReadyGyroDataEventArgs()
                            {
                                Yaw = gyroData[0],
                                Pitch = gyroData[1],
                                Roll = gyroData[2],
                            });
                        }
                    }
                    else
                    {
                        await Task.Delay(100);
                    }
                }
            });
        }

        private List<float> ParseData(byte[] buffer)
        {
            List<float> data = new List<float>();
            byte[] byteData = new byte[4];
            byteData[0] = buffer[0];
            byteData[1] = buffer[1];
            byteData[2] = buffer[2];
            byteData[3] = buffer[3];
            data.Add(BitConverter.ToSingle(byteData, 0));
            byteData[0] = buffer[4];
            byteData[1] = buffer[5];
            byteData[2] = buffer[6];
            byteData[3] = buffer[7];
            data.Add(BitConverter.ToSingle(byteData, 0));
            byteData[0] = buffer[8];
            byteData[1] = buffer[9];
            byteData[2] = buffer[10];
            byteData[3] = buffer[11];
            data.Add(BitConverter.ToSingle(byteData, 0));
            return data;
           //List<float> data = new List<float>();
           //byte[] byteData = new byte[4];
           //for(int i = 0; i < buffer.Count; i+=4)
           //{
           //    byteData[0] = buffer[i];
           //    byteData[1] = buffer[i+1];
           //    byteData[2] = buffer[i+2];
           //    byteData[3] = buffer[i+3];
           //    data.Add(BitConverter.ToSingle(byteData, 0));
           //}
           //return data;
        }

        public void Dispose()
        {
        }
    }
}
