using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

   
    public class Shuju 
    {
        public string Name;
        public int Score;
    
    }
    List<Shuju> tmp = new List<Shuju>();
    public static int Score;
    public InputField inputField;
    public Text[] texts;
    bool Cun;
    void Start()
    {
        Cun = false;


    }
    public void Show() 
    {
        string path = Application.dataPath + "/fen.txt";
      
        string[] strs = File.ReadAllLines(path);
        tmp.Clear();
        foreach (string item in strs)
        {
            Shuju shuju1 = new Shuju();
            shuju1.Name = item.Split("|")[0];
            shuju1.Score = int.Parse(item.Split("|")[1]);
            tmp.Add(shuju1);
        }
        tmp.Sort((x, y) => x.Score.CompareTo(y.Score));
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
            if (i < tmp.Count)
            {
                texts[i].text = tmp[i].Name + ":" + tmp[i].Score.ToString();
                texts[i].gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void BaoCun() 
    {
        if (inputField.text != null&& Score!=0&& !Cun)
        {
            Cun = true;
            StreamWriter sw;
            FileInfo fi = new FileInfo(Application.dataPath + "/fen.txt");
            //sw = fi.CreateText();      
            sw = fi.AppendText();
            string info = inputField.text + "|" + Score.ToString();
            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
        }
       

    }
    public void Return() 
    {
        SceneManager.LoadScene(0);
    }
   
}
