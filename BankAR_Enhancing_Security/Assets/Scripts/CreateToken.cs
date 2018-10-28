using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BOC_APIs;
using UnityEngine.UI;
using UnityEditor;

public class CreateToken : MonoBehaviour
{

    public APIs helperClass = new APIs();

    public Text ResponseText;


    // Use this for initialization
    void Start()
    {
        CreateTokenFunction();
    }

    //Create Token
    public void CreateTokenFunction()
    {
        string tokentextdata = helperClass.Token();
        Debug.Log(helperClass.Token());
        ResponseText.text = tokentextdata;
    }
}