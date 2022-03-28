using System;

namespace Library.Models
{
    public class RecordBook
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Book Book { get; set; }

        public int DateForReturn
        {
            get
            {
                return (DateEnd - DateStart).Days;
            }
        }
        public string Path
        {
            get
            {
                if (DateForReturn > 7)
                {
                    return "../../src/images/NOOK.png";
                }
                else
                {
                    return "../../src/images/OK.png";
                }
            }
        }
    }
}