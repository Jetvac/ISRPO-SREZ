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
                int date = DateEnd.Day - DateStart.Day;
                return date >= 0? date : 0;
            }
        }

        public string Path
        {
            get
            {
                if (DateForReturn > 7)
                {
                    return "../../src/images/unnamed.png";
                }
                else
                {
                    return "../../src/images/img_28615 (2).png";
                }
            }
        }
    }
}