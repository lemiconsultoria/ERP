using ERP.Core.Commands;
using ERP.Core.Controllers;
using ERP.Core.Interfaces;
using ERP.Crud.Application.Queries;
using ERP.Crud.Domain.Commands.EntryCredit;
using ERP.Crud.Domain.Commands.EntryCredit.Results;
using ERP.Crud.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERP.Crud.API.Controllers
{
    [Route("api/credit")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class EntryCreditController : MasterController
    {
        private readonly ICommandHandler<CreateEntryCreditCommand, EntryCreditCommandResultBase> _createEntryCreditCommandHandler;
        private readonly ICommandHandler<UpdateEntryCreditCommand, EntryCreditCommandResultBase> _updateEntryCreditCommandHandler;
        private readonly ICommandHandler<DeleteEntryCreditCommand, DeleteEntryCreditCommandResult> _deleteEntryCreditCommandHandler;
        private readonly IEntryCreditQueries _entryCreditQueries;

        public EntryCreditController(ICommandHandler<CreateEntryCreditCommand, EntryCreditCommandResultBase> createEntryCreditCommandHandler,
            ICommandHandler<UpdateEntryCreditCommand, EntryCreditCommandResultBase> updateEntryCreditCommandHandler,
            ICommandHandler<DeleteEntryCreditCommand, DeleteEntryCreditCommandResult> deleteEntryCreditCommandHandler,
            IEntryCreditQueries entryCreditQueries)
        {
            _createEntryCreditCommandHandler = createEntryCreditCommandHandler;
            _updateEntryCreditCommandHandler = updateEntryCreditCommandHandler;
            _deleteEntryCreditCommandHandler = deleteEntryCreditCommandHandler;
            _entryCreditQueries = entryCreditQueries;
        }


        /// <summary>
        /// Obtém todos os lançamentos de crédito        
        /// </summary>        
        /// <returns>Lista de lançamentos</returns>
        /// <response code="200">Lançamentos de Crédito</response>
        /// <response code="500">Exceção</response>
        [HttpGet]
        [ProducesResponseType(typeof(EntryCredit), (int)HttpStatusCode.OK)]        
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var result = await _entryCreditQueries.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }

        /// <summary>
        /// Obtém um lançamento de crédito pelo Identificador
        /// </summary>        
        /// <returns>Lista de lançamentos</returns>
        /// <response code="200">Lançamento de Crédito</response>        
        /// <response code="500">Exceção</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EntryCredit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            try
            {
                var result = await _entryCreditQueries.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }

        /// <summary>
        /// Inclui um lançamento de crédito
        /// </summary>        
        /// <returns>Dados do lançamento</returns>
        /// <response code="200">Lançamento de Crédito</response>        
        /// <response code="400">Erros encontrados</response> 
        /// <response code="500">Exceção</response>
        [HttpPost]
        [ProducesResponseType(typeof(EntryCredit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CommandResult<EntryCreditCommandResultBase>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Post(CreateEntryCreditCommand command)
        {
            try
            {
                var result = _createEntryCreditCommandHandler.Handle(command);

                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }

        /// <summary>
        /// Altera um lançamento de crédito
        /// </summary>        
        /// <returns>Dados do lançamento</returns>
        /// <response code="200">Lançamento de Crédito</response>  
        /// <response code="400">Erros encontrados</response> 
        /// <response code="500">Exceção</response>
        [HttpPut]
        [ProducesResponseType(typeof(EntryCredit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CommandResult<EntryCreditCommandResultBase>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Put(UpdateEntryCreditCommand command)
        {
            try
            {
                var result = _updateEntryCreditCommandHandler.Handle(command);

                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }

        /// <summary>
        /// Exclui um lançamento de crédito
        /// </summary>        
        /// <returns>Dados do lançamento</returns>
        /// <response code="200">Lançamento de Crédito</response>  
        /// <response code="400">Erros encontrados</response> 
        /// <response code="500">Exceção</response>
        [HttpDelete]
        [ProducesResponseType(typeof(EntryCredit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CommandResult<DeleteEntryCreditCommandResult>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Delete(DeleteEntryCreditCommand command)
        {
            try
            {
                var result = _deleteEntryCreditCommandHandler.Handle(command);

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