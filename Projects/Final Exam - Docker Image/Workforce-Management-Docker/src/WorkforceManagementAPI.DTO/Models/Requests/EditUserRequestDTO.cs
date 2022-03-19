using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class EditUserRequest : IValidatableObject
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string NewEmail { get; set; }

        [RequiredPassword]
        [MinLength(8)]
        [MaxLength(50)]
        public string NewPassword { get; set; }

        [RequiredPassword]
        [MinLength(8)]
        [MaxLength(50)]
        public string RepeatPassword { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string NewFirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string NewLastName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (NewPassword != RepeatPassword)
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