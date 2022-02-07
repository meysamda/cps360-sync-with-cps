using System.ComponentModel.DataAnnotations;
using CPS360.Sync.CSD.Presentation.Attributes;

namespace CPS360.Sync.CSD.Presentation.Commands
{
    public class CreateContactPersonCommand
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required, PhoneNumberValidation]
        public string PhoneNumber { get; set; }
        
        [Required, EmailValidation]
        public string Email { get; set; }
    }
}