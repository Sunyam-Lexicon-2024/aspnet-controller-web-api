using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControllerApi.Web.Models;
using ControllerApi.Web.Models.DTO;

namespace ControllerApi.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController(ApiDbContext context) : ControllerBase
{
    private readonly ApiDbContext _context = context;

    // GET: api/Items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        return await _context.Items.

                             .Select(i => ItemToDTO(i))
                             .ToListAsync();
    }

    // GET: api/Items/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDTO>> GetItem(long id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return ItemToDTO(item);
    }

    // PUT: api/Items/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(long id, Item item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        _context.Entry(item).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Items
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Item>> PostItem(ItemDTO itemDTO)
    {
        var item = new Item()
        {
            Name = itemDTO.Name,
            ItemSize = itemDTO.ItemSize,
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        // return CreatedAtAction("GetItem", new { id = item.Id }, item);
        return CreatedAtAction(
            nameof(PostItem),
            new { id = item.Id },
            ItemToDTO(item));
    }

    // DELETE: api/Items/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(long id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ItemExists(long id)
    {
        return _context.Items.Any(e => e.Id == id);
    }

    private static ItemDTO ItemToDTO(Item i)
    {
        return new ItemDTO()
        {
            Id = i.Id,
            Name = i.Name,
            ItemSize = i.ItemSize
        };
    }
}
