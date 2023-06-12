using ERP.Consolidator.Domain.Commands.EntryBalance;
using ERP.Core.Controllers;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERP.Consolidator.API.Controllers
{
    [Route("api/consolidate")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class ConsolidateController : MasterController
    {
        private readonly ICommandHandler<ConsolidateCommand, ConsolidateCommandResult> _consolidateCommandHandler;

        public ConsolidateController(ICommandHandler<ConsolidateCommand, ConsolidateCommandResult> consolidateCommandHandler)
        {
            _consolidateCommandHandler = consolidateCommandHandler;
        }

        /// <summary>
        /// Dispara o processo em segundo plano para consolidação dos dados
        /// Caso o atributo "OnlyToday" seja true, será realizada a consolidação considerando somente a data atual.        
        /// </summary>        
        /// <returns>Resultado do processamento de consolidação</returns>
        /// <response code="200">Datas processadas com sucesso</response>
        /// <response code="400">Ocorreu erro durante do processamento</response>
        /// <response code="500">Exceção</response>
        [HttpPost]        
        [ProducesResponseType(typeof(ConsolidateCommandResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.BadRequest)]        
        public IActionResult Post(ConsolidateCommand command)
        {
            try
            {
                var result = _consolidateCommandHandler.Handle(command);

                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }
    }
}