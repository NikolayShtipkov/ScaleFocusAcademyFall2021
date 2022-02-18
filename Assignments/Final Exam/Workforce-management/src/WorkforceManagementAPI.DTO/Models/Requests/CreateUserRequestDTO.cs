using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class CreateUserRequestDTO : IValidatableObject
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Email { get; set; }

        [RequiredPassword]
        [MinLength(8)]
        [MaxLength(50)]
        public string Password { get; set; }

        [RequiredPassword]
        [MinLength(8)]
        [MaxLength(50)]
        public string RepeatPassword { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (Password != RepeatPassword)
            {
                result.Add(new ValidationResult("Passwords do not match", new string[] { "Password" }));
            }
            return result;
        }

        public class RequiredPasswordAttribute : RequiredAttribute
        {
            public override bool IsValid(object value)
            {
                return base.IsValid(value);
            }
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                return base.IsValid(value, validationContext);
            }
        }


    }
}
