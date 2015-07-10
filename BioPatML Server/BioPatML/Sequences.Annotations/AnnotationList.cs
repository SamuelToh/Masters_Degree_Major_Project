using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Sequences.Annotations
{
    /// <summary>
    /// 
    ///  This class implements a list of <see cref="QUT.Bio.BioPatML.Sequences.Annotations.Annotation">Annotation</see>. 
    ///  Annotation lists are
    ///  usually attached to a <see cref="QUT.Bio.BioPatML.Sequences.Sequence">Sequence </see> to describe properties of a
    ///  sequence such as name, or id.
    /// <para></para> 
    /// An annotation list can contain multiple
    ///  annotation with the same name. 
    ///  Annotation list are implemented as simple array lists. It is quite efficient
    ///  as long as the number of annotations is small and the annotations are
    ///  retrieved by constant/internally pooled string names (see String.intern(),
    ///  String.equals() and Annotation(String, Object)).
    /// </summary>
    [Serializable()]
    public class AnnotationList 
    {
        #region -- Private Fields --

        /// <summary>
        /// List containning annotations 
        /// </summary>
        private List<Annotation> ListAnnotation { get; set; }
        
        #endregion

        /// <summary>
        /// Default Constructor, initializes the necessary attributes
        /// </summary>
        public AnnotationList() { ListAnnotation = new List<Annotation>();  }

        #region -- Public Methods --

        /// <summary>
        ///  Adds an annotation to the list.
        /// </summary>
        /// <param name="annotation"> Annotation to add. </param>
        /// <see> Annotation </see>
        public void Add(Annotation annotation)
        {
            ListAnnotation.Add(annotation);
        }

        /// <summary>
        ///  Adds an annotation given by its name and string value to the list.
        /// </summary>
        /// <param name="name"> 
        ///  Name of the annotation.
        ///  </param>
        /// <param name="value"> 
        ///  value String value of the annotation. 
        ///  </param>
        ///  <see> Annotation </see>
        public void Add(String name, String value)
        {
            ListAnnotation.Add(new Annotation(name, value));
        }

        /// <summary>
        /// Adds an annotation given by its name and int value to the list.
        /// </summary>
        /// <param name="name">
        /// Name of the annotation.
        /// </param>
        /// <param name="value">value Int value of the annotation.</param>
        /// <see> Annotation </see>
        public void Add(String name, int value)
        {
            ListAnnotation.Add(new Annotation(name, value));
        }

        /// <summary>
        ///  Adds an annotation given by its name and double value to the list.
        /// </summary>
        /// <param name="name">
        /// Name of the annotation.
        /// </param>
        /// <param name="value">
        /// Double value of the annotation.
        /// </param>
        /// <see> Annotation </see>
        public void Add(String name, double value)
        {
            ListAnnotation.Add(new Annotation(name, value));
        }

        /// <summary>
        /// Gets the number of Annotation actually resided in the AnnotationList
        /// </summary>
        public int Count
        {
            get
            {
                return (ListAnnotation.Count);
            }
        }

        /// <summary>
        ///  Getter for an annotation within the list.
        /// </summary>
        /// <param name="index"> Index of the annotation to retrieve. </param>
        /// <returns> Returns the annotation for the given index. </returns>
        public Annotation this[int index]
        {
            get
            {
                return (ListAnnotation[index]);
            }
        }

        /// <summary>
        /// Getter for an annotation within the list. If there are more than one
        /// annotations with the same name only the first occurence will be returned
        /// </summary>
        /// <param name="name">
        ///  Name of the annotation to retrieve.
        /// </param>
        /// <returns> Returns the annotation for the given name or null if no 
        /// annotation with this name exists.
        /// </returns>
        public Annotation this[string name]
        {
            get
            {
                for (int i = 0; i < Count; i++)

                    if (name.Equals(this[i].Name))
                        return (this[i]);

                return null;
            }
        }

        /// <summary>
        ///  Get all annotations with a given name.
        /// </summary>
        /// <param name="name"> Name of the annotations to retrieve. </param>
        /// <returns> 
        /// Returns a of list annotations that matches the
        /// specified name. 
        /// </returns>
        public AnnotationList GetAll(String name)
        {
            AnnotationList aList = new AnnotationList();

            foreach (Annotation a in ListAnnotation)
            
                if(a.Name.Equals(name))
                    aList.Add(a);


            return aList;
        }

        /// <summary>
        /// Gets an annotation which name matches the given regular expression.
        /// </summary>
        /// <param name="regExp"> 
        /// Regular expression, <see>Pattern package</see>
        /// </param>
        /// <returns>
        /// Returns the first (not all) annotation that name matches the
        /// regular expression or null if no such annotation exists.
        /// </returns>
        public Annotation this[Regex regExp]
        {
            get
            {
                foreach (Annotation a in ListAnnotation)

                    if (regExp.IsMatch(a.Name))
                        return a;

                return (null);
            }

        }

        /// <summary>
        /// Gets all annotations which name matches the given regular expression.
        /// </summary>
        /// <param name="regExp">
        /// Regular expression, <see> Pattern package </see>
        /// </param>
        /// <returns> 
        /// Returns an annotation list with all annotations matching the given name.
        /// </returns>
        public AnnotationList GetAll(Regex regExp)
        {
            AnnotationList aList = new AnnotationList();

            foreach (Annotation a in ListAnnotation)

                if (regExp.IsMatch(a.Name))
                    aList.Add(a);


            return (aList);
        }

        /// <summary>
        /// Gets the number of annotation that has the param name.
        /// </summary>
        /// <param name="name"> Name of the annotation. </param>
        /// <returns> 
        /// Returns the number of annotations matching the
        /// given name.
        /// </returns>
        public int CountBy(String name)
        {
            int number = 0;

            foreach (Annotation a in ListAnnotation)

                if (a.Name.Equals(name))
                    number++;

            return number;
        }

        /// <summary>
        /// Gets the number of annotation that has the matching param name and value.
        /// </summary>
        /// <param name="name"> Name of the annotation. </param>
        /// <param name="value"> Value of the annotation. </param>
        /// <returns> Returns the number of annotations with the
        /// given name and value. </returns>
        public int CountBy(String name, Object value)
        {
            int number = 0;

            foreach (Annotation a in ListAnnotation)

                if (a.Name.Equals(name) &&
                    a.AnnotationValue.Equals(value))

                    number++;

            return (number);
        }

        /// <summary>
        /// Test if there is an annotation with the given name and value contained
        /// in the annotation list.
        /// </summary>
        /// <param name="name"> Name of the annotation. </param>
        /// <param name="value"> Value of the annotation. </param>
        /// <returns>
        /// true: if the annotation with the given name and value is 
        /// contained, false: otherwise.
        ///</returns>
        public bool Contains(String name, Object value)
        {
            AnnotationList aList = GetAll(name);

            for (int i = 0; i < aList.Count; i++)

                if (aList[i].Equals(value))
                    return true;


            return false;
        }

        /// <summary>
        ///  Creates a string representation of an annotation list.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Annotation a in ListAnnotation)
                sb.Append("   " + a.Name + "=" + a.AnnotationValue + "\n");

            return (sb.ToString());
        }

        #endregion
    }
}
