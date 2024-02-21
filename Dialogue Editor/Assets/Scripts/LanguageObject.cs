using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LanguageObject", menuName = "CreateLanguageObject", order = 1)]
public class LanguageObject : ScriptableObject
{
    [SerializeField]
    public List<LanguageData> LanguageList = new List<LanguageData>();
}
