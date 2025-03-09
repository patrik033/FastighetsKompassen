namespace FastighetsKompassen.API.HATEOAS
{
    public class Resource<T>
    {
        public T Data { get;}
        public List<Link> Links { get; }

        public Resource(T data,List<Link> links)
        {
            Data = data;
            Links = links ?? new List<Link>();
        }
    }
}
