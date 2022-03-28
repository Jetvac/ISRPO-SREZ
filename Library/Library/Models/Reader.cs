using System.Text.Json.Serialization;

namespace Library.Models
{
    public class Reader
    {
        [JsonPropertyName ("lastname")]
        public string LastName { get; set; }   = null;
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }  = null;
        [JsonPropertyName("middlename")]
        public string MiddleName { get; set; } = null;
        [JsonPropertyName("photo")]
        public byte[] Photo { get; set; } = null;

        public string FullName { get
            {
                return $"{FirstName} {LastName} {MiddleName}";
            } }
        public string FullNameInitial
        {
            get
            {
                string FirstNameInitial = "", MiddleNameInitial = "";
                if (FirstName != "")
                {
                    FirstNameInitial = FirstName.Substring(0, 1);
                }
                if (MiddleName != "")
                {
                    MiddleNameInitial = MiddleName.Substring(0, 1);
                }
                return $"{LastName} {FirstNameInitial}. {MiddleNameInitial}.";
            }
    }
    }
}