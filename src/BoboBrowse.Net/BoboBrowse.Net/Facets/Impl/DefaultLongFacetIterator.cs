﻿// Version compatibility level: 3.2.0
namespace BoboBrowse.Net.Facets.Impl
{
    using BoboBrowse.Net.Facets.Data;
    using BoboBrowse.Net.Util;
    using System;

    /// <summary>
    /// author "Xiaoyang Gu &lt;xgu@linkedin.com&gt;"
    /// </summary>
    public class DefaultLongFacetIterator : LongFacetIterator
    {
        private readonly TermLongList _valList;
        private BigSegmentedArray _count;
        private int _countlength;
        private int _countLengthMinusOne;
        private int _index;

        public DefaultLongFacetIterator(TermLongList valList, BigSegmentedArray countarray, int countlength, bool zeroBased)
        {
            _valList = valList;
            _countlength = countlength;
            _count = countarray;
            _countLengthMinusOne = _countlength - 1;
            _index = -1;
            if (!zeroBased)
                _index++;
            _facet = TermLongList.VALUE_MISSING;
            count = 0;
        }

        /// <summary>
        /// Added in .NET version as as an accessor to the _valList field.
        /// </summary>
        public virtual TermLongList ValList
        {
            get { return _valList; }
        }

        /// <summary>
        /// (non-Javadoc)
        /// see com.browseengine.bobo.api.FacetIterator#getFacet()
        /// </summary>
        new public virtual string Facet
        {
            get
            {
                if (_facet == TermLongList.VALUE_MISSING) return null;
                return _valList.Format(_facet);
            }
        }

        public override string Format(long val)
        {
            return _valList.Format(val);
        }

        public override string Format(object val)
        {
            return _valList.Format(val);
        }

        /// <summary>
        /// (non-Javadoc)
        /// see com.browseengine.bobo.api.FacetIterator#getFacetCount()
        /// </summary>
        public virtual int FacetCount
        {
            get { return count; }
        }

        /// <summary>
        /// (non-Javadoc)
        /// see java.util.Iterator#hasNext()
        /// </summary>
        /// <returns></returns>
        public override bool HasNext()
        {
            return (_index < _countLengthMinusOne);
        }

        /// <summary>
        /// (non-Javadoc)
        /// see java.util.Iterator#next()
        /// </summary>
        /// <returns></returns>
        public override string Next()
        {
            if ((_index >= 0) && (_index >= _countLengthMinusOne))
                throw new IndexOutOfRangeException("No more facets in this iteration");
            _index++;
            _facet = _valList.GetPrimitiveValue(_index);
            count = _count.Get(_index);
            return _valList.Get(_index);
        }

        /// <summary>
        /// (non-Javadoc)
        /// see com.browseengine.bobo.api.LongFacetIterator#nextLong()
        /// </summary>
        /// <returns></returns>
        public override long NextLong()
        {
            if (_index >= _countLengthMinusOne)
                throw new IndexOutOfRangeException("No more facets in this iteration");
            _index++;
            _facet = _valList.GetPrimitiveValue(_index);
            count = _count.Get(_index);
            return _facet;
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
            while (++_index < _countlength)
            {
                if (_count.Get(_index) >= minHits)
                {
                    _facet = _valList.GetPrimitiveValue(_index);
                    count = _count.Get(_index);
                    return _valList.Format(_facet);
                }
            }
            _facet = TermLongList.VALUE_MISSING;
            count = 0;
            return null;
        }

        /// <summary>
        /// (non-Javadoc)
        /// see com.browseengine.bobo.api.LongFacetIterator#nextLong(int)
        /// </summary>
        /// <param name="minHits"></param>
        /// <returns></returns>
        public override long NextLong(int minHits)
        {
            while (++_index < _countlength)
            {
                if (_count.Get(_index) >= minHits)
                {
                    _facet = _valList.GetPrimitiveValue(_index);
                    count = _count.Get(_index);
                    return _facet;
                }
            }
            _facet = TermLongList.VALUE_MISSING;
            count = 0;
            return _facet;
        }
    }
}
