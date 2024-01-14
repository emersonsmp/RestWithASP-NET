using RestWithASPNET.Hypermedia.Abstract;

namespace RestWithASPNET.Hypermedia.Filters
{
    public class HyperMediaFiltersOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
