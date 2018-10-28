// based on this: http://answers.unity3d.com/questions/1085293/www-not-getting-site-that-doesnt-end-on-html.html 
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using UnityEngine.UI;
using System.Collections;
using System;
using BOC_APIs;

public class ReadStream : MonoBehaviour
{
    //public GameObject trigger1;

    public APIs helperClass = new APIs();
    public Text ResponseText;

    public string PhotonParticleURL = ""; //This API will be RESET
    WebStreamReader request = null;

    DataClassButton parseDataButton = new DataClassButton();


    bool ButtonTrue = false;

    public class DataClass
    {
        public int data;
    }


    public class DataClassButton
    {
        public int data;
    }


    void Start()
    {
        StartCoroutine(WRequest());
    }

    void Update()
    {

    }

    IEnumerator WRequest()
    {
        request = new WebStreamReader();
        request.Start(PhotonParticleURL); 
        string stream = "";
        while (true)
        {
            string block = request.GetNextBlock();
            if (!string.IsNullOrEmpty(block))
            {
                stream += block;

                string[] data = stream.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);
                //Debug.Log ("Data length: " + data.Length);
                stream = data[data.Length - 1];

                for (int i = 0; i < data.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(data[i]))
                    {
                        //	Debug.Log ("Data: " + data [i]); // print all block of data (event + data)

                        if (data[i].Contains("ON"))
                        {
                            ButtonTrue = true;

                            string output = data[i].Substring(data[i].IndexOf("{"));
                            parseDataButton = JsonUtility.FromJson<DataClassButton>(output);
                            //Debug.Log (data [i]);
                            CreateTokenFunction();

                            //trigger1.SetActive(true);

                        }

                        else if (data[i].Contains("OFF"))
                        {
                            //trigger1.SetActive(false);
                            ResponseText.text = "The device is OFF. Please set your device ON by pressing the button!";
                        }                        
                        
                    }
                }

            }
            yield return new WaitForSecondsRealtime(1);
        }
    }

    void OnApplicationQuit()
    {
        if (request != null)
            request.Dispose();
    }

    public void CreateTokenFunction()
    {
        string tokentextdata = helperClass.Token();
        Debug.Log(helperClass.Token());
        ResponseText.text = tokentextdata;
    }

    //void OnDataUpdate(decimal aAmount)
    //{
    //    Debug.Log("Received new amount: " + aAmount);
    //    // Do whatever you want with the value
    //}
}