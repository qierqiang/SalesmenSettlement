using SalesmenSettlement.LocalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public static class Msgbox
    {
        public static void Information(string msg) =>
            MessageBox.Show(msg, AppConfig.Instance.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Warning(string msg) =>
            MessageBox.Show(msg, AppConfig.Instance.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static void Error(string msg) =>
            MessageBox.Show(msg, AppConfig.Instance.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static bool Confirm(string msg) =>
            MessageBox.Show(msg, AppConfig.Instance.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        public static bool DontSaveConfirm(string msg = "当前内容已经修改，确定要放弃修改并退出吗？") =>
            MessageBox.Show(msg, AppConfig.Instance.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

        public static DialogResult SaveAndQuitConfirm(string msg = "当前内容已经修改，需要保存并退出吗？") =>
            MessageBox.Show(msg, AppConfig.Instance.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button3);
    }
}
