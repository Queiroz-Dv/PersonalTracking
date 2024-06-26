﻿using Atron.Application.DTO;
using Atron.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Notification.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atron.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private ITarefaService _service;

        public TarefaController(ITarefaService service)
        {
            _service = service;
        }

        [Route("CriarTarefa")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TarefaDTO tarefa)
        {
            if (tarefa is null)
            {
                return BadRequest(new NotificationMessage("Registro inválido, tente novamente"));
            }

            await _service.CriarAsync(tarefa);

            return Ok(_service.Messages);
        }

        [Route("ObterTarefas")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarefaDTO>>> Get()
        {
            var tarefas = await _service.ObterTodosAsync();

            if (tarefas is null)
            {
                return NotFound("Não foi encontrado nenhum registro");
            }

            return Ok(tarefas);
        }
    }
}