using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;
using Microsoft.Data.SqlClient;

namespace Application.GymUser
{
    public record GymUserGetAllCommand : IRequest<PageResult<GymUserGetResult>>
    {
        public string SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
        public string SortParam { get; set; }
    }

    public class GymUserGetAllCommandHandler : IRequestHandler<GymUserGetAllCommand, PageResult<GymUserGetResult>>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserGetAllCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<PageResult<GymUserGetResult>> Handle(GymUserGetAllCommand request, CancellationToken cancellationToken) =>
            await _gymUserService.GetAll(request.SearchString, request.Page, request.PageSize, request.SortOrder, request.SortParam);
    }
}