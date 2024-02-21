using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInfo
{
    public int Title;
    public List<int> SelectId;
    //Next dialog
    public List<DialogInfo> NextDialog;
}
