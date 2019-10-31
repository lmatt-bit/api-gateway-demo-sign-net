using aliyun_api_gateway_sdk.Constant;
using aliyun_api_gateway_sdk.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace aliyun_api_gateway_sdk
{
    class Demo
    {
        private const String appKey = "appKey";
        private const String appSecret = "appSecret";
        private const String host = "https://apiatman.market.alicloudapi.com";        

        static void Main(string[] args)
        {
            doPostString();
            Console.Read();
        }

        private static void doPostString()
        {
            String bobyContent = "{\"qs\": [\"nice to meet you.\", \"hello world\"], \"source\": \"en\", \"target\": \"zh\", \"domain\":\"medical\"}";

            String path = "/translate_batch_v2";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            Dictionary<String, String> bodys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_JSON);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            //注意：如果有非Form形式数据(body中只有value，没有key)；如果body中是key/value形式数据，不要指定此行
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(Encoding.UTF8.GetBytes(bobyContent)));
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务body部分
            bodys.Add("", bobyContent);

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);

            using (HttpWebResponse response = HttpUtil.HttpPost(host, path, appKey, appSecret, 30000, headers, querys, bodys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }
   
    }
}
