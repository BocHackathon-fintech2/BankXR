using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
using System.IO;
using System.Net;

namespace BOC_APIs
{
    public class APIs
    {
        public string Token()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            var request = (HttpWebRequest)WebRequest.Create("https://sandbox-apis.bankofcyprus.com/df-boc-org-sb/sb/psd2/oauth2/token");

            var postData = "client_id=c1dda663-92ba-4202-9132-35bccf094188&client_secret=cS0vB7gA2kN2wR3lG2cU0mK2lE5iW7wO5uX0fY4hT0bD2bM2wK&grant_type=client_credentials&scope=TPPOAuth2Security";

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            //request.Headers.Add("Postman-Token", "8d0eaf1e-8543-44eb-8373-49e2e8749167");
            //request.Headers.Add("cache-control", "no-cache");

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //MessageBox.Show(responseString);

            string[] alldata = responseString.Split(new string[] { "access_token" }, StringSplitOptions.None);

            //MessageBox.Show(alldata[1]);
            string[] alldata2 = alldata[1].Split(new string[] { "expires_in" }, StringSplitOptions.None);
            //MessageBox.Show(alldata2[0]);

            string alldata3 = alldata2[0].Trim('"');
            //MessageBox.Show(alldata3);
            string alldata4 = alldata3.Trim(':');
            //MessageBox.Show(alldata4);


            string alldata5 = alldata4.Substring(0, alldata4.Length - 2);
            //MessageBox.Show(alldata5);
            string TokenValue = alldata5.Trim('"');
            //MessageBox.Show(TokenValue);
            //return TokenValue;
            return TokenValue;
        }

        public string CreateSubscription(string BodyJSON)
        {
            string tokenValue = Token();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            var request = (HttpWebRequest)WebRequest.Create("https://sandbox-apis.bankofcyprus.com/df-boc-org-sb/sb/psd2/v1/subscriptions?client_id=c1dda663-92ba-4202-9132-35bccf094188&client_secret=cS0vB7gA2kN2wR3lG2cU0mK2lE5iW7wO5uX0fY4hT0bD2bM2wK");

            string text = BodyJSON;
            var postData = text;

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";

            request.Headers.Add("journeyId", "{{journeyId}}");
            request.Headers.Add("timeStamp", "{{$timestamp}}");
            request.Headers.Add("originUserId", "0001");
            request.Headers.Add("tppid", "singpaymentdata");
            request.Headers.Add("app_name", "BankXR");
            request.Headers.Add("Authorization", "Bearer " + tokenValue);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }


            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        public string RetrieveSubscription(string Subscription_ID)
        {
            string tokenValue = Token();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //string Subscription_ID = "Subid000001-1539701552810";

            var request = (HttpWebRequest)WebRequest.Create("https://sandbox-apis.bankofcyprus.com/df-boc-org-sb/sb/psd2/v1/subscriptions/" + Subscription_ID + "?client_id=c1dda663-92ba-4202-9132-35bccf094188&client_secret=cS0vB7gA2kN2wR3lG2cU0mK2lE5iW7wO5uX0fY4hT0bD2bM2wK");


            request.Method = "GET";
            request.ContentType = "application/json";

            request.Headers.Add("journeyId", "{{journeyId}}");
            request.Headers.Add("timeStamp", "{{$timestamp}}");
            request.Headers.Add("originUserId", "0001");
            request.Headers.Add("tppid", "singpaymentdata");
            request.Headers.Add("Authorization", "Bearer " + tokenValue);
            //MessageBox.Show("Bearer " + '"' + tokenValue + '"');


            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }
    }
}


