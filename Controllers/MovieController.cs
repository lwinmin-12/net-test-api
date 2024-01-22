using netCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace netTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MovieController : ControllerBase{
        private readonly MovieContext _dbContext;

        public MovieController(MovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET : api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies () {

            if(_dbContext.Movies == null) {
                return NotFound();
            }

            return await _dbContext.Movies.ToListAsync();
        }
        
        //GEt : api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie (int id) {


            if(_dbContext.Movies == null) {
                return NotFound();
            }

            var movie = await _dbContext.Movies.FindAsync(id);

            if(movie == null){
                return NotFound();
            }
            return movie;
    }
    
    //POST  : api/Movies
    [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie (Movie movie) {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovie) , new {id  = movie.Id} , movie);
        }

    //PUT : api/Movies/1
    [HttpPut("{id}")]
    public async Task<IActionResult> putMovie(int id , Movie movie)
    {

        if (id != movie.Id){
            return BadRequest();
        }

        _dbContext.Entry(movie).State = EntityState.Modified;

        try{
            await _dbContext.SaveChangesAsync();
        }catch (DbUpdateConcurrencyException) {
            if(!MovieExists(id)){
                return NotFound();
            }else{
                throw;
            }
        }
        return NoContent();
    }

    //DELETE : api/Movies/1

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteMovie (int id) {
        if(_dbContext.Movies == null){
            return NoContent();
        }
        var movie = await _dbContext.Movies.FindAsync(id);
        if(movie == null) {
            return NoContent();
        }
        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    private bool MovieExists (long id) {
        return (_dbContext.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    }
}