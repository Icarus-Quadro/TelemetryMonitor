using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrientationText : MonoBehaviour
{
    public Text TextComponent;
    public GameObject Target;
    // Update is called once per frame
    void Update()
    {
        TextComponent.text = Target.transform.localRotation.ToString("f3");
    }
}
