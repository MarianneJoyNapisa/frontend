using HomeownersMS.Models;

namespace HomeownersMS.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HomeownersContext context) 
        {
            if(context.Users.Any())
            {
                return;
            }

            var admin = new Admin
            {
                LName = "Smith",
                FName = "John",
                Email = "admin@example.com",
                ContactNo = "123-456-7890",
                Job = "Administrator",
                HireDate = DateTime.Now
            };

            var staff = new Staff
            {
                LName = "Doe",
                FName = "Jane",
                Email = "staff@example.com",
                ContactNo = "098-765-4321",
                Job = "Support Staff",
                HireDate = DateTime.Now
            };

            var resident = new Resident
            {
                LName = "Brown",
                FName = "Charlie",
                Email = "resident@example.com",
                ContactNo = "111-222-3333",
                Address = "ABC Street, Block D, Lot E, House No. 1",
                MoveInDate= DateTime.Now
            };

            context.Admins.AddRange(admin);
            context.Staffs.AddRange(staff);
            context.Residents.AddRange(resident);

            context.SaveChanges();

            var users = new User[]
            {
                new User
                {
                    Username = "adminUser",
                    Privilege = Privileges.admin,
                    AdminId = admin.AdminId // Linking to the Admin record
                },
                new User
                {
                    Username = "staffUser",
                    Privilege = Privileges.staff,
                    StaffId = staff.StaffId // Linking to the Staff record
                },
                new User
                {
                    Username = "residentUser",
                    Privilege = Privileges.resident,
                    ResidentId = resident.ResidentId // Linking to the Resident record
                }
            };

            foreach (var user in users)
            {
                if (user.Username == "adminUser")
                {
                    user.SetPassword("adminPassword123"); // Use a secure password
                }
                else if (user.Username == "staffUser")
                {
                    user.SetPassword("staffPassword123"); // Use a secure password
                }
                else if (user.Username == "residentUser")
                {
                    user.SetPassword("residentPassword123"); // Use a secure password
                }
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
