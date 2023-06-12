using ERP.Consolidator.Domain.Entities;

namespace ERP.Consolidator.Domain.Interfaces.Repositories
{
    public interface IDateToProcessRepository
    {
        IList<DateToProcess> GetDatesToProcess();

        bool DateNotExists(DateOnly dateRef);

        void Add(DateToProcess entity);

        void RemoveDate(DateOnly dateRef);
    }
}