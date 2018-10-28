using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BOC_APIs;
using UnityEngine.UI;
using UnityEditor;

public class CreateSubscription : MonoBehaviour {

    public APIs helperClass = new APIs();

    public Text ResponseText;

    //public Text m_MyText2;

    //public InputField Subscription_ID;
    //public Text m_MyText3;


    // Use this for initialization
    void Start () {

        //CreateSubscriptionFunction();

    }

    public void CreateSubscriptionFunction()
    {
        string JSONBody = @"{""accounts"": {""transactionHistory"": true,""balance"": true, ""details"": true, ""checkFundsAvailability"": true}, ""payments"": {""limit"": 99999999, ""currency"": ""EUR"", ""amount"":12345678}}";
        string createsubscriptiontext = helperClass.CreateSubscription(JSONBody);
        Debug.Log(helperClass.CreateSubscription(JSONBody));
        ResponseText.text = createsubscriptiontext;
    }

}
