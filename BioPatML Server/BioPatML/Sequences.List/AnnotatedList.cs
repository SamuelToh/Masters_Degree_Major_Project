using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Sequences.Annotations;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Sequences.List
{
    /// <summary>
    /// This class is a base class for lists of regions, sequences, features and
    /// other objects if required. 
    /// </summary>
    public class AnnotatedList : List<Object>, IAnnotated 
    {
        #region -- Private Field --

        /// <summary>
        /// List of annotations 
        /// </summary>
        private AnnotationList AnnotationList { get; set; }

        #endregion

        #region -- Public Constructors --
        /// <summary>
        ///  Creates an empty object list.
        /// </summary>
        public AnnotatedList() { }

        /// <summary>
        ///  Creates an object list with the given name. The name will be stored
        ///  as a String under the tag "Name" in the annotation list of the object list. 
        ///  This is just a conveniency method.
        /// </summary>
        /// <param name="name">Name of the object list. 
        /// </param>
        public AnnotatedList(String name)
        {
            if (name != null) 
                Annotations().Add("Name", name);
        }
        #endregion

        #region -- Protected Methods --

        /// <summary>
        /// Creates an object list with the given name. The name will be stored
        /// as a String under the tag "Name" in the annotation list of the object list. 
        /// This is just a conveniency method. Calling annotations().add("Name", name);
        /// would have the same effect.
        /// </summary>
        /// <param name="name"> Name of the object list. </param>
        /// <returns></returns>
        protected virtual AnnotatedList Create(String name)
        {
            return (new AnnotatedList(name));
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Gets the name of annotationList
        /// </summary>
        /// <returns> string representation of the object list name list </returns>
        public String Name
        {
            get
            {
                return (AnnotationValue("Name"));
            }
        }

        /// <summary>
        ///  Setter method for our list of elements. This setter is cyclic. 
        /// </summary>
        /// <param name="index"> Index of the element to set. Indicies out of the interval [0, size-1] 
        /// are wrapped to valid indices making the list cyclic.</param>
        /// <param name="annotated"> An annotated object.</param>
        public void Set(int index, IAnnotated annotated)
        {
            index %= Count;

            if (index < 0)
                index = Count + index;

            base[index] = annotated; //Replace the index element with the specified annotated Interface
        }

        #region Method : The mass Getter properties of our class

        /// <summary>
        ///  Gets an IAnnotated element by the given name.
        /// </summary>
        /// <param name="name"> Name of element.</param>
        /// <returns>Returns the first list element with the given name or null 
        /// if no such element exists.</returns>
        public IAnnotated Get(String name)
        {
            return (GetFirst("Name", name));
        }

       
        /// <summary>
        ///  Gets an list element which has an annotation variable of the given 
        ///  name and value.
        /// </summary>
        /// <param name="annotationName">  Name of the annotation variable, e.g. AccessionNumber </param>
        /// <param name="annotationValue"> Value of the annotation, e.g. the accession number </param>
        /// <returns> Returns the first list element with the specified annotation or 
        ///  null if no such element exists.
        ///  </returns>
        public IAnnotated GetFirst
            (String annotationName, String annotationValue)
        {
            for (int i = 0; i < Count; i++)
            {
                IAnnotated element = (IAnnotated)base[i];
                String value = element.AnnotationValue(annotationName);

                if(value != null 
                    && value.Equals(annotationValue))

                    return element;
            
            }

            return null;
        }

        /// <summary>
        ///  Gets a list of elements which have an annotation variable of the given 
        ///  name and value.
        /// </summary>
        /// <param name="annotationName"> Name of the annotation variable, e.g. AccessionNumber </param>
        /// <param name="annotationValue"> Value of the annotation.</param>
        /// <returns> Returns a list with elements that have the required annotation. </returns>
        public AnnotatedList Get(String annotationName, String annotationValue)
        {
            AnnotatedList list = Create(null);

            for (int i = 0; i < Count; i++)
            {
                IAnnotated element = (IAnnotated)base[i];
                String value = element.AnnotationValue(annotationName);

                if (value != null 
                    && value.Equals(annotationValue))

                    list.Add(element);

            }

            return (list);
        }

        /// <summary>
        /// Gets a of list filled with elements which have an annotation variable of the given 
        /// name and value contained in the provide list of annotation values.
        /// </summary>
        /// <param name="annotationName"> Name of the annotation variable, e.g. subcellular localization </param>
        /// <param name="aanotationValues"> Array of annotation values </param>
        /// <returns> Returns a list with elements that have annotation variables
        /// with values contained in the annotation values list. 
        /// </returns>
        public AnnotatedList Get(String annotationName, String[] aanotationValues)
        {
            AnnotatedList list = Create(null);

            for (int i = 0; i < Count; i++)
            {
                IAnnotated element = (IAnnotated)base[i];
                String value = element.AnnotationValue(annotationName);

                if(value != null)
                {
                    for(int j = 0 ; j < aanotationValues.Length; j ++)
                    
                        if (aanotationValues[j].Equals(value))
                        {
                            list.Add(element);
                            break;
                        }
                }
        
            }
            return (list);
        }

        

        /// <summary>
        ///  Getter for all list elements which have an annotation variable of the given 
        ///  name and value contained in the provide list of annotation values.
        ///  This method is the same as the above.
        /// </summary>
        /// <param name="annotationName"> Name of the annotation variable, e.g. subcellular localization  </param>
        /// <param name="aanotationValues"> List of annotation values</param>
        /// <returns>Returns a list with elements that have annotation variables
        /// with values contained in the annotation values list. 
        /// </returns>
        public AnnotatedList Get(String annotationName, List<String> aanotationValues)
        {
            AnnotatedList list = Create(null);

            for (int i = 0; i < Count; i++)
            {
                IAnnotated element = (IAnnotated)base[i];
                String value = element.AnnotationValue(annotationName);

                if (value != null)
                {
                    for (int j = 0; j < aanotationValues.Count; j++)

                        if (aanotationValues[j].Equals(value))
                        {
                            list.Add(element);
                            break;
                        }
                }
            }
            return list;
        }

        /// <summary>
        ///  Getter for a list element. This getter is cyclic. 
        /// </summary>
        /// <param name="index">
        ///  Index of the element to get. Indicies out of the interval [0, size-1]
        ///  are wrapped to valid indices making the list cyclic.</param>
        /// <returns> Return the specified list element. </returns>
        public Object Get(int index)
        {
            index %= Count;

            if (index < 0)
                index = Count + index;

            return (base[index]);
        }

        #endregion

        /// <summary>
        ///  Query method if the list has annotations or not. This method is more
        ///  efficent than calling  "annotations().size() > 0" because no empty
        ///  annotation list will be created if there are no annotations.
        /// </summary>
        /// <returns>Returns true if the list has at least one annotation and
        /// false otherwise.</returns>
        public bool HasAnnotations()
        {
            if (AnnotationList == null)
                return (false);


            return
                (AnnotationList.Count < 0 
                            ? true : false);
        }

        /// <summary>
        ///  Gets the list of annotations attached to the list. As soon as
        ///  this method is called an empty annotation list will be attached to the
        ///  list if none is existing before.
        /// </summary>
        /// <returns> Returns the list of annotations attached to the list. </returns>
        public AnnotationList Annotations()
        {
            if (AnnotationList == null)
                return (this.AnnotationList = new AnnotationList());

            return (AnnotationList);  
        }

        
        /// <summary>
        ///  Gets the annotation variable with the specified name.
        /// </summary>
        /// <param name="name"> Name of the annotation variable. </param>
        /// <returns> Returns an annotation or null if no annotation variable with the 
        /// given name is in the annotation list attached to the list.
        /// </returns>
        public Annotation Annotations(String name)
        {
            return 
                (this.Annotations()[name]);
        }

        /// <summary>
        ///  Adds a list of annotations to the already existing annotations of the
        ///  object list. 
        /// </summary>
        /// <param name="annotationList"> List of annotations to add. </param>
        public void AddAnnotations(AnnotationList annotationList)
        {
            if (annotationList != null)
            {
                AnnotationList a = this.Annotations();

                for (int i = 0; i < annotationList.Count; i++)
                    a.Add(annotationList[i]);

            }
        }

        /// <summary>
        /// Gets the string value of the annotation variable with the given name.
        /// See the value methods of {<see> Annotation </see>} class for other ways to
        /// retrieve values of a certain type from an annotation.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public String AnnotationValue(String name)
        {
            Annotation annotation = Annotations()[name];

            return 
                (annotation == null 
                    ? null : annotation.AnnotationValue.ToString());
        }

        /// <summary>
        /// Appends a list to the list. All elements of the given list are appended to
        /// the list.
        /// </summary>
        /// <param name="list"> List to add. </param>
        public void Append(AnnotatedList list)
        {
            for (int i = 0; i < list.Count; i++)
                Add(list[i]);
        }

        /// <summary>
        ///  Creates a list of annotated lists which are a split of this list according
        ///  to the values of the specified annotation. This means each list of the
        ///  split contains only elements which annotations where the specified
        ///  annotation name has the same value. 
        ///  * Note that the source list is not changed.
        /// </summary>
        /// <param name="annotationName"> Name of the annotation which is used for the splitting. </param>
        /// <returns> Returns a list of annotated lists. The name of the lists is the
        /// annotation value for this list.</returns>
        public AnnotatedList Split(String annotationName)
        {
            AnnotatedList lists = new AnnotatedList();

            for (int i = 0; i < this.Count; i++)
            {
                IAnnotated element = (IAnnotated)base[i];

                if (element.HasAnnotations())
                {
                    String value = element.AnnotationValue(annotationName);

                    if (value != null)
                    {
                        AnnotatedList list = (AnnotatedList)lists.Get(value);

                        if (list == null)
                        {
                            list = Create(value);
                            lists.Add(list);
                        }
                        list.Add(element);
                    }
                }
            }
            
            return (lists);
        }

        #endregion
    }
}
