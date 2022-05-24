using System;
using System.Collections.Generic;
namespace HospitalManagmentSytem.Dto
{
    public class AuthDto
    {
        public string Message { get; set; }
        public string username { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
