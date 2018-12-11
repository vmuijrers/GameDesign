using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeInIntro : MonoBehaviour {
    private Image image;
    public float time;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        image.CrossFadeAlpha(0, time, false);
    }
    //IEnumerator FadeIn()
    //{
    //    float t = 0;
    //    float step = 1f / time;
    //    while(t < 1)
    //    {
            
    //        yield return null;
    //        t +=Time.deltaTime * step;
    //    }

        
    //}
}
