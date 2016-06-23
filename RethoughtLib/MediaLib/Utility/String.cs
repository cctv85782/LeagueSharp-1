namespace RethoughtLib.Utility
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    using RethoughtLib.Design;

    using SharpDX;
    using SharpDX.Direct3D9;

    #endregion

    /// <summary>
    ///     Class that offers string utilities
    /// </summary>
    internal class String
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Capitalizes the first char of the specified string.
        ///     IE: word > Word, i'am a sentece > I'am a sentence
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string Capitalize(string str)
        {
            switch (str.Length)
            {
                case 0:
                    return "";
                case 1:
                    return str.ToUpper();
                default:
                    return str.Substring(0, 1).ToUpper() + str.Substring(1);
            }
        }

        /// <summary>
        ///     Shortens the specified string.
        ///     IE: I'm a long sentence > I'm a long sen...
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Shorten(string str, int length)
        {
            if (str.Length < length + 2)
            {
                return str;
            }

            if (str.Length <= 3)
            {
                return "...";
            }

            var removedResult = str.Remove(length - 3, str.Length - length);

            return removedResult + "...";
        }

        /// <summary>
        ///     Formats the string into all first letters uppercase.
        ///     Won't work if title is all uppercase.
        /// </summary>
        /// <param name="str">the string</param>
        /// <returns></returns>
        public static string ToTitleCase(string str)
        {
            var textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(str);
        }

        public static List<string> WrapIntoRectangle(
            string str,
            Sprite sprite,
            Rectangle rectangle,
            Offset<int> offset,
            Font font = default(Font))
        {
            //var sb = new StringBuilder();

            //var availableWidth = rectangle.Width - (offset.Left + offset.Right);
            //var availableHeight = rectangle.Height - (offset.Top + offset.Bottom);

            //if (font == null) return new List<string>() { str };

            //var expectedRawRec = font.MeasureText(sprite, str, rectangle, FontDrawFlags.WordBreak);
            ////font.DrawText(sprite, str, rectangle, FontDrawFlags.WordBreak, ColorBGRA.FromRgba(5));

            //if (expectedRawRec.Height > rectangle.Height) return new List<string>() {str};

            //int next;

            //// Parse each line of text
            //for (var pos = 0; pos < str.Length; pos = next)
            //{
            //    // Find end of line
            //    var eol = str.IndexOf(Environment.NewLine, pos, StringComparison.Ordinal);
            //    if (eol == -1) next = eol = text.Length;
            //    else next = eol + Environment.NewLine.Length;

            //    // Copy this line of text, breaking into smaller lines as needed
            //    if (eol > pos)
            //    {
            //        do
            //        {
            //            var len = eol - pos;
            //            if (len > width) len = BreakLine(str, pos, width);
            //            sb.Append(str, pos, len);
            //            sb.Append(Environment.NewLine);

            //            // Trim whitespace following break
            //            pos += len;
            //            while (pos < eol && char.IsWhiteSpace(str[pos])) pos++;
            //        }
            //        while (eol > pos);
            //    }
            //    else sb.Append(Environment.NewLine); // Empty line
            //}
            //return sb.ToString();

            return null;
        }

        #endregion
    }
}