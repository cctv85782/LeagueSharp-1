﻿namespace Yasuo.Common.Utility
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
        ///     force update
        /// </summary>
        public bool ForceUpdate;

        /// <summary>
        ///     The local version
        /// </summary>
        public Version LocalVersion = Assembly.GetCallingAssembly().GetName().Version;

        /// <summary>
        ///     update available
        /// </summary>
        public bool UpdateAvailable ;

        /// <summary>
        ///     The git hub path
        /// </summary>
        private readonly string gitHubPath;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="VersionChecker" /> class.
        /// </summary>
        /// <param name="gitHubPath">The git hub path.</param>
        public VersionChecker(string gitHubPath)
        {
            this.gitHubPath = gitHubPath;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Checks the new version.
        /// </summary>
        /// <param name="path">The path.</param>
        public void CheckNewVersion(string path)
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

        /// <summary>
        ///     Gets the new version.
        /// </summary>
        /// <returns></returns>
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