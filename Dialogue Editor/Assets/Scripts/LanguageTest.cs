using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class LanguageTest : MonoBehaviour
{
    public Dropdown _dropDown;
    void Start()
    {
        _dropDown.ClearOptions();
        var lans = Resources.LoadAll<LanguageItemObject>("");
        for (int i = 0; i < lans.Length; i++)
        {
            Debug.Log(lans[i].name);
        }
        Config.UpdateLanguageType(lans);


        for (int i = 0; i < lans.Length; i++)
        {
            _dropDown.AddOptions(new List<Dropdown.OptionData>() {
             new Dropdown.OptionData{  text = lans[ i].name.ToString()}
            });
        }
        _dropDown.onValueChanged.AddListener(OnDropDownChange);
    }
    void OnDropDownChange(int index)
    {
        Config.CurrentFileName = Config.LanguangeTypeList[index];

        RefreshText();
    }
    void RefreshText()
    {
        var datas = FindObjectsOfType<LanguageText>();
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i].Refresh();
        }
    }
}
