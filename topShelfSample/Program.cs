using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Topshelf;
//using Topshelf.Dashboard;
using log4net;
using log4net.Config;
using System.IO;

namespace topShelfSample
{

    public class TownCrier
    {
        readonly Timer _timer;
        readonly ILog _log = LogManager.GetLogger(typeof(TownCrier));
        public TownCrier()
        {
            _timer = new Timer(1000) {AutoReset = true};
            _timer.Elapsed += (sender, eventArgs) =>
                                  {
                                      //Console.WriteLine("It is {0} an all is well", DateTime.Now);
                                      _log.Debug("Tick:" + DateTime.Now.ToLongTimeString());
                                  };

        }

        public void Start() { _log.Info("SampleService is Started"); _timer.Start(); }
        public void Stop() { _log.Info("SampleService is stopped"); _timer.Stop(); }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 //1
                                {
                //x.EnableDashboard();
                x.Service<TownCrier>(s =>                        //2
                {
                    s.ConstructUsing(name => new TownCrier());     //3
                    s.WhenStarted(tc => {
                        XmlConfigurator.ConfigureAndWatch(
                            new FileInfo(".\\log4net.config"));
                        tc.Start(); 

                    }
                    );              
                    s.WhenStopped(tc => tc.Stop());               //5
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Sample Topshelf Host");        //7
                x.SetDisplayName("Stuff");                       //8
                x.SetServiceName("stuff");                       //9
            });     
        }
    }
}
