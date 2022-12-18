using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightSwitch : MonoBehaviour
{
    public Light lightToToggle;
    public GameObject global;


    public void ToggleLightSwitch (){
        lightToToggle.enabled = !lightToToggle.enabled;
        global.SetActive(false);
    }
}
