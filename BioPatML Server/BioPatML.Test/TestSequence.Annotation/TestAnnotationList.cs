using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences.Annotations;
using QUT.Bio.BioPatML;
using System.Text.RegularExpressions;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSequence.TestAnnotation
{
    [TestClass]
    public class TestAnnotationList
    {
        private AnnotationList list;

         [TestInitialize]
         public void SetUp()
         {
             this.list = new AnnotationList();



         }

         [TestMethod]
         public void TestConstructor()
         {

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                           ("name1", "val1"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name2", "val2"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val3"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val3"));

             Assert.AreEqual(4, list.Count);
             Assert.AreEqual("name1", list[0].Name);
             Assert.AreEqual("name2", list[1].Name);
             Assert.AreEqual("name3", list[2].Name);
             Assert.AreEqual("name3", list[3].Name);
         }

         [TestMethod]
         public void TestAdd()
         {
             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name1", "val1"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name2", 10));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", 10.1));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name4", (Object) "val4"));

             Assert.AreEqual("name1", list[0].Name);
             Assert.AreEqual("val1", list[0].Value);

             Assert.AreEqual("name2", list[1].Name);
             Assert.AreEqual(10, list[1].Value);

             Assert.AreEqual("name3", list[2].Name);
             Assert.AreEqual(10.1, (double) list[2].Value, 1e-1);

             Assert.AreEqual("name4", list[3].Name);
             Assert.AreEqual("val4", list[3].Value);
         }

         [TestMethod]
         public void TestGetByName()
         {
             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                           ("name1", "val1"));

             Assert.AreEqual("name1", list["name1"].Name);
             Assert.AreEqual(null, list["Name1"]);

             char[] test = { 'n', 'a', 'm', 'e', '1' };
             Assert.AreEqual("name1", list[new String(test)].Name);

             
         }

         [TestMethod]
         public void TestGetByRegExp()
         {
             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", 10.1));


             Assert.AreEqual("name3", list[new Regex(".*3")].Name);


             Assert.AreEqual(null, list[new Regex(".*5")]);

         }

         [TestMethod]
         public void TestGetAllByName()
         {
             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name1", "val1"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name2", 10));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", 10.1));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", (Object)"val4"));


             AnnotationList alist = list.GetAll("name3");
             Assert.AreEqual(2, alist.Count);
             Assert.AreEqual("name3", alist[0].Name);
             Assert.AreEqual("name3", alist[1].Name);
         }

         [TestMethod]
         public void TestGetAllByRegExp()
         {
             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name1", "val1"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name2", 10));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", 10.1));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", (Object)"val4"));


             AnnotationList alist = list.GetAll(new Regex(".*3"));
             Assert.AreEqual(2, alist.Count);
             Assert.AreEqual("name3", alist[0].Name);
             Assert.AreEqual("name3", alist[1].Name);
         }

         [TestMethod]
         public void TestContains()
         {
             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name1", "val1"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name2", "val2"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val3"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val4"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val5"));

             Assert.AreEqual(true, list.Contains("name1", "val1"));
             Assert.AreEqual(true, list.Contains("name2", "val2"));
             Assert.AreEqual(true, list.Contains("name3", "val3"));
             Assert.AreEqual(true, list.Contains("name3", "val4"));
             Assert.AreEqual(true, list.Contains("name3", "val5"));
             Assert.AreEqual(false, list.Contains("name3", "val6"));
             Assert.AreEqual(false, list.Contains("name2", "val3"));
             Assert.AreEqual(false, list.Contains("name5", "val3"));
         }

         [TestMethod]
         public void TestNumber()
         {
             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name1", "val1"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name2", "val2"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val3"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val3"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val4"));

             this.list.Add(new QUT.Bio.BioPatML.Sequences.Annotations.Annotation
                                                        ("name3", "val5"));

             Assert.AreEqual(0, list.CountBy("name0", "val0"));
             Assert.AreEqual(1, list.CountBy("name1", "val1"));
             Assert.AreEqual(2, list.CountBy("name3", "val3"));
             Assert.AreEqual(1, list.CountBy("name3", "val4"));
             Assert.AreEqual(1, list.CountBy("name3", "val5"));
             Assert.AreEqual(0, list.CountBy("name3", "val6"));

             Assert.AreEqual(0, list.CountBy("name0"));
             Assert.AreEqual(1, list.CountBy("name1"));
             Assert.AreEqual(1, list.CountBy("name2"));
             Assert.AreEqual(4, list.CountBy("name3"));
         }
    }
}
