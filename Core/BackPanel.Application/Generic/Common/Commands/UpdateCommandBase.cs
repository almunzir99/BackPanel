using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;

namespace BackPanel.Application.Generic.Common.Commands
{
    public record UpdateCommandBase<TDTORequest, TDTO>(int Id, TDTORequest Request) : IRequest<TDTO>;

}