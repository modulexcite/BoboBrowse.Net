﻿// Version compatibility level: 3.2.0
namespace BoboBrowse.Net.Sort
{
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using System;
    using System.Globalization;

    public abstract class DocComparatorSource
    {
        public virtual bool IsReverse { get; set; }

        public abstract DocComparator GetComparator(IndexReader reader, int docbase);

        public class IntDocComparatorSource : DocComparatorSource
        {
            private readonly string field;

            public IntDocComparatorSource(string field)
            {
                this.field = field;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                int[] values = FieldCache_Fields.DEFAULT.GetInts(reader, this.field);
                return new IntDocComparator(values);
            }

            private class IntDocComparator : DocComparator
            {
                private readonly int[] values;

                public IntDocComparator(int[] values)
                {
                    this.values = values;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    // cannot return v1-v2 because it could overflow
                    if (values[doc1.Doc] < values[doc2.Doc])
                    {
                        return -1;
                    }
                    else if (values[doc1.Doc] > values[doc2.Doc])
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values[doc.Doc];
                }
            }
        }

        public class StringLocaleComparatorSource : DocComparatorSource
        {
            private readonly string field;
            private readonly CultureInfo _cultureInfo;

            public StringLocaleComparatorSource(string field, CultureInfo cultureInfo)
            {
                this.field = field;
                _cultureInfo = cultureInfo;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                string[] values = FieldCache_Fields.DEFAULT.GetStrings(reader, this.field);
                return new StringLocaleDocComparator(values, _cultureInfo);
            }

            private class StringLocaleDocComparator : DocComparator
            {
                private readonly string[] values;
                private readonly CultureInfo cultureInfo;

                public StringLocaleDocComparator(string[] values, CultureInfo cultureInfo)
                {
                    this.values = values;
                    this.cultureInfo = cultureInfo;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    if (values[doc1.Doc] == null)
                    {
                        if (values[doc2.Doc] == null)
                        {
                            return 0;
                        }
                        return -1;
                    }
                    else if (values[doc2.Doc] == null)
                    {
                        return 1;
                    }
                    return string.Compare(values[doc1.Doc], values[doc2.Doc], false, this.cultureInfo);
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values[doc.Doc];
                }
            }
        }

        public class StringValComparatorSource : DocComparatorSource
        {
            private readonly string field;

            public StringValComparatorSource(string field)
            {
                this.field = field;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                string[] values = FieldCache_Fields.DEFAULT.GetStrings(reader, this.field);
                return new StringValDocComparator(values);
            }

            private class StringValDocComparator : DocComparator
            {
                private readonly string[] values;

                public StringValDocComparator(string[] values)
                {
                    this.values = values;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    if (values[doc1.Doc] == null)
                    {
                        if (values[doc2.Doc] == null)
                        {
                            return 0;
                        }
                        return -1;
                    }
                    else if (values[doc2.Doc] == null)
                    {
                        return 1;
                    }
                    return string.CompareOrdinal(values[doc1.Doc], values[doc2.Doc]);
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values[doc.Doc];
                }
            }
        }

        public class StringOrdComparatorSource : DocComparatorSource
        {
            private readonly string field;

            public StringOrdComparatorSource(string field)
            {
                this.field = field;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                StringIndex values = FieldCache_Fields.DEFAULT.GetStringIndex(reader, this.field);
                return new StringOrdDocComparator(values);
            }

            private class StringOrdDocComparator : DocComparator
            {
                private readonly StringIndex values;

                public StringOrdDocComparator(StringIndex values)
                {
                    this.values = values;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    return values.order[doc1.Doc] - values.order[doc2.Doc];
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values.lookup[values.order[doc.Doc]];
                }
            }
        }

        public class ShortDocComparatorSource : DocComparatorSource
        {
            private readonly string field;

            public ShortDocComparatorSource(string field)
            {
                this.field = field;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                short[] values = FieldCache_Fields.DEFAULT.GetShorts(reader, this.field);
                return new ShortDocComparator(values);
            }

            private class ShortDocComparator : DocComparator
            {
                private readonly short[] values;

