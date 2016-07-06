using System;
using System.Threading;
using ntrclient.Extra;

namespace ntrclient.Prog.CS
{
    public class Heartbeat
    {
        public volatile bool ShouldStop;
        public volatile bool Enabled = true;

        public Gateshark GsCode;

        public void Beat()
        {
            while (!ShouldStop)
            {
                // WORKING
                if (GsCode != null && Enabled)
                {
                    GsCode.Execute();
                    Console.WriteLine(@"Executing code");
                }

                Thread.Sleep(4000);
            }
        }

        public void RequestStop()
        {
            ShouldStop = true;
        }

        public void SetCode(string s)
        {
            GsCode = new Gateshark(s);
        }

        public void SetCode(Gateshark s)
        {
            GsCode = s;
        }

        public bool Toggle()
        {
            Enabled = !Enabled;
            return Enabled;
        }
    }

    public class HeartbeatController
    {
        private Heartbeat _hb;
        private Thread _heartbeatThread;

        public void Start()
        {
            Console.WriteLine(@"Creating Objects");
            _hb = new Heartbeat();
            _heartbeatThread = new Thread(_hb.Beat);
            Console.WriteLine(@"Created Objects");
            _heartbeatThread.Start();
            Console.WriteLine(@"Started Thread");

        }

        public void Stop()
        {
            if (_heartbeatThread?.IsAlive ?? false)
                _hb.RequestStop();
        }

        public void SetCode(string s)
        {
            if (_heartbeatThread?.IsAlive ?? false)
                _hb.SetCode(s);
        }

        public void SetCode(Gateshark s)
        {
            if (_heartbeatThread?.IsAlive ?? false)
                _hb.SetCode(s);
        }

        public void Toggle()
        {
            if (_heartbeatThread?.IsAlive ?? false)
                _hb.Toggle();
        }

        public bool Status()
        {
            if (_heartbeatThread?.IsAlive ?? false)
                return _hb.Enabled;

            return false;
        }
    }
}
