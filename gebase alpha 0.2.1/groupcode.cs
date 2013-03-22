using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraBars;
using MongoDB.Bson;
using System.Data;
using DevExpress.XtraGrid.Columns;

namespace gebase_alpha_0._2._1
{
    public class groupcode
    {
        public static string connectionString;
        public static MongoClient client;
        public static MongoServer server;
        public static MongoDatabase gebase;
        public static MongoCollection<groupcolls> groupcollection;

        public groupcolls _groupentity;

        public static void GroupGridColHide(MainAppForm mainapp)
        {
            mainapp.bandedGroupGridView.Columns["_id"].Visible = false;
            mainapp.bandedGroupGridView.Columns["suntime"].Visible = false;
            mainapp.bandedGroupGridView.Columns["montime"].Visible = false;
            mainapp.bandedGroupGridView.Columns["tuetime"].Visible = false;
            mainapp.bandedGroupGridView.Columns["wedtime"].Visible = false;
            mainapp.bandedGroupGridView.Columns["thutime"].Visible = false;
            mainapp.bandedGroupGridView.Columns["fritime"].Visible = false;
            mainapp.bandedGroupGridView.Columns["sattime"].Visible = false;
            mainapp.bandedGroupGridView.Columns["start"].Visible = false;
        }

        public static void MongoInitiate() /* COMPL */
        {
            connectionString = Properties.Settings.Default.ServerOne/* + "," + Properties.Settings.Default.ServerTwo + "/?connect=replicaset"*/;
            client = new MongoClient(connectionString);
            server = client.GetServer();
            gebase = server.GetDatabase("gebase");
            groupcollection = gebase.GetCollection<groupcolls>("groups");
        }

        //public static void GroupGridUnsortedRefreshFilter(MainAppForm mainapp)
        //{
        //    MongoCollection<groupcolls> groupcollection = gebase.GetCollection<groupcolls>("groups");
        //    var query = Query.EQ("status", Properties.Settings.Default.GroupFilterFlag);

        //    BindingList<groupcolls> groupresult = new BindingList<groupcolls>(groupcollection.Find(query).ToList());
        //    mainapp.gridGroup.DataSource = groupresult;

        //    StatusTextRefresh(mainapp);
        //}

        //public static void StatusTextRefresh(MainAppForm mainapp)
        //{
        //    MongoCollection<groupcolls> groupcount = gebase.GetCollection<groupcolls>("groups");

        //    var query = Query.EQ("status", Properties.Settings.Default.GroupFilterFlag);
        //    int count = Convert.ToInt16(groupcount.Find(query).Count());
        //    mainapp.ItemsCountStatusText.Caption = Convert.ToString(Properties.Settings.Default.GroupFilterFlag) + " groups count: " + Convert.ToString(count);
        //}

        public static void ActionButtonSwitch(MainAppForm mainapp, string groupstatus, int focusedrow)
        {
            switch (groupstatus)
            {
                case "active":
                        mainapp.PauseGroupButton.Enabled = true;
                        mainapp.ResumeGroupButton.Enabled = false;
                        mainapp.FinishGroupButton.Enabled = true;
                        mainapp.StartGroupButton.Enabled = false;
                        mainapp.EditGroupButton.Enabled = true;
                        mainapp.DeleteGroupButton.Enabled = true;

                        mainapp.bandedGroupGridView.FocusedRowHandle = focusedrow;
                        stdcode.GroupDetails(mainapp);
                    break;
                case "paused":
                        mainapp.PauseGroupButton.Enabled = false;
                        mainapp.ResumeGroupButton.Enabled = true;
                        mainapp.FinishGroupButton.Enabled = true;
                        mainapp.StartGroupButton.Enabled = false;
                        mainapp.EditGroupButton.Enabled = true;
                        mainapp.DeleteGroupButton.Enabled = true;
                        mainapp.bandedGroupGridView.Focus();

                        mainapp.bandedGroupGridView.FocusedRowHandle = focusedrow;
                        stdcode.GroupDetails(mainapp);
                    break;
                case "awaiting":
                        mainapp.PauseGroupButton.Enabled = false;
                        mainapp.ResumeGroupButton.Enabled = false;
                        mainapp.FinishGroupButton.Enabled = false;
                        mainapp.StartGroupButton.Enabled = true;
                        mainapp.EditGroupButton.Enabled = true;
                        mainapp.DeleteGroupButton.Enabled = true;

                        mainapp.bandedGroupGridView.FocusedRowHandle = focusedrow;
                        stdcode.GroupDetails(mainapp);
                        break;
                case "finished":
                        mainapp.PauseGroupButton.Enabled = false;
                        mainapp.ResumeGroupButton.Enabled = false;
                        mainapp.FinishGroupButton.Enabled = false;
                        mainapp.StartGroupButton.Enabled = false;
                        mainapp.EditGroupButton.Enabled = true;
                        mainapp.DeleteGroupButton.Enabled = true;

                        mainapp.bandedGroupGridView.FocusedRowHandle = focusedrow;
                        stdcode.GroupDetails(mainapp);
                    break;
            }
        }

