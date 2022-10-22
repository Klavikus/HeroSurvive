using System;
using TMPro;
using UnityEngine;

public class PoolUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private int _count;


    public void UpdateStats()
    {
        _text.text = GetComponentsInChildren<Transform>(true).Length.ToString();
    }
}