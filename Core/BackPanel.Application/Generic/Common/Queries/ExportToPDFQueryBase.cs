using MediatR;

namespace BackPanel.Application.Generic.Common.Queries
{
    public record ExportToPDFQueryBase<TEntity> : IRequest<byte[]>
       where TEntity : class;

}
