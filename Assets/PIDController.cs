using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PIDController : MonoBehaviour
{
    public Telemetry telemetry;

    private float p = 0, i = 0, d = 0;

    public float P
    {
        set
        {
            p = value;
            Send();
        }
    }

    public float I
    {
        set
        {
            i = value;
            Send();
        }
    }

    public float D
    {
        set
        {
            d = value;
            Send();
        }
    }

    private void Send()
    {
        telemetry.SendPID(p, i, -d);
    }
}
