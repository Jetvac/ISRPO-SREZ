namespace Library.Models
{
    public class Reader
    {
        public string LastName { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string MiddleName { get; set; } = null;
        public string FullName { get { return $"{FirstName} {LastName} {MiddleName}"; } }
        public string FullNameInitial { get { 
                string FirstNameInitial = FirstName == ""? string.Empty : FirstName[0].ToString();
                string MiddleNameInitial = MiddleName == ""? string.Empty : MiddleName[0].ToString();
                return $"{LastName} {FirstNameInitial}. {MiddleNameInitial}."; 
            } }
        public byte[] Photo { get; set; } = null;
    }
}