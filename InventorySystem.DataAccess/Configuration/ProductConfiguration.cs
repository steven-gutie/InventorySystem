﻿using InventorySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DataAccess.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.SerialNumber).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ProdDescription).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired(false);
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.BrandId).IsRequired();
            builder.Property(x => x.ParentId).IsRequired(false);
            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId)
                                            .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Brand).WithMany().HasForeignKey(x => x.BrandId)
                                            .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Parent).WithMany().HasForeignKey(x => x.ParentId)
                                            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
