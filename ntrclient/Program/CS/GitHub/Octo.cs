using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ntrclient
{
    internal class Octo
    {
        public static GitHubClient git;
        public static IRepositoriesClient rep;

        public static Release lastRelease;    

        public static void init()
        {
            git = new GitHubClient(new ProductHeaderValue("ntrclient"));
            rep = git.Repository;
        }

        public async static Task<Release> getLastUpdate()
        {

            var lastReleases = await rep.Release.GetAll("imthe666st", "ntrclient");
            lastRelease = lastReleases[0];
            return lastRelease;
        }

        public static String getLastVersionName()
        {
            if (lastRelease != null)
                return lastRelease.Name;
            else
                return "ERROR";
        }

        public static String getLastVersionBody()
        {
            if (lastRelease != null)
                return lastRelease.Body;
            else
                return "ERROR";
        }
    }
}
