using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Config
{
	public class BookConfig : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.HasData(
				new Book { Id = 1,CategoryId=1, Title = "Anan", Price = 100 },
				new Book { Id = 2, CategoryId = 2, Title = "Baban", Price = 5 },
				new Book { Id = 3, CategoryId = 1, Title = "AnanAnan", Price = 20 }
				);
		}
	}
}
