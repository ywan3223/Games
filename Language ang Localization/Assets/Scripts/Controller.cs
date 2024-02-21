using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class Controller : MonoSingleton<Controller>
{
    public Dropdown languageSelector;

    private string languageAssetPath = "Localization/TextAssets";
    private string defaultLanguage = "English";
    private string selectedLanguage;

    private List<string> languageArr = new List<string>();
    private Dictionary<string, LocalizationAsset> assetMap = new Dictionary<string, LocalizationAsset>();

    public string DefaultLanguage { get => defaultLanguage; set => defaultLanguage = value; }
    public string SelectedLanguage { get => selectedLanguage; set => selectedLanguage = value; }
    public List<string> LanguageIDArr { get => languageArr; set => languageArr = value; }

    private void Awake()
    {
        languageSelector = GetComponent<Dropdown>();
        languageSelector.onValueChanged.AddListener((index) => OnSelectedLanguage(languageSelector.options[index].text));
        LoadAsset();
        ReloadSelector();
    }

    private void Start()
    {
        OnSelectedLanguage(defaultLanguage);
    }

    protected void LoadAsset()
    {
        TextAsset[] assets = Resources.LoadAll<TextAsset>(languageAssetPath);
        for (int i = 0; i < assets.Length; i++)
        {
            languageArr.Add(assets[i].language);
            assetMap.Add(assets[i].language, assets[i]);
        }
    }

    private void ReloadSelector()
    {
        languageSelector.options.Clear();
        for (int i = 0; i < languageArr.Count; i++)
        {
            languageSelector.options.Add(new OptionData(languageArr[i]));
            if (languageArr[i] == DefaultLanguage)
            {
                languageSelector.value = i;
            }
        }
    }

    public void OnSelectedLanguage(string language)
    {
        selectedLanguage = language;
        var localizableComponents = FindObjectsOfType<MonoBehaviour>().OfType<ILocalizable>();
        foreach (var component in localizableComponents)
        {
            component.Refresh(assetMap[selectedLanguage]);
        }
    }
}

