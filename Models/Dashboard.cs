using System.Collections.Generic;

namespace beltExamTwo.Models
{
    public class Dashboard
    {
        public User ActiveUser { get; set; }
        public List<Hobby> AllHobbies { get; set; }
        public List<Hobby> Novice {get; set;}
        public List<Hobby> Intermediate {get; set;}

        public List<Hobby> Expert {get; set;}
    }
}