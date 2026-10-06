using Microsoft.AspNetCore.Mvc;
using PPEManagement.Services.Contracts;
using PPEManagement.Services.Contracts.Models.PPEStatement;

namespace PPEManagement.Controllers;

/// <summary>
/// Контроллер для управления ведомостями выдачи СИЗ.
/// </summary>
[Route("[controller]")]
public class PpeStatementController : Controller
{
    private readonly IPPEStatementService _statementService;

    public PpeStatementController(IPPEStatementService statementService)
    {
        _statementService = statementService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Страница просмотра ведомости.
    /// </summary>
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var detailModel = await _statementService.GetPPEStatementByIdAsync(id, cancellationToken);
        
        if (detailModel == null)
        {
            return NotFound();
        }
        
        var viewModel = new PPEStatementViewModel
        {
            OrganizationName = detailModel.OrganizationName,
            DepartmentName = detailModel.DepartmentName,
            Reason = detailModel.Reason,
            IssueDate = detailModel.IssueDate,
            ResponsiblePerson = detailModel.ResponsiblePerson,
            TotalGasMasks = detailModel.TotalGasMasks,
            TotalKIMGZ = detailModel.TotalKIMGZ,
            TotalOtherPPE = detailModel.TotalOtherPPE,
            Items = detailModel.Items.Select((item, index) => new PPEStatementItemViewModel
            {
                RowNumber = index + 1,
                EmployeeFullName = item.EmployeeFullName,
                PersonnelNumber = item.PersonnelNumber,
                PPEName = item.PPEName,
                BatchNumber = item.BatchNumber,
                Size = item.Size,
                Quantity = item.Quantity,
                IssueDate = item.IssueDate
            }).ToList()
        };

        return View(viewModel);
    }

    /// <summary>
    /// Страница редактирования ведомости.
    /// </summary>
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var detailModel = await _statementService.GetPPEStatementByIdAsync(id, cancellationToken);
        
        if (detailModel == null)
        {
            return NotFound();
        }
        
        var viewModel = new PPEStatementViewModel
        {
            OrganizationName = detailModel.OrganizationName,
            DepartmentName = detailModel.DepartmentName,
            Reason = detailModel.Reason,
            IssueDate = detailModel.IssueDate,
            ResponsiblePerson = detailModel.ResponsiblePerson,
            TotalGasMasks = detailModel.TotalGasMasks,
            TotalKIMGZ = detailModel.TotalKIMGZ,
            TotalOtherPPE = detailModel.TotalOtherPPE,
            Items = detailModel.Items.Select((item, index) => new PPEStatementItemViewModel
            {
                RowNumber = index + 1,
                EmployeeFullName = item.EmployeeFullName,
                PersonnelNumber = item.PersonnelNumber,
                PPEName = item.PPEName,
                BatchNumber = item.BatchNumber,
                Size = item.Size,
                Quantity = item.Quantity,
                IssueDate = item.IssueDate
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost("api/create")]
    [ProducesResponseType(typeof(PPEStatementDetailModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateStatement(
        [FromBody] PPEStatementCreateModel model,
        CancellationToken cancellationToken)
    {
        var created = await _statementService.AddPPEStatementAsync(model, cancellationToken);
        return CreatedAtAction(nameof(Details), new { id = created.Id }, created);
    }

    [HttpGet("api/{id}")]
    public async Task<IActionResult> GetStatement(Guid id, CancellationToken cancellationToken)
    {
        var statement = await _statementService.GetPPEStatementByIdAsync(id, cancellationToken);
        return Ok(statement);
    }

    [HttpGet("api")]
    public async Task<IActionResult> GetAllStatements(CancellationToken cancellationToken)
    {
        var statements = await _statementService.GetPPEStatementsAsync(cancellationToken);
        return Ok(statements);
    }

    [HttpDelete("api/{id}")]
    public async Task<IActionResult> DeleteStatement(Guid id, CancellationToken cancellationToken)
    {
        await _statementService.DeletePPEStatementAsync(id, cancellationToken);
        return NoContent();
    }
}
