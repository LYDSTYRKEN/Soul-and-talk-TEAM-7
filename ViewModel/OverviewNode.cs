using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Soul_and_talk.ViewModel
{
    public class OverviewNode : ViewModelBase
    {
        public string Title { get; set; }
        public ObservableCollection<OverviewNode> Children { get; set; }
        public object Data { get; set; }

        public OverviewNode(string title)
        {
            Title = title;
            Data = null;
            Children = new ObservableCollection<OverviewNode>();
        }

        public OverviewNode(string title, object data)
        {
            Title = title;
            Data = data;
            Children = new ObservableCollection<OverviewNode>();
        }
    }
}
