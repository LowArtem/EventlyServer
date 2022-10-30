using EventlyServer.Controllers.Abstracts;
using EventlyServer.Data.Dto;
using EventlyServer.Extensions;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

/// <summary>
/// Работа с шаблонами приглашений
/// </summary>
public class TemplateController : BaseApiController
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
        return await this.SendResponseAsync(async () => await _templateService.GetTemplates());
    }

    /// <summary>
    /// Получить информацию о конкретном шаблоне
    /// </summary>
    /// <param name="id">ID выбранного шаблона</param>
    /// <returns>Подробная информация о выбранном шаблоне</returns>
    /// <response code="200">Подробная информация о выбранном шаблоне</response>
    /// <response code="400">Шаблон с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(TemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TemplateDto>> GetTemplateDetails([FromRoute] int id)
    {
        return await this.SendResponseAsync(async () => await _templateService.GetTemplateDetails(id));
    }

    /// <summary>
    /// Добавить новый шаблон в базу
    /// </summary>
    /// <param name="template">Описание нового шаблона</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Шаблон создан успешно</response>
    /// <response code="409">Шаблон с таким именем уже существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPost]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AddNewTemplate([FromBody] TemplateCreatingDto template)
    {
        return await this.SendResponseAsync(async () => await _templateService.AddTemplate(template));
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
    /// <response code="400">Шаблон с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteTemplate([FromRoute] int id)
    {
        return await this.SendResponseAsync(async () => await _templateService.DeleteTemplate(id));
    }
}