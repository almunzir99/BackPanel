using DocumentFormat.OpenXml.Spreadsheet;
using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Accounts.Commands
{
    public record ValidateCodeCommandBase<TEntity>(string Email, int Code, bool IsEmailValidation = false) : IRequest<EmailRecoveryRequest>
            where TEntity : UserEntityBase
        ;
}
