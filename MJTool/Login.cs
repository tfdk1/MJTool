﻿/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012-7-8
 * Time: 9:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MJTool
{
	partial class QueryManager
	{
		public string strUserName;
		public string strPassword;
		public bool CheckIfLogined(string strQueryResult)
		{
			return true;
		}
		
		public string QueryLoginPage()
		{
			this.QrySta = QueryStatus.NotLogined;
			return PageQuery("game.weibo.com", "mengjiangwushuang/?origin=1407", null);
		}
		
		public void Login()
		{
			string result = QueryLoginPage();
			/*
			 * <div class=\"box\">
			 * href="http://weibo.com/login.php?
			 * entry=weiyouxi&param=r%3D2337087298&url=http%3A%2F%2Fgame.weibo.com%2Fmengjiangwushuang%2F%3Forigin%3D1407"
			 *  class="btn-login">登录微博</a>
			 * */
			Match m = Regex.Match(result, "<div class=\"box\">\\s*?<a\\s*?href=\"http://weibo\\.com/(.*?)\"\\s*?class=\"btn-login\">登录微博</a>", RegexOptions.Singleline);
			if (!m.Success)
			{
				DebugLog("无法解析[mengjiangwushuang/?origin=1407]");
			}
			string strLoginURL = m.Groups[1].Value;
			result = PageQuery("weibo.com", strLoginURL, null);
			
			Dictionary<string, string> data = new Dictionary<string, string>();
			data.Add("entry", "sso");
			data.Add("callback", "sinaSSOController.preloginCallBack");
			data.Add("su", base64_encode("undefined"));
			data.Add("rsakt", "mod");
			data.Add("client", "ssologin.js(v1.4.2)");
			data.Add("_", UnixTimeStamp(DateTime.Now).ToString());
			string strSvrTimeURL = "sso/prelogin.php?"
				+ CreateQueryString(data);
			result = PageQuery("login.sina.com.cn", strSvrTimeURL, null);
			
			string strSvrTime, pcid, nonce, pubkey, rsakv;
			m = Regex.Match(result, "\"servertime\":(.*?),\"pcid\":\"(.*?)\",\"nonce\":\"(.*?)\"," +
			                "\"pubkey\":\"(.*?)\",\"rsakv\":\"(.*?)\"", RegexOptions.Singleline);
			if (!m.Success)
			{
				DebugLog("无法解析[sso/prelogin.php]");
			}
			strSvrTime = m.Groups[1].Value;
			pcid = m.Groups[2].Value;
			nonce = m.Groups[3].Value;
			pubkey = m.Groups[4].Value;
			rsakv = m.Groups[5].Value;
			
			Random r = new Random();
			data.Clear();
			data.Add("entry", "weibo");
			data.Add("gateway", "1");
			data.Add("from", "");
			data.Add("savestate", "7");
			data.Add("useticket", "1");
			data.Add("vsnf", "1");
			data.Add("ssosimplelogin", "1");
			data.Add("su", base64_encode(strUserName));
			data.Add("service", "miniblog");
			data.Add("servertime", strSvrTime);
			data.Add("nonce", nonce);
			data.Add("pwencode", "rsa2");
			data.Add("rsakv", rsakv);
			data.Add("sp", PswRSAEncode(strPassword, pubkey));
			data.Add("encoding", "UTF-8");
			data.Add("prelt", r.Next(100, 150).ToString());
			data.Add("url", "http://ajaxlogin.php?framelogin=1&callback=parent.sinaSSOController.feedBackUrlCallBack");
			data.Add("returntype", "META");
			
			result = PageQuery("login.sina.com.cn", "sso/login.php?client=ssologin.js(v1.4.2)", data);
		}
		
		public static string sKey = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
		public static string base64_encode(string str)
		{
			byte[] temp1 = Encoding.UTF8.GetBytes(HttpUtility.UrlEncode(str));
			string temp2 = Convert.ToBase64String(temp1);
			return temp2;
		}
		
		public static string base64_decode(string str)
		{
			byte[] sa = Convert.FromBase64String(str);
	        Encoding Ansi = Encoding.GetEncoding("GB2312");
	        string wa = Ansi.GetString(sa);
	        return wa;
		}
		
		public static string PswRSAEncode(string strPassword, string pubkey)
		{
			return strPassword;
		}
		
		public static string CreateQueryString(Dictionary<string, string> Data)
		{
			StringBuilder sb = new StringBuilder();
			foreach(KeyValuePair<string, string> x in Data)
			{
				if(sb.Length != 0)
					sb.Append("&");

				sb.Append(HttpUtility.UrlEncode(x.Key));
				sb.Append("=");
				sb.Append(HttpUtility.UrlEncode(x.Value));
			}
			return sb.ToString();
		}
		
		private long UnixTimeStamp(DateTime time)
		{
			return Convert.ToInt64(time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
		}
		
		public bool ParseLogin(string strQueryResult)
		{
			return true;
		}
	}
}