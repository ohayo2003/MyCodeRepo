using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SMSManagement.Web.SP
{
    public class DllImports
    {
        //User
        [DllImport("PlagiarismDetection.dll", CharSet = CharSet.Ansi, EntryPoint = "pdget_simReg_simPos")]
        public static extern int pdget_simReg_simPos(byte[] Lbuf, byte[] Rbuf, int type);
        public void SetInfo_Main(ref string id_main, ref string pw_main)
        {
            try
            {
                byte[] user = new byte[100];
                byte[] pwd = new byte[100];
                int rs = pdget_simReg_simPos(user, pwd, 8779);

                string temp_id = System.Text.Encoding.Default.GetString(user);
                string temp_pw = System.Text.Encoding.Default.GetString(pwd);

                if (temp_id.IndexOf("\0") != -1)
                {
                    temp_id = temp_id.Substring(0, temp_id.IndexOf("\0"));
                }
                if (temp_pw.IndexOf("\0") != -1)
                {
                    temp_pw = temp_pw.Substring(0, temp_pw.IndexOf("\0"));
                }

                id_main = temp_id;
                pw_main = temp_pw;
            }
            catch
            {
                id_main = string.Empty;
                pw_main = string.Empty;

            }
        }

        //Server
        [DllImport("PlagiarismDetection.dll", CharSet = CharSet.Ansi, EntryPoint = "pdget_simFile_simPos")]
        public static extern int pdget_simFile_simPos(byte[] Lbuf, byte[] Rbuf, int type);

        public void SetInfo(ref string id, ref string pw)
        {
            byte[] user = new byte[100];
            byte[] pwd = new byte[100];
            int rs = pdget_simFile_simPos(user, pwd, 8615);

            string temp_id = System.Text.Encoding.Default.GetString(user);
            string temp_pw = System.Text.Encoding.Default.GetString(pwd);

            if (temp_id.IndexOf("\0") != -1)
            {
                temp_id = temp_id.Substring(0, temp_id.IndexOf("\0"));
            }
            if (temp_pw.IndexOf("\0") != -1)
            {
                temp_pw = temp_pw.Substring(0, temp_pw.IndexOf("\0"));
            }

            id = temp_id;
            pw = temp_pw;
        }



    }
}
