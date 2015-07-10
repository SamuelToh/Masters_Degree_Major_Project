using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Sequences.Annotations;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSequence
{
    [TestClass]
    public class TestFeature
    {
        [TestMethod]
        public void TestConstructor()
        {
            Feature feature = new Feature("test", 1, 2, +1);
			Assert.AreEqual("test", feature.Name);

            Sequence seq = new Sequence(AlphabetType.DNA, "acgta");
			seq.Annotations.Add( new Annotation("Name", "test"));
            feature = new Feature(seq, 2, 4, +1);

            Assert.AreEqual("test", feature.Name);
            Assert.AreEqual("cgt", feature.Letters());
        }

        [TestMethod]
        public void TestGetSequence()
        {
            FeatureList featureList = new FeatureList("test");
			Sequence seq = new Sequence( AlphabetType.DNA, "acgta" );
			seq.AddFeatures( featureList );
            Feature feature = new Feature("feature", 2, 4, +1);
            featureList.Add(feature);
            Assert.AreEqual("acgta", feature.BaseSequence.Letters());
        }

        [TestMethod]
        public void TestGetFeatureSequence()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "acgta");
            FeatureList featureList = new FeatureList("test");
            Feature feature = new Feature("feature1", 2, 4, +1);
            featureList.Add(feature);
            seq.AddFeatures(featureList);
            Assert.AreEqual("cgt", feature.Letters());
        }

        [TestMethod]
        public void TestDistanceStartStart()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "acgtactg");
            Feature feature1 = new Feature("feature1", 2, 4, +1);
            Feature feature2 = new Feature("feature2", 5, 7, +1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(3, feature1.DistanceEndEnd(feature2));
            Assert.AreEqual(5, feature2.DistanceEndEnd(feature1));
        }

        [TestMethod]
        public void TestDistanceStartEnd()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "acgtactg");
            Feature feature1 = new Feature("feature1", 2, 4, +1);
            Feature feature2 = new Feature("feature2", 5, 7, +1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(5, feature1.DistanceStartEnd(feature2));
            Assert.AreEqual(7, feature2.DistanceStartEnd(feature1));
        }

        [TestMethod]
        public void TestDistanceEndStart()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "acgtactg");
            Feature feature1 = new Feature("feature1", 2, 4, +1);
            Feature feature2 = new Feature("feature2", 5, 7, +1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(1, feature1.DistanceEndStart(feature2));
            Assert.AreEqual(3, feature2.DistanceEndStart(feature1));
        }

        [TestMethod]
        public void TestIsOverlaaping()
        {
            Feature feature1 = new Feature("feature", 10, 14, +1);

            Assert.AreEqual(true, feature1.IsOverlapping(new Feature("f", 10,14, +1)));
            Assert.AreEqual(true, feature1.IsOverlapping(new Feature("f", 10, 14, -1)));
            Assert.AreEqual(true, feature1.IsOverlapping(new Feature("f", 7, 11, -1)));

            Assert.AreEqual(true, feature1.IsOverlapping(new Feature("f", 12, 16, -1)));
            Assert.AreEqual(false, feature1.IsOverlapping(new Feature("f", 5, 9, +1)));
            Assert.AreEqual(true, feature1.IsOverlapping(new Feature("f", 5, 19, +1)));

            Assert.AreEqual(false, feature1.IsOverlapping(new Feature("f", 15, 19, +1)));
            Assert.AreEqual(true, feature1.IsOverlapping(new Feature("f", 14, 18, +1)));
        }

        [TestMethod]
        public void TestIsInside()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "acgtgat");
            Feature feature1 = new Feature("feature1", 2, 4, +1);
            feature1.SetSequence(seq);

            Assert.AreEqual(true, feature1.IsInside(seq, 2));
            Assert.AreEqual(false, feature1.IsInside(seq, 1));
            Assert.AreEqual(true, feature1.IsInside(seq, 4));
            Assert.AreEqual(false, feature1.IsInside(seq, 5));

            Sequence sub1 = new Sequence(seq, 2, 4, +1);
            Assert.AreEqual(true, feature1.IsInside(sub1, 1));
            Assert.AreEqual(false, feature1.IsInside(sub1, 0));
            Assert.AreEqual(true, feature1.IsInside(sub1, 3));
            Assert.AreEqual(false, feature1.IsInside(sub1, 4));

            Sequence sub2 = new Sequence(sub1, 1, 3, +1);
            Assert.AreEqual(true, feature1.IsInside(sub2, 1));
            Assert.AreEqual(false, feature1.IsInside(sub2, 0));
            Assert.AreEqual(true, feature1.IsInside(sub2, 3));
            Assert.AreEqual(false, feature1.IsInside(sub2, 4));

            sub2 = new Sequence(sub1, 1, 3, -1);
            Assert.AreEqual(true, feature1.IsInside(sub2, 1));
            Assert.AreEqual(false, feature1.IsInside(sub2, 0));
            Assert.AreEqual(true, feature1.IsInside(sub2, 3));
            Assert.AreEqual(false, feature1.IsInside(sub2, 4));

            sub1 = new Sequence(seq, 2, 4, -1);
            Assert.AreEqual(true, feature1.IsInside(sub1, 1));
            Assert.AreEqual(false, feature1.IsInside(sub1, 0));
            Assert.AreEqual(true, feature1.IsInside(sub1, 3));
            Assert.AreEqual(false, feature1.IsInside(sub1, 4));

            sub2 = new Sequence(sub1, 1, 3, +1);
            Assert.AreEqual(true, feature1.IsInside(sub2, 1));
            Assert.AreEqual(false, feature1.IsInside(sub2, 0));
            Assert.AreEqual(true, feature1.IsInside(sub2, 3));
            Assert.AreEqual(false, feature1.IsInside(sub2, 4));

            sub2 = new Sequence(sub1, 1, 3, -1);
            Assert.AreEqual(true, feature1.IsInside(sub2, 1));
            Assert.AreEqual(false, feature1.IsInside(sub2, 0));
            Assert.AreEqual(true, feature1.IsInside(sub2, 3));
            Assert.AreEqual(false, feature1.IsInside(sub2, 4));
        }

        [TestMethod]
        public void TestDistanceUpstream()
        {
			Sequence seq = new Sequence( AlphabetType.DNA, "acgtactgactg", true );
            Feature feature1;
            Feature feature2;

            feature1 = new Feature("feature1", 6, 8, +1);
            feature2 = new Feature("feature2", 2, 3, +1);
            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(2, feature1.DistanceUpstream(feature2));
            Assert.AreEqual(5, feature2.DistanceUpstream(feature1));

            feature1 = new Feature("feature1", 6, 8, -1);
            feature2 = new Feature("feature2", 2, 3, -1);
            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(2, feature2.DistanceUpstream(feature1));
            Assert.AreEqual(5, feature1.DistanceUpstream(feature2));

            //Circular mix
            feature1 = new Feature("feature1", 6, 8, +1);
            feature2 = new Feature("feature2", 2, 3, -1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(2, feature1.DistanceUpstream(feature2));
            Assert.AreEqual(2, feature2.DistanceUpstream(feature1));

            //Linear forward scenario
            seq = new Sequence(AlphabetType.DNA, "acgtactgactg", false);
            feature1 = new Feature("feature1", 6, 8, +1);
            feature2 = new Feature("feature2", 2, 3, +1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(2, feature1.DistanceUpstream(feature2));
            Assert.AreEqual(1, feature2.DistanceUpstream(feature1));


            //Linear reverse
            feature1 = new Feature("feature1", 6, 8, -1);
            feature2 = new Feature("feature2", 2, 3, -1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(2, feature2.DistanceUpstream(feature1));
            Assert.AreEqual(4, feature1.DistanceUpstream(feature2));

            //Linear Mixed strand
            feature1 = new Feature("feature1", 6, 8, +1);
            feature2 = new Feature("feature2", 2, 3, -1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);


            Assert.AreEqual(2, feature1.DistanceUpstream(feature2));
            Assert.AreEqual(2, feature2.DistanceUpstream(feature1));
        }

        [TestMethod]
        public void TestDistanceDownStream()
        {
			Sequence seq = new Sequence( AlphabetType.DNA, "acgtactgactg", true );

            Feature feature1;
            Feature feature2;

            feature1 = new Feature("feature1", 6, 8, +1);
            feature2 = new Feature("feature2", 2, 3, +1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(5, feature1.DistanceDownstream(feature2));
            Assert.AreEqual(2, feature2.DistanceDownstream(feature1));

            //Reverse circular strand
            feature1 = new Feature("feature1", 6, 8, -1);
            feature2 = new Feature("feature2", 2, 3, -1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(5, feature2.DistanceDownstream(feature1));
            Assert.AreEqual(2, feature1.DistanceDownstream(feature2));

            //Circular mixed strand
            feature1 = new Feature("feature1", 6, 8, +1);
            feature2 = new Feature("feature2", 2, 3, -1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(5, feature2.DistanceDownstream(feature1));
            Assert.AreEqual(5, feature1.DistanceDownstream(feature2));

            seq = new Sequence(AlphabetType.DNA, "acgtactgactg", false);
            //linear strand
            feature1 = new Feature("feature1", 6, 8, +1);
            feature2 = new Feature("feature2", 2, 3, +1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(4, feature1.DistanceDownstream(feature2));
            Assert.AreEqual(2, feature2.DistanceDownstream(feature1));

            //Linear mixed strand
            feature1 = new Feature("feature1", 6, 8, +1);
            feature2 = new Feature("feature2", 2, 3, -1);

            feature1.SetSequence(seq);
            feature2.SetSequence(seq);

            Assert.AreEqual(4, feature1.DistanceDownstream(feature2));
            Assert.AreEqual(1, feature2.DistanceDownstream(feature1));
        }
    }
}
