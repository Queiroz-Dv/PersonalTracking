﻿using Atron.Application.DTO;
using Atron.Application.Interfaces;
using Atron.Domain.Entities;
using Atron.Domain.Interfaces;
using AutoMapper;
using Notification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atron.Application.Services
{
    public class SalarioService : ISalarioService
    {
        private readonly IMapper _mapper;
        private readonly ICargoRepository _cargoRepository;
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly ITarefaRepository _tarefaRepository;
        private readonly ITarefaEstadoRepository _tarefaEstadoRepository;
        private readonly ISalarioRepository _salarioRepository;
        private readonly IMesRepository _mesRepository;
        private readonly NotificationModel<Salario> _notification;

        public SalarioService(IMapper mapper,
                              ICargoRepository cargoRepository,
                              IDepartamentoRepository departamentoRepository,
                              IUsuarioRepository usuarioRepository,
                              IUsuarioService usuarioService,
                              ITarefaRepository tarefaRepository,
                              ITarefaEstadoRepository tarefaEstadoRepository,
                              ISalarioRepository salarioRepository,
                              IMesRepository mesRepository,
                              NotificationModel<Salario> notification)
        {
            _mapper = mapper;
            _cargoRepository = cargoRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _tarefaRepository = tarefaRepository;
            _tarefaEstadoRepository = tarefaEstadoRepository;
            _salarioRepository = salarioRepository;
            _mesRepository = mesRepository;
            _notification = notification;
            Messages = new List<NotificationMessage>();
        }

        public List<NotificationMessage> Messages { get; set; }

        public async Task CriarAsync(SalarioDTO salarioDTO)
        {
            var usuario = await _usuarioRepository.ObterUsuarioPorCodigoAsync(salarioDTO.Usuario.Codigo);
            var mes = await _mesRepository.ObterMesesRepositoryAsync();
            var mesDoSalario = mes.Where(ms => ms.MesId == salarioDTO.Mes.Id).FirstOrDefault();

            if (usuario is not null)
            {
                salarioDTO.Usuario.Id = usuario.Id;
            }

            if (mesDoSalario is not null)
            {
                salarioDTO.Mes.Id = mesDoSalario.MesId;
            }

            var salario = _mapper.Map<Salario>(salarioDTO);
            //var diaAtual = DateTime.Now.Day;
            //salario.Ano = new DateTime(salarioDTO.Ano, salarioDTO.Mes.Id, diaAtual);

            _notification.Validate(salario);

            if (!_notification.Messages.HasErrors())
            {
                await _salarioRepository.CriarSalarioAsync(salario);
                Messages.Add(new NotificationMessage($"Salário incluso para o usuário {salarioDTO.Usuario.Nome}"));
            }
        }

        public async Task<List<SalarioDTO>> ObterTodosAsync()
        {
            var usuariosDTO = await _usuarioService.ObterTodosAsync();
            var mesesRepository = await _mesRepository.ObterMesesRepositoryAsync();
            var salariosRepository = await _salarioRepository.ObterSalariosRepositoryAsync();

            var salariosDTO = _mapper.Map<IEnumerable<SalarioDTO>>(salariosRepository);

            var salarios = salariosDTO
                          // Faz a junção da tabela de usuários com salários             
                          .Join(usuariosDTO,
                                salario => salario.Usuario.Id, // FK de salario
                                usuario => usuario.Id, // PK de usuário
                                (salario, usuario) => new { salario, usuario }) // resultado 

                          // Faz a junção da tabela de meses com salários
                          .Join(mesesRepository,
                                slr => slr.salario.Mes.Id, // FK de mês
                                mes => mes.MesId, // PK de mês

                                // Monta a entidade completa
                                (slr, mes) => new SalarioDTO
                                {
                                    Id = slr.salario.Id,
                                    Usuario = new UsuarioDTO()
                                    {
                                        Codigo = slr.usuario.Codigo,
                                        Nome = slr.usuario.Nome,
                                        Sobrenome = slr.usuario.Sobrenome,
                                        DataNascimento = slr.usuario.DataNascimento,
                                        Salario = slr.usuario.Salario,

                                        Departamento = new DepartamentoDTO()
                                        {
                                            Codigo = slr.usuario.Departamento.Codigo,
                                            Descricao = slr.usuario.Departamento.Descricao
                                        },

                                        Cargo = new CargoDTO()
                                        {
                                            Codigo = slr.usuario.Cargo.Codigo,
                                            Descricao = slr.usuario.Cargo.Descricao,
                                            DepartamentoCodigo = slr.usuario.Cargo.DepartamentoCodigo,
                                            DepartamentoDescricao = slr.usuario.Cargo.DepartamentoDescricao,
                                        }
                                    },

                                    Mes = new MesDTO()
                                    {
                                        Id = mes.MesId,
                                        Descricao = mes.Descricao
                                    },
                                    Ano = slr.salario.Ano,
                                    QuantidadeTotal = slr.salario.QuantidadeTotal
                                }).OrderByDescending(slr => slr.Ano).ToList();

            return salarios;
        }
    }
}