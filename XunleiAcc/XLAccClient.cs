using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace XunleiAcc
{
    class XLAccClient
    {
        #region consts
        protected readonly string url = "http://api.portal.swjsq.vip.xunlei.com:81/v2/";
        #endregion

        public delegate void ErrorHandler(string message, int errcode, string additioninfo, object sender);

        public event ErrorHandler? onError;

        #region session private vars
        //private string mac;
        #endregion

        #region Login ess
        public XLAuthClient xlaccount;
        #endregion

        public XLAccClient(XLAuthClient thunderAccount)
        {
            xlaccount = thunderAccount;
        }

        #region Basic HTTP request builder & performer
        public JObject ApiGet(string Url)
        {
            string retString = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "";
            request.KeepAlive = true;
            //request.Connection = "Keep-Alive";
            //request.Headers.Set("Accept-Encoding", "gzip");
            request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android " + DeviceInfo.OS_VERSION + "; " + DeviceInfo.DEVICE_MODEL + " Build/" + DeviceInfo.OS_BUILD + ")";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(myResponseStream);
            retString = streamReader.ReadToEnd();
            streamReader.Close();
            myResponseStream.Close();
            return (JObject)JsonConvert.DeserializeObject(retString);
        }

        private string BuildUrl(string command, string args = "")
        {
            return url + command + "?sessionid=" + xlaccount.sessionid + "client_type=android-swjsq-" + DeviceInfo.APP_VERSION +
                "&peerid=" + DeviceInfo.mac + "&time_and=" + Timestamp13() + "&client_version=android-swjsq-" + DeviceInfo.APP_VERSION +
                "&userid=" + xlaccount.userid + "&os=android-" + HttpUtility.UrlEncode(DeviceInfo.OS_VERSION + "." + DeviceInfo.OS_API_LEVEL + DeviceInfo.DEVICE_MODEL)
                + args;
        }

        public static long Timestamp13()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        #endregion

        #region privateAPIS
        /// <summary>
        /// 加速下行
        /// </summary>
        /// <returns></returns>
        private bool AccelerateDownlink()
        {
            return false;
        }

        #endregion

        #region FunctionAPIs
        public JObject getBandwidthInfo()
        {
            return ApiGet(BuildUrl("bandwidth"));
        }
        #endregion
    }
}
