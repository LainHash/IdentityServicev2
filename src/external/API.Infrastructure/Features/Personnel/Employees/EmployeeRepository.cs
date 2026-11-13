using API.Domain.Features.Personnel;
using API.Domain.Features.Personnel.Employees;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Features.Personnel.Employees
{
    internal class EmployeeRepository(AppDbContext context)
        : Repository<Employee>(context), IEmployeeRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Employee?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Employee?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public async Task<Employee?> FindByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public async Task<Employee?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.PublicId == userId, cancellationToken);
        }

        public async Task<Employee?> FindWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(x => x.User)
                    .ThenInclude(u => u.PersonalProfile)
                .Include(x => x.User)
                    .ThenInclude(u => u.Role)
                .Include(x => x.Position)
                    .ThenInclude(p => p.Department)
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }
    }
}
