using cmclean.Domain.Repositories;

namespace cmclean.Infrastructure.Domain
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly CleanDbContext _context;
        private IContactRepository _contactRepository;
        public IContactRepository ContactRepository => _contactRepository ??= new ContactRepository(_context);
        public RepositoryWrapper(CleanDbContext context)
        {
            _context = context;
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}