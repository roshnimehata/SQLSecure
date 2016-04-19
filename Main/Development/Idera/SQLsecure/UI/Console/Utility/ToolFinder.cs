using System;
using System.Collections.Generic;
using System.Diagnostics ;
using System.IO ;
using System.Reflection ;
using Microsoft.Win32 ;

namespace Idera.SQLsecure.UI.Console.Utility
{
	/// <summary>
	/// ToolFinder class is used to find any registered Idera products
	/// </summary>
	public class ToolFinder
	{
	    public ToolFinder()
	    {
	    }

        public static RegisteredToolList GetRegisteredTools(string containerKeyName)
        {
            if (containerKeyName == null)
                return new RegisteredToolList();

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(containerKeyName, false))
            {
                if (key == null)
                    return new RegisteredToolList();
                else
                    return GetRegisteredTools(key);
            }
        }

        public static RegisteredToolList GetRegisteredTools(RegistryKey containerKey)
        {
            RegisteredToolList retVal = new RegisteredToolList();
            if (containerKey == null)
                return retVal;

            string sAssembly = Assembly.GetExecutingAssembly().Location;

            foreach (string subKeyName in containerKey.GetSubKeyNames())
            {
                using (RegistryKey subKey = containerKey.OpenSubKey(subKeyName, false))
                {
                    string sName = null;
                    string sPath = null;
                    object o;

                    o = subKey.GetValue("ProductName");
                    if (o is string)
                        sName = (string)o;
                    o = subKey.GetValue("ProductPath");
                    if (o is string)
                        sPath = (string)o;
                    if (sName != null && sPath != null)
                    {
                        // We don't add ourselves
                        if (String.Compare(sPath.Trim(), sAssembly, true) != 0)
                            retVal.Add(new RegisteredTool(sName, sPath));
                    }
                    else
                    {
                        //Invalid entries
                    }
                }
            }
            return retVal;
        }
    }

    public class RegisteredToolList : List<RegisteredTool>
    {
    }

    public class RegisteredTool
    {
        private string _name;
        private string _path;

        public RegisteredTool(string name, string path)
        {
            _name = name;
            _path = path;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public bool IsValid
        {
            get
            {
                // No null names or empty names (spaces)
                if (_name == null || _name.Trim().Length == 0)
                    return false;
                // No null paths or invalid files
                if (_path == null || !File.Exists(_path))
                    return false;
                return true;
            }
        }

        public void Launch()
        {
            if (IsValid)
            {
                ProcessStartInfo pInfo = new ProcessStartInfo(_path);
                FileInfo file = new FileInfo(_path);

                pInfo.UseShellExecute = false;
                pInfo.WorkingDirectory = file.DirectoryName;
                Process.Start(pInfo);
            }
            else
                throw new Exception("Invalid Registered Tool");
        }

        public void LaunchEvent(object sender, EventArgs e)
        {
            Launch();
        }
    }
}
