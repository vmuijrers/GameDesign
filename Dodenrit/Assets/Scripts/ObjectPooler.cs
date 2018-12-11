using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {


    public static ObjectPooler instance;

    public List<ObjectItem> items = new List<ObjectItem>();
    private Dictionary<System.Type, List<GameObject>> objectPool = new Dictionary<System.Type, List<GameObject>>();

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        

        foreach (ObjectItem item in items)
        {
            objectPool.Add(item.Prefab.GetType(), new List<GameObject>());
            for (int i = 0; i < item.amount; i++)
            {
                GameObject go = Instantiate(item.Prefab).gameObject;
                go.SetActive(false);
                objectPool[item.Prefab.GetType()].Add(go);
            }
        }
    }

    public GameObject RequestObjectOfType<T>()
    {
        if (objectPool.ContainsKey(typeof(T)))
        {
            foreach(GameObject go in objectPool[typeof(T)])
            {
                if (!go.activeSelf)
                {
                    go.SetActive(true);
                    return go;
                }
            }
            foreach(ObjectItem obj in items)
            {
                if(obj.Prefab.GetType() == typeof(T))
                {
                    GameObject go = Instantiate(obj.Prefab).gameObject;
                    objectPool[typeof(T)].Add(go);
                    go.SetActive(true);
                    return go;
                }
            }
            
        }
        return null;
    }
}



