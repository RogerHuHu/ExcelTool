﻿using System;

namespace HIIUtils.Commands
{
    public interface IActiveAware
    {
        bool IsActive { get; set; }

        event EventHandler IsActiveChanged;
    }
}