using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrientationText : MonoBehaviour
{
    public Text PressureText;
    public Text AccelerometerText;
    public Text AccelerationText;
    public Text AltitudeText;

    private StdDevCalculator pressureStdDev = new StdDevCalculator(500);
    private StdDevCalculator accelStdDev = new StdDevCalculator(500);
    private StdDevCalculator angVelStdDev = new StdDevCalculator(500);

    public Telemetry Target;
    // Update is called once per frame
    void Update()
    {
        PressureText.text = string.Format("{0:f0}+-{1:f1} Pa", Target.Pressure, pressureStdDev.AddSample(new Vector3(Target.Pressure, 0, 0)));
        AccelerometerText.text = string.Format("{0}", accelStdDev.AddSample(Target.Acceleration));
        AccelerationText.text = string.Format("{0}", angVelStdDev.AddSample(Target.AngularVelocity));
        AltitudeText.text = string.Format("{0,5} m ", Target.Position.y);
    }

}
