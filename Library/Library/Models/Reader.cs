namespace LibraryWEBAPI.Models
{
    public class Reader
    {
        public string LastName { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string MiddleName { get; set; } = null;
        public string FullName 
        { 
            get 
            {
                string MiddleNameInitial = MiddleName.Length == 0 ? "" : MiddleName.Substring(0, 1);
                string FirstNameInitial = FirstName.Length == 0 ? "" : FirstName.Substring(0, 1);
                return $"{LastName} {FirstNameInitial}. {MiddleNameInitial}."; 
            } 
        }
        public byte[] Photo { get; set; } = null;
    }
}