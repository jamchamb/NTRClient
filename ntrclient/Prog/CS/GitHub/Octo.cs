using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using System;
using System.Windows.Forms;

namespace ntrclient.Prog.CS.GitHub
{
    internal class Octo
    {
        public static GitHubClient Git;
        public static IRepositoriesClient Rep;

        public static Release LastRelease;

        public static void Init()
        {
            Git = new GitHubClient(new ProductHeaderValue("ntrclient"));
            Rep = Git.Repository;
        }

        public static async Task<Release> GetLastUpdate()
        {
            try {
                IReadOnlyList<Release> lastReleases = await Rep.Release.GetAll("imthe666st", "ntrclient");
                LastRelease = lastReleases[0];
                return LastRelease;
            } catch ( Exception e )
            {
                MessageBox.Show("An error occured trying to look for updates!");
                return null;
            }
        }

        public static string GetLastVersionName()
        {
            return LastRelease != null ? LastRelease.Name : "ERROR";
        }

        public static string GetLastVersionBody()
        {
            return LastRelease != null ? LastRelease.Body : "ERROR";
        }
    }
}
