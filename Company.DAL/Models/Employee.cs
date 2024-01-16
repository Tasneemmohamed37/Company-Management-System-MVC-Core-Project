using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }


        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }


        [Range(18, 60)]
        public int? Age { get; set; }


        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string ImageName { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; } // Foreign Key => Allow NULL

        public Department Department { get; set; } // nav pro one


    }
}
