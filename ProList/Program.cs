using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProList
{
    class Program
    {
        static void Main(string[] args)
        {

            var obj = JObject.Parse("{\"access_token\":\"b424f3ce-6fde-4567-9876-e0bceda9d946\",\"token_type\":\"bearer\",\"refresh_token\":\"87fb2465-bb2f-46ec-8546-06d63c7b0087\",\"refresh_token_expires_in\":11544205,\"expires_in\":7199,\"scope\":\"default\",\"public_account_id\":\"4020541464018\",\"business_id\":\"1501948018\"}");
            var ak = obj.SelectToken("ak", false);
            var sk = obj.SelectToken("access_token", false);

            var a= ak?.ToString();
            var s = sk.ToString();

            Console.ReadKey();
        }

        

    }


   
}
