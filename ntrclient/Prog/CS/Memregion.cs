using System;

namespace ntrclient.Prog.CS
{
    public class Memregion
    {
        public uint Start, End, Length;

        public Memregion(string mem)
        {
            string[] memparts = mem.Split(' ');
            try
            {
                Start = Convert.ToUInt32(memparts[0], 16);
                End = Convert.ToUInt32(memparts[2], 16);
                Length = Convert.ToUInt32(memparts[5], 16);
            }
            catch (Exception)
            {
                Start = 0;
                End = 0;
                Length = 0;
            }
        }

        public bool Contains(uint v)
        {
            return ((Start <= v) && (v <= End));
        }
    }
}
