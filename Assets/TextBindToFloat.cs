using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBindToFloat : MonoBehaviour
{
    private Text text;

    public float Value
    {
        set
        {
            text.text = value.ToString();
        }
    }

    private void Start()
    {
        text = GetComponent<Text>();
    }
}
