﻿using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymWorker;
using MediatR;
using Microsoft.Data.SqlClient;

namespace Application.GymWorker
{
    public class GymWorkerGetAllCommand : IRequest<PageResult<GymWorkerGetResult>>
    {
        public string SearchString { get; set; } = "";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    }

    public class GymWorkerGetAllCommandHandler : IRequestHandler<GymWorkerGetAllCommand, PageResult<GymWorkerGetResult>>
    {
        private readonly IGymWorkerService _gymWorkerService;

        public GymWorkerGetAllCommandHandler(IGymWorkerService gymWorkerService) => _gymWorkerService = gymWorkerService;

        public async Task<PageResult<GymWorkerGetResult>> Handle(GymWorkerGetAllCommand request, CancellationToken cancellationToken)
        {
            var gymWorkersResult = await _gymWorkerService.GetAll(request.SearchString, request.Page, request.PageSize, request.SortOrder);
            return gymWorkersResult;
        }
    }
}