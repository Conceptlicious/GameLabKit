using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Player name")]
    [SerializeField] string playerName;

    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }

    [Header("Player currency")]
    [SerializeField] int coinAmount;

    public int CoinAmount
    {
        get => coinAmount;
        set => coinAmount = value;
    }

    [Header("Player inventory")]
    [SerializeField] List<Trophy> inventory = new List<Trophy>();

    public List<Trophy> Inventory
    {
        get => inventory;
        set => inventory = value;
    }

    private void Awake()
    {
        instance = this;
        PlayerName = "Nighel";
    }

    /// <summary>
    /// Handles adding a trophy to the player his inventory
    /// </summary>
    /// <param name="trophy"></param>
    public void AddTrophy(Trophy trophy)
    {
        Inventory.Add(trophy);
        //TODO: Give some feedback screen
    }

    public void RemoveTrophy(Trophy trophy)
    {
        Inventory.Remove(trophy);
    }
}
