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

namespace MQuter_eLabApp.ViewModel
{
    public class CategoryModel
    {
        public Guid Id { get; set; }

        public string Label { get; set; }

        public string ImgSource { get; set; }

        public string CatDescription { get; set; }

        public string Name { get; set; }

        public List<ActivityModel> Activities { get; set; }

        public override string ToString() { return Name + CatDescription; }
    }
}
