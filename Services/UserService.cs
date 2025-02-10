using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HomeownersMS.Services
{
    public class UserService
    {
        private readonly HomeownersContext _context;

        public UserService(HomeownersContext context)
        {
            _context = context;
        }

        public async Task ChangeUserPrivilege(int userId, Privileges newPrivilege)
        {
            var user = await _context.User
                .Include(u => u.Resident)
                .Include(u => u.Staff)
                .Include(u => u.Admin)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            user.ResidentId = null;
            user.StaffId = null;
            user.AdminId = null;

            switch (newPrivilege)
            {
                case Privileges.resident:
                    user.ResidentId = user.UserId;
                    break;
                case Privileges.staff:
                    user.StaffId = user.UserId;
                    break;
                case Privileges.admin:
                    user.AdminId = user.UserId;
                    break;
            }

            await _context.SaveChangesAsync();
        }
    }
}
