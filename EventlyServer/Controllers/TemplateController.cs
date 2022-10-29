using EventlyServer.Data.Dto;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

/// <summary>
/// Работа с шаблонами приглашений
/// </summary>
[ApiController]
[Route("api/template")]
[Produces("application/json")]
public class TemplateController : ControllerBase
{
    private readonly TemplateService _templateService;

    public TemplateController(TemplateService templateService)
    {
        _templateService = templateService;
    }

    /// <summary>
    /// Получить список всех доступных шаблонов
    /// </summary>
    /// <returns>Список доступных шаблонов</returns>
    /// <response code="200">Список доступных шаблонов</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(List<TemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<TemplateDto>>> GetAllTemplates()
    {
        try
        {
            return await _templateService.GetTemplates();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Получить информацию о конкретном шаблоне
    /// </summary>
    /// <param name="id">ID выбранного шаблона</param>
    /// <returns>Подробная информация о выбранном шаблоне</returns>
    /// <response code="200">Подробная информация о выбранном шаблоне</response>
    /// <response code="404">Шаблон с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(TemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TemplateDto>> GetTemplateDetails([FromRoute] int id)
    {
        try
        {
            return await _templateService.GetTemplateDetails(id);
        }
        catch (InvalidDataException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Добавить новый шаблон в базу
    /// </summary>
    /// <param name="template">Описание нового шаблона</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="201">Шаблон создан успешно</response>
    /// <response code="409">Шаблон с таким именем уже существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPost]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AddNewTemplate([FromBody] TemplateCreatingDto template)
    {
        try
        {
            await _templateService.AddTemplate(template);
            return Ok();
        }
        catch (ArgumentException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Удалить шаблон
    /// </summary>
    /// <param name="id">ID шаблона</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Шаблон удален успешно</response>
    /// <response code="404">Шаблон с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteTemplate([FromRoute] int id)
    {
        try
        {
            await _templateService.DeleteTemplate(id);
            return Ok();
        }
        catch (InvalidDataException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}