        public static void GroupAction(MainAppForm mainapp, int rowhandle, string groupstatus)
        {

            BindingList<groupcolls> grouplist = mainapp.gridGroup.DataSource as BindingList<groupcolls>;
            groupcolls _groupentity = grouplist[rowhandle];

            gebase.GetCollection<groupcolls>("groups").Update(
                Query.EQ("_id", _groupentity._id),
                MongoDB.Driver.Builders.Update.Set("status", groupstatus));

            GroupGridRefresh(mainapp);

            mainapp.StatusEventsText.Caption = "Group " + _groupentity.number.ToString() + " state changed to " + groupstatus.ToUpper();
            mainapp.StatusEventsText.Visibility = BarItemVisibility.Always;

            mainapp.bandedGroupGridView.FocusedRowHandle = Properties.Settings.Default.GroupSelectedRowIndex;
            //StatusTextRefresh(mainapp);
        }

        public static void GroupGridRefreshFilter(MainAppForm mainapp, string filter)
        {
            Properties.Settings.Default.GroupFilterFlag = filter;
            Properties.Settings.Default.Save();

            MongoCollection<groupcolls> groupcollection = gebase.GetCollection<groupcolls>("groups");
            var query = Query.EQ("status", Properties.Settings.Default.GroupFilterFlag);

            BindingList<groupcolls> groupresult = new BindingList<groupcolls>(groupcollection.Find(query).ToList());
            mainapp.gridGroup.DataSource = groupresult;
        }

        //public static void GroupGridSort(MainAppForm mainapp)
        //{
        //    MongoCollection<groupcolls> groupcollection = gebase.GetCollection<groupcolls>("groups");
        //    var query = Query.EQ("status", Properties.Settings.Default.GroupFilterFlag);
        //    string filter = Properties.Settings.Default.GroupFilterFlag;

        //    switch (Properties.Settings.Default.GroupSortDirection)
        //    {
        //        case "asc":
        //            {
        //                BindingList<groupcolls> groupresult = new BindingList<groupcolls>(groupcollection.Find(query).SetSortOrder(SortBy.Ascending(Properties.Settings.Default.GroupSortField)).ToList());
        //                mainapp.gridGroup.DataSource = groupresult;
        //                Properties.Settings.Default.GroupSortDirection = "desc";
        //                Properties.Settings.Default.Save();
        //            }
        //            break;
        //        case "desc":
        //            {
        //                BindingList<groupcolls> groupresult = new BindingList<groupcolls>(groupcollection.Find(query).SetSortOrder(SortBy.Descending(Properties.Settings.Default.GroupSortField)).ToList());
        //                mainapp.gridGroup.DataSource = groupresult;
        //                Properties.Settings.Default.GroupSortDirection = "asc";
        //                Properties.Settings.Default.Save();
        //            }
        //            break;
        //    }
        //}

        public static void GroupStdCount(MainAppForm mainapp)
        {
            var query =
                from e in gebase.GetCollection<groupcolls>("groups").AsQueryable<groupcolls>()
                select e.number;

            foreach (var number in query)
            {
                int stdcount = Convert.ToInt16(stdcode.gebase.GetCollection<stdcolls>("stds").FindAs<stdcolls>(Query.EQ("group", number)).Count());

                gebase.GetCollection<groupcolls>("groups").Update(
                Query.EQ("number", number),
                MongoDB.Driver.Builders.Update.Set("stdcount", stdcount));
            }
        }

        public static void GroupGridRefresh(MainAppForm mainapp)
        {
            var query = Query.EQ("status", Properties.Settings.Default.GroupFilterFlag);

            BindingList<groupcolls> groupresult = new BindingList<groupcolls>(gebase.GetCollection<groupcolls>("groups").Find(query).ToList());
            mainapp.gridGroup.DataSource = groupresult;

            int count = Convert.ToInt16(groupresult.Count());
            mainapp.ItemsCountStatusText.Caption = Convert.ToString(Properties.Settings.Default.GroupFilterFlag) + " groups count: " + Convert.ToString(count);
            mainapp.bandedGroupGridView.GroupPanelText = Convert.ToString(Properties.Settings.Default.GroupFilterFlag) + " groups list";        
        }

        public static void GroupDel(MainAppForm mainapp, string _id)
        {
            gebase.GetCollection<groupcolls>("groups").Remove(
                Query.EQ("_id", ObjectId.Parse(_id)));

            groupcode.GroupGridRefresh(mainapp);

            mainapp.bandedGroupGridView.FocusedRowHandle = Properties.Settings.Default.GroupSelectedRowIndex;
        }

        public static void GroupTabShow(MainAppForm mainapp)
        {
            switch (Properties.Settings.Default.GroupFilterFlag)
            {
                case "active":
                    mainapp.ActiveGroupButton.Down = true;
                    GroupGridRefresh(mainapp);
                    break;
                case "paused":
                    mainapp.PausedGroupButton.Down = true;
                    GroupGridRefresh(mainapp);
                    break;
                case "awaiting":
                    mainapp.AwaitingGroupButton.Down = true;
                    GroupGridRefresh(mainapp);
                    break;
                case "finished":
                    mainapp.FinishedGroupButton.Down = true;
                    GroupGridRefresh(mainapp);
                    break;
            }
        }

        public static void GroupDetailView(MainAppForm mainapp)
        {
            switch (Properties.Settings.Default.GroupDetailsShow)
            {
                case true:
                    mainapp.DetailGroupButton.Down = true;
                    mainapp.DetailGroupButton_DownChanged(null, null);
                    break;
                case false:
                    mainapp.DetailGroupButton.Down = false;
                    mainapp.DetailGroupButton_DownChanged(null, null);
                    break;
            }
        }
    }
}
