namespace AssemblyName.MediaLib.Utility
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

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

        public void Check(string path)
        {
            Task.Factory.StartNew(() => { this.CheckNewVersion(path); });
        }

        /// <summary>
        ///     Checks the new version.
        /// </summary>
        /// <param name="path">The path.</param>
        private void CheckNewVersion(string path)
        {
            try
            {
                var gitVersion = GetNewVersion();

                var version = this.LocalVersion;

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

                if (differential < 0)
                {
                    return;
                }

                this.UpdateAvailable = true;

                if (differential >= 10)
                {
                    this.ForceUpdate = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        #region Methods

        // TODO
        /// <summary>
        ///     Gets the new version.
        /// </summary>
        /// <returns></returns>
        private static Version GetNewVersion()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var data =
                        client.DownloadString(
                            $@"https://raw.githubusercontent.com/{GlobalVariables.GitHubPath}/Properties/AssemblyInfo.cs");

                    var match =
                        new Regex(
                            @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]")
                            .Match(data);

                    if (!match.Success) return new Version(0, 0, 0, 0);

                    var version =
                        new Version(
                            $"{match.Groups[1]}, {match.Groups[2]}, {match.Groups[3]}, {match.Groups[4]}");

                    return version;
                }
            }
            catch (Exception)
            {
                Console.WriteLine(@"{0}: Failed to get new Version!", GlobalVariables.Name);
            }
            return null;
        }

        #endregion
    }
}