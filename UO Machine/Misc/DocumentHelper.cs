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
using System.Text;

namespace UOMachine
{
    internal static class DocumentHelper
    {
        /// <summary>
        /// Simple method to tabify source code according to curly brace placement.
        /// </summary>
        /// <param name="text">String to format.</param>
        /// <param name="tab">String which represents tab to use.</param>
        public static string Format(string text, string tab)
        {
            char[] whitespace = new char[] { '\t', ' ' };
            StringReader sr = new StringReader(text);
            StringBuilder newText = new StringBuilder();
            string line;
            int oldIndent = 0, indent = 0;

            while ((line = sr.ReadLine()) != null)
            {
                line = line.TrimStart(whitespace);
                foreach (char c in line)
                {
                    if (c == '{') indent++;
                    else if (c == '}') indent--;
                }
                if (indent >= oldIndent)
                    for (int x = 0; x < oldIndent; x++)
                        line = tab + line;
                else
                    for (int x = 0; x < indent; x++)
                        line = tab + line;
                newText.AppendLine(line);
                oldIndent = indent;
            }
            return newText.ToString();
        }
    }
}