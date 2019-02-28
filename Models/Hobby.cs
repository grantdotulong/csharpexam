using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace beltExamTwo.Models
{
    public class Hobby
    {
       [Key]
       public int HobbyId {get; set;}
       public User CreatedBy {get; set;}
       public int UserId {get;set;}
       [Required]
       [MinLength(3, ErrorMessage = "Hobby name must be 3 characters or longer.")]
       [Display(Name="Name: ")]
       public string HobbyName {get; set;}
       [Required]
       [MinLength(10, ErrorMessage = "Description must be 10 characters or longer.")]
       [Display(Name="Description: ")]
       public string Description {get; set;}
       public List<Association> Users {get; set;}
       public DateTime CreatedAt {get; set;} = DateTime.Now;
       public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}