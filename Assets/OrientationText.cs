using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrientationText : MonoBehaviour
{
    public Text PressureText;
    public Text AccelerometerText;
    public Text AccelerationText;

    public Telemetry Target;
    // Update is called once per frame
    void Update()
    {
        PressureText.text = Target.Pressure.ToString("f1") + " Pa";
        AccelerometerText.text = Target.Acceleration.ToString("f1") + ", " + Target.Acceleration.magnitude.ToString("f2") + " m/s";
        AccelerationText.text = Target.WorldAcceleration.ToString("f1") + ", " + Target.WorldAcceleration.magnitude.ToString("f2") + " m/s";
    }
}
