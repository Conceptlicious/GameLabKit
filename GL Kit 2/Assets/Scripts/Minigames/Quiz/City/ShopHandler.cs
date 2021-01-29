using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    private Player player => Player.instance;
    private DialogueHandler dialogueHandler => DialogueHandler.instance;
    private CityHandler cityHandler => CityHandler.instance;

    [SerializeField] GameObject shopCanvas;
    [SerializeField] GameObject shopInformation;

    private bool shopPauze;

    /// <summary>
    /// Handles the attempt of purchasing an item from the shop
    /// </summary>
    /// <param name="shopItem"></param>
    public void PurchaseItem(ShopItem shopItem)
    {
        if (shopPauze) return;

        Debug.Log($"currentCoins = {player.CoinAmount}, itemPrice = {shopItem.price}");

        if (shopItem.price > player.CoinAmount)
        {
            dialogueHandler.SetDialogue("Lokaal", "Te duur");
            return;
        }

        player.CoinAmount -= shopItem.price;
        player.AddTrophy(shopItem.trophy);

        shopCanvas.SetActive(false);
        dialogueHandler.SetDialogue("Lokaal", "Voucher");
        cityHandler.CurrentState = CityHandler.State.BOUGHT_VOUCHER;
    }

    public void OpenInfo()
    {
        Time.timeScale = 0;
        shopPauze = true;
        shopInformation.SetActive(true);
    }

    public void CloseInfo()
    {
        Time.timeScale = 1;
        shopPauze = false;
        shopInformation.SetActive(false);
    }

}
