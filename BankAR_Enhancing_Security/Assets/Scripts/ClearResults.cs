using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BOC_APIs;
using UnityEngine.UI;
using UnityEditor;


public class ClearResults : MonoBehaviour {

    public Text ResponseText;

    public void ClearFunction()
    {
        ResponseText.text = "";
    }
}
