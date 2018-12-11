using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Character/Create")]
public class CharacterStats : ScriptableObject {

    public string characterName;
    public AudioClip instrumentClip;
    public bool isMale;
    public LayerMask slotLayer;
    public bool isHorse;
}
