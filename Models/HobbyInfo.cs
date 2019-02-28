using System.Collections.Generic;

namespace beltExamTwo.Models
{
    public class HobbyInfo
    {
        public User currentUser {get; set;}
        public Hobby currentHobby { get; set; }
        public Association association { get; set; }
    }
}