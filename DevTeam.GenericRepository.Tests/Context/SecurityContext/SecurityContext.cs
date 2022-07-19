using DevTeam.EntityFrameworkExtensions;
using DevTeam.GenericRepository.Tests.Context.SecurityContext.Entities;
using DevTeam.GenericRepository.Tests.Tests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DevTeam.GenericRepository.Tests.Context.SecurityContext
{
    public interface ISecurityContext : IDbContext { }

    public class SecurityContext : DbContext, ISecurityContext
    {
        public IEnumerable<User> Users => TestData.Users;
    }
}
