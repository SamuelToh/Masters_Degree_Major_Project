using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Readers;
using BioPatML.Test;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/

namespace TestBioPatML.TestReader {
	[TestClass]
	public class TestMBFReader {
		private BioPatMBF_Reader reader;

		private const string _singleDnaSeqGenBankFilename = @"data\GenBank\D12555.gbk";
		private const string _sampleGenBankFile1 = @"data\GenBank\AE001582.gbk";
		private const string _sampleGenBankFile2 = @"data\GenBank\AnnIX-v003.gbk";
		private const string _diannaBTest = @"data\Genbank\1ChlamydophilaCaviaeGPIC.gbk";

		[TestMethod]
		public void TestReadSampleGenbankFileAsFilePath () {
			using ( reader = new BioPatMBF_Reader() ) {
				// TODO: this is not a unit test. We need some way of automatically determining if it worked.
				System.Diagnostics.Debug.WriteLine( reader.Read(
					Global.GetResourceReader( _sampleGenBankFile2 )
				) );
			}
		}

		[TestMethod]
		public void TestReadSampleGenbankFileAsTextReaderObj () {
			using ( reader = new BioPatMBF_Reader() ) {
				System.Diagnostics.Debug.WriteLine( reader.Read(
					Global.GetResourceReader( _sampleGenBankFile1 )
				) );
			}
		}

	}
}
