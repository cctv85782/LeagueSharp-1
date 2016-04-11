namespace Yasuo.Common.Utility
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Class that offers different kinds of version checking
    /// </summary>
    public class VersionChecker
    {
        #region Fields

        /// <summary>
        ///     If there is a big version jump that indicates an important update
        /// </summary>
        public bool ForceUpdate = false;

        /// <summary>
        ///     The assembly version
        /// </summary>
        public Version LocalVersion = Assembly.GetCallingAssembly().GetName().Version;

        /// <summary>
        ///     If an update is available
        /// </summary>
        public bool UpdateAvailable = false;

        /// <summary>
        ///     Where to search for an update
        /// </summary>
        private readonly string gitHubPath;

        #endregion

        #region Constructors and Destructors

        public VersionChecker(string gitHubPath)
        {
            this.gitHubPath = gitHubPath;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Checks if an updated version is available
        /// </summary>
        /// <param name="path"></param>
        public void Check(string path)
        {
            try
            {
                var gitVersion = this.GetNewVersion();
                var version = LocalVersion;

                var differential = 0;

                differential += gitVersion.Revision - version.Revision;
                differential += gitVersion.Build - version.Build;
                differential += gitVersion.Minor - version.Minor;
                differential += gitVersion.Major - version.Major;

                if (differential == 0)
                {
                    this.UpdateAvailable = false;
                    this.ForceUpdate = false;
                }
                if (differential > 0)
                {
                    UpdateAvailable = true;

                    if (differential > 9)
                    {
                        ForceUpdate = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        #region Methods

        private Version GetNewVersion()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var data =
                        client.DownloadString(
                            string.Format(
                                "https://raw.githubusercontent.com/{0}/Properties/AssemblyInfo.cs",
                                this.gitHubPath));

                    var gitVersion =
                        Version.Parse(
                            new Regex("AssemblyFileVersion\\((\"(.+?)\")\\)").Match(data).Groups[1].Value.Replace(
                                "\"",
                                ""));

                    return gitVersion;
                }
            }
            catch (Exception)
            {
                Console.WriteLine(@"MediaSuo: Failed to get new Version!");
            }
            return new Version();
        }

        #endregion
    }
}