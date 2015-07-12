namespace MQuter_eLabApp.ViewModel
{
    #region Directives

    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    #endregion

    public class ParamBinder
    {
        private string _sourceId;

        private string _property;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityName">The unique name for the activity; the source</param>
        /// <param name="property">Its property name</param>
        public ParamBinder(string activityName, string property)
        {
            #region Validation of Arguments
            if (string.IsNullOrEmpty(activityName))
            {
                throw new ArgumentNullException("sourceActivity");
            }

            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentNullException("property");
            }
            #endregion

            Source = activityName;
            Property = property;
        }

        public string Source
        {
            get { return this._sourceId; }
            set { this._sourceId = value; }
        }

        public string Property
        {
            get { return this._property; }
            set { this._property = value; }
        }

        public override string ToString()
        {
            return Source + "." + Property;
        }

    }
}
