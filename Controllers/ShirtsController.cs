﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Drawing;
using WebAPIDemo.Filter;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Controllers
{

    [ApiController]
    [Route("api/shirts")]
    public class ShirtsController : ControllerBase
    {
       
        [HttpGet]
        public IActionResult GetShirts()
        {
            return Ok(ShirtRepository.GetShirts());
        }

        [HttpGet("{id}")]
        [Shirt_ValidateShirtIdFilter]
        public IActionResult GetShirtById(int id)
        {
            return Ok(ShirtRepository.GetShirtById(id));
        }

        [HttpPost]
        [Shirt_ValidateCreateShirtIdFilter]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            ShirtRepository.AddShirt(shirt);

            return CreatedAtAction(nameof(GetShirtById),
                new { id = shirt.ShirtId }, shirt);
        }

        [HttpPut ("{id}")]
        [Shirt_ValidateShirtIdFilter]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            if (id != shirt.ShirtId)
            {
                return BadRequest();

            }
            try
            {
                ShirtRepository.UpdateShirt(shirt);
            }
            catch
            {
                if(!ShirtRepository.ShirtExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete ("{id}")]
        [Shirt_ValidateShirtIdFilter]
        public IActionResult DeleteShirt(int id)
        {
            var shirt = ShirtRepository.GetShirtById (id);
            ShirtRepository.DeleteShirt(id);

            return Ok(shirt); 
        }
    }
}
