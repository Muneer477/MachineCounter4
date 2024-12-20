﻿namespace SMTS.DTOs
{
    public class ContactDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }        
    }
}
