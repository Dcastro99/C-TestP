using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository,ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        //---------GET ALL OWNERS----------//

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]

        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        //---------GET OWNER BY ID----------//

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]

        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }

        //---------GET POKEMON BY OWNER----------//

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var owner = _mapper.Map<List<PokemonDto>>(
                _ownerRepository.GetPokemonsByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            var category = _ownerRepository.GetOwners()
                .Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", $"Category {ownerCreate.LastName} already exists");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Console.WriteLine($"countryId>>>>>>>>>>>>>>>>>: {countryId}");

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", $"Something went wrong saving {ownerMap.LastName}");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully Created");
        }



    }
}
