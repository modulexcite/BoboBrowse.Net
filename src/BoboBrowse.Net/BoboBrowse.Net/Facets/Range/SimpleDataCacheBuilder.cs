﻿// Version compatibility level: 3.2.0
namespace BoboBrowse.Net.Facets.Range
{
    using BoboBrowse.Net.Facets.Data;
    using BoboBrowse.Net.Facets.Filter;
    using System;

    public class SimpleDataCacheBuilder : IFacetDataCacheBuilder
    {
        private string name;
        private string indexFieldName;

        public SimpleDataCacheBuilder(string name, string indexFieldName)
        {
            this.name = name;
            this.indexFieldName = indexFieldName;
        }

        public virtual FacetDataCache Build(BoboIndexReader reader)
        {
            return (FacetDataCache)reader.GetFacetData(name);
        }

        public virtual string Name
        {
            get { return name; }
        }

        public virtual string IndexFieldName
        {
            get { return indexFieldName; }
        }
    }
}
