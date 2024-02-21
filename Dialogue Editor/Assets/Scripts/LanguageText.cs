using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class LanguageText : MonoBehaviour
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _text;
    private Text _textComponent;
    /// <summary>
    /// Text ID号
    /// </summary>
    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }
    public string Text
    {
        get { return _text; }
        set
        {
            _text = value;
            if (GetComponent<Text>() != null)
            {
                GetComponent<Text>().text = value;
            }
        }
    }
    void Awake()
    {
        _textComponent = GetComponent<Text>();
        Refresh();
    }
    public void Refresh()
    {
//#if UNITY_EDITOR
//        var data = Resources.Load<LanguageObject>("LanguageObject");
//        for (int i = 0; i < data.LanguageList.Count; i++)
//        {
//            var item = data.LanguageList[i];
//            if (item.Id == _id)
//            {
//                _textComponent.text = Config.GetLanguageText(item);
//            }
//        }
//#endif
        var data = Config.GetLanguageItem();
        if (data != null)
        {
            for (int i = 0; i < data.LanguageList.Count; i++)
            {
                var item = data.LanguageList[i];
                if (item.Id == _id)
                {
                    _textComponent.text = item.Content;
                }
            }
        }
    }
}
