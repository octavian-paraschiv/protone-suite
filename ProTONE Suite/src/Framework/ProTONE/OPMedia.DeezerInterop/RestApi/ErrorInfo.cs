using Newtonsoft.Json;

namespace OPMedia.DeezerInterop.RestApi
{
    [JsonObject("error")]
    public class ErrorInfo
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }

        public bool IsEmpty
        {
            get
            {
                bool ret = true;

                ret &= string.IsNullOrEmpty(Type);
                ret &= string.IsNullOrEmpty(Message);
                ret &= string.IsNullOrEmpty(Code);

                return ret;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} Message={1} Code={2}", Type, Message, Code);
        }

        public static bool IsNullOrEmpty(ErrorInfo err)
        {
            return (err == null || err.IsEmpty);
        }
    }
}
