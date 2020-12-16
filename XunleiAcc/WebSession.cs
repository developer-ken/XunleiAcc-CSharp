using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace XunleiAcc
{
    class WebSession
    {
        #region consts
        protected readonly string url = "http://api.portal.swjsq.vip.xunlei.com:81/v2/";

        protected readonly string APP_VERSION = "2.4.1.3";
        protected readonly short PROTOCOL_VERSION = 200;
        protected readonly short VASID_DOWN = 14; // vasid for downstream accel
        protected readonly string FALLBACK_MAC = "000000000000";

        protected readonly string DEVICE = "Microstorm S1";
        protected readonly string DEVICE_MODEL = "S1";
        protected readonly string OS_VERSION = "10.0.1";
        protected readonly string OS_API_LEVEL = "29";
        protected readonly string OS_BUILD = "PUA66B";
        #endregion

        #region session private vars
        private string mac;
        #endregion

        #region Login ess
        private string thunderUID;
        private string sessionID;
        #endregion

        #region Basic HTTP request builder & performer
        public string ApiGet(string Url)
        {
            try
            {
                string retString = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "";
                request.KeepAlive = true;
                request.Connection = "Keep-Alive";
                //request.Headers.Set("Accept-Encoding", "gzip");
                request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android " + OS_VERSION + "; " + DEVICE_MODEL + " Build/" + OS_BUILD + ")";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(myResponseStream);
                retString = streamReader.ReadToEnd();
                streamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string BuildUrl(string command, string args = "")
        {
            return url + command + "?sessionid=" + sessionID + "client_type=android-swjsq-" + APP_VERSION +
                "&peerid=" + mac + "&time_and=" + Timestamp13() + "&client_version=android-swjsq-" + APP_VERSION +
                "&userid=" + thunderUID + "&os=android-" + HttpUtility.UrlEncode(OS_VERSION + "." + OS_API_LEVEL + DEVICE_MODEL)
                + args;
        }

        public static long Timestamp13()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        #endregion
    }
}
