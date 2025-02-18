using System.Text;

namespace Nefta.Events
{
    public abstract class GameEvent
    {
        protected abstract int _eventType { get; }
        protected abstract int _category { get; }
        protected abstract int _subCategory { get; }
        
        public string _name;
        /// <summary>
        /// Value field, must be non-negative.
        /// </summary>
        public long _value;
        public string _customString;
        
        public void Record()
        {
            string name = null;
            if (_name != null)
            {
                name = JavaScriptStringEncode(_name);
            }
            string customPayload = null;
            if (_customString != null)
            {
                customPayload = JavaScriptStringEncode(_customString);
            }
            NeftaPluginWrapper.Instance.Record(_eventType, _category, _subCategory, name, _value, customPayload);
        }
		
        private static string JavaScriptStringEncode(string value)
        {
            int len = value.Length;
            bool needEncode = false;
            char c;
            for (int i = 0; i < len; i++)
            {
                c = value [i];

                if (c >= 0 && c <= 31 || c == 34 || c == 39 || c == 60 || c == 62 || c == 92)
                {
                    needEncode = true;
                    break;
                }
            }

            if (!needEncode)
            {
                return value;
            }
            
            var sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                c = value [i];
                if (c >= 0 && c <= 7 || c == 11 || c >= 14 && c <= 31 || c == 39 || c == 60 || c == 62)
                {
                    sb.AppendFormat ("\\u{0:x4}", (int)c);
                }
                else switch ((int)c)
                {
                    case 8:
                        sb.Append ("\\b");
                        break;

                    case 9:
                        sb.Append ("\\t");
                        break;

                    case 10:
                        sb.Append ("\\n");
                        break;

                    case 12:
                        sb.Append ("\\f");
                        break;

                    case 13:
                        sb.Append ("\\r");
                        break;

                    case 34:
                        sb.Append ("\\\"");
                        break;

                    case 92:
                        sb.Append ("\\\\");
                        break;

                    default:
                        sb.Append (c);
                        break;
                }
            }
            return sb.ToString ();
        }
    }
}