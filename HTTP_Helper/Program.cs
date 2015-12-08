using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading;

namespace HTTP_Helper
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.ResetColor();
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Environment.NewLine);

                doPOST();
                Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["PollingInterval"]));
            }
        }

        static void doPOST()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    var mac = rand.Next() % 2 == 0 ? "deadbeeffeed" : "baddcafefeed";
                    values["mac"] = mac;
                    values["Temperature"] = Math.Round(rand.Next(20, 35) * rand.NextDouble(), 1).ToString();
                    values["Humidity"] = Math.Round(rand.Next(40, 60) * rand.NextDouble(), 1).ToString();

                    var postUrl = ConfigurationManager.AppSettings["PostUrl"].ToString();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(string.Format(" > {0} {1}", postUrl, ConstructQueryString(values)));
                    var response = client.UploadValues(postUrl, values);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(string.Format(" < {0}", Encoding.Default.GetString(response)));
                }
            }
            catch (Exception exp)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(exp.Message);
            }
        }

        static String ConstructQueryString(NameValueCollection parameters)
        {
            List<String> items = new List<String>();

            foreach (String name in parameters)
                items.Add(String.Concat(name, "=", parameters[name]));

            return String.Join("&", items.ToArray());
        }
    }
}
