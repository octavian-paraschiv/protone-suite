namespace OPMedia.Core
{
    public partial class Constants
    {
        public const string SuiteName = "ProTONE Suite";
        public const string CompanyName = "OPMedia Research";
        public const string CopyrightNotice = "Copyright © " + CompanyName;
        public const string SuiteAppPrefix = "OPMedia.";


        public const int E_FAIL = -2147467259;

        public const int MAX_PATH = 260;
        public const int MAX_FILES = 65536;
        public const int MAX_FILE_BUFFER = 4 * MAX_PATH * MAX_FILES;

        public const string PlayMenu = "Play with ProTONE";
        public const string EnqueueMenu = "Add to ProTONE Playlist";

        public const string SubtitleFileTypeDesc = "Video Subtitle File";
        public const string CatalogFileTypeDesc = "OPMedia Catalog File (.CTX)";
        public const string BookmarkFileTypeDesc = "OPMedia Bookmark File (.BMK)";

        public const string LibraryName = SuiteAppPrefix + "MediaLibrary";
        public const string PlayerName = SuiteAppPrefix + "ProTONE";
        public const string PersistenceServiceShortName = SuiteAppPrefix + "PersistenceService";
        public const string GalaxyServiceShortName = SuiteAppPrefix + "GalaxyService";

        public const string LibraryBinary = LibraryName + ".exe";
        public const string PlayerBinary = PlayerName + ".exe";

        public const string PersistenceServiceLongName = "OPMedia Persistence Service";
        public const string PersistenceServiceDescription = "Provides caching and long-time persistence support for OPMedia applications settings.";
        public const string PersistenceServiceBinary = PersistenceServiceShortName + ".exe";

        public const string GalaxyServiceLongName = "OPMedia Galaxy Backend Service";
        public const string GalaxyServiceDescription = "Provides backend services for ProTONE Galaxy database.";
        public const string GalaxyServiceBinary = GalaxyServiceShortName + ".exe";
    }
}
