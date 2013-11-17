using InjectCC.Model.Domain;
using InjectCC.Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InjectCC.Model.Repositories
{
    public class UserRepository : CrudRepository<User>
    {
        public UserRepository(UnitOfWork context)
            : base(context)
        {
        }

        public User GetById(int id)
        {
            return Context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public IList<Medication> GetMedicationsFor(User user)
        {
            return Context.Medications.Where(m => m.User == user).ToList();
        }

        public User GetByEmail(string email)
        {
            return Context.Users.SingleOrDefault(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
