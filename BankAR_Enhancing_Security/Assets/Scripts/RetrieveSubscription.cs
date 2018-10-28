using BOC_APIs;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;

public class RetrieveSubscription : MonoBehaviour{

    public APIs helperClass = new APIs();


    public InputField Subscription_IDText;
    public Text ResponseText;

    // Use this for initialization
    void Start()
    {

        //RetrieveSubscriptionFunction();

    }

    public void RetrieveSubscriptionFunction(string Subscription_ID)
    {
        Subscription_ID = Subscription_IDText.text;
        string Retrievesubscriptiontext = helperClass.RetrieveSubscription(Subscription_ID);
        Debug.Log(helperClass.RetrieveSubscription(Subscription_ID));
        ResponseText.text = Retrievesubscriptiontext;
    }

}