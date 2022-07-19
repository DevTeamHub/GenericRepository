using DevTeam.EntityFrameworkExtensions;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext.Entities
{
    public class Person : IDeleted
    {
        public int Id { get; set; }
        public int AppartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }

        public Apartment Appartment { get; set; }
    }
}
