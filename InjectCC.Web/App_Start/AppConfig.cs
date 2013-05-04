using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace InjectCC.Web
{
    public class AppConfig
    {
        public ApplicationEnvironment Environment { get; private set; }

        public static AppConfig _config = null;
        public static void RegisterConfig()
        {
            _config = new AppConfig {
                Environment = (ApplicationEnvironment)Enum.Parse(typeof(ApplicationEnvironment), ConfigurationManager.AppSettings["Environment"])
            };
        }
    }

    public enum ApplicationEnvironment { Development, Testing, Production }
}
