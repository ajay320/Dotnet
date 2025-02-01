using System.ComponentModel.DataAnnotations;

namespace Task10Field.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateOnly JoiningDate { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string  Designation { get; set; }
        public string Address { get; set; }
        public int Salary { get; set; }
        public int EmployeeCode { get; set; }
        public string AdharCard { get; set; }

    }
}
