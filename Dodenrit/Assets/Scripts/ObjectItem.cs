using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectItem", menuName = "Poolable/item")]
public class ObjectItem : ScriptableObject
{
    public MonoBehaviour Prefab;
    public int amount;
}
