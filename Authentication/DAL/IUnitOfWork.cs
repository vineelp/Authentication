﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.DAL
{
    public interface IUnitOfWork: IDisposable
    {
        void SaveChanges();
    }
}