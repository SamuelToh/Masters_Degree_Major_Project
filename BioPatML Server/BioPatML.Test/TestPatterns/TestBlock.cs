
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using BioPatML.Test;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestPatterns
{
    [TestClass]
    public class TestBlock
    {
        [TestMethod]
        /** Tests constructor */
        public void TestConstructor()
        {
            SequenceList list = new SequenceList();
            list.Add(new Sequence(AlphabetType.DNA, "aa", false));
			list.Add( new Sequence( AlphabetType.DNA, "at", false ) );
            Block block = new Block("test", list, null, 0.0);
            Assert.AreEqual(1.000, block.Get('a', 0), 1e-3);
            Assert.AreEqual(-0.584, block.Get('c', 0), 1e-3);
            Assert.AreEqual(-0.584, block.Get('t', 0), 1e-3);
            Assert.AreEqual(-0.584, block.Get('g', 0), 1e-3);
            Assert.AreEqual(0.415, block.Get('a', 1), 1e-3);
            Assert.AreEqual(-0.584, block.Get('c', 1), 1e-3);
            Assert.AreEqual(0.415, block.Get('t', 1), 1e-3);
            Assert.AreEqual(-0.584, block.Get('g', 1), 1e-3);
        }

        [TestMethod]
        public void TestRead()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Block.xml" ) );
            Block pattern = (Block)definition.Pattern;

            Assert.AreEqual("Block", definition.Name);
            Assert.AreEqual("block", pattern.Name);
            Assert.AreEqual(0.7, pattern.Threshold, 1e-3);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
            Assert.AreEqual(6, pattern.WeightedVectorLength);
            Assert.AreEqual("tataat", pattern.Consensus.Letters());
        }

        [TestMethod]
        public void TestToXml ()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Block.xml" ) );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );
			
			Block pattern = (Block) def2.Pattern;

            Assert.AreEqual("Block", definition.Name);
            Assert.AreEqual("block", pattern.Name);
            Assert.AreEqual(0.7, pattern.Threshold, 1e-3);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
            Assert.AreEqual(6, pattern.WeightedVectorLength);
            Assert.AreEqual("tataat", pattern.Consensus.Letters());
			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
		}
    }
}
