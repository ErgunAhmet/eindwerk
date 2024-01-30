using Domain.Entities;
using IInfrasctructure.MongoDB;

namespace IRepository
{
    public interface ICandidateAccountRepository : IMongoDBReadWriteRepository<CandidateAccount>
    {
    }
}
