﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentitySample.Data
{
    /// <summary>
    /// AppDbContext
    /// Which essentially gives us all the functionality to communicate with the database
    /// 
    /// IdentityDbContext contains all the user tables
    /// </summary>
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
