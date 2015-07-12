namespace MQuter_eLabApp.View.ConnectionLines
{
    #region Directives

    using System;
    using System.Net;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using MQuter_eLabApp.View.Editors;
    using MQuter_eLabApp.View.Components.Activity;
    using MQuter_eLabApp.ViewModel;
    using MQuter_eLabApp.Events;

    #endregion

    public class ActivityConnection : BaseConnection
    {
        #region Private variables

        private IActivityComponent Source, Target;
        private ActivityEditor _editorMenu;

        #endregion 

        #region Event - On connection line deleted

        public delegate void OnConnRemovedEventHandler(object sender, EventArgs e);

        public event OnConnRemovedEventHandler ConnectionRemove;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void OnConnRemove(EventArgs e)
        {
            if (ConnectionRemove != null)
                ConnectionRemove(this, e);
        }

        #endregion 

        #region Constructors

        public ActivityConnection(IActivityComponent input, Point startPtr)
            : base(startPtr)
        {
            Source = input;
            Loaded += new RoutedEventHandler(Line_Loaded);
        }

        void ActivityConnection_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("rrrr");
        }

        void Line_Loaded(object sender, RoutedEventArgs e)
        {
            Connection.MouseLeftButtonDown +=
                             new MouseButtonEventHandler(ShowEditorMenu);
            Connection.MouseRightButtonDown += new MouseButtonEventHandler(ActivityConnection_MouseRightButtonDown);
        }

        #endregion Constructors

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main">The object that we want to scan its siblings for menu</param>
        private ActivityEditor RetrieveSiblingsEditorMenu(IActivityComponent main)
        {
            if (main is ForLoopComponent)
                return null; //Not supported for now

            else 
                {
                    foreach (ActivityConnection conn in main.InputConn)
                        if (conn._editorMenu != null)
                            return conn._editorMenu;
                }

            return null;
        }

        private void UpdateSiblingsEditorMenu(ActivityEditor menu)
        {
            foreach (ActivityConnection conn in Target.InputConn)
                conn.SetEditorMenu = menu;

        }

        private void ShowEditorMenu(object sender, MouseButtonEventArgs e)
        {
            //Scan for menu from its siblings first.
            _editorMenu = RetrieveSiblingsEditorMenu(Target);

            #region If we seriously don't have a menu yet, then create one

            if (_editorMenu == null)
            {
                //if target is forloop && source is forloop
                //else if source is forloop
                //else if target is forloop
                //else standard...

                switch ((Target.DataContext as ActivityModel).ActivityClassification)
                {
                        //nested switch make it a method to loop thru...
                    case ActivityModel.ActivityType.ForLoop :
                        {
                            ForLoopComponent forLoop = Target as ForLoopComponent;
                            IActivityComponent[] components = forLoop.GetNestedActivity.ToArray();
                            _editorMenu = new ActivityEditor(Target.GetInputs, forLoop, components);

                            break;
                        }

                    case ActivityModel.ActivityType.Standard:
                        {
                            if (Source is ForLoopComponent)
                            {
                                _editorMenu = new ActivityEditor
                                                    (Target.GetInputs, (Source as ForLoopComponent).GetNestedActivity.ToArray(), Target);
                                break;
                            }
                            //Else Move to the standard 
                            if (Target.InputConn.Count > 1)
                            {
                                _editorMenu = new ActivityEditor
                                                    (Target.GetInputs, Target);

                            }
                            else
                                _editorMenu = new ActivityEditor(Source, Target);

                            _editorMenu.ValidationFail += new ActivityEditor.ValidationFailEventHandler(_editorMenu_ValidationFail);

                            break;
                        }
                }       
            }
            #endregion If we do not have a menu yet, create one
            #region Else just add the new item into the retrieved menu

            _editorMenu.UpdateSources(Target.GetInputs);

            #endregion

            this.UpdateSiblingsEditorMenu(_editorMenu);

            _editorMenu.Show();
        }

        void _editorMenu_ValidationFail(object sender, EventArgs e)
        {
            ActivityValidationEventArgs args = e as ActivityValidationEventArgs;

            if (args != null)
                Target.ActivityParamError = args.Error;
        }

    
        #endregion

        #region Public Methods

        public ActivityEditor SetEditorMenu
        {
            set
            {
                this._editorMenu = value;
            }
        }

        public IActivityComponent InputSource
        {
            get { return this.Source; }
        }

        public IActivityComponent OutputSource
        {
            get { return this.Target; }
            set { this.Target = value; }
        }

        public void DeleteLine(IActivityComponent deleter)
        {
            base.DeleteLine();
            //First thing to do is update the parameter menu to reflect this change.
            UpdateParamEditorMenu(deleter);

            OnConnRemove(new ConnectionEventArgs(InputSource.Name, OutputSource.Name, false));

            //In the line's perspective point of view the input source would be the output component.
            if (deleter.Equals(InputSource)) //if this line is an outgoing line for the deleter
            {
                Target.InputConn.Remove(this);
                

                #region Then remove all inputs related to this source on its target source

                ActivityModel model = OutputSource.DataContext as ActivityModel;

                if (model != null)
                {
                    foreach (ParamInputModel input in model.InputParam)
                    {
                        if (input.Value != null) //remove all the bindings
                        {
                            input.Value = null;
                            input.ValueStr = null;
                            (OutputSource as ActivityComponent).InputGate.GateStatus
                                                = IOGateComponent.Status.NeedsConfiguration;
                        }
                    }
                }

                #endregion
            }
        }

        private void UpdateParamEditorMenu(IActivityComponent deleter)
        {
            //Basically if a menu already exist, obviously something has to do something about it.
            if (_editorMenu != null)
            {
                _editorMenu.RemoveSource(deleter);
            }
        }

        #endregion Public Methods
    }
}
