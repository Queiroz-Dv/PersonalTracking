﻿using Atron.Domain.Entities;
using Atron.Domain.Interfaces;
using Atron.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atron.Infrastructure.Repositories
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private AtronDbContext _context;

        public DepartamentoRepository(AtronDbContext context)
        {
            _context = context;
        }

        public async Task<Departamento> AtualizarDepartamentoRepositoryAsync(Departamento departamento)
        {
            var entidade = await _context.Departamentos.FirstOrDefaultAsync(dpt => dpt.Codigo == departamento.Codigo);

            try
            {
                if (entidade is not null)
                {
                    entidade.AtualizarDescricao(departamento.Descricao);
                    await _context.SaveChangesAsync();
                    return entidade;
                }
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                throw;
            }

            return departamento;
        }

        public async Task<Departamento> CriarDepartamentoRepositoryAsync(Departamento departamento)
        {
            _context.Add(departamento);
            await _context.SaveChangesAsync();
            return departamento;
        }

        public bool DepartamentoExiste(string codigo)
        {
            var departamentoExiste = _context.Departamentos.Any(dpt => dpt.Codigo == codigo);
            return departamentoExiste;
        }

        public async Task<Departamento> ObterDepartamentoPorCodigoRepositoryAsync(string codigo)
        {
            var departamento = await _context.Departamentos.Where(dpt => dpt.Codigo == codigo).FirstOrDefaultAsync();
            return departamento;
        }

        public async Task<Departamento> ObterDepartamentoPorIdRepositoryAsync(int? id)
        {
            var departamento = await _context.Departamentos.AsNoTracking().FirstOrDefaultAsync(dpt => dpt.Id == id);
            return departamento;
        }

        public async Task<IEnumerable<Departamento>> ObterDepartmentosAsync()
        {
            var departamentos = await _context.Departamentos
                                    .AsNoTracking()
                                    .OrderByDescending(order => order.Codigo)
                                    .ToListAsync();

            return departamentos;
        }

        public async Task<Departamento> RemoverDepartmentoRepositoryAsync(Departamento departamento)
        {
            _context.Remove(departamento);
            await _context.SaveChangesAsync();
            return departamento;
        }
    }
}