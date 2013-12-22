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
using System.Text;
using System.Threading;
using UOMachine.Utility;
using UOMachine.Events;
using System.Collections.Generic;

namespace UOMachine.Data
{
    internal static class GumpParser
    {
        private static void OnError(string elementName, string layout)
        {
            Log.LogMessage("Error parsing gump element " + elementName + " in layout:\r\n\r\n" + layout);
        }

        private static string[] GetTokens(string input)
        {
            //@0@1@ = {"0","1"}
            if (input.Contains("@"))
            {
                input = input.Trim('@');
                if (input.Contains("@")) return input.Split('@');
                else
                {
                    string[] output = new string[1];
                    output[0] = input;
                    return output;
                }
            }
            return null;
        }

        private static bool GetInt(string input, out int output)
        {
            if (input.Contains("="))
            {
                string[] strArray = input.Split('=');
                return Int32.TryParse(strArray[1].Trim(), out output);
            }

            output = -1;
            return false;
        }

        public static Gump Parse(int client, int serial, int ID, int x, int y, string layout, string[] text)
        {
            bool closable = true, movable = true, disposable = true, resizable = true;

            List<GumpPage> pageList = new List<GumpPage>();
            List<GumpElement> gumpElementList = new List<GumpElement>();
            GumpElement lastGumpElement = null;
            GumpPage currentPage = null;
            if (String.IsNullOrEmpty(layout)) return null;
            string[] split = layout.Substring(layout.IndexOf('{')).TrimEnd('}', ' ').Split(new char[] { '}' }, StringSplitOptions.RemoveEmptyEntries);
            string[] formatted;
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].TrimStart('{', ' ').Trim();
                formatted = split[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < formatted.Length; j++)
                    formatted[j] = formatted[j].Trim();

