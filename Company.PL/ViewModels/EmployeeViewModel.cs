﻿using Company.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Company.PL.ViewModels
{
    public class EmployeeViewModel
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Name is Required!")]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars")]
        [MinLength(5, ErrorMessage = "Min Length of Name is 5 Chars")]
        public string Name { get; set; }


        [Range(22, 30)]
        public int? Age { get; set; }



        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
            , ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }


        public decimal Salary { get; set; }
        public bool IsActive { get; set; }

        [EmailAddress]
        //[DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Phone]
        //[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }

        public IFormFile Image { get; set; }

        public string? ImageName { get; set; }

        //[ForeignKey("Department")]
        public int? DepartmentId { get; set; } // Foreign Key => Allow NULL

        // Navigational Property [ONE]
        public Department? Department { get; set; }
    }
}
