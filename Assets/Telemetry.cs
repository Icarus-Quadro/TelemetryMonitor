using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

public static class ReaderExtensions {
    public static Vector3 ReadVector(this BinaryReader reader) {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        return new Vector3(x, z, y);
    }

    public static Quaternion ReadQuaternion(this BinaryReader reader) {
        var x = -reader.ReadSingle();
        var y = -reader.ReadSingle();
        var z = -reader.ReadSingle();
        var w = reader.ReadSingle();
        return new Quaternion(x, z, y, w);
    }
}

public class Telemetry : MonoBehaviour
{
    void Start() {
        udpClient  = new UdpClient(1234);
    }

    void Update()
    {
        while (udpClient.Available > 0) {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receiveBytes = udpClient.Receive(ref RemoteIpEndPoint); 
            using (var inputStream = new MemoryStream(receiveBytes)) {
                using (var reader = new BinaryReader(inputStream)) {
                    Acceleration = reader.ReadVector();
                    AngularVelocity = reader.ReadVector();
                    MagneticField = reader.ReadVector();
                    Pressure = reader.ReadSingle();
                    Temperature = reader.ReadSingle();
                    Orientation = reader.ReadQuaternion();
                    AngularMomentum = reader.ReadVector();
                }
            }
        }
    }

    UdpClient udpClient;
    
    public Vector3 Acceleration { get; set; }
    public Vector3 AngularVelocity {
        set {
            var vec = transform.localRotation * value;
            Debug.DrawLine(transform.position, transform.position + vec, Color.red, 0.1f);
        }
    }
    private Vector3 mMagneticField;
    public Vector3 MagneticField { 
        get {
            return mMagneticField;
        }
        set {
            Debug.DrawLine(transform.position, transform.position + value * 100f, Color.white, 0.1f);
            mMagneticField = value;
        }
    }
    public float Pressure { get; set; }
    public float Temperature { get; set; }
    public Quaternion Orientation {
        get {
            return transform.localRotation;
        }
        set {
            transform.localRotation = value;
        }
    }
    public Vector3 AngularMomentum {
        set {
            Debug.DrawLine(transform.position, transform.position + value, Color.white, 0.1f);
        }
    }
}
