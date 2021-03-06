﻿// Version compatibility level: 3.2.0
namespace BoboBrowse.Net.Facets.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// author nnarkhed
    /// </summary>
    public class PathFacetIterator : FacetIterator
    {
        private BrowseFacet[] _facets;
        private int _index;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="facets">a value ascending sorted list of BrowseFacets</param>
        public PathFacetIterator(IEnumerable<BrowseFacet> facets)
        {
            _facets = facets.ToArray();
            _index = -1;
            facet = null;
            count = 0;
        }

        /// <summary>
        /// (non-Javadoc)
        /// see com.browseengine.bobo.api.FacetIterator#next()
        /// </summary>
        /// <returns></returns>
        public override string Next()
        {
            if ((_index >= 0) && !HasNext())
                throw new IndexOutOfRangeException("No more facets in this iteration");
            _index++;
            facet = _facets[_index].Value;
            count = _facets[_index].FacetValueHitCount;
            return facet;
        }

        /// <summary>
        /// (non-Javadoc)
        /// see java.util.Iterator#hasNext()
        /// </summary>
        /// <returns></returns>
        public override bool HasNext()
        {
            return ((_index >= 0) && (_index < (_facets.Length - 1)));
        }


        /// <summary>
        /// (non-Javadoc)
        /// see java.util.Iterator#remove()
        /// </summary>
        public override void Remove()
        {
            throw new NotSupportedException("remove() method not supported for Facet Iterators");
        }

        /// <summary>
        /// (non-Javadoc)
        /// see com.browseengine.bobo.api.FacetIterator#next(int)
        /// </summary>
        /// <param name="minHits"></param>
        /// <returns></returns>
        public override string Next(int minHits)
        {
            while (++_index < _facets.Length)
            {
                if (_facets[_index].FacetValueHitCount >= minHits)
                {
                    facet = _facets[_index].Value;
                    count = _facets[_index].FacetValueHitCount;
                    return facet;
                }
            }
            facet = null;
            count = 0;
            return facet; 
        }

        /// <summary>
        /// The string from here should be already formatted. No need to reformat.
        /// see com.browseengine.bobo.api.FacetIterator#format(java.lang.Object)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override string Format(object val)
        {
            return Convert.ToString(val);
        }
    }
}