                switch (formatted[0])
                {
                    case "noclose":
                        closable = false;
                        break;
                    case "nodispose":
                        disposable = false;
                        break;
                    case "nomove":
                        movable = false;
                        break;
                    case "noresize":
                        resizable = false;
                        break;
                    case "button":
                        try
                        {
                            int gbX, gbY, gbNormalID, gbPressedID, gbID, gbType, gbParam;
                            if (Int32.TryParse(formatted[1], out gbX) &&
                                Int32.TryParse(formatted[2], out gbY) &&
                                Int32.TryParse(formatted[3], out gbNormalID) &&
                                Int32.TryParse(formatted[4], out gbPressedID) &&
                                Int32.TryParse(formatted[5], out gbType) &&
                                Int32.TryParse(formatted[6], out gbParam) &&
                                Int32.TryParse(formatted[7], out gbID))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.button;
                                ge.ParentPage = currentPage;
                                ge.X = gbX;
                                ge.Y = gbY;
                                ge.InactiveID = gbNormalID;
                                ge.ActiveID = gbPressedID;
                                ge.ElementID = gbID;
                                ge.ButtonType = gbType;
                                ge.Param = gbParam;
                                gumpElementList.Add(ge);
                            }
                            else OnError("button", layout);
                        }
                        catch { OnError("button", layout); }
                        break;
                    case "buttontileart":
                        try
                        {
                            int btX, btY, btNormalID, btPressedID, btID, btType, btParam, btItemID, btHue, btWidth, btHeight;
                            if (Int32.TryParse(formatted[1], out btX) &&
                                Int32.TryParse(formatted[2], out btY) &&
                                Int32.TryParse(formatted[3], out btNormalID) &&
                                Int32.TryParse(formatted[4], out btPressedID) &&
                                Int32.TryParse(formatted[5], out btType) &&
                                Int32.TryParse(formatted[6], out btParam) &&
                                Int32.TryParse(formatted[7], out btID) &&
                                Int32.TryParse(formatted[8], out btItemID) &&
                                Int32.TryParse(formatted[9], out btHue) &&
                                Int32.TryParse(formatted[10], out btWidth) &&
                                Int32.TryParse(formatted[11], out btHeight))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.buttontileart;
                                ge.ParentPage = currentPage;
                                ge.X = btX;
                                ge.Y = btY;
                                ge.ElementID = btID;
                                ge.ButtonType = btType;
                                ge.Height = btHeight;
                                ge.Width = btWidth;
                                ge.Hue = btHue;
                                ge.ItemID = btItemID;
                                ge.InactiveID = btNormalID;
                                ge.Param = btParam;
                                ge.ActiveID = btPressedID;
                                gumpElementList.Add(ge);
                                lastGumpElement = ge;
                            }
                            else OnError("buttontileart", layout);
                        }
                        catch { OnError("buttontileart", layout); }
                        break;
                    case "checkertrans":
                        try
                        {
                            int ctX, ctY, ctWidth, ctHeight;
                            if (Int32.TryParse(formatted[1], out ctX) &&
                                Int32.TryParse(formatted[2], out ctY) &&
                                Int32.TryParse(formatted[3], out ctWidth) &&
                                Int32.TryParse(formatted[4], out ctHeight))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.checkertrans;
                                ge.ParentPage = currentPage;
                                ge.X = ctX;
                                ge.Y = ctY;
                                ge.Width = ctWidth;
                                ge.Height = ctHeight;
                                gumpElementList.Add(ge);
                            }
                            else OnError("checkertrans", layout);
                        }
                        catch { OnError("checkertrans", layout); }
                        break;
                    case "croppedtext":
                        try
                        {
                            int crX, crY, crWidth, crHeight, crHue, crText;
                            if (Int32.TryParse(formatted[1], out crX) &&
                                Int32.TryParse(formatted[2], out crY) &&
                                Int32.TryParse(formatted[3], out crWidth) &&
                                Int32.TryParse(formatted[4], out crHeight) &&
                                Int32.TryParse(formatted[5], out crHue) &&
                                Int32.TryParse(formatted[6], out crText))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.croppedtext;
                                ge.ParentPage = currentPage;
                                ge.X = crX;
                                ge.Y = crY;
                                ge.Height = crHeight;
                                ge.Width = crWidth;
                                ge.Hue = crHue;
                                ge.Text = text[crText];
                                gumpElementList.Add(ge);
                            }
                            else OnError("croppedtext", layout);
                        }
                        catch { OnError("croppedtext", layout); }
                        break;
                    case "checkbox":
                        try
                        {
                            int cbX, cbY, cbInactiveID, cbActiveID, cbID, cbState;
                            if (Int32.TryParse(formatted[1], out cbX) &&
                                Int32.TryParse(formatted[2], out cbY) &&
                                Int32.TryParse(formatted[3], out cbInactiveID) &&
                                Int32.TryParse(formatted[4], out cbActiveID) &&
                                Int32.TryParse(formatted[5], out cbState) &&
                                Int32.TryParse(formatted[6], out cbID))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.checkbox;
                                ge.ParentPage = currentPage;
                                ge.X = cbX;
                                ge.Y = cbY;
                                ge.InactiveID = cbInactiveID;
                                ge.ActiveID = cbActiveID;
                                ge.ElementID = cbID;
                                ge.InitialState = cbState == 1;
                                gumpElementList.Add(ge);
                            }
                            else OnError("checkbox", layout);
                        }
                        catch { OnError("checkbox", layout); }
                        break;
                    case "page":
                        if (currentPage == null)
                            currentPage = new GumpPage();
                        else
                        {
                            currentPage.GumpElements = gumpElementList.ToArray();
                            pageList.Add(currentPage);
                            currentPage = new GumpPage();
                            gumpElementList = new List<GumpElement>();
                        }
                        int page;
                        if (Int32.TryParse(formatted[1], out page))
                        {
                            currentPage.Page = page;
                        }
                        else OnError("page", layout);
                        break;
                    case "gumppic":
                        try
                        {
                            int gpX, gpY, gpID, gpHue;
                            if (Int32.TryParse(formatted[1], out gpX) &&
                                Int32.TryParse(formatted[2], out gpY) &&
                                Int32.TryParse(formatted[3], out gpID))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.gumppic;
                                ge.ParentPage = currentPage;
                                if (formatted.Length > 4)
                                {
                                    for (int gp = 4; gp < formatted.Length; gp++)
                                    {
                                        if (formatted[gp].Contains("hue"))
                                        {
                                            if (GetInt(formatted[gp], out gpHue))
                                                ge.Hue = gpHue;
                                            else OnError("gumppic", layout);
                                        }
                                        else ge.Args = formatted[gp];
                                    }
                                }
                                ge.X = gpX;
                                ge.Y = gpY;
                                ge.ElementID = gpID;
                                gumpElementList.Add(ge);
                            }
                            else OnError("gumppic", layout);
                        }
                        catch { OnError("gumppic", layout); }
                        break;
                    case "gumppictiled":
                        try
                        {
                            int gtX, gtY, gtWidth, gtHeight, gtID;
                            if (Int32.TryParse(formatted[1], out gtX) &&
                                Int32.TryParse(formatted[2], out gtY) &&
                                Int32.TryParse(formatted[3], out gtWidth) &&
                                Int32.TryParse(formatted[4], out gtHeight) &&
                                Int32.TryParse(formatted[5], out gtID))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.gumppictiled;
                                ge.ParentPage = currentPage;
                                ge.X = gtX;
                                ge.Y = gtY;
                                ge.Width = gtWidth;
                                ge.Height = gtHeight;
                                ge.ElementID = gtID;
                                gumpElementList.Add(ge);
                            }
                            else OnError("gumppictiled", layout);
                        }
                        catch { OnError("gumppictiled", layout); }
                        break;
                    case "kr_xmfhtmlgump":
                        try
                        {
                            int xgX, xgY, xgWidth, xgHeight, xgCliloc, xgBackground, xgScrollbar;
                            if (Int32.TryParse(formatted[1], out xgX) &&
                               Int32.TryParse(formatted[2], out xgY) &&
                               Int32.TryParse(formatted[3], out xgWidth) &&
                               Int32.TryParse(formatted[4], out xgHeight) &&
                               Int32.TryParse(formatted[5], out xgCliloc) &&
                               Int32.TryParse(formatted[6], out xgBackground) &&
                               Int32.TryParse(formatted[7], out xgScrollbar))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.kr_xmfhtmlgump;
                                ge.ParentPage = currentPage;
                                ge.X = xgX;
                                ge.Y = xgY;
                                ge.Width = xgWidth;
                                ge.Height = xgHeight;
                                ge.Cliloc = xgCliloc;
                                ge.Background = xgBackground == 1;
                                ge.ScrollBar = xgScrollbar == 1;
                                if (xgCliloc != 0)
                                    ge.Text = Cliloc.GetProperty(xgCliloc);
                                gumpElementList.Add(ge);
                            }
                            else OnError("kr_xmfhtmlgump", layout);
                        }
                        catch { OnError("kr_xmfhtmlgump", layout); }
                        break;
                    case "mastergump":
                        try
                        {
                            int mgID;
                            if (Int32.TryParse(formatted[1], out mgID))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.mastergump;
                                ge.ParentPage = currentPage;
                                ge.ElementID = mgID;
                                gumpElementList.Add(ge);
                            }
                            else OnError("mastergump", layout);
                        }
                        catch { OnError("mastergump", layout); }
                        break;
                    case "radio":
                        try
                        {
                            int rX, rY, rInactiveID, rActiveID, rID, rState;
                            if (Int32.TryParse(formatted[1], out rX) &&
                                Int32.TryParse(formatted[2], out rY) &&
                                Int32.TryParse(formatted[3], out rInactiveID) &&
                                Int32.TryParse(formatted[4], out rActiveID) &&
                                Int32.TryParse(formatted[5], out rState) &&
                                Int32.TryParse(formatted[6], out rID))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.radio;
                                ge.ParentPage = currentPage;
                                ge.X = rX;
                                ge.Y = rY;
                                ge.InactiveID = rInactiveID;
                                ge.ActiveID = rActiveID;
                                ge.ElementID = rID;
                                ge.InitialState = rState == 1;
                                gumpElementList.Add(ge);
                            }
                            else OnError("radio", layout);
                        }
                        catch { OnError("radio", layout); }
                        break;
                    case "resizepic":
                        try
                        {
                            int rpX, rpY, rpWidth, rpHeight, rpID;
                            if (Int32.TryParse(formatted[1], out rpX) &&
                                Int32.TryParse(formatted[2], out rpY) &&
                                Int32.TryParse(formatted[3], out rpID) &&
                                Int32.TryParse(formatted[4], out rpWidth) &&
                                Int32.TryParse(formatted[5], out rpHeight))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.resizepic;
                                ge.ParentPage = currentPage;
                                ge.X = rpX;
                                ge.Y = rpY;
                                ge.Width = rpWidth;
                                ge.Height = rpWidth;
                                ge.ElementID = rpID;
                                gumpElementList.Add(ge);
                            }
                            else OnError("resizepic", layout);
                        }
                        catch { OnError("resizepic", layout); }
                        break;
                    case "text":
                        try
                        {
                            int tX, tY, tHue, tText;
                            if (Int32.TryParse(formatted[1], out tX) &&
                                Int32.TryParse(formatted[2], out tY) &&
                                Int32.TryParse(formatted[3], out tHue) &&
                                Int32.TryParse(formatted[4], out tText))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.text;
                                ge.ParentPage = currentPage;
                                ge.X = tX;
                                ge.Y = tY;
                                ge.Hue = tHue;
                                ge.Text = text[tText];
                                gumpElementList.Add(ge);
                            }
                            else OnError("text", layout);
                        }
                        catch { OnError("text", layout); }
                        break;
                    case "tooltip":
                        try
                        {
                            int tooltip;
                            if (Int32.TryParse(formatted[1], out tooltip) && lastGumpElement != null)
                            {
                                lastGumpElement.Tooltip = tooltip;
                                lastGumpElement.Text = Cliloc.GetProperty(tooltip);
                            }
                            else OnError("tooltip", layout);
                        }
                        catch { OnError("tooltip", layout); }
                        break;
                    case "htmlgump":
                        try
                        {
                            int hgX, hgY, hgWidth, hgHeight, hgText, hgBackground, hgScrollbar;
                            if (Int32.TryParse(formatted[1], out hgX) &&
                                Int32.TryParse(formatted[2], out hgY) &&
                                Int32.TryParse(formatted[3], out hgWidth) &&
                                Int32.TryParse(formatted[4], out hgHeight) &&
                                Int32.TryParse(formatted[5], out hgText) &&
                                Int32.TryParse(formatted[6], out hgBackground) &&
                                Int32.TryParse(formatted[7], out hgScrollbar))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.htmlgump;
                                ge.ParentPage = currentPage;
                                ge.X = hgX;
                                ge.Y = hgY;
                                ge.Width = hgWidth;
                                ge.Height = hgHeight;
                                ge.Text = text[hgText];
                                ge.ScrollBar = hgScrollbar == 1;
                                ge.Background = hgBackground == 1;
                                gumpElementList.Add(ge);
                            }
                            else OnError("htmlgump", layout);
                        }
                        catch { OnError("htmlgump", layout); }
                        break;
                    case "textentry":
                        try
                        {
                            int teX, teY, teWidth, teHeight, teText, teHue, teID;
                            if (Int32.TryParse(formatted[1], out teX) &&
                               Int32.TryParse(formatted[2], out teY) &&
                               Int32.TryParse(formatted[3], out teWidth) &&
                               Int32.TryParse(formatted[4], out teHeight) &&
                               Int32.TryParse(formatted[5], out teHue) &&
                               Int32.TryParse(formatted[6], out teID) &&
                               Int32.TryParse(formatted[7], out teText))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.textentry;
                                ge.ParentPage = currentPage;
                                ge.X = teX;
                                ge.Y = teY;
                                ge.Height = teHeight;
                                ge.Width = teWidth;
                                ge.Hue = teHue;
                                ge.ElementID = teID;
                                ge.Text = text[teText];
                                gumpElementList.Add(ge);
                            }
                            else OnError("textentry", layout);
                        }
                        catch { OnError("textentry", layout); }
                        break;
                    case "textentrylimited":
                        try
                        {
                            int tlX, tlY, tlWidth, tlHeight, tlText, tlHue, tlID, tlSize;
                            if (Int32.TryParse(formatted[1], out tlX) &&
                               Int32.TryParse(formatted[2], out tlY) &&
                               Int32.TryParse(formatted[3], out tlWidth) &&
                               Int32.TryParse(formatted[4], out tlHeight) &&
                               Int32.TryParse(formatted[5], out tlHue) &&
                               Int32.TryParse(formatted[6], out tlID) &&
                               Int32.TryParse(formatted[7], out tlText) &&
                               Int32.TryParse(formatted[8], out tlSize))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.textentrylimited;
                                ge.ParentPage = currentPage;
                                ge.X = tlX;
                                ge.Y = tlY;
                                ge.Height = tlHeight;
                                ge.Width = tlWidth;
                                ge.Hue = tlHue;
                                ge.ElementID = tlID;
                                ge.Text = text[tlText];
                                ge.Size = tlSize;
                                gumpElementList.Add(ge);
                            }
                            else OnError("textentrylimited", layout);
                        }
                        catch { OnError("textentrylimited", layout); }
                        break;
                    case "tilepic":
                        try
                        {
                            int tpX, tpY, tpID;
                            if (Int32.TryParse(formatted[1], out tpX) &&
                               Int32.TryParse(formatted[2], out tpY) &&
                               Int32.TryParse(formatted[3], out tpID))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.tilepic;
                                ge.ParentPage = currentPage;
                                ge.X = tpX;
                                ge.Y = tpY;
                                ge.ElementID = tpID;
                                gumpElementList.Add(ge);
                            }
                            else OnError("tilepic", layout);
                        }
                        catch { OnError("tilepic", layout); }
                        break;
                    case "tilepichue":
                        try
                        {
                            int tpX, tpY, tpID, tpHue;
                            if (Int32.TryParse(formatted[1], out tpX) &&
                               Int32.TryParse(formatted[2], out tpY) &&
                               Int32.TryParse(formatted[3], out tpID) &&
                               Int32.TryParse(formatted[4], out tpHue))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.tilepichue;
                                ge.ParentPage = currentPage;
                                ge.X = tpX;
                                ge.Y = tpY;
                                ge.ElementID = tpID;
                                ge.Hue = tpHue;
                                gumpElementList.Add(ge);
                            }
                            else OnError("tilepichue", layout);
                        }
                        catch { OnError("tilepichue", layout); }
                        break;
                    case "xmfhtmlgump":
                        try
                        {
                            int xgX, xgY, xgWidth, xgHeight, xgCliloc, xgBackground, xgScrollbar;
                            if (Int32.TryParse(formatted[1], out xgX) &&
                               Int32.TryParse(formatted[2], out xgY) &&
                               Int32.TryParse(formatted[3], out xgWidth) &&
                               Int32.TryParse(formatted[4], out xgHeight) &&
                               Int32.TryParse(formatted[5], out xgCliloc) &&
                               Int32.TryParse(formatted[6], out xgBackground) &&
                               Int32.TryParse(formatted[7], out xgScrollbar))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.xmfhtmlgump;
                                ge.ParentPage = currentPage;
                                ge.X = xgX;
                                ge.Y = xgY;
                                ge.Width = xgWidth;
                                ge.Height = xgHeight;
                                ge.Cliloc = xgCliloc;
                                ge.Background = xgBackground == 1;
                                ge.ScrollBar = xgScrollbar == 1;
                                if (xgCliloc != 0)
                                    ge.Text = Cliloc.GetProperty(xgCliloc);
                                gumpElementList.Add(ge);
                            }
                            else OnError("xmfhtmlgump", layout);
                        }
                        catch { OnError("xmfhtmlgump", layout); }
                        break;
                    case "xmfhtmlgumpcolor":
                        try
                        {
                            int xcX, xcY, xcWidth, xcHeight, xcCliloc, xcBackground, xcScrollbar, xcHue;
                            if (Int32.TryParse(formatted[1], out xcX) &&
                               Int32.TryParse(formatted[2], out xcY) &&
                               Int32.TryParse(formatted[3], out xcWidth) &&
                               Int32.TryParse(formatted[4], out xcHeight) &&
                               Int32.TryParse(formatted[5], out xcCliloc) &&
                               Int32.TryParse(formatted[6], out xcBackground) &&
                               Int32.TryParse(formatted[7], out xcScrollbar) &&
                               Int32.TryParse(formatted[8], out xcHue))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.xmfhtmlgumpcolor;
                                ge.ParentPage = currentPage;
                                ge.X = xcX;
                                ge.Y = xcY;
                                ge.Width = xcWidth;
                                ge.Height = xcHeight;
                                ge.Cliloc = xcCliloc;
                                ge.Background = xcBackground == 1;
                                ge.ScrollBar = xcScrollbar == 1;
                                ge.Hue = xcHue;
                                if (xcCliloc != 0)
                                    ge.Text = Cliloc.GetProperty(xcCliloc);
                                gumpElementList.Add(ge);
                            }
                            else OnError("xmfhtmlgumpcolor", layout);
                        }
                        catch { OnError("xmfhtmlgumpcolor", layout); }
                        break;
                    case "xmfhtmltok":
                        try
                        {
                            int xtX, xtY, xtWidth, xtHeight, xtCliloc, xtBackground, xtScrollbar, xtHue;
                            if (Int32.TryParse(formatted[1], out xtX) &&
                               Int32.TryParse(formatted[2], out xtY) &&
                               Int32.TryParse(formatted[3], out xtWidth) &&
                               Int32.TryParse(formatted[4], out xtHeight) &&
                               Int32.TryParse(formatted[5], out xtCliloc) &&
                               Int32.TryParse(formatted[6], out xtBackground) &&
                               Int32.TryParse(formatted[7], out xtScrollbar) &&
                               Int32.TryParse(formatted[8], out xtHue))
                            {
                                GumpElement ge = new GumpElement();
                                ge.Type = ElementType.xmfhtmltok;
                                ge.ParentPage = currentPage;
                                string[] args = GetTokens(formatted[9]);
                                ge.Text = Cliloc.GetLocalString(xtCliloc, args);
                                ge.Args = formatted[9];
                                ge.X = xtX;
                                ge.Y = xtY;
                                ge.Width = xtWidth;
                                ge.Height = xtHeight;
                                ge.Hue = xtHue;
                                ge.Cliloc = xtCliloc;
                                ge.ScrollBar = xtScrollbar == 1;
                                ge.Background = xtBackground == 1;
                                gumpElementList.Add(ge);
                            }
                            else OnError("xmfhtmltok", layout);
                        }
                        catch { OnError("xmfhtmltok", layout); }
                        break;
                    default:
                        Log.LogMessage(string.Format("Unknown element \"{0}\" in custom gump layout:\r\n\r\n{1}", formatted[0], layout));
                        break;
                }
            }

            if (currentPage != null)
            {
                currentPage.GumpElements = gumpElementList.ToArray();
                pageList.Add(currentPage);
            }
            Gump g = new Gump(client, x, y, ID, serial, layout, text, gumpElementList.ToArray(), pageList.ToArray());
            g.Closable = closable;
            g.Disposable = disposable;
            g.Movable = movable;
            g.Resizable = resizable;
            return g;
        }
    }
}