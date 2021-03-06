﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteController : MonoBehaviour
{
    public Telemetry telemetry;
    private bool motorsEnabled = false;

    float time = 0.0f;

    public float Throttle { get; set; }

    private bool reset;

    void Update()
    {

        if (Input.GetButtonDown("Switch"))
        {
            motorsEnabled = !motorsEnabled;
            if (motorsEnabled)
            {
                reset = true;
            }
        }

        if (Time.time > time)
        {
            time = Time.time + 0.1f;

            float value = Throttle + Input.GetAxis("Throttle");
            telemetry.SendSteering(motorsEnabled, value, reset);
            reset = false;
        }
    }
}
