using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace BankingManagementSystem.Membership.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public int Balance { get; set; }
    }
}
