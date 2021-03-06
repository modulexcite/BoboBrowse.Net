﻿//* Bobo Browse Engine - High performance faceted/parametric search implementation 
//* that handles various types of semi-structured data.  Written in Java.
//* 
//* Copyright (C) 2005-2006  John Wang
//*
//* This library is free software; you can redistribute it and/or
//* modify it under the terms of the GNU Lesser General Public
//* License as published by the Free Software Foundation; either
//* version 2.1 of the License, or (at your option) any later version.
//*
//* This library is distributed in the hope that it will be useful,
//* but WITHOUT ANY WARRANTY; without even the implied warranty of
//* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//* Lesser General Public License for more details.
//*
//* You should have received a copy of the GNU Lesser General Public
//* License along with this library; if not, write to the Free Software
//* Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//* 
//* To contact the project administrators for the bobo-browse project, 
//* please go to https://sourceforge.net/projects/bobo-browse/, or 
//* send mail to owner@browseengine.com. 

// Version compatibility level: 3.2.0
namespace BoboBrowse.Net
{
    using BoboBrowse.Net.Facets;
    using BoboBrowse.Net.Sort;
    using Lucene.Net.Search;
    using System.Collections.Generic;

    public interface IBrowsable : Searchable
    {
        /// <summary>
        /// Generates a merged BrowseResult from the supplied <see cref="T:BrowseRequest"/>.
        /// The results are put into a Lucene.Net <see cref="T:Lucene.Net.Search.Collector"/> and a <see cref="T:System.Collections.Generic.IDictionary{System.String, IFacetAccessible}"/>.
        /// </summary>
        /// <param name="req"><see cref="T:BrowseRequest"/> for generating the facets.</param>
        /// <param name="hitCollector">A <see cref="T:Lucene.Net.Search.Collector"/> for the hits generated during a search.</param>
        /// <param name="facets">A dictionary of all of the facet collections (output).</param>
        void Browse(BrowseRequest req, 
	        Collector hitCollector,
	        IDictionary<string, IFacetAccessible> facets);

        /// <summary>
        /// Generates a merged BrowseResult from the supplied <see cref="T:BrowseRequest"/>.
        /// The results are put into a Lucene.Net <see cref="T:Lucene.Net.Search.Collector"/> and a <see cref="T:System.Collections.Generic.IDictionary{System.String, IFacetAccessible}"/>.
        /// </summary>
        /// <param name="req"><see cref="T:BrowseRequest"/> for generating the facets.</param>
        /// <param name="hitCollector">A <see cref="T:Lucene.Net.Search.Collector"/> for the hits generated during a search.</param>
        /// <param name="facets">A dictionary of all of the facet collections (output).</param>
        /// <param name="start">The offset value for the document number.</param>
        void Browse(BrowseRequest req, 
	        Collector hitCollector,
	        IDictionary<string, IFacetAccessible> facets,
	        int start);

        /// <summary>
        /// Generates a merged BrowseResult from the supplied <see cref="T:BrowseRequest"/> and a <see cref="T:Lucene.Net.Search.Weight"/>.
        /// The results are put into a Lucene.Net <see cref="T:Lucene.Net.Search.Collector"/> and a <see cref="T:System.Collections.Generic.IDictionary{System.String, IFacetAccessible}"/>.
        /// </summary>
        /// <param name="req"><see cref="T:BrowseRequest"/> for generating the facets.</param>
        /// <param name="weight">A <see cref="T:Lucene.Net.Search.Weight"/> instance to alter the score of the queries in a multiple index scenario.</param>
        /// <param name="hitCollector">A <see cref="T:Lucene.Net.Search.Collector"/> for the hits generated during a search.</param>
        /// <param name="facets">A dictionary of all of the facet collections (output).</param>
        /// <param name="start">The offset value for the document number.</param>
        void Browse(BrowseRequest req, 
	        Weight weight,
	        Collector hitCollector,
	        IDictionary<string, IFacetAccessible> facets,
	        int start);

        /// <summary>
        /// Generates a merged BrowseResult from the supplied <see cref="T:BrowseRequest"/>.
        /// </summary>
        /// <param name="req"><see cref="T:BrowseRequest"/> for generating the facets.</param>
        /// <returns><see cref="T:BrowseResult"/> of the results corresponding to the <see cref="T:BrowseRequest"/>.</returns>
        BrowseResult Browse(BrowseRequest req);

        /// <summary>
        /// Gets a set of facet names.
        /// </summary>
        /// <returns>set of facet names</returns>
        IEnumerable<string> FacetNames { get; }

        /// <summary>
        /// Sets a facet handler for each sub-browser instance.
        /// </summary>
        /// <param name="facetHandler">A facet handler.</param>
        void SetFacetHandler(IFacetHandler facetHandler);

        /// <summary>
        /// Gets a facet handler by facet name.
        /// </summary>
        /// <param name="name">The facet name.</param>
        /// <returns>The facet handler instance.</returns>
        IFacetHandler GetFacetHandler(string name);

        Similarity Similarity { get; set; }

        /// <summary>
        /// Return the string representation of the values of a field for the given doc.
        /// </summary>
        /// <param name="docid">The document id.</param>
        /// <param name="fieldname">The field name.</param>
        /// <returns>A string array of field values.</returns>
        string[] GetFieldVal(int docid, string fieldname);

        /// <summary>
        /// Return the raw (primitive) field values for the given doc.
        /// </summary>
        /// <param name="docid">The document id.</param>
        /// <param name="fieldname">The field name.</param>
        /// <returns>An object array of raw field values.</returns>
        object[] GetRawFieldVal(int docid, string fieldname);

        /// <summary>
        /// Gets the total number of documents in all sub browser instances.
        /// </summary>
        /// <returns>The total number of documents.</returns>
        int NumDocs();

        SortCollector GetSortCollector(SortField[] sort, Lucene.Net.Search.Query q, int offset, int count, bool fetchStoredFields, IEnumerable<string> termVectorsToFetch, bool forceScoring, string[] groupBy, int maxPerGroup, bool collectDocIdCache);

        Explanation Explain(Lucene.Net.Search.Query q, int docid);

        IDictionary<string, IFacetHandler> FacetHandlerMap { get; }
    }
}
