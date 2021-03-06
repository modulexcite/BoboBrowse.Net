﻿/**
 * Bobo Browse Engine - High performance faceted/parametric search implementation 
 * that handles various types of semi-structured data.  Written in Java.
 * 
 * Copyright (C) 2005-2006  John Wang
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 * 
 * To contact the project administrators for the bobo-browse project, 
 * please go to https://sourceforge.net/projects/bobo-browse/, or 
 * send mail to owner@browseengine.com.
 */

// Version compatibility level: 3.2.0
namespace BoboBrowse.Tests
{
    using BoboBrowse.Net;
    using BoboBrowse.Net.Facets;
    using BoboBrowse.Net.Facets.Impl;
    using BoboBrowse.Net.Index;
    using BoboBrowse.Net.Index.Digest;
    using Common.Logging;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Store;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class is to test the case when facetName is different from the underlying indexingFieldName for simpleFacetHandler
    /// 
    /// author hyan
    /// </summary>
    [TestFixture]
    public class FacetNameTest
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(FacetNameTest));
        private IEnumerable<IFacetHandler> _facetHandlers;
        private int _documentSize;

        private class TestDataDigester : DataDigester
        {
            private IEnumerable<IFacetHandler> _facetHandlers;
            private Document[] _data;

            public TestDataDigester(IEnumerable<IFacetHandler> facetHandlers, Document[] data)
            {
                _facetHandlers = facetHandlers;
                _data = data;
            }

            public override void Digest(DataDigester.IDataHandler handler)
            {
                for (int i = 0; i < _data.Length; ++i)
                {
                    handler.HandleDocument(_data[i]);
                }
            }
        }

        [SetUp]
        public void Init()
        {
            _facetHandlers = CreateFacetHandlers();
            _documentSize = 10;
        }

        [TearDown]
        public void Dispose()
        {
            _facetHandlers = null;
            _documentSize = 0;
        }

        public Document[] CreateData()
        {
            var dataList = new List<Document>();
            for (int i = 0; i < _documentSize; ++i)
            {
                String color = null;
                if (i == 0) color = "red";
                else if (i == 1) color = "green";
                else if (i == 2) color = "blue";
                else if (i % 2 == 0) color = "yellow";
                else color = "white";

                String make = null;
                if (i == 0) make = "camry";
                else if (i == 1) make = "accord";
                else if (i == 2) make = "4runner";
                else if (i % 2 == 0) make = "rav4";
                else make = "prius";

                String ID = i.ToString();
                Document d = new Document();
                d.Add(new Field("id", ID, Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
                d.Add(new Field("color", color, Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
                d.Add(new Field("make", make, Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
                dataList.Add(d);
            }
            return dataList.ToArray();
        }

        private Directory CreateIndex()
        {
            Directory dir = new RAMDirectory();

            Document[] data = CreateData();

            TestDataDigester testDigester = new TestDataDigester(_facetHandlers, data);
            BoboIndexer indexer = new BoboIndexer(testDigester, dir);
            indexer.Index();
            using (var r = IndexReader.Open(dir, false))
            {
            }

            return dir;
        }

        public static IEnumerable<IFacetHandler> CreateFacetHandlers()
        {
            var facetHandlers = new List<IFacetHandler>();
            facetHandlers.Add(new SimpleFacetHandler("id"));
            facetHandlers.Add(new SimpleFacetHandler("make"));
            facetHandlers.Add(new SimpleFacetHandler("mycolor", "color"));

            return facetHandlers;
        }

        [Test]
        public void TestFacetNameForSimpleFacetHandler()
        {
            BrowseRequest br = new BrowseRequest();
            br.Count = 20;
            br.Offset = 0;

            BrowseSelection colorSel = new BrowseSelection("mycolor");
            colorSel.AddValue("yellow");
            br.AddSelection(colorSel);

            BrowseSelection makeSel = new BrowseSelection("make");
            makeSel.AddValue("rav4");
            br.AddSelection(makeSel);

            FacetSpec spec = new FacetSpec();
            spec.ExpandSelection = true;
            spec.OrderBy = FacetSpec.FacetSortSpec.OrderHitsDesc;
            spec.MaxCount = 15;

            br.SetFacetSpec("mycolor", spec);
            br.SetFacetSpec("id", spec);
            br.SetFacetSpec("make", spec);

            int expectedHitNum = 3;

            Directory ramIndexDir = CreateIndex();
            using (IndexReader srcReader = IndexReader.Open(ramIndexDir, true))
            {
                using (BoboIndexReader boboReader = BoboIndexReader.GetInstance(srcReader, _facetHandlers, null))
                {
                    using (BoboBrowser boboBrowser = new BoboBrowser(boboReader))
                    {
                        using (BrowseResult result = boboBrowser.Browse(br))
                        {

                            Assert.AreEqual(expectedHitNum, result.NumHits);
                        }
                    }
                }
            }
        }
    }
}
