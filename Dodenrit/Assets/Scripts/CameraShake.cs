using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static System.Action<float, float, float> OnShake;
    Vector3 originalPos;
    private float strength;
    private float decay;
    private float duration;
    // Use this for initialization
    void Start () {
        originalPos = transform.localPosition;
        OnShake += DoShake;
    }
    private void DoShake(float strength, float duration, float decay)
    {
        StartCoroutine(Shake(strength, duration, decay));
    }
    void OnDisable()
    {
        OnShake -= DoShake;
    }
    IEnumerator Shake(float strength, float duration, float decay)
    {
        float t = 0;
        float step = 1 / duration;
        this.strength = strength;
        this.duration = duration;
        this.decay = decay;
        while(t < 1)
        {
            transform.localPosition = new Vector3(Random.Range(-this.strength, this.strength), Random.Range(0, this.strength), transform.localPosition.z);
            t += step * Time.deltaTime;
            this.strength -= this.decay * Time.deltaTime;
            this.strength = Mathf.Clamp(this.strength, 0, this.strength);
            yield return null;
            transform.localPosition = originalPos;
        }

        transform.localPosition = originalPos;
    }
}
