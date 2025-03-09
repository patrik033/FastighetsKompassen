using Microsoft.AspNetCore.Routing;

namespace FastighetsKompassen.API.HATEOAS
{


    public class HateoasService : IHateoasService
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContext _httpContext;

        public HateoasService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext är inte tillgängligt.");
        }

        public Resource<T> Wrap<T>(T data, IEnumerable<Link> links)
        {
            return new Resource<T>(data, links.ToList());
        }

        public Link CreateLink(string routeName, object? values, string rel, string method)
        {
            var uri = _linkGenerator.GetUriByName(_httpContext, routeName, values);
            if (uri == null)
            {
                throw new InvalidOperationException($"Kan inte generera länk för {routeName}.");
            }

            return new Link(uri, rel, method);
        }

        public Resource<T> WrapLinksAndData<T>(T resultData, string routeName, object? queryParameters, string rel, string method)
        {
            if (resultData == null)
            {
                throw new ArgumentNullException(nameof(resultData), "Result data cannot be null.");
            }
            var links = ReturnLink(routeName, queryParameters, rel, method);
            return Wrap(resultData, links);
        }

        private  List<Link> ReturnLink(string routeName, object? queryParameters,string rel,string method)
        {
            var links = new List<Link>
            {
                CreateLink(routeName,queryParameters,rel,method)
            };
            return links;

        }
    }

}
