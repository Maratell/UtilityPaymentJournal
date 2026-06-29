using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.DTO.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using Microsoft.EntityFrameworkCore;

namespace UtilityPaymentJournal.Services
{
    public class ComplaintBoardService : IComplaintBoardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IComplaintMapper _complaintMapper;

        public ComplaintBoardService(
            ApplicationDbContext context,
            IComplaintMapper complaintMapper)
        {
            _context = context;
            _complaintMapper = complaintMapper;
        }

        public async Task<ComplaintDTO> CreateAsync(CreateComplaintDTO dto)
        {
            Complaint entity = _complaintMapper.ToEntity(dto);

            await _context.Complaints.AddAsync(entity);
            await _context.SaveChangesAsync();

            Complaint? savedEntity = await FindByIdOrThrowAsync(entity.Id);
            return _complaintMapper.ToDto(savedEntity);
        }

        public async Task DeleteAsync(long id)
        {
            Complaint entity = await FindByIdOrThrowAsync(id);

            _context.Complaints.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ComplaintDTO> EditAsync(long id, EditComplaintDTO dto)
        {
            Complaint entity = await FindByIdOrThrowAsync(id);

            _complaintMapper.UpdateEntity(dto, entity);

            await _context.SaveChangesAsync();

            // Принудительно загружаем из БД данные НОВОЙ услуги 
            await _context.Entry(entity).Reference(c => c.Utility).LoadAsync();

            return _complaintMapper.ToDto(entity);
        }

        public async Task<IEnumerable<ComplaintDTO>> GetAllAsync()
        {
            List<Complaint> entities = await GetComplaintsAsync();

            return entities.Select(e => _complaintMapper.ToDto(e));
        }

        private async Task<List<Complaint>> GetComplaintsAsync()
        {
            return await _context.Complaints.ToListAsync();
        }

        private async Task<Complaint> FindByIdOrThrowAsync(long id)
        {
            Complaint? entity = await _context.Complaints
                .Include(e => e.Utility)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Жалобы с ID {id} не найдено.");
            }

            return entity;
        }
    }
}
