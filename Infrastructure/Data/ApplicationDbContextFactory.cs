﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // optionsBuilder.UseSqlServer(args[0]);
        optionsBuilder.UseSqlServer("Server=TAMARA;Database=TestDB;Trusted_Connection=True;MultipleActiveResultSets=True;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}