using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SharedGroceriesWebService.Models;
using SharedGroceriesWebService.Services;

namespace SharedGroceriesWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingListsController : ControllerBase
    {
        [HttpGet("{userId:guid}")]
        public ActionResult<List<ShoppingList>> GetShoppingListsForUser(Guid? userId)
            => DataService.GetShoppingListsForUser(userId);

        [HttpDelete("{listId:guid}")]
        public IActionResult DeleteShoppingList(Guid? listId)
        {
            DataService.DeleteShoppingList(listId);
            return NoContent();
        }

        [HttpPut]
        public IActionResult AddOrUpdateShoppingList([FromBody] ShoppingList list)
        {
            DataService.AddOrUpdateShoppingList(list);
            return NoContent();
        }
    }
}
