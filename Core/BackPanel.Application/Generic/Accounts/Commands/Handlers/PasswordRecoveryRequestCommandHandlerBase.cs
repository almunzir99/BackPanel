using DocumentFormat.OpenXml.Spreadsheet;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.FilesManager.Interfaces;
using BackPanel.SMTP.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace BackPanel.Application.Generic.Accounts.Commands.Handlers
{
    public abstract class PasswordRecoveryRequestCommandHandlerBase<TEntity, TCommand> : IRequestHandler<TCommand, bool>
        where TEntity : UserEntityBase
        where TCommand : PasswordRecoveryRequestCommandBase<TEntity>
    {
        private readonly IMemoryCache _memoryCache;
        protected readonly IRepositoryBase<TEntity> Repository;
        private readonly IPathProvider pathProvider;
        private readonly IConfiguration _configuration;
        private readonly ISmtpService _smtpService;
        public PasswordRecoveryRequestCommandHandlerBase(IRepositoryBase<TEntity> repository, IMemoryCache memoryCache, IPathProvider pathProvider, IConfiguration configuration, ISmtpService smtpService)
        {
            Repository = repository;
            _memoryCache = memoryCache;
            this.pathProvider = pathProvider;
            _configuration = configuration;
            _smtpService = smtpService;
        }
        public async Task<bool> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var user = await Repository.FindAsync(c => c.Email!.ToLower() == request.Email.ToLower());
            if (user == null)
                throw new Exception("invalid user email");
            var random = new Random();
            var Code = random.Next(10000, 1000000);
            var path = Path.Combine(pathProvider.GetRootPath(), "templates/passwordRecovery.html");
            var htmlContent = File.ReadAllText(path);
            htmlContent = htmlContent.Replace("{{CODE}}", Code.ToString());
            var senderEmail = _configuration.GetValue<string>("Smtp:email");
            await _smtpService.SendMessageAsync(senderEmail, request.Email, "Password Recovery", htmlContent, MimeKit.Text.TextFormat.Html);
            var emailRequest = new EmailRecoveryRequest()
            {
                UserId = user.Id,
                Code = Code,
                ExpireAt = DateTime.Now.AddDays(1)
            };
            _memoryCache.Set($"pr_{user.Email}", emailRequest);
            return true;
        }
    }
}
