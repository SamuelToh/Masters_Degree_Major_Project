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
    ///  This interfaces describes accessor methods for annotated objects.
    /// </summary>
    public interface IAnnotated
    {
        /// <summary>
        ///  Checks whether the list has any annotations
        /// </summary>
        /// <returns> Returns true if the object has at least one annotation and
        /// false otherwise.</returns>
        bool HasAnnotations();

        /// <summary>
        ///  Gets a list of available annotations. 
        /// </summary>
        /// <returns>  Returns the list of annotations attached to the object. </returns>
        AnnotationList Annotations();

        /// <summary>
        ///   Gets an annotation by a specified name.
        /// </summary>
        /// <param name="annotationName"> Name of the annotation variable. </param>
        /// <returns>Returns an annotation or null if no annotation with the given
        /// name is in the annotation list.</returns>
        Annotation Annotations(String annotationName);

        /// <summary>
        ///  Gets the string value of an annotation by supplying the annotation name
        /// </summary>
        /// <param name="annotationName"> Name of the annotation variable. </param>
        /// <returns> Returns the value of the annotation as a string or null if no 
        ///           annotions with the given name exits. </returns>
        String AnnotationValue(String annotationName);

    }
}
