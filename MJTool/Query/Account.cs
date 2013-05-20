﻿using System;

namespace MJTool
{
	public partial class Account
	{
		public QueryManager upCall = null;
		
		// 新浪的账号和密码
		public string strUserName;
		public string strPassword;

		// 利用验证码从微博获得的会话信息
		public string wyx_user_id;
		public string wyx_session_key;
		public string wyx_create;
		public string wyx_expire;
		public string wyx_signature;
		
		// 新手引导完成情况
		public int finishGuide = 0;
		
		public UserRoot root = new UserRoot();
		
		// 判断用户是否登录
		public bool bIsLogined = false;
		
		public Account(string name, string pswd)
		{
			strUserName = name;
			strPassword = pswd;
		}
	}
}
