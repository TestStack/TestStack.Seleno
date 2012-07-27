using System.Linq;
using System.Web.Mvc;
using TestStack.Seleno.Samples.Movies.Infrastructure.Data;
using TestStack.Seleno.Samples.Movies.Infrastructure.Data.InMemory;

namespace TestStack.Seleno.Samples.Movies.Controllers
{
    public class MoviesController : Controller
    {
        readonly IMovieRepository _repository;

        public MoviesController(IMovieRepository repository)
        {
            _repository = repository;
        }

        public MoviesController()
        {
            _repository = new MovieRepository();
        }

        public ActionResult Index()
        {
            var list = _repository.GetAll().ToList();
            return View(list);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}