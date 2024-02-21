using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LanguageItem", menuName = "CreateLanguageItem", order = 1)]
public class LanguageItemObject : ScriptableObject
{
    [SerializeField]
    public List<LanguageItem> LanguageList = new List<LanguageItem>();
}
