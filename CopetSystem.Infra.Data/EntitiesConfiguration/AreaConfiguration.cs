﻿using System;
using CopetSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CopetSystem.Infra.Data.EntitiesConfiguration
{
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.ToTable("Areas");
            builder.HasKey(t => t.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(100).IsRequired();
            builder.Property(p => p.MainAreaId).IsRequired();

            builder.HasData(
                new Area(new Guid(), "ABC-123", "Area 1"),
                new Area(new Guid(), "DEF-456", "Area 2"),
                new Area(new Guid(), "GHI-789", "Area 3")
            );
        }
    }
}