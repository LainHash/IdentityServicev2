using API.Application.Features.Business;
using API.Application.Features.Identity.PersonalProfiles;
using API.Application.Features.Identity.PersonalProfiles.Commands.CompleteProfile;
using API.Application.Features.Identity.PersonalProfiles.Commands.UpdateByOwner;
using API.Contract.DTOs.Identity.PersonalProfiles;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Identity.PersonalProfiles;
using API.Domain.Features.Identity.Users;
using AutoMapper;
using System.Net;

namespace API.Infrastructure.Features.Identity.PersonalProfiles
{
    internal class PersonalProfileService : IPersonalProfileService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonalProfileRepository _personalProfileRepository;

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PersonalProfileService(
            IUserRepository userRepository,
            IPersonalProfileRepository personalProfileRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _personalProfileRepository = personalProfileRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CompleteProfileAsync(
            CompleteProfileCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByEmailAsync(command.Body.Email, cancellationToken);
            if (user is null)
            {
                return Result
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            if (!user.IsConfirmed)
            {
                return Result
                    .Fail("Account is not confirmed. Please verify your email first.", HttpStatusCode.PreconditionRequired);
            }

            if (!user.IsActive)
            {
                return Result
                    .Fail("Account is inactive. Please contact administrator.", HttpStatusCode.Forbidden);
            }

            var personalProfile = await _personalProfileRepository
                .FindByUserIdAsync(user.Id, cancellationToken);
            if (personalProfile is not null)
            {
                return Result
                    .Fail("Profile has already been completed.", HttpStatusCode.Conflict);
            }

            personalProfile = _mapper.Map<PersonalProfile>(command.Body)
                .SetUser(user.Id);
            _personalProfileRepository.Add(personalProfile);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result
                .Succeed("Profile completed successfully.", HttpStatusCode.Accepted);
        }

        public async Task<Result<PersonalProfileResponse>> UpdatePersonalProfileAsync(
            UpdatePersonalProfileCommand command,
            UpdatePersonalProfileSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var personalProfile = await _personalProfileRepository.FindAsync(specification, cancellationToken);
            if(personalProfile is null)
            {
                return Result<PersonalProfileResponse>
                    .Fail(Error.NotFound("Personal Profile"), HttpStatusCode.NotFound);
            }

            _mapper.Map(command.Body, personalProfile);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<PersonalProfileResponse>(personalProfile);
            return Result<PersonalProfileResponse>
                .Succeed(response, Success.Updated("Personal Profile"));
        }
    }
}
