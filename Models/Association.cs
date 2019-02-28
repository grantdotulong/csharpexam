using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace beltExamTwo.Models
{
    public class Association
    {
       [Key]
       public int AssociationId {get; set;}
       public int UserId {get; set;}
       public int HobbyId {get; set;}
       public Hobby Hobby {get; set;}
       public User User {get; set;}
       [Required]
       public string Proficiency {get; set;}
    }
}