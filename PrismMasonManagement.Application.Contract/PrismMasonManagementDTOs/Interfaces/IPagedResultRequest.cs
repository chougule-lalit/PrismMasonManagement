﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.Interfaces
{
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        //
        // Summary:
        //     Skip count (beginning of the page).
        int SkipCount { get; set; }
    }
}
