﻿using System;
using CopetSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CopetSystem.Infra.Data.EntitiesConfiguration
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(t => t.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasMaxLength(300).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(300).IsRequired();
            builder.Property(p => p.Password).HasMaxLength(300).IsRequired();
            builder.Property(p => p.CPF).HasMaxLength(15).IsRequired();
            builder.Property(p => p.Role).HasMaxLength(30).IsRequired();
            builder.Property(p => p.DeletedAt);

            builder.HasData(
                new User("User Name", "user.name@email.com", "123456", "15162901784", "ADMIN", null)
            );
        }
    }
}

