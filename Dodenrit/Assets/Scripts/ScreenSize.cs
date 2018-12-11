using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour {
    public static Vector3 screenSize = new Vector3(18,10,0);
    public static Vector3 halfScreenSize;
    // Use this for initialization
    void Awake () {
        halfScreenSize = screenSize / 2;
        //screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }
}
