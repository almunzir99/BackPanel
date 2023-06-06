using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Application.Static;
using BackPanel.Domain.Entities;
using BackPanel.FilesManager.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Application.Services;

public class CompanyInfosService : ServiceBase<CompanyInfo,CompanyInfoDto,CompanyInfoDtoRequest>, ICompanyInfosService
{
    public CompanyInfosService(IMapper mapper, IRepositoryBase<CompanyInfo> repository, IRepositoryBase<Admin> adminsRepository,IPathProvider pathProvider)
    : base(mapper, repository, adminsRepository,pathProvider)
    {
        Repository.IncludeableDbSet = Repository.IncludeableDbSet.Include(c => c.Logo);
    }
    public async Task<CompanyInfoDto> GetCompanyInfoAsync()
    {
        var infos = await Repository.ListAsync();
        var info = infos.FirstOrDefault();
        if (info == null)
            throw new Exception("no company info added");
        var result = Mapper.Map<CompanyInfoDto>(info);
        StaticData.CompanyInfo = result;
        return result;
    }
}