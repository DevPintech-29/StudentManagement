using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Sex { get; set; }
        public int? DistrictId { get; set; }
        public int? ThanaId { get; set; }
        public int? VillageId { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public string? InterestNames { get; set; }

        [NotMapped]
        public string? InterestIDs { get; set; }

    }
}
