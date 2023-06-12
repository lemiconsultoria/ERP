using ERP.Core.Commands;
using ERP.Core.Controllers;
using ERP.Core.Interfaces;
using ERP.Crud.Application.Queries;
using ERP.Crud.Domain.Commands.EntryCredit.Results;
using ERP.Crud.Domain.Commands.EntryDebit;
using ERP.Crud.Domain.Commands.EntryDebit.Results;
using ERP.Crud.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERP.Crud.API.Controllers
{
    [Route("api/debit")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class EntryDebitController : MasterController
    {
        private readonly ICommandHandler<CreateEntryDebitCommand, EntryDebitCommandResultBase> _createEntryDebitCommandHandler;
        private readonly ICommandHandler<UpdateEntryDebitCommand, EntryDebitCommandResultBase> _updateEntryDebitCommandHandler;
        private readonly ICommandHandler<DeleteEntryDebitCommand, DeleteEntryDebitCommandResult> _deleteEntryDebitCommandHandler;
        private readonly IEntryDebitQueries _entryDebitQueries;

        public EntryDebitController(ICommandHandler<CreateEntryDebitCommand, EntryDebitCommandResultBase> createEntryDebitCommandHandler,
            ICommandHandler<UpdateEntryDebitCommand, EntryDebitCommandResultBase> updateEntryDebitCommandHandler,
            ICommandHandler<DeleteEntryDebitCommand, DeleteEntryDebitCommandResult> deleteEntryDebitCommandHandler,
            IEntryDebitQueries entryDebitQueries)
        {
            _createEntryDebitCommandHandler = createEntryDebitCommandHandler;
            _updateEntryDebitCommandHandler = updateEntryDebitCommandHandler;
            _deleteEntryDebitCommandHandler = deleteEntryDebitCommandHandler;
            _entryDebitQueries = entryDebitQueries;
        }

        /// <summary>
        /// Obtém todos os lançamentos de débito        
        /// </summary>        
        /// <returns>Lista de lançamentos</returns>
        /// <response code="200">Lançamentos de Débito</response>
        /// <response code="500">Exceção</response>
        [HttpGet]
        [ProducesResponseType(typeof(EntryDebit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var result = await _entryDebitQueries.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }

        /// <summary>
        /// Obtém um lançamento de débito pelo Identificador
        /// </summary>        
        /// <returns>Lista de lançamentos</returns>
        /// <response code="200">Lançamento de Débito</response>        
        /// <response code="500">Exceção</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EntryDebit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            try
            {
                var result = await _entryDebitQueries.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }

        /// <summary>
        /// Inclui um lançamento de débito
        /// </summary>        
        /// <returns>Dados do lançamento</returns>
        /// <response code="200">Lançamento de Débito</response>        
        /// <response code="400">Erros encontrados</response> 
        /// <response code="500">Exceção</response>
        [HttpPost]
        [ProducesResponseType(typeof(EntryDebit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CommandResult<EntryDebitCommandResultBase>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Post(CreateEntryDebitCommand command)
        {
            try
            {
                var result = _createEntryDebitCommandHandler.Handle(command);

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
        /// Altera um lançamento de débito
        /// </summary>        
        /// <returns>Dados do lançamento</returns>
        /// <response code="200">Lançamento de Débito</response>  
        /// <response code="400">Erros encontrados</response> 
        /// <response code="500">Exceção</response>
        [HttpPut]
        [ProducesResponseType(typeof(EntryDebit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CommandResult<EntryDebitCommandResultBase>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Put(UpdateEntryDebitCommand command)
        {
            try
            {
                var result = _updateEntryDebitCommandHandler.Handle(command);

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
        /// Exclui um lançamento de débito
        /// </summary>        
        /// <returns>Dados do lançamento</returns>
        /// <response code="200">Lançamento de Débito</response>  
        /// <response code="400">Erros encontrados</response> 
        /// <response code="500">Exceção</response>
        [HttpDelete]
        [ProducesResponseType(typeof(EntryDebit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CommandResult<DeleteEntryDebitCommandResult>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Delete(DeleteEntryDebitCommand command)
        {
            try
            {
                var result = _deleteEntryDebitCommandHandler.Handle(command);

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