namespace SmartServiceBookingPlatform_API.Controllers;

[Authorize(Roles = Roles.Admin)]
public class CategoriesController : ApiControllerBase
{
    /// <summary>
    /// Returns a paginated list of categories, filtered by ParentId and IsActive.
    /// Response is cached for 10 minutes. Cache varies by all query parameters.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [OutputCache(PolicyName = CachePolicyNames.Categories)]
    public async Task<IActionResult> GetCategoriesList([FromQuery] GetCategoriesListQuery query)
    {
        return HandleResult(await Sender.Send(query));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        return HandleResult(await Sender.Send(new GetCategoryByIdQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var result = await Sender.Send(request.ToCommand());

        if (result.IsFailure)
            return HandleResult(result);

        return CreatedAtAction(
            actionName: nameof(GetCategoryById),
            routeValues: new { id = result.Value },
            value: result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
    {
        return HandleResult(await Sender.Send(request.ToCommand(id)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        return HandleResult(await Sender.Send(new DeleteCategoryCommand(id)));
    }

    [HttpPut("{id:int}/restore")]
    public async Task<IActionResult> RestoreCategory(int id)
    {
        return HandleResult(await Sender.Send(new RestoreCategoryCommand(id)));
    }
}
