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
        public static Int64 lastReleaseTime = 635946110170000000 + 1;

        public static void init()
        {
            git = new GitHubClient(new ProductHeaderValue("ntrclient"));
            rep = git.Repository;
        }

        public async static Task<bool> isUpdate()
        {
            var lastReleases = await rep.Release.GetAll("imthe666st", "ntrclient");
            lastRelease = lastReleases[0];

            return lastRelease.CreatedAt.UtcTicks > lastReleaseTime;
        }

        public async static Task<Int64> getLastUpdate()
        {

            var lastReleases = await rep.Release.GetAll("imthe666st", "ntrclient");
            lastRelease = lastReleases[0];
            return lastRelease.CreatedAt.UtcTicks;
        }
    }
}
