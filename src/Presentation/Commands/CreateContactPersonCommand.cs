using System.ComponentModel.DataAnnotations;
using Cps360.SyncWithCps.Presentation.Attributes;

namespace Cps360.SyncWithCps.Presentation.Commands
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