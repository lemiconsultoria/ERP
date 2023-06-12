using ERP.Core.Controllers;
using ERP.Report.Application.Queries;
using ERP.Report.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERP.Report.API.Controllers
{
    [Route("api/balance")]
    [ApiController]
    [Authorize(Roles = "ADMIN,REPORT")]
    public class EntryBalanceController : MasterController
    {
        private readonly IEntryBalanceQueries _entryBalanceQueries;

        public EntryBalanceController(IEntryBalanceQueries entryBalanceQueries)
        {
            _entryBalanceQueries = entryBalanceQueries;
        }

        /// <summary>
        /// Obtém o relatório consolidado da data atual
        /// </summary>        
        /// <returns>Resultado do processamento de consolidação</returns>
        /// <response code="200">Dados consolidados</response>
        /// <response code="400">Ocorreu erro durante do processamento</response>
        /// <response code="500">Exceção</response>
        [HttpGet]        
        [ProducesResponseType(typeof(EntryBalanceDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.BadRequest)]        
        [Route("today")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Now);

                var result = await _entryBalanceQueries.GetByDateAsync(today);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }

        /// <summary>
        /// Obtém o relatório consolidado com base na data informada
        /// </summary>        
        /// <returns>Resultado do processamento de consolidação</returns>
        /// <response code="200">Dados consolidados</response>
        /// <response code="400">Ocorreu erro durante do processamento</response>
        /// <response code="500">Exceção</response>
        [HttpGet]        
        [ProducesResponseType(typeof(EntryBalanceDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
        [Route("date/{dateRef}")]
        public async Task<IActionResult> GetByDateAsync(DateTime dateRef)
        {
            try
            {
                var validation = EntryBalanceFiltersHelper.IsValidDateRef(dateRef);

                if (validation.Any())
                    return BadRequest(validation);

                var dateOnlyRef = DateOnly.FromDateTime(dateRef);

                var result = await _entryBalanceQueries.GetByDateAsync(dateOnlyRef);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }

        /// <summary>
        /// Obtém o relatório consolidado com base no período informado
        /// </summary>        
        /// <returns>Resultado do processamento de consolidação</returns>
        /// <response code="200">Dados consolidados</response>
        /// <response code="400">Ocorreu erro durante do processamento</response>
        /// <response code="500">Exceção</response>
        [HttpGet]        
        [ProducesResponseType(typeof(EntryBalanceDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
        [Route("before/{dateStart}/end/{dateEnd}")]
        public async Task<IActionResult> GetByPeriodAsync(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var validation = EntryBalanceFiltersHelper.IsValidPeriod(dateStart, dateEnd);

                if (validation.Any())
                    return BadRequest(validation);

                var dateOnlyStartRef = DateOnly.FromDateTime(dateStart);
                var dateOnlyEndRef = DateOnly.FromDateTime(dateEnd);

                var result = await _entryBalanceQueries.GetByPeriodAsync(dateOnlyStartRef, dateOnlyEndRef);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ParseException(ex);
            }
        }
    }
}