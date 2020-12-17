using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace XunleiAcc
{
    class XLAuthClient
    {
        #region Status vars
        DateTime lastlogin_try;
        DateTime lastlogin_succ;
        public string sessionid { get; private set; }
        public string userid { get; private set; }
        public string loginkey { get; private set; }
        JObject loginPayload;
        #endregion

        #region consts
        #endregion

        #region private actions
        private JObject PostJson(string url, JObject json)
        {
            //json参数
            string jsonParam = json.ToString();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.UserAgent = "android-async-http/xl-acc-sdk/version-2.1.1.177662";
            byte[] byteData = Encoding.UTF8.GetBytes(jsonParam);
            int length = byteData.Length;
            request.ContentLength = length;
            Stream writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
            return (JObject)JsonConvert.DeserializeObject(responseString);
        }
        #endregion

        #region public actions
        public bool login(string username, string password)
        {
            lastlogin_try = DateTime.Now;
            string deviceID = Encrypt.MD5("HOST_NAME_IS:" + Dns.GetHostName() + "&UNAME_IS:" + username);
            string deviceSign = "div101." + deviceID + Encrypt.MD5(
                Encrypt.SHA1(deviceID + "com.xunlei.vip.swjsq68c7f21687eed3cdb400ca11fc2263c998"));

            JObject jb = new JObject
            {
                { "protocolVersion", DeviceInfo.PROTOCOL_VERSION },
                { "sequenceNo", 1000001 },
                { "platformVersion", "2" },
                { "sdkVersion", "177662" },
                { "peerID", DeviceInfo.mac },
                { "businessType", "68" },
                { "clientVersion", DeviceInfo.APP_VERSION },
                { "devicesign", deviceSign },
                { "isCompressed", "0" },
                { "userName", username },
                { "passWord", password },
                { "sessionID", "" },
                { "verifyKey", "" },
                { "verifyCode", "" },
                { "appName", "ANDROID-com.xunlei.vip.swjsq" },
                { "deviceModel", DeviceInfo.DEVICE_MODEL },
                { "deviceName", DeviceInfo.DEVICE },
                { "OSVersion", DeviceInfo.OS_VERSION }
            };
            loginPayload = (JObject)jb.DeepClone();
            JObject res = PostJson("https://mobile-login.xunlei.com:443/login", jb);
            if (res.Value<int>("errorCode") != 0) return false;//登录出错
            if ((res["sessionID"] == null) ||
                (res["userID"] == null) ||
                (res["loginKey"] == null))//登录数据不完整
                return false;

            //登录检查完成

            sessionid = res.Value<string>("sessionID");
            userid = res.Value<string>("userID");
            loginkey = res.Value<string>("loginKey");

            return true;//登录成功
        }

        public bool renew_stat()
        {
            JObject payload = (JObject)loginPayload.DeepClone();
            payload["sequenceNo"] = 1000001;
            payload["userName"] = userid;
            payload["loginKey"] = loginkey;
            payload.Remove("passWord");
            payload.Remove("verifyKey");
            payload.Remove("verifyCode");
            payload.Remove("sessionID");
            var res = PostJson("https://mobile-login.xunlei.com:443/loginkey", payload);

            if (res.Value<int>("errorCode") != 0) return false;//登录出错
            if ((res["sessionID"] == null) ||
                (res["userID"] == null) ||
                (res["loginKey"] == null))//登录数据不完整
                return false;

            //登录检查完成

            sessionid = res.Value<string>("sessionID");
            userid = res.Value<string>("userID");
            loginkey = res.Value<string>("loginKey");

            return true;//登录成功
        }

        public JObject checkVas(short vasid)
        {
            //https://mobile-login.xunlei.com:443/getuserinfo
            JObject payload = (JObject)loginPayload.DeepClone();
            payload["sequenceNo"] = 1000002;
            payload["vasid"] = vasid;
            payload["userID"] = userid;
            payload["sessionID"] = sessionid;
            payload.Remove("passWord");
            payload.Remove("verifyKey");
            payload.Remove("verifyCode");
            payload.Remove("userName");
            var j = PostJson("https://mobile-login.xunlei.com:443/getuserinfo", payload);
            //if(j.)
            return j;
        }
        #endregion
    }
}