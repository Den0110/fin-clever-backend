using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FinClever.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinCleverDbContext context;

        public UserRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<User> Create(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> Get(string id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var user = await context.Users.FindAsync(id);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
