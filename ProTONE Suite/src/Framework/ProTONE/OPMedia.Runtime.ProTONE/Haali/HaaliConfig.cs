namespace OPMedia.Runtime.ProTONE.Haali
{
    public static class HaaliConfig
    {
        public const string CLSID = "{55DA30FC-F16B-49FC-BAA5-AE59FC65F82D}";
        public static string InstallLocation { get; } = RegistrationSupport.GetInstalledLocationForCLSID(CLSID);
    }
}
