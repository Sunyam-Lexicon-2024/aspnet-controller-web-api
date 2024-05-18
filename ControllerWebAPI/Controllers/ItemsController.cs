using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControllerWebAPI.Models;
using ControllerWebAPI.Models.APIModels;
using AutoMapper;

namespace ControllerWebAPI.Controllers;

/** 
<summary>
Creates a new ItemsController Instance
</summary>
<returns> A new ItemsController Instance </returns>
*/
[Route("api/[controller]")]
[ApiController]
public class ItemsController(
    ApiDbContext context,
    IMapper mapper,
    ILogger<ItemsController> logger) : Controller
{
    private readonly ApiDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ItemsController> _logger = logger;

    /** 
   <summary>
   Returns all Items
   </summary>
    <remarks>
   Sample request: 
       GET /Items
   </remarks>
   <returns> A list of all Items </returns>
   <response code="200"> Returns All Items</response>
   <response code="204"> If no Items Exist</response>
   */
    [HttpGet]
    [ProducesResponseType<IEnumerable<ItemAPIModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetItems()
    {
        var items = await _context.Items.ToListAsync();
        if (items.Count > 0)
        {
            var apiModels = await Task.Run(() => _mapper
                .Map<IEnumerable<ItemAPIModel>>(items));
            return Ok(apiModels);
        }
        else
        {
            return NoContent();
        }
    }

    /** 
   <summary>
   Gets a specific Item by ID
   </summary>
   <param name="itemId"></param>
   <returns> A Item matching the given ID </returns>
   <remarks>
   Sample request: 
           GET /Items/1
   </remarks>
   <response code="200"> Returns the specified Item</response>
   <response code="404"> If the specified Item does not exist</response>
   */
    [HttpGet("{id}")]
    [ProducesResponseType<ItemAPIModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItem(long itemId)
    {
        var item = await _context.Items.FindAsync(itemId);

        if (item == null)
        {
            return NotFound();
        }

        var apiModel = await Task.Run(() => _mapper
                       .Map<ItemAPIModel>(item));
        return Ok(apiModel);
    }

    /** 
    <summary>
    Updates a specific Item
    </summary>
    <param name="itemId"></param>
    <param name="editModel"></param>
    <returns> The newly updated Item </returns>
    <remarks>
    Sample request: 
            PUT /Items
            {
                "id": 1,
                "title": "Updated Item"
                "tournamentId": 2
                "startTime": "2024-05-23T13:39:43.974Z",
            }
    </remarks>
    <response code="200"> Returns the updated Item</response>
    <response code="400"> If one or more input attributes do not validate</response>
    <response code="404"> If the specified Item does not exist</response>
    <response code="500"> If an unexpected result is produced by the server</response>
    */
    [HttpPut("{id}")]
    [ProducesResponseType<ItemAPIModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutItem(long itemId, ItemAPIModel editModel)
    {
        if (itemId != editModel.Id)
        {
            return BadRequest();
        }

        if (!ItemExists(itemId))
        {
            return NotFound();
        }

        try
        {
            var itemToupdate = await Task.Run(() => _mapper.Map<Item>(editModel));
            var updatedItem = await Task.Run(() => _context.Items.Update(itemToupdate).Entity);
            await _context.SaveChangesAsync();
            var apiModel = await Task.Run(() => _mapper.Map<ItemAPIModel>(updatedItem));
            return Ok(apiModel);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError("{Message}", ex);
            return StatusCode(500);
        }
    }

    /** 
   <summary>
   Creates a new Item
   </summary>
   <param name="createModel"></param>
   <returns> The newly created Item </returns>
   <remarks>
   Sample request: 
           POST /Items {
           {
               "id": 1,
               "title": "New Item"
               "tournamentId": 2
               "startTime": "2024-05-23T13:39:43.974Z",
           }
   </remarks>
   <response code="200"> Returns the newly created Item</response>
   <response code="400"> If one or more input attributes do not validate</response>
   <response code="500"> If an unexpected result is produced by the server</response>
   */
    [HttpPost]
    [ProducesResponseType<ItemAPIModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostItem(ItemAPIModel createModel)
    {
        var itemToCreate = new Item()
        {
            Name = createModel.Name,
            ItemSize = createModel.ItemSize,
        };

        try
        {
            var createdItem = (await _context.Items.AddAsync(itemToCreate)).Entity;
            await _context.SaveChangesAsync();
            var apiModel = await Task.Run(() => _mapper
                  .Map<ItemAPIModel>(createdItem));
            return Ok(apiModel);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError("{Message}", ex);
            return StatusCode(500);
        }
    }

    /** 
    <summary>
    Deletes a Item
    </summary>
    <param name="itemId"></param>
    <returns> The newly deleted Items </returns>
    <remarks>
    Sample request: 
            DELETE /Items/1
    </remarks>
    <response code="200"> Returns the deleted Item</response>
    <response code="404"> If the specified Item does not exist</response>
    <response code="500"> If an unexpected result is produced by the server</response>
    */
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(long itemId)
    {
        var item = await _context.Items.FindAsync(itemId);
        if (item == null)
        {
            return NotFound();
        }

        try
        {
            var deletedItem = _context.Items.Remove(item).Entity;
            await _context.SaveChangesAsync();

            return Ok(deletedItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError("{Message}", ex);
            return StatusCode(500);
        }
    }

    private bool ItemExists(long itemId)
    {
        return _context.Items.Any(e => e.Id == itemId);
    }
}
