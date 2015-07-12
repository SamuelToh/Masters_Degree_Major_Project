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
using System.Collections.Generic;
using MQuter_eLabApp.View.ConnectionLines;

namespace MQuter_eLabApp.View.Components.Activity
{
    public interface IActivityComponent
    {
        string Name { get; set; }

        Object DataContext { get; set; }

        List<ActivityConnection> InputConn { get; set; }

        List<ActivityConnection> OutputConn { get; set; }

        List<IActivityComponent> GetInputs { get; }

        bool OutputGatesHasFocus();

        bool InputGateHasFocus();

        bool HasFocus { get; set; }

        IOGateComponent GetForcusedGate();

        Exception ActivityParamError { get; set; } 

        ActivityConnection GetLatestOutputLine();

        void CaptureMouse();

        void ReleaseMouse();

        IOGateComponent GetOutputGate { get; }

        Point GetComponentPosition();

        void SetComponentPosition(Point newPosition);

    }
}
