using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Dialogsystem : MonoBehaviour
{
    public Button _btnRestart;
    public LanguageText _contentText;
    public List<int> ContentIds = new List<int>();
    public List<Button> _btnList = new List<Button>();

    public DialogInfo Data = new DialogInfo
    {
        Title = 10,
        SelectId = new List<int> { 11, 12 },
        NextDialog = new List<DialogInfo>
        {
         new DialogInfo
         {
             Title = 13, SelectId = new List<int>{ 14},
             NextDialog = new List<DialogInfo>
             {
                 new DialogInfo
                     {
                         Title = 15, SelectId = new List<int> { 16}, NextDialog = null
                     },
             },
         },
         new DialogInfo
            {
                Title = 20,SelectId= new List<int>{ 21,22},
                NextDialog = new List<DialogInfo>
                {
                new DialogInfo
                {  Title = 23, SelectId = new List<int>{ 24}, NextDialog = null },
                new DialogInfo
                {  Title = 30, SelectId = new List<int>{ 31}, NextDialog = null },
                },
            },
        },
    };
    public DialogInfo PreData = new DialogInfo
    {
        Title = 10,
        SelectId = new List<int> { 11, 12 },
        NextDialog = new List<DialogInfo>
        {
         new DialogInfo
         {
             Title = 13, SelectId = new List<int>{ 14},
             NextDialog = new List<DialogInfo>
             {
                 new DialogInfo
                     {
                         Title = 15, SelectId = new List<int> { 16}, NextDialog = null
                     },
             },
         },
         new DialogInfo
            {
                Title = 20,SelectId= new List<int>{ 21,22},
                NextDialog = new List<DialogInfo>
                {
                new DialogInfo
                {  Title = 23, SelectId = new List<int>{ 24}, NextDialog = null },
                new DialogInfo
                {  Title = 30, SelectId = new List<int>{ 31}, NextDialog = null },
                },
            },
        },
    };
    void Start()
    {
        _btnRestart.onClick.AddListener(BtnStart);
        //Regist Button
        for (int i = 0; i < _btnList.Count; i++)
        {
            int _index = i;
            _btnList[i].onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                BtnClick(_index);
            }));
        }
        RefreshUI();
    }
    void BtnStart()
    {
        Data =MyDeepCopy.XmlDeepCopy<DialogInfo>(PreData);
        RefreshUI();
    }
    void BtnClick(int index)
    {
        if (Data.NextDialog != null && Data.NextDialog.Count > index)
        {
            Data = Data.NextDialog[index];
            RefreshUI();
        }
        else
        {
            _contentText.gameObject.SetActive(false);
            for (int i = 0; i < _btnList.Count; i++)
            {
                _btnList[i].gameObject.SetActive(false);
            }
        }
    }
    void RefreshUI()
    {
        _contentText.Text = Config.GetLanguageItem(Data.Title).Content;
        _contentText.gameObject.SetActive(true);
        if (Data.SelectId.Count > 0)
        {
            for (int i = 0; i < _btnList.Count; i++)
            {
                if (Data.SelectId.Count > i)
                {
                    _btnList[i].GetComponentInChildren<Text>().text = Config.GetLanguageItem(Data.SelectId[i]).Content;
                    _btnList[i].gameObject.SetActive(true);
                }
                else
                {
                    _btnList[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {

        }
    }

    public static class MyDeepCopy
    {
        public static T XmlDeepCopy<T>(T t)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xml.Serialize(ms, t);
                ms.Position = default;
                return (T)xml.Deserialize(ms);
            }
        }
        public static T BinaryDeepCopy<T>(T t)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, t);
                ms.Position = default;
                return (T)bf.Deserialize(ms);
            }
        }
    }
}
