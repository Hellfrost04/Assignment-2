using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public Light lightToToggle;

    public void ToggleLightSwitch (){
        lightToToggle.enabled = !lightToToggle.enabled;
    }
}
