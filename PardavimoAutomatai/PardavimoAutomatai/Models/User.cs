using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PardavimoAutomatai.Models
{
    public class User
    {

        public int Id { get; set; }
        [Required(ErrorMessage ="First name is required")]
        [Display(Name="First Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[A-Za-z]+@[A-Za-z]+.[A-Za-z]+", ErrorMessage ="Incorrect email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Role is required")]
        [Display(Name="Role")]
        public int Role { get; set; }
        public int? fk_Shipment { get; set; }

    }
}