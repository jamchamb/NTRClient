using System;

namespace ntrclient.Prog.CS
{
    public class Memregion
    {
        public int Start, End, Length;

        public Memregion(string mem)
        {

            string[] memparts = mem.Split(' ');
            try
            {
                Start = Convert.ToInt32(memparts[0], 16);
                End = Convert.ToInt32(memparts[2], 16);
                Length = Convert.ToInt32(memparts[5], 16);
            }
            catch (Exception)
            {
                Start = 0;
                End = 0;
                Length = 0;
            }
        }

        public bool Contains(int v)
        {
            return ((Start <= v) && (v <= End));
        }
    }
}
