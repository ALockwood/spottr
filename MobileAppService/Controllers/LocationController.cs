using System;
using Microsoft.AspNetCore.Mvc;
using spottr.Models;
using spottr.PostModels;
using System.Threading.Tasks;

namespace spottr.Controllers
{
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepo;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepo = locationRepository;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetLocation(Int32 id)
        {
            ILocation location = await _locationRepo.GetAsync(id);
            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody]PostedLocation location)
        {
            Int32 key;

            try
            {
                if (location == null || !ModelState.IsValid)
                {
                    return BadRequest("Location body supplied is invalid!");
                }

                if (string.IsNullOrWhiteSpace(location.Name) || string.IsNullOrWhiteSpace(location.Description))
                {
                    return BadRequest("One or more required paramaters were empty.");
                }

                key =  await _locationRepo.Add(location);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return BadRequest("Error while creating location!");
            }

            return Ok(key);
        }

        [HttpGet]
        public IActionResult GetLatestLocations()
        {
            return Ok(_locationRepo.GetLatestLocations(25));
        }

    }
}
/*
namespace spottr.Controllers
{
    [Route("api/[controller]")]
    public class ItemController : Controller
    {

        private readonly IItemRepository ItemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            ItemRepository = itemRepository;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(ItemRepository.GetAll());
        }

        [HttpGet("{Id}")]
        public Item GetItem(string id)
        {
            Item item = ItemRepository.Get(id);
            return item;
        }

        [HttpPost]
        public IActionResult Create([FromBody]Item item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid State");
                }

                ItemRepository.Add(item);

            }
            catch (Exception)
            {
                return BadRequest("Error while creating");
            }
            return Ok(item);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] Item item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid State");
                }
                ItemRepository.Update(item);
            }
            catch (Exception)
            {
                return BadRequest("Error while creating");
            }
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public void Delete(string id)
        {
            ItemRepository.Remove(id);
        }
    }
}
*/