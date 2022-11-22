using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Linq;
using System.Threading.Tasks;
using GyroscopeControl;
using UnityEngine.UI;

public class ConnectToCom : MonoBehaviour
{
    #region parameters
    public string portName = "";
    private SerialPort gyroPort;
    public GameObject xCube;
    public GameObject yCube;
    private DeviceFinder deviceFinder;
    private GyroDataHandler gyroDataHandler;
    private GyroAngleHandler gyroAngleHandler;
    private bool readyToConnect = false;
    public Text text;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        deviceFinder = new DeviceFinder();
        FindDevices();
        while (!readyToConnect)
        {
            Task.Delay(100);
        }
        Connect();
        gyroPort.Write("bla");
        gyroDataHandler = new GyroDataHandler(gyroPort);
        gyroAngleHandler = new GyroAngleHandler(gyroDataHandler);
    }

    private async void FindDevices()
    {
        await FindDevicesTask();
    }

    private Task FindDevicesTask()
    {
        return Task.Run(async () =>
        {
            while (deviceFinder.TargetDeviceList.Count == 0)
            {
                deviceFinder.DeviceChecker();
                await Task.Delay(100);
            }
            portName = deviceFinder.TargetDeviceList[0];
            readyToConnect = true;
        });
    }

    public bool Connect()
    {
        gyroPort = new SerialPort()
        {
            PortName = portName,
            BaudRate = 9600,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            ReadTimeout = 100,
            WriteTimeout = 100
        };
        try
        {
            gyroPort.Open();
            Debug.Log("Port is opened");
            return gyroPort.IsOpen;
        }
        catch(Exception ex)
        {
            string message = ex.Message;
            Debug.Log(message);
            return gyroPort.IsOpen;
        }
    }

    public void Close()
    {
        gyroPort.Close();
    }

    private Vector3 rodriguesRotation(Vector3 v, Vector3 k, float angle)
    {
        return v * (float)Math.Cos(angle) + Vector3.Cross(k, v) * (float)Math.Sin(angle);

    }

    // Update is called once per frame
    void Update()
    {
        // roll - x, pitch - y, yaw - z
        text.text = "ROLL " + (gyroAngleHandler.Roll /* (float)(180/Math.PI)*/).ToString() + "\n" + "YAW " + (gyroAngleHandler.Yaw /* (float)(180/Math.PI)*/).ToString() + "\n" + "PITCH " + (gyroAngleHandler.Pitch /* (float)(180/Math.PI)*/).ToString();
        xCube.transform.Rotate(-1*gyroAngleHandler.Roll * Time.deltaTime, gyroAngleHandler.Yaw * Time.deltaTime, gyroAngleHandler.Pitch * Time.deltaTime, Space.World);
    }
}
