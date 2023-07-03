namespace Jwt.POC.Helpers
{
    public class AppSettings
    {
        public string OriginLiquidationMNR { get; set; }
        public string Secret{ get; set; }
        public string Issuer{ get; set; }
        public string Audience { get; set; }
        public string MinutesToExpire { get; set; }
    }
}
