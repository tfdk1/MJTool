﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MJTool
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private delegate void dlgWriteLog(string log);
		private delegate void dlgClearLog();
		
		private QueryManager sInsMgr = new QueryManager();
		private Account curAcc= null;
		
		void MainFormLoad(object sender, EventArgs e)
		{
			sInsMgr.OnUIUpdate += new EventHandler<UIUpdateArgs>(CallBack_UIUpdate);
		}
		
		void CallBack_UIUpdate(object sender, UIUpdateArgs e)
		{
			try
			{
				if (e.uiType == UIUpdateTypes.LogAppending)
				{
					LogArgs e_log = e as LogArgs;
					Invoke(new dlgWriteLog(WriteLog)
					       , new object[] { e_log.strLog });
				}
				else if (e.uiType == UIUpdateTypes.LogClear)
				{
					Invoke(new dlgClearLog(ClearLog));
				}
			}
			catch (Exception)
			{ }
		}
		
		private int nLogCnt = 0;
		private void WriteLog(string log)
		{
			if (nLogCnt >= 100)
			{
				this.tbLog.Text = "";
				nLogCnt = 0;
			}
			this.tbLog.AppendText("[" + DateTime.Now.ToString() + "] " + log + "\r\n");
			nLogCnt++;
		}
		
		private void ClearLog()
		{
			this.tbLog.Text = "";
		}
		
		void BtLoginClick(object sender, EventArgs e)
		{
			curAcc = new Account(this.tbAccount.Text, this.tbPassword.Text);
			curAcc.upCall = sInsMgr;
			sInsMgr.Login(curAcc);
			
			this.btLogin.Enabled = false;
			this.btLogout.Enabled = true;
		}
		
		void BtLogoutClick(object sender, EventArgs e)
		{
			sInsMgr.Logout(curAcc);
			this.btLogin.Enabled = true;
			this.btLogout.Enabled = false;
		}
	}
}
