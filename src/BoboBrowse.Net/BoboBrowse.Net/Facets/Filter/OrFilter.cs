// Version compatibility level: 3.2.0
namespace BoboBrowse.Net.Facets.Filter
{
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using LuceneExt.Impl;
    using System.Collections.Generic;
    using System.Linq;

    public class OrFilter : Filter
    {
        //private static long serialVersionUID = 1L; // NOT USED

        private readonly IEnumerable<Filter> _filters;

        public OrFilter(IEnumerable<Filter> filters)
        {
            _filters = filters;
        }

        public override DocIdSet GetDocIdSet(IndexReader reader)
        {
            var count = _filters.Count();
            if (count == 1)
            {
                return _filters.ElementAt(0).GetDocIdSet(reader);
            }
            else
            {
                List<DocIdSet> list = new List<DocIdSet>(count);
                foreach (Filter f in _filters)
                {
                    list.Add(f.GetDocIdSet(reader));
                }
                return new OrDocIdSet(list);
            }
        }
    }
}