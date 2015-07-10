using System.Xml.Linq;
using System.Windows.Controls;
using System.Collections.Generic;

namespace BioPatMLEditor.PatternControls.PatternModels
{
    /// <summary>
    /// Interface of all pattern model
    /// </summary>
    public interface IPatternElement
    {
        /// <summary> All model should implement this method to write out their
        /// unique xml string. </summary>
        /// <returns></returns>
		
        XElement ToXml( params object [] attributesAndContent );

		/// <summary> Loads child elements from a collection of tree view items. </summary>
		/// <param name="viewItem"></param>

		void LoadFromTree ( IEnumerable<TreeViewItem> childViewItems );
    }
}
