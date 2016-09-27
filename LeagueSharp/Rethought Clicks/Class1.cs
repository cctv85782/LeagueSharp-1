namespace SupportExtraGozIsBae
{
    #region Using Directives

    using System;
    using System.Runtime.InteropServices;

    #endregion

    internal class Program
    {
        #region Constants

        public const uint Keydown = 0x100;

        // not sure about that one can't remember what it was and cba googling
        public const uint Keyup = 0x0101;

        #endregion

        #region Fields

        /// <summary>
        ///     The targeted process
        /// </summary>
        private readonly IntPtr targetedProcess = FindWindow(null, "WindowName");

        #endregion

        #region Public Methods and Operators

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // usage: var sendMessage = SendMessage(FindWindow(null, notepad), WM_KEYDOWN, ((IntPtr)a, ((IntPts)0);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        // usage: var window = FindWindow(null, "notepad");

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     Sends the keystroke. Behavior could be Keydown or Keyup for example.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="behavior">The behavior.</param>
        public void SendKeystroke(ushort key, uint behavior)
        {
            SendMessage(this.targetedProcess, behavior, (IntPtr)key, (IntPtr)0);
        }

        #endregion
    }
}