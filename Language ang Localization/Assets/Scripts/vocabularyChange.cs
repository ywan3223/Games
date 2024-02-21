using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class vocabularyChange : MonoBehaviour, ILocalizable
{
    private string id;
    private Text target;

    private void Awake()
    {
        id = gameObject.name;
        target = GetComponent<Text>();
    }

    public void Refresh(LocalizationAsset localizationAsset)
    {
        TextAsset textAsset = localizationAsset as TextAsset;
        string text;
        if (textAsset.TryGetText(id, out text))
        {target.text = text;}
        else { 
            target.text = "undefined";}
    }
}