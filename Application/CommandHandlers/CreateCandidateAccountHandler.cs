using Domain.Entities;
using Domain.Resources;
using IRepository;
using MediatR;
using Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CommandHandlers
{

    public class CreateCandidateAccountHandler : IRequestHandler<CreateCandidateAccountRequest, CreateCandidateAccountResponse>
    {
        private readonly ICandidateAccountRepository _candidateAccountRepository;

        public CreateCandidateAccountHandler(ICandidateAccountRepository candidateAccountRepository)
        {
            _candidateAccountRepository = candidateAccountRepository;
        }

        public async Task<CreateCandidateAccountResponse> Handle(CreateCandidateAccountRequest request, CancellationToken cancellationToken)
        {
            Guid userId;
            Guard.ThatIsFalse(String.IsNullOrEmpty(request.UserId), nameof(request.UserId), nameof(ErrorCodeResource.Property_IncorrectValue));
            Guard.ThatIsTrue(Guid.TryParse(request.UserId, out userId), nameof(request.UserId), nameof(ErrorCodeResource.Property_IncorrectValue));

            var account = await _candidateAccountRepository.GetByIdAsync(userId);
            if (account is null)
            {
                account = CandidateAccount.Create(userId, request.FirstName, request.LastName, request.Email, request.PhoneNumber);
                await _candidateAccountRepository.InsertAsync(account);
            }

            return new CreateCandidateAccountResponse
            {
                CandidateAccount = account,
            };
        }
    }

    public class CreateCandidateAccountRequest : IRequest<CreateCandidateAccountResponse>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class CreateCandidateAccountResponse
    {
        public CandidateAccount CandidateAccount { get; set; }
    }
}
