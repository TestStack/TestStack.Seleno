using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestStack.Seleno.Samples.Movies.Bootstrap;
using TestStack.Seleno.Samples.Movies.Core.Domain;
using TestStack.Seleno.Samples.Movies.Core.Services;
using TestStack.Seleno.Samples.Movies.Core.Services.InMemoryDataProvider;
using TestStack.Seleno.Samples.Movies.ViewModels;

namespace TestStack.Seleno.Samples.Movies.Controllers
{
    public class MoviesController : Controller
    {
        readonly IRepository<Movie> _repository;

        public MoviesController(IRepository<Movie> repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var domain = _repository.GetAll().OrderBy(x => x.Title).ToList();
            var list = AutoMapperBootstrapper.map.Map<IEnumerable<Movie>, IEnumerable<MovieListViewModel>>(domain);
            return View(list);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}