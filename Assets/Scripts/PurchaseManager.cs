using System.Threading.Tasks;
using Bazaar.Data;
using Bazaar.Poolakey;
using Bazaar.Poolakey.Data;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] private string appKey = "";
    private Payment _payment;

    public async Task<bool> Init()
    {
        var securityCheck = SecurityCheck.Enable(appKey);
        var paymentConfiguration = new PaymentConfiguration(securityCheck);
        _payment = new Payment(paymentConfiguration);
        
        var result = await _payment.Connect();
        return result.status == Status.Success;
    }

    public async Task<Result<PurchaseInfo>> Purchase(string productId)
    {
        var result = await _payment.Purchase(productId);
        
        return result;
    }

    public async Task<Result<bool>> Consume(string purchaseToken)
    {
        var result = await _payment.Consume(purchaseToken);
        
        return result;
    }
    
    private void OnApplicationQuit()
    {
        _payment.Disconnect();
    }

}
