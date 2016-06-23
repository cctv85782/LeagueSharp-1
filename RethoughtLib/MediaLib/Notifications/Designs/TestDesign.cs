namespace RethoughtLib.Notifications.Designs
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Text;

    using LeagueSharp;
    using RethoughtLib.Transitions;

    using System.Windows.Forms;

    using SharpDX.Direct3D9;

    #endregion

    internal class TestDesign : NotificationDesign
    {
        #region Constants

        /// <summary>
        ///     The maximum body line length.
        /// </summary>
        private int MaximumBodyLineLength = 283;

        /// <summary>
        ///     The maximum header line length.
        /// </summary>
        private int MaximumHeaderLineLength => (int)this.Width - (this.PaddingLeft + this.PaddingRight);

        public int PaddingRight = 15;

        public int PaddingLeft = 15;

        public int PaddingTop = 20;

        public int PaddingBot = 30;

        #endregion

        #region Static Fields

        /// <summary>
        ///     The body font.
        /// </summary>
        private static readonly Font BodyFont = new Font(
            Drawing.Direct3DDevice,
            13,
            0,
            FontWeight.DoNotCare,
            5,
            false,
            FontCharacterSet.Default,
            FontPrecision.Character,
            FontQuality.Antialiased,
            FontPitchAndFamily.Mono | FontPitchAndFamily.Decorative,
            "Tahoma");

        /// <summary>
        ///     The header font.
        /// </summary>
        private static readonly Font HeaderFont = new Font(
            Drawing.Direct3DDevice,
            16,
            0,
            FontWeight.Bold,
            5,
            false,
            FontCharacterSet.Default,
            FontPrecision.Character,
            FontQuality.Antialiased,
            FontPitchAndFamily.Mono | FontPitchAndFamily.Decorative,
            "Tahoma");

        /// <summary>
        ///     The sprite.
        /// </summary>
        private static readonly Sprite Sprite = new Sprite(Drawing.Direct3DDevice);

        #endregion

        #region Fields

        private string content;

        private string title;

        #endregion

        #region Constructors and Destructors

        public TestDesign(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        /// <value>
        ///     The content.
        /// </value>
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                if (BodyFont.MeasureText(Sprite, value, 0).Width > MaximumBodyLineLength)
                {
                    string final = null;
                    for (var i = value.Length; i > 0; --i)
                    {
                        if (BodyFont.MeasureText(Sprite, value.Substring(0, i) + "...", 0).Width
                            <= MaximumBodyLineLength)
                        {
                            final = value.Substring(0, i) + "...";
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(final))
                    {
                        this.content = final;
                        return;
                    }
                }

                this.content = string.IsNullOrEmpty(value) ? " " : value;
            }
        }

        /// <summary>
        ///     Gets or sets the height.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public override int Height { get; set; } = 500;

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (HeaderFont.MeasureText(Sprite, value, 0).Width > MaximumHeaderLineLength)
                {
                    string final = null;
                    for (var i = value.Length; i > 0; --i)
                    {
                        if (HeaderFont.MeasureText(Sprite, value.Substring(0, i) + "...", 0).Width
                            <= MaximumHeaderLineLength)
                        {
                            final = value.Substring(0, i) + "...";
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(final))
                    {
                        this.title = final;
                        return;
                    }   
                }

                this.title = string.IsNullOrEmpty(value) ? " " : value;
            }
        }

        // TODO
        private List<string> GetTextAsStringList(string text, int width, int height)
        {
            return new List<string>();
        }

        /// <summary>
        ///     Gets or sets the transition.
        /// </summary>
        /// <value>
        ///     The transition.
        /// </value>
        public override Transition Transition { get; set; } = new ElasticEaseInOut(500);

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public override int Width { get; set; } = 800;

        #endregion
    }
}