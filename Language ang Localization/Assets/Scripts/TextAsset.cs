using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "Localization/addLanguage", order = 10)]
public class TextAsset : LocalizationAsset
{
    public string language;
    public List<Pair> vocabulary = new List<Pair>();

    private void OnEnable()
    {
        language = name;
    }

    public bool TryGetText(string id, out string text)
    {
        Pair pair = vocabulary.Find(x => x.id == id);
        if (pair == null)
        {
            text = string.Empty;
            return false;
        }
        text = pair.text;
        return true;
    }
}