using System.Collections.Immutable;
using BtkAkademi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BtkAkademi.Repositories.EFCore.Config;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CategoryName).IsRequired();

        builder.HasData(
            new Category
            {
                Id = 1,
                CategoryName = "Computer Science"
            },
            new Category
            {
                Id = 2,
                CategoryName = "Database Management"
            },
            new Category
            {
                Id = 3,
                CategoryName = "Network"
            }
            );
    }
}