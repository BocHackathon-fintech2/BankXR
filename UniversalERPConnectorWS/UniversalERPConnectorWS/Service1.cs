using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;

namespace UniversalERPConnectorWS
{
    public partial class Service1 : ServiceBase
    {
        string WatchPath1 = ConfigurationManager.AppSettings["WatchPath1"];


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            watch();
            //CreateSubscription();
        }

        protected override void OnStop()
        {
        }

        private void watch()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = "C:/BoCB2B/Create_Subscription/Calls/ERP/";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName; ;
            watcher.Filter = "*.txt";
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            CreateSubscription();
        }

        public string C_id()
        {
            string client_id = File.ReadLines("C:/BoCB2B/CC.appid").First();
            //MessageBox.Show(client_id);
            return client_id;
        }

        public string C_secret()
        {

            string client_secret = File.ReadLines("C:/BoCB2B/CC.appid").ElementAtOrDefault(1);
            //MessageBox.Show(client_secret);
            return client_secret;
        }


        public string Token()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            var request = (HttpWebRequest)WebRequest.Create("https://sandbox-apis.bankofcyprus.com/df-boc-org-sb/sb/psd2/oauth2/token");

            //var postData = "client_id=c1dda663-92ba-4202-9132-35bccf094188&client_secret=cS0vB7gA2kN2wR3lG2cU0mK2lE5iW7wO5uX0fY4hT0bD2bM2wK&grant_type=client_credentials&scope=TPPOAuth2Security";

            var postData = "client_id=" + C_id() + "&client_secret=" + C_secret() + "&grant_type=client_credentials&scope=TPPOAuth2Security";
            //MessageBox.Show(postData);


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


            System.IO.File.WriteAllText("C:/BoCB2B/Token/LastToken.txt", TokenValue);



            string sourcePath = @"C:/BoCB2B/Token/";
            string targetPath = @"C:/BoCB2B/Token/logbook/";

            string fileName = "LastToken.txt";

            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            string datestring = System.DateTime.Now.ToString("yyyy_MM_dd");
            string timestring = System.DateTime.Now.ToString("hh_mm_ss_ff");

            System.IO.File.Copy(sourceFile, destFile, true);
            //MessageBox.Show("C:/BoCB2B/Token" + "Token" + datestring + "_" + timestring + ".txt");

            System.IO.File.Move(destFile, "C:/BoCB2B/Token/logbook/" + "Token" + datestring + "_" + timestring + ".txt");


            return TokenValue;


        }


        public void CreateSubscription()
        {
            string tokenValue = Token();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            var request = (HttpWebRequest)WebRequest.Create("https://sandbox-apis.bankofcyprus.com/df-boc-org-sb/sb/psd2/v1/subscriptions?client_id=c1dda663-92ba-4202-9132-35bccf094188&client_secret=cS0vB7gA2kN2wR3lG2cU0mK2lE5iW7wO5uX0fY4hT0bD2bM2wK");

            //string path = "C:/BoCB2B/Create_Subscription/Calls/ERP/LastCreateSubscriptionCall.txt";


            DirectoryInfo d = new DirectoryInfo("C:/BoCB2B/Create_Subscription/Calls/ERP/");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
            //string str = "";
            foreach (FileInfo file in Files)
            {
                //str = str + ", " + file.Name;

           

                string transactionHistory = File.ReadLines("C:/BoCB2B/Create_Subscription/Calls/ERP/" + file.Name).First();
                string balance = File.ReadLines("C:/BoCB2B/Create_Subscription/Calls/ERP/" + file.Name).ElementAtOrDefault(1);
                string details = File.ReadLines("C:/BoCB2B/Create_Subscription/Calls/ERP/" + file.Name).ElementAtOrDefault(2);
                string checkFundsAvailability = File.ReadLines("C:/BoCB2B/Create_Subscription/Calls/ERP/" + file.Name).ElementAtOrDefault(3);
                string limit = File.ReadLines("C:/BoCB2B/Create_Subscription/Calls/ERP/" + file.Name).ElementAtOrDefault(4);
                string currency = File.ReadLines("C:/BoCB2B/Create_Subscription/Calls/ERP/" + file.Name).ElementAtOrDefault(5);
                string amount = File.ReadLines("C:/BoCB2B/Create_Subscription/Calls/ERP/" + file.Name).ElementAtOrDefault(6);


                string datestring = System.DateTime.Now.ToString("yyyy_MM_dd");
                string timestring = System.DateTime.Now.ToString("hh_mm_ss_ff");

                string sourcePathoriginalcall = "C:/BoCB2B/Create_Subscription/Calls/ERP/";
                string targetPathoriginalcall = "C:/BoCB2B/Create_Subscription/Calls/ERP/" + "logbook/";

                string sourceFileoriginalcall = System.IO.Path.Combine(sourcePathoriginalcall, file.Name);
                string destFileoriginalcall = System.IO.Path.Combine(targetPathoriginalcall, file.Name);

                System.IO.File.Copy(sourceFileoriginalcall, destFileoriginalcall, true);
                System.IO.File.Move(destFileoriginalcall, "C:/BoCB2B/Create_Subscription/Calls/ERP/" + "logbook/" + "CreateSubscriptionCall" + datestring + "_" + timestring + ".txt");
                System.IO.File.Delete(sourceFileoriginalcall);


                //MessageBox.Show(amount);


                //string Json = "{\"ErrorMessage\": \"\",\"ErrorDetails\": {\"ErrorID\": 111,\"Description\":{\"Short\": 0,\"Verbose\": 20},\"ErrorDate\": \"\"}}";

                //string text = System.IO.File.ReadAllText(@"C:\create_subscription.json");



                //Edit
                //string text = System.IO.File.ReadAllText(filelocation);
                //MessageBox.Show(text);
                //var postData = text;

                var postData = "{\r\n \"accounts\": {\r\n   \"transactionHistory\": " + transactionHistory + ",\r\n   \"balance\": " + balance + ",\r\n   \"details\": " + details + ",\r\n  \"checkFundsAvailability\": " + checkFundsAvailability + "\r\n   },\r\n \"payments\": {\r\n     \"limit\": " + limit + ",\r\n    \"currency\": " + currency + ",\r\n     \"amount\": " + amount + "\r\n  }\r\n}";

                System.IO.File.WriteAllText("C:/BoCB2B/Create_Subscription/LastCreateSubscription.json", postData);


                string sourcePath = @"C:/BoCB2B/Create_Subscription/";
                string targetPath = @"C:/BoCB2B/Create_Subscription/Calls/";

                string fileName = "LastCreateSubscription.json";

                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName);



                System.IO.File.Copy(sourceFile, destFile, true);
                //MessageBox.Show("C:/BoCB2B/Token" + "Token" + datestring + "_" + timestring + ".txt");

                System.IO.File.Move(destFile, "C:/BoCB2B/Create_Subscription/Calls/logbook/" + "CreateSubscription" + datestring + "_" + timestring + ".json");


                //MessageBox.Show(postData);




                //string text = BodyJSON;
                //MessageBox.Show(text);
                //var postData = text;

                //var postData = "{\"accounts\": {\"transactionHistory\": true, \"balance\": true, \"details\": true,  \"checkFundsAvailability\": true}, \"payments\": {\"limit\": 99999999, \"currency\": EUR, \"amount\": 99998888}}";



                //var postData = "client_id=c1dda663-92ba-4202-9132-35bccf094188&client_secret=cS0vB7gA2kN2wR3lG2cU0mK2lE5iW7wO5uX0fY4hT0bD2bM2wK&";
                //var postData2 = "client_id=c1dda663-92ba-4202-9132-35bccf094188&client_secret=cS0vB7gA2kN2wR3lG2cU0mK2lE5iW7wO5uX0fY4hT0bD2bM2wK&Authorization Bearer " + '"' + tokenValue + '"' + "&Content-Type=application/json&APIm-Debug-Trans-Id=true&app_name=BankXR&tppid=singpaymentdata&originUserId=0001&timeStamp={{$timestamp}}&journeyId={{journeyId}}&{\r\n \"accounts\": {\r\n    \"transactionHistory\": true,\r\n    \"balance\": true,\r\n    \"details\": true,\r\n    \"checkFundsAvailability\": true\r\n  },\r\n  \"payments\": {\r\n    \"limit\": 99999999,\r\n    \"currency\": \"EUR\",\r\n    \"amount\": 99998888\r\n  }\r\n}";
                var data = Encoding.ASCII.GetBytes(postData);
                //var data2 = Encoding.ASCII.GetBytes(postData2);


                request.Method = "POST";
                request.ContentType = "application/json";
                //request.ContentLength = data.Length;
                //request.Headers.Add("Postman-Token", "8d0eaf1e-8543-44eb-8373-49e2e8749167");
                //request.Headers.Add("cache-control", "no-cache");

                //var request = new RestRequest(Method.POST);
                //request.Headers.Add("Postman-Token", "4d336b7f-8826-4d21-b252-f40651777e5e");
                //request.Headers.Add("cache-control", "no-cache");
                request.Headers.Add("journeyId", "{{journeyId}}");
                request.Headers.Add("timeStamp", "{{$timestamp}}");
                request.Headers.Add("originUserId", "0001");
                request.Headers.Add("tppid", "singpaymentdata");
                request.Headers.Add("app_name", "BankXR");
                //request.Headers.Add("APIm-Debug-Trans-Id", "true");
                //request.Headers.Add("client_id", "c1dda663-92ba-4202-9132-35bccf094188");
                //request.Headers.Add("client_secret", "cS0vB7gA2kN2wR3lG2cU0mK2lE5iW7wO5uX0fY4hT0bD2bM2wK");
                //request.Headers.Add("Content-Type", "application/json");
                request.Headers.Add("Authorization", "Bearer " + tokenValue);
                //request.("undefined", "{\r\n \"accounts\": {\r\n    \"transactionHistory\": true,\r\n    \"balance\": true,\r\n    \"details\": true,\r\n    \"checkFundsAvailability\": true\r\n  },\r\n  \"payments\": {\r\n    \"limit\": 99999999,\r\n    \"currency\": \"EUR\",\r\n    \"amount\": 99998888\r\n  }\r\n}", ParameterType.RequestBody);
                //MessageBox.Show("Bearer " + '"' + tokenValue + '"');


                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }





                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


            


                System.IO.File.WriteAllText("C:/BoCB2B/Create_Subscription/BoCResponses/CreateSubscription" + datestring + "_" + timestring + ".json", responseString);
                                
            }
        }

    }
}
