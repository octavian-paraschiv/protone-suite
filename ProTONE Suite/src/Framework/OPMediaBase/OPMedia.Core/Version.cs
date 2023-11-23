namespace OPMedia.Core
{
    public class SuiteVersion
    {
        // Keep these constants as defined, they will be replaced with their actual values
        // from within prep_build.ps1, which gets called by the AppVeyor build system.
        public const string Version = "4.0.21";
        public const bool IsRelease = false;
    }
}
