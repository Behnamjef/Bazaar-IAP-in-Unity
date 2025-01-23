using System;
using System.Collections;
using System.Collections.Generic;
using Bazaar.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] private Button InitButton;
    [SerializeField] private Button PurchaseButton;
    [SerializeField] private Button ConsumeButton;
    
    [Header("Inputs")]
    [SerializeField] private TMP_InputField ProductIdInputField;
    [SerializeField] private TMP_InputField PurchaseTokenInputField;
    
    [Header("Status")]
    [SerializeField] private TextMeshProUGUI StatusText;
    
    [Header("Managers")]
    [SerializeField] private PurchaseManager PurchaseManager;
    
    private bool _isInitialized;

    private void Start()
    {
        SetStatusText("Waiting for initialization...");
        
        InitButton.onClick.AddListener(OnInitButtonClick);
        PurchaseButton.onClick.AddListener(OnPurchaseButtonClick);
        ConsumeButton.onClick.AddListener(OnConsumeButtonClick);
        
        ActivePurchaseButtons(false);
    }

    private async void OnInitButtonClick()
    {
        SetStatusText("Initializing...");

        var isSuccess = await PurchaseManager.Init();

        if (!isSuccess)
        {
            SetStatusText("Initialize failed!","red");
            return;
        }
        
        SetStatusText("Initialize success!","green");
        _isInitialized = true;

        ActivePurchaseButtons(true);
    }

    private async void OnPurchaseButtonClick()
    {
        SetStatusText($"Purchasing item {ProductIdInputField.text}");
        var result = await PurchaseManager.Purchase(ProductIdInputField.text);
        if (result.status != Status.Success)
        {
            SetStatusText($"Purchase failed! {result.message}","red");
            return;
        }
        SetStatusText("Purchase success.","green");
        PurchaseTokenInputField.text = result.data.purchaseToken;
    }

    private async void OnConsumeButtonClick()
    {
        SetStatusText($"Consuming token {PurchaseTokenInputField.text}");
        var result = await PurchaseManager.Consume(PurchaseTokenInputField.text);
        if (result.status != Status.Success)
        {
            SetStatusText($"Consume failed! {result.message}","red");
            return;
        }
        
        SetStatusText("Consume success.","green");
    }

    private void ActivePurchaseButtons(bool active)
    {
        InitButton.interactable = !active;
        
        PurchaseButton.interactable = active;
        ProductIdInputField.interactable = active;
        
        ConsumeButton.interactable = active;
        PurchaseTokenInputField.interactable = active;
    }

    private void SetStatusText(string status, string color = "white")
    {
        StatusText.text = $"<color={color}>{status}</color>" + "\n------------------\n" + StatusText.text;
    }
}
