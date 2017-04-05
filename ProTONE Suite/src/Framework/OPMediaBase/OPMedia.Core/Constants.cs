using System;
using System.Collections.Generic;
using System.Text;

namespace OPMedia.Core
{
    public partial class Constants
    {
        public const string SuiteName = "ProTONE Suite";

        public const string CompanyName =           "OPMedia Research";
        public const string CopyrightNotice =       "Copyright � " + CompanyName;

        public const string SuiteAppPrefix = "OPMedia.";

        public const string UtilityName = "OPMedia.Utility";
        public const string UtilityBinary = Constants.UtilityName + ".exe";

        public const string PersistenceServiceShortName = "OPMedia.PersistenceService";
        public const string PersistenceServiceLongName = "OPMedia Persistence Service";
        public const string PersistenceServiceDescription = "Provides caching and long-time persistence support for OPMedia applications settings.";
        public const string PersistenceServiceBinary = Constants.PersistenceServiceShortName + ".exe";
    }
}