                public ShortDocComparator(short[] values)
                {
                    this.values = values;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    return values[doc1.Doc] - values[doc2.Doc];
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values[doc.Doc];
                }
            }
        }

        public class LongDocComparatorSource : DocComparatorSource
        {
            private readonly string field;

            public LongDocComparatorSource(string field)
            {
                this.field = field;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                long[] values = FieldCache_Fields.DEFAULT.GetLongs(reader, this.field);
                return new LongDocComparator(values);
            }

            private class LongDocComparator : DocComparator
            {
                private readonly long[] values;

                public LongDocComparator(long[] values)
                {
                    this.values = values;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    // cannot return v1-v2 because it could overflow
                    if (values[doc1.Doc] < values[doc2.Doc])
                    {
                        return -1;
                    }
                    else if (values[doc1.Doc] > values[doc2.Doc])
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values[doc.Doc];
                }
            }
        }

        public class FloatDocComparatorSource : DocComparatorSource
        {
            private readonly string field;

            public FloatDocComparatorSource(string field)
            {
                this.field = field;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                float[] values = FieldCache_Fields.DEFAULT.GetFloats(reader, this.field);
                return new FloatDocComparator(values);
            }

            private class FloatDocComparator : DocComparator
            {
                private readonly float[] values;

                public FloatDocComparator(float[] values)
                {
                    this.values = values;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    // cannot return v1-v2 because it could overflow
                    if (values[doc1.Doc] < values[doc2.Doc])
                    {
                        return -1;
                    }
                    else if (values[doc1.Doc] > values[doc2.Doc])
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values[doc.Doc];
                }
            }
        }

        public class DoubleDocComparatorSource : DocComparatorSource
        {
            private readonly string field;

            public DoubleDocComparatorSource(string field)
            {
                this.field = field;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                double[] values = FieldCache_Fields.DEFAULT.GetDoubles(reader, this.field);
                return new DoubleDocComparator(values);
            }

            private class DoubleDocComparator : DocComparator
            {
                private readonly double[] values;

                public DoubleDocComparator(double[] values)
                {
                    this.values = values;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    // cannot return v1-v2 because it could overflow
                    if (values[doc1.Doc] < values[doc2.Doc])
                    {
                        return -1;
                    }
                    else if (values[doc1.Doc] > values[doc2.Doc])
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values[doc.Doc];
                }
            }
        }

        public class RelevanceDocComparatorSource : DocComparatorSource
        {
            public RelevanceDocComparatorSource()
            {
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                return new RelevanceDocComparator();
            }

            private class RelevanceDocComparator : DocComparator
            {
                public RelevanceDocComparator()
                {
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    // cannot return v1-v2 because it could overflow
                    if (doc1.Score < doc2.Score)
                    {
                        return -1;
                    }
                    else if (doc1.Score > doc2.Score)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)doc.Score;
                }
            }
        }

        public class DocIdDocComparatorSource : DocComparatorSource
        {
            public DocIdDocComparatorSource()
            {
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                return new DocIdDocComparator(docbase);
            }

            private class DocIdDocComparator : DocComparator
            {
                private readonly int docbase;

                public DocIdDocComparator(int docbase)
                {
                    this.docbase = docbase;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    return doc1.Doc - doc2.Doc;
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)(doc.Doc + this.docbase);
                }
            }
        }

        public class ByteDocComparatorSource : DocComparatorSource
        {
            private readonly string field;

            public ByteDocComparatorSource(string field)
            {
                this.field = field;
            }

            public override DocComparator GetComparator(IndexReader reader, int docbase)
            {
                sbyte[] values = FieldCache_Fields.DEFAULT.GetBytes(reader, this.field);
                return new ByteDocComparator(values);
            }

            private class ByteDocComparator : DocComparator
            {
                private readonly sbyte[] values;

                public ByteDocComparator(sbyte[] values)
                {
                    this.values = values;
                }

                public override int Compare(ScoreDoc doc1, ScoreDoc doc2)
                {
                    return values[doc1.Doc] - values[doc2.Doc];
                }

                public override IComparable Value(ScoreDoc doc)
                {
                    return (IComparable)values[doc.Doc];
                }
            }
        }
    }
}
