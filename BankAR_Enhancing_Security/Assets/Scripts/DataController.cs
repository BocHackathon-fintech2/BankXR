using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

    //public float SpeedInput;        //Declare Variables and Strings
    //public float RPMInput;
    //float AutomaticVariableSpeed;
    //float AutomaticVariableRPM;

    public string buttonInput;

    string path;
	string Url;

    string ButtonValue;

    //string jsonRateSpeed;
    //string jsonRateRPM;


    WWW www;

	//public float Speed_data;
    //public float RPM_data;
	
        
    // Use this for initialization
	void Start () {

	}

	IEnumerator WaitForRequest(WWW www)				//Obtain Variables from the Photon Cloud using JSON
	{
		yield return www;
		// check for errors
		if (www.error == null)
		{
			string work = www.text;

            Debug.Log(work);

            //_Particle fields = JsonUtility.FromJson<_Particle>(work);
            _Automatic fields = JsonUtility.FromJson<_Automatic>(work);


            string ButtonValue = fields.Button_Status;

            //Debug.Log(ButtonValue);

            //AutomaticVariableSpeed = float.Parse (jsonRateSpeed);
            //Debug.Log (AutomaticVariableSpeed);			//Debug to console
            //SpeedInput = AutomaticVariableSpeed;
            buttonInput = ButtonValue;

            //Debug.Log(buttonInput);
        } 
		else {}    
	}

	[System.Serializable]
	public class _Automatic
    {							//Class defined to obtain the Cloud Variable Name and Result
		public string Button_Status;
        //public string RPM;
		//public string result;
	}

	// Update is called once per frame
	void Update () {
        //Insert your cURL here:
        //string url = "https://api.particle.io/v1/devices/230039000a47353138383138/Force?access_token=fc30489129ccbad879a2e3921485501418ada51c";
        //string url = "https://api.particle.io/v1/devices/430058000351353530373132/Button_Status?access_token=54afe08cc076614f15ca97fd8bc0b2f6e0a48159";

        string url = "https://api.particle.io/v1/devices/events/Button_Status?access_token=54afe08cc076614f15ca97fd8bc0b2f6e0a48159";
                
        www = new WWW(url);
		StartCoroutine(WaitForRequest(www));

        //Debug.Log(buttonInput); 
        //AnalogueSpeedConverter.ShowSpeed (SpeedInput, 0, 180); //Send Speed reading to analog dial.
        //AnalogueRPMConverter.ShowRPM(RPMInput, 0, 10000); //Send RPM reading to analog dial.
    }
}
