using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class AudioManager : MonoBehaviour
{
    public string filePath = "/Music/Audio/Resources/"; 
    public static AudioManager Instance;
    public List<string> folderNames = new List<string>();
    private Dictionary<string, AudioClip[]> allClips = new Dictionary<string, AudioClip[]>();
    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        foreach (string s in folderNames)
        {
            AudioClip[] clips = Resources.LoadAll<AudioClip>(s);
            allClips.Add(s, clips);
        }


    }
    public void LoadPaths()
    {
        folderNames.Clear();
        Debug.Log(Application.dataPath);
        string[] dirs = Directory.GetDirectories(Application.dataPath+ filePath);
        foreach(string s in dirs)
        {
            string[] split = s.Split('/');
            string folderName = split[split.Length - 1];
            folderNames.Add(folderName);
            Debug.Log(folderName);
        }
        

    }

    public void PlayAudio(Vector3 position, string name, float pitch = 1, float volume = 1, bool is3D = true, float maxDistance = 80)
    {
        if (allClips.ContainsKey(name))
        {
            AudioClip clip = allClips[name][Random.Range(0, allClips[name].Length)];
            GameObject g = new GameObject("AudioShot_" + name);
            g.transform.position = position;
            AudioSource s = g.AddComponent<AudioSource>();
            s.volume = volume;
            s.pitch = pitch;
            s.spatialBlend = is3D ? 1 : 0;
            s.rolloffMode = AudioRolloffMode.Linear;
            s.maxDistance = maxDistance;
            s.clip = clip;
            s.dopplerLevel = 0;
            s.Play();
            Destroy(g, clip.length + 0.1f);

        }


    }
}


