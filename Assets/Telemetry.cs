using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

public static class ReaderExtensions
{
    public static Vector3 ReadVector(this BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        return new Vector3(x, z, y);
    }

    public static Quaternion ReadQuaternion(this BinaryReader reader)
    {
        var x = -reader.ReadSingle();
        var y = -reader.ReadSingle();
        var z = -reader.ReadSingle();
        var w = reader.ReadSingle();
        return new Quaternion(x, z, y, w);
    }


    public static float[] ReadArray(this BinaryReader reader, int count)
    {
        float[] ret = new float[count];
        for (int i = 0; i < count; ++i)
        {
            ret[i] = reader.ReadSingle();
        }
        return ret;
    }


}

public class Telemetry : MonoBehaviour
{
    UdpClient udpClient;
    IPEndPoint RemoteIpEndPoint;

    void Start()
    {
        udpClient = new UdpClient();
        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 42424);
        udpClient.Client.Bind(remote);
    }

    void Update()
    {
        while (udpClient.Available > 0)
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 42424);
            var receiveBytes = udpClient.Receive(ref remote);

            if (receiveBytes.Length > 0)
            {
                RemoteIpEndPoint = remote;

                using (var inputStream = new MemoryStream(receiveBytes))
                {
                    using (var reader = new BinaryReader(inputStream))
                    {
                        Acceleration = reader.ReadVector();
                        AngularVelocity = reader.ReadVector();
                        MagneticField = reader.ReadVector();
                        Pressure = reader.ReadSingle();
                        Temperature = reader.ReadSingle();
                        Orientation = reader.ReadQuaternion();
                        AngularMomentum = reader.ReadVector();
                        Position = reader.ReadVector();
                        Velocity = reader.ReadVector();
                        WorldAcceleration = reader.ReadVector();
                        Motors = reader.ReadArray(4); // motors
                    }
                }
            }
        }


    }


    private void Send(System.Action<BinaryWriter> callback)
    {
        using (var inputStream = new MemoryStream())
        {
            using (var writer = new BinaryWriter(inputStream))
            {
                callback.Invoke(writer);
            }

            var bytes = inputStream.ToArray();
            try
            {
                udpClient.Send(bytes, bytes.Length, RemoteIpEndPoint);
            }
            catch
            {

            }
        }
    }

    public void SendPID(float P, float I, float D)
    {
        Send(writer =>
        {
            writer.Write((byte)0);
            writer.Write(P);
            writer.Write(I);
            writer.Write(D);
            writer.Write(P);
            writer.Write(I);
            writer.Write(D);
        });
    }

    public void SendSteering(bool enabled, float throttle, bool reset)
    {
        Send(writer =>
        {
            writer.Write((byte)1);
            writer.Write((int)(enabled ? 1 : 0));
            writer.Write(throttle);
            writer.Write((int)(reset ? 1 : 0));
        });
    }

    private Vector3 mAcceleration;
    public Vector3 Acceleration
    {
        get
        {
            return mAcceleration;
        }
        set
        {
            var vec = transform.localRotation * value / 9.81f;
            Debug.DrawLine(transform.position, transform.position + vec, Color.yellow, 0.1f);
            mAcceleration = value;
        }
    }

    private Vector3 mAngularVelocity;
    public Vector3 AngularVelocity
    {
        set
        {
            mAngularVelocity = value;
            var vec = transform.localRotation * value;
            Debug.DrawLine(transform.position, transform.position + vec, Color.red, 0.1f);
        }
        get
        {
            return mAngularVelocity;
        }
    }

    private Vector3 mMagneticField;
    public Vector3 MagneticField
    {
        get
        {
            return mMagneticField;
        }
        set
        {
            Debug.DrawLine(transform.position, transform.position + transform.localRotation * value, Color.white, 0.1f);
            mMagneticField = value;
        }
    }

    public float Pressure { get; set; }
    public float Temperature { get; set; }
    public Quaternion Orientation
    {
        get
        {
            return transform.localRotation;
        }
        set
        {
            transform.localRotation = value;
        }
    }

    public Vector3 AngularMomentum
    {
        set
        {
            Debug.DrawLine(transform.position, transform.position + value, Color.white, 0.1f);
        }
    }

    public Vector3 Position
    {
        get;
        set;
    }

    public Vector3 Velocity
    {
        get; set;
    }

    public Vector3 WorldAcceleration
    {
        get; set;
    }

    private float[] motors;
    public float[] Motors
    {
        get
        {
            return motors;
        }
        set
        {
            motors = value;
            for (int i = 0; i < 4; ++i)
            {
                Vector3 begin = transform.position + transform.right * Mathf.Pow(-1, i) + transform.forward * Mathf.Pow(-1, i + 1);
                Debug.DrawLine(begin, begin + transform.up * motors[i], Color.green, 0.1f);
            }
        }
    }
}
