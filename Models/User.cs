using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace beltExamTwo.Models
{
    public class User
    {
        //Mapped
       [Key]
       public int UserId {get; set;}
       [Required]
       [Display(Name="Firstname: ")]
       public string FirstName {get; set;}
       [Required]
       [Display(Name="Surname: ")]
       public string LastName {get; set;}
       [Required]
       [EmailAddress]
       [Display(Name="Email: ")]
       public string Email {get; set;}
       [Required]
       [DataType(DataType.Password)]
       [MinLength(8, ErrorMessage = "Password must be 8 characters or longer.")]
       [Display(Name="Enter Password: ")]
       public string Password {get; set;}
       public decimal Wallet {get; set;}
       public List<Association> Hobbies {get; set;}

       public DateTime CreatedAt {get; set;} = DateTime.Now;
       public DateTime UpdatedAt {get; set;} = DateTime.Now;

       // Not Mapped
       [NotMapped]
       [Required (ErrorMessage = "Please confirm the password")]
       [Compare("Password", ErrorMessage = "Password entered does not match!")]
       [DataType(DataType.Password)]
       [Display(Name="Confirm Password: ")]
       public string Confirm {get; set;}
    }
}