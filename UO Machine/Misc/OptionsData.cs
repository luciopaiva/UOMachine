/* Copyright (C) 2009 Matthew Geyer
 * 
 * This file is part of UO Machine.
 * 
 * UO Machine is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * UO Machine is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with UO Machine.  If not, see <http://www.gnu.org/licenses/>. */

using System;
using System.IO;
using UOMachine.Utility;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using ICSharpCode.AvalonEdit;
using System.Windows.Forms;

namespace UOMachine
{
    [Serializable, XmlRootAttribute(ElementName = "Options")]
    public sealed class OptionsData
    {
        public string UOFolder { get; set; }
        public string UOClientPath { get; set; }
        public string RazorFolder { get; set; }
        public string UOSFolder { get; set; }
        public bool PatchClientEncryptionUOM { get; set; }
        public bool PatchStaminaCheck { get; set; }
        public bool PatchClientEncryption { get; set; }
        public bool PatchAlwaysLight { get; set; }
        public bool EncryptedServer { get; set; }
        public string Server { get; set; }
        public ushort Port { get; set; }
        public int CacheLevel { get; set; }
        public TextEditorOptions TextEditorOptions { get; set; }

        public OptionsData()
        {
            this.TextEditorOptions = new TextEditorOptions();
        }

        public static OptionsData CreateDefault()
        {
            OptionsData od = new OptionsData();
            od.UOFolder = RegistryHelper.GetUOPath();
            od.UOClientPath = Path.Combine(od.UOFolder, "client.exe");
            od.Server = "localhost";
            od.PatchAlwaysLight = true;
            od.Port = 2593;
            od.PatchClientEncryption = true;
            od.RazorFolder = RegistryHelper.GetRazorPath();
            od.CacheLevel = 0;
            od.TextEditorOptions = new TextEditorOptions();
            od.PatchStaminaCheck = true;
            od.PatchClientEncryptionUOM = false;
            Serialize("options.xml", od);
            return od;
        }

        private static void ShowErrorMessage(string operation, Exception x)
        {
            string message;
            if (x.InnerException != null)
                message = x.InnerException.Message;
            else message = x.Message;
            MessageBox.Show(string.Format("Error occured while {0}:\r\n{1}", operation, message));
        }

        public static void Serialize(string fileName, OptionsData optionsData)
        {
            XmlWriter xw = null;
            try
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Encoding = Encoding.Unicode;
                xws.CloseOutput = true;
                xws.Indent = true;
                xws.NewLineOnAttributes = true;
                xw = XmlWriter.Create(fileName, xws);
                XmlSerializer xs = new XmlSerializer(typeof(OptionsData));
                xs.Serialize(xw, optionsData);
                xw.Flush();
                xw.Close();
            }
            catch (Exception ex)
            {
                if (xw != null)
                {
                    try
                    {

                        xw.Flush();
                        xw.Close();
                    }
                    catch { }
                }
                ShowErrorMessage("saving options to file", ex);
            }
        }

        public static OptionsData Deserialize(string fileName)
        {
            if (!File.Exists(fileName)) return CreateDefault();
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(OptionsData));
                OptionsData optionsData = (OptionsData)xs.Deserialize(fs);
                fs.Close();
                return optionsData;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    try
                    {
                        fs.Close();
                    }
                    catch { }
                }
                ShowErrorMessage("loading options from file", ex);
                return CreateDefault();
            }
        }

        public bool IsValid()
        {
            return ((this.CacheLevel >= 0 && this.CacheLevel <= 3) &&
                Directory.Exists(this.UOFolder) &&
                File.Exists(this.UOClientPath));
        }

    }
}