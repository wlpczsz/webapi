﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace webApi调用
{
    class NetUtils
    {
        #region Post请求
        /// <summary>
        /// http Post请求
        /// </summary>
        /// <param name="parameterData">参数</param>
        /// <param name="serviceUrl">访问地址</param>
        /// <param name="ContentType">默认 application/json , application/x-www-form-urlencoded,multipart/form-data,raw,binary </param>
        /// <param name="Accept">默认application/json</param>
        /// <returns></returns>
        public static string Post(string serviceUrl, string parameterData, string ContentType = "application/json", string Accept = "application/json")
        {
            try
            {
                //先根据用户请求的uri构造请求地址
                //string serviceUrl = string.Format("{0}/{1}", this.BaseUri, uri);

                //创建Web访问对象
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                //把用户传过来的数据转成“UTF-8”的字节流
                byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(parameterData);

                myRequest.Method = "POST";
                //myRequest.Accept = "application/json";
                //myRequest.ContentType = "application/json";  // //Content-Type: application/x-www-form-urlencoded 
                myRequest.AutomaticDecompression = DecompressionMethods.GZip;
                myRequest.Accept = Accept;
                //myRequest.ContentType = ContentType;
                myRequest.ContentType = "application/json; charset=UTF-8";
                myRequest.ContentLength = buf.Length;
                myRequest.MaximumAutomaticRedirections = 1;
                myRequest.AllowAutoRedirect = true;

                //myRequest.Headers.Add("content-type", "application/json");
                //myRequest.Headers.Add("accept-encoding", "gzip");
                //myRequest.Headers.Add("accept-charset", "utf-8");

                //发送请求
                Stream stream = myRequest.GetRequestStream();
                stream.Write(buf, 0, buf.Length);
                stream.Close();

                //通过Web访问对象获取响应内容
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
                string returnData = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾

                reader.Close();
                myResponse.Close();

                JObject jo = (JObject)JsonConvert.DeserializeObject(returnData);
                try
                {
                    return jo["result"]["output"].ToString();
                }
                catch (Exception ex)
                {
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                return "接口调用失败";
            }
        }
        #endregion
    }
}
