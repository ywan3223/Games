using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Config
{
    public enum ELangugeType : byte
    {
        EN,
        CN,
        JAPAN,
    }
    public static List<string> LanguangeTypeList = new List<string>();
    public static void UpdateLanguageType(LanguageItemObject[] len)
    {
        LanguangeTypeList.Clear();
        for (int i = 0; i < len.Length; i++)
        {
            LanguangeTypeList.Add(len[i].name);
        }
    }
    public static string CurrentFileName = "EN";
    public static ELangugeType LangugeType = ELangugeType.EN;
    public static string GetLanguageText(LanguageData data)
    {
        switch (Config.LangugeType)
        {
            case Config.ELangugeType.CN:
                return data.CN;
            case Config.ELangugeType.EN:
                return data.EN;
            case Config.ELangugeType.JAPAN:
                return data.JAPAN;
            default:
                return data.EN;
        }
    }
    public static string GetLanguageText(LanguageItem data)
    {
        return data.Content;
    }
    public static LanguageItemObject GetLanguageItem()
    {
        return Resources.Load<LanguageItemObject>(CurrentFileName);
    }
    public static LanguageItem GetLanguageItem(int Id)
    {
        var data = GetLanguageItem().LanguageList;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].Id == Id)
            {
                return data[i];
            }
        }
        return new LanguageItem { Id = 0, Content = "" };
    }
}
