﻿// Version compatibility level: 3.2.0
namespace BoboBrowse.Net.Facets.Impl
{
    using BoboBrowse.Net.Facets.Data;
    using BoboBrowse.Net.Facets.Filter;
    using BoboBrowse.Net.Facets.Range;
    using BoboBrowse.Net.Support;
    using System.Collections.Generic;

    public class MultiValueWithWeightFacetHandler : MultiValueFacetHandler
    {
        public MultiValueWithWeightFacetHandler(string name, string indexFieldName, TermListFactory termListFactory)
            : base(name, indexFieldName, termListFactory, null, null)
        {
        }

        public MultiValueWithWeightFacetHandler(string name, string indexFieldName)
            : base(name, indexFieldName, null, null, null)
        {
        }

        public MultiValueWithWeightFacetHandler(string name)
            : base(name, name, null, null, null)
        {
        }

        public override RandomAccessFilter BuildRandomAccessFilter(string value, IDictionary<string, string> prop)
        {
            MultiValueFacetFilter f = new MultiValueFacetFilter(new MultiDataCacheBuilder(Name, _indexFieldName), value);
            return f;
        }

        public override MultiValueFacetDataCache Load(BoboIndexReader reader, BoboIndexReader.WorkArea workArea)
        {
            MultiValueWithWeightFacetDataCache dataCache = new MultiValueWithWeightFacetDataCache();

            dataCache.MaxItems = _maxItems;

            if (_sizePayloadTerm == null)
            {
                dataCache.Load(_indexFieldName, reader, _termListFactory, workArea);
            }
            else
            {
                dataCache.Load(_indexFieldName, reader, _termListFactory, _sizePayloadTerm);
            }
            return dataCache;
        }
    }
}
