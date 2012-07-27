using System.Linq;
using System.Web.Mvc;
using MvcMovie.Infrastructure.Data;
using MvcMovie.Infrastructure.Data.InMemory;

namespace MvcMovie.Controllers
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