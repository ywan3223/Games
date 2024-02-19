using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI Score;

    void Start()
    {
        Score = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(PlayerInventory playerInventory)
    {
        Debug.Log(playerInventory.NumberOfCube.ToString());
        Score.text = playerInventory.NumberOfCube.ToString();
    }
}
