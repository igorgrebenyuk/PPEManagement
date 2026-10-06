using Microsoft.AspNetCore.Mvc;
using PPEManagement.Services.Contracts;
using PPEManagement.Services.Contracts.Models.PPEStatement;

namespace PPEManagement.Controllers;

/// <summary>
/// Контроллер для управления ведомостями выдачи СИЗ.
/// </summary>
public class PpeStatementController : Controller
{
    private readonly IPPEStatementService statementService;

    public PpeStatementController(IPPEStatementService statementService)
    {
        this.statementService = statementService;
    }

    #region MVC Views
    
    [HttpGet("")]
    [HttpGet("PpeStatement")]
    [HttpGet("PpeStatement/Index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("PpeStatement/Create")]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Страница просмотра ведомости.
    /// </summary>
    [HttpGet("PpeStatement/Details/{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var detailModel = await statementService.GetPPEStatementByIdAsync(id, cancellationToken);
        
        if (detailModel == null)
        {
            return NotFound();
        }
        
        return View(detailModel);
    }

    /// <summary>
    /// Страница редактирования ведомости.
    /// </summary>
    [HttpGet("PpeStatement/Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var detailModel = await statementService.GetPPEStatementByIdAsync(id, cancellationToken);
        
        if (detailModel == null)
        {
            return NotFound();
        }
        
        return View(detailModel);
    }

    #endregion

    #region REST API Endpoints

    [HttpPost("api/PpeStatement")]
    [ProducesResponseType(typeof(PPEStatementDetailModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateStatement(
        [FromBody] PPEStatementCreateModel model,
        CancellationToken cancellationToken)
    {
        var created = await statementService.AddPPEStatementAsync(model, cancellationToken);
        return CreatedAtAction(nameof(Details), new { id = created.Id }, created);
    }

    [HttpGet("api/PpeStatement/{id:guid}")]
    public async Task<IActionResult> GetStatement(Guid id, CancellationToken cancellationToken)
    {
        var statement = await statementService.GetPPEStatementByIdAsync(id, cancellationToken);
        if (statement == null)
        {
            return NotFound();
        }
        return Ok(statement);
    }

    [HttpGet("api/PpeStatement")]
    public async Task<IActionResult> GetAllStatements(CancellationToken cancellationToken)
    {
        var statements = await statementService.GetPPEStatementsAsync(cancellationToken);
        return Ok(statements);
    }

    [HttpPut("api/PpeStatement/{id:guid}")]
    public async Task<IActionResult> UpdateStatement(
        Guid id,
        [FromBody] PPEStatementUpdateModel model,
        CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest("Идентификаторы ведомости не совпадают.");
        }

        await statementService.UpdatePPEStatementAsync(id , model, cancellationToken);
        return NoContent();
    }

    [HttpDelete("api/PpeStatement/{id:guid}")]
    public async Task<IActionResult> DeleteStatement(Guid id, CancellationToken cancellationToken)
    {
        await statementService.DeletePPEStatementAsync(id, cancellationToken);
        return NoContent();
    }

    #endregion
}