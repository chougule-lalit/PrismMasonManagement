﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Core.PrismMasonManagementCoreBase.Interface
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
