using Syncfusion.DataSource;
using Syncfusion.DataSource.Extensions;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ListViewSample
{
   public class Behavior : Behavior<SfListView>
    {

        #region Fields

        GroupResult expandedGroup;
        SfListView listView;
        #endregion

        protected override void OnAttachedTo(SfListView bindable)
        {
            listView = bindable;
            listView.DataSource.SortDescriptors.Add(new SortDescriptor() { PropertyName = "ContactName", Direction = ListSortDirection.Ascending });
            listView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "ContactName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Contacts);
                    return item.ContactName[0].ToString();
                },
            });
            listView.Loaded += ListView_Loaded;
            listView.GroupExpanding += ListView_GroupExpanding;

            base.OnAttachedTo(bindable);
        }
        protected override void OnDetachingFrom(SfListView bindable)
        {
            listView.Loaded -= ListView_Loaded;
            listView.GroupExpanding -= ListView_GroupExpanding;
            base.OnDetachingFrom(bindable);
        }
        private void ListView_Loaded(object sender, ListViewLoadedEventArgs e)
        {
            listView.CollapseAll();
        }

        private void ListView_GroupExpanding(object sender, GroupExpandCollapseChangingEventArgs e)
        {
            if (e.Groups.Count > 0)
            {
                var group = e.Groups[0];
                if (expandedGroup == null || group.Key != expandedGroup.Key)
                {
                    foreach (var otherGroup in listView.DataSource.Groups)
                    {
                        if (group.Key != otherGroup.Key)
                        {
                            listView.CollapseGroup(otherGroup);
                        }
                    }
                    expandedGroup = group;
                    listView.ExpandGroup(expandedGroup);
                }
            }
        }

    }
}
