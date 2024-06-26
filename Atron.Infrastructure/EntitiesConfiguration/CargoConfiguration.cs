﻿using Atron.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atron.Infrastructure.EntitiesConfiguration
{
    public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            builder.HasKey(pst => pst.Id);

            builder.Property(dpt => dpt.Id)
                   .ValueGeneratedNever();

            builder.Property(pst => pst.Codigo)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(pst => pst.Descricao)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(dpt => dpt.Departmento) // Tem um departamento
                   .WithMany() // com muitos cargos 
                   .HasForeignKey(key => key.DepartmentoId); // FK da relação

            // Exemplo pra preencher a tabela 
            //builder.HasData(new Position(1, "Mkrt Manager"));
        }
    }
}