using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ntrclient
{
    class Heartbeat
    {
        public volatile bool shouldStop = false;
        public volatile bool enabled = true;

        public gateshark gs_code;

        public void beat()
        {
            while (!shouldStop)
            {
                // WORKING
                if (gs_code != null && enabled)
                {
                    gs_code.execute();
                    Console.WriteLine("Executing code");
                }

                Thread.Sleep(4000);
            }
            // Stopped working
            // Cleaning

            //Console.WriteLine("Cleaning Thread");

            // Finished
        }

        public void requestStop()
        {
            shouldStop = true;
            //Console.WriteLine("Requesting stop");
        }

        public void setCode(String s)
        {
            gs_code = new gateshark(s);

            //Console.WriteLine("Set code by String");
        }

        public void setCode(gateshark s)
        {
            gs_code = s;
            //Console.WriteLine("Set code by instance");
        }

        public bool toggle()
        {
            enabled = !enabled;
            return enabled;
        }
    }

    class Heartbeat_controller
    {
        Heartbeat hb;
        Thread heartbeatThread;

        public void start()
        {
            Console.WriteLine("Creating Objects");
            hb = new Heartbeat();
            heartbeatThread = new Thread(hb.beat);
            Console.WriteLine("Created Objects");
            heartbeatThread.Start();
            Console.WriteLine("Started Thread");

        }

        public void stop()
        {
            if (heartbeatThread != null)
                if (heartbeatThread.IsAlive)
                    hb.requestStop();
        }

        public void setCode(String s)
        {
            if (heartbeatThread != null)
                if (heartbeatThread.IsAlive)
                    hb.setCode(s);
        }

        public void setCode(gateshark s)
        {
            if (heartbeatThread != null)
                if (heartbeatThread.IsAlive)
                    hb.setCode(s);
        }

        public void toggle()
        {
            if (heartbeatThread != null)
                if (heartbeatThread.IsAlive)
                    hb.toggle();
        }

        public bool status()
        {
            if (heartbeatThread != null)
                if (heartbeatThread.IsAlive)
                    return hb.enabled;

            return false;
        }
    }
}
