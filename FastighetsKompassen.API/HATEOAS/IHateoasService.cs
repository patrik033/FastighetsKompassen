namespace FastighetsKompassen.API.HATEOAS
{
    public interface IHateoasService
    {
        Resource<T> Wrap<T>(T data, IEnumerable<Link> links);
        Resource<T> WrapLinksAndData<T>(T resultData, string routeName, object? queryParameters, string rel, string method);
        Link CreateLink(string routeName, object? values, string rel, string method);
    }
}
