using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Emailing.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Emailing.Commands.Handlers
{
    public class EmailingCommandHandler : ResponseHandler
                                          , IRequestHandler<SendEmailCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IEmailService _emailService;
        #endregion
        #region Constructors
        public EmailingCommandHandler(IStringLocalizer<SharedResources> localizer, IEmailService emailService) : base(localizer)
        {
            _localizer = localizer;
            _emailService = emailService;
        }
        #endregion
        #region Handle Methods
        public async Task<Response<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _emailService.SendEmailAsync(request.Email, request.Message, null);
            if (response == "Success")
                return Success<string>("");
            return BadRequest<string>(_localizer[SharedResourcesKeys.SendEmailFailed]);
        }
        #endregion
    }
}
