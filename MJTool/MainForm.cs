﻿/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012-9-2
 * Time: 13:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
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
		
		private void WriteLog(string log)
        {
			this.tbLog.AppendText("[" + DateTime.Now.ToString() + "] " + log + "\r\n");
        }
		
		private void ClearLog()
		{
			this.tbLog.Text = "";
		}
		
		void BtLoginClick(object sender, EventArgs e)
		{
			sInsMgr.strUserName = this.tbAccount.Text;
			sInsMgr.strPassword = this.tbPassword.Text;
			sInsMgr.Login();
		}
	}
}