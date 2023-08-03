using BtkAkademi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtkAkademi.Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "İnsan Ne İle Yaşar" , Price = 334},
                new Book { Id = 2, Title = "Mai ve Siyah" , Price = 34},
                new Book { Id = 3, Title = "Yaban" , Price = 264}
                );
        }
    }
}
