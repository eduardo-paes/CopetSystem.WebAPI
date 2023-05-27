﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntitiesConfiguration
{
    public class StudentAssistanceScholarshipConfiguration : IEntityTypeConfiguration<StudentAssistanceScholarship>
    {
        public void Configure(EntityTypeBuilder<StudentAssistanceScholarship> builder)
        {
            builder.ToTable("StudentAssistanceScholarships");
            builder.HasKey(t => t.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasMaxLength(300).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(300);
            builder.Property(p => p.DeletedAt);

            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}