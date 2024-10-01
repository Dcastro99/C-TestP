using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        //---------GET ALL CATEGORIES----------//

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        //---------GET CATEGORY BY ID----------//

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]

        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        //---------GET POKEMON BY CATEGORY----------//

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonByCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(
                _categoryRepository.GetPokemonByCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        //---------CREATE CATEGORY----------//

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryToCreate)
        {
            if (categoryToCreate == null)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", $"Category {categoryToCreate.Name} already exists");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryToCreate);

            if(!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", $"Something went wrong saving {categoryMap.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully Created");
        }


    }
}
