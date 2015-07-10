using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

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
    ///  This class implements an annotation to any sequences. An annotation should consist of
    ///  an item name and a value describing the item and these information are stored in 
    ///  <see cref="QUT.Bio.BioPatML.Sequences.Annotations.AnnotationList"> AnnotationList </see>.
    /// </summary>
    [Serializable()]
    public class Annotation 
    {
        #region -- Public fields --

        /// <summary>
        /// Name of the annotation
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Value of the annotation
        /// </summary>
        public Object AnnotationValue { get; set; }

        #endregion

        #region -- Public Constructors --
        /// <summary>
        ///  Creates an annotation with the given name and object value.
        /// </summary>
        /// <param name="name"> Annotation name </param>
        /// <param name="value"> Annotation value as object. </param>
        public Annotation(String name, Object value)
        {
            SetAnnotationAttr(name, value);
        }

        /// <summary>
        /// Creates an annotation with the given name and string value.
        /// </summary>
        /// <param name="name"> Annotation name. </param>
        /// <param name="value"> Annotation value as string. </param>
        public Annotation(String name, String value)
        {
            SetAnnotationAttr(name, (Object) value);
        }

        /// <summary>
        ///  Creates an annotation with the given name and integer value.
        /// </summary>
        /// <param name="name"> Annotation name. </param>
        /// <param name="value"> Annotation value as int. </param>
        public Annotation(String name, int value)
        {
            SetAnnotationAttr(name, value);
        }

        /// <summary>
        /// Creates an annotation with the given name and double value.
        /// </summary>
        /// <param name="name"> Annotation name</param>
        /// <param name="value"> Annotation value as double. </param>
        public Annotation(String name, double value)
        {
            SetAnnotationAttr(name, value);
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        ///  Setup the name and value of this annotation
        ///  Common method for constructors
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void SetAnnotationAttr
            (String name, Object value)
        {
            Name = String.Intern(name);
            AnnotationValue = value;
        }

        /// <summary>
        ///  Returns the value of an annotation as an object.
        /// </summary>
        /// <returns></returns>
        public Object ToObject()
        {
            return AnnotationValue;
        }  

        /// <summary>
        /// Tests if the given object is equal to the value of the annotation.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.AnnotationValue.Equals(obj);
        }

        /// <summary>
        ///  Creates a string representation of the annotation consisting of the
        ///  annotations name and its value. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                (Name + "='" + AnnotationValue + "'\n");
        }

        /// <summary>
        /// Returns the Hash code
        /// </summary>
        /// <returns>Hash code of annotation object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
