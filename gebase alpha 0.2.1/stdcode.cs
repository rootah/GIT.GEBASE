using DevExpress.XtraEditors;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gebase_alpha_0._2._1
{
    class stdcode
    {
        public static string connectionString;
        public static MongoClient client;
        public static MongoServer server;
        public static MongoDatabase gebase;
        public static MongoCollection<stdcolls> stdcollection;

        public static stdcolls _stdentity;

        public static void MongoInitiate(MainAppForm mainapp)
        {
            connectionString = Properties.Settings.Default.ServerOne/* + "," + Properties.Settings.Default.ServerTwo + "/?connect=replicaset"*/;
            client = new MongoClient(connectionString);
            server = client.GetServer();
            gebase = server.GetDatabase("gebase");
            stdcollection = gebase.GetCollection<stdcolls>("stds");

            StdGridRefresh(mainapp);
        }

        public static void StdGridRefresh(MainAppForm mainapp)
        {
            //if (mainapp.bandedStudentsGridView.RowCount > 0)
            //{
                var query = Query.EQ("status", Properties.Settings.Default.StdFilterFlag);

                BindingList<stdcolls> stdresult = new BindingList<stdcolls>(stdcollection.Find(query).ToList());
                mainapp.gridStudents.DataSource = stdresult;

                int count = Convert.ToInt16(stdresult.Count());
                mainapp.ItemsCountStatusText.Caption = Convert.ToString(Properties.Settings.Default.StdFilterFlag) + " students count: " + Convert.ToString(count);
            //}
            //else
            //{
            //    int count = 0;
            //    mainapp.ItemsCountStatusText.Caption = Convert.ToString(Properties.Settings.Default.StdFilterFlag) + " students count: " + Convert.ToString(count);
            //}
        }

        public static void GroupComboListFill(studentform stdform)
        {
            MongoCollection<groupcolls> groupcoll = gebase.GetCollection<groupcolls>("groups");

            var result = groupcoll.FindAll()
                 .SetFields(Fields.Include("number"))
                 .ToList();

            stdform.stgroup.Properties.DataSource = result;
            stdform.stgroup.Properties.DisplayMember = "number";
            stdform.stgroup.Properties.ValueMember = "number";
            stdform.stgroup.Properties.ShowHeader = false;

            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("number", 25);
            col.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

            stdform.stgroup.Properties.Columns.Add(col);
        }
        
        public static void StdRemove(MainAppForm mainapp, string _id)
        {
            stdcollection.Remove(
                Query.EQ("_id", ObjectId.Parse(_id)));
            StdGridRefresh(mainapp);
        }

        public static void StdPause(MainAppForm mainapp, string _id)
        {
            stdcollection.Update(Query.EQ("_id", ObjectId.Parse(_id)), 
                MongoDB.Driver.Builders.Update.Set("status", "paused"));

            StdGridRefresh(mainapp);
        }

        public static void StdResume(MainAppForm mainapp, string _id)
        {
            stdcollection.Update(Query.EQ("_id", ObjectId.Parse(_id)),
                MongoDB.Driver.Builders.Update.Set("status", "active"));

            StdGridRefresh(mainapp);
        }

        public static void StdActionButton(MainAppForm mainapp, string _id, string status)
        {
            stdcollection.Update(Query.EQ("_id", ObjectId.Parse(_id)),
                MongoDB.Driver.Builders.Update.Set("status", status));

            StdGridRefresh(mainapp);
        }

        public static void StdTabShow(MainAppForm mainapp)
        {
            switch (Properties.Settings.Default.StdFilterFlag)
            {
                case "active":
                    mainapp.ActiveStudentsButton.Down = true;
                    StdGridRefresh(mainapp);
                    break;
                case "paused":
                    mainapp.PausedStudentsButton.Down = true;
                    StdGridRefresh(mainapp);
                    break;
                case "awaiting":
                    mainapp.AwaitingStudentsButton.Down = true;
                    StdGridRefresh(mainapp);
                    break;
                case "finished":
                    mainapp.FinishedStudentsButton.Down = true;
                    StdGridRefresh(mainapp);
                    break;
            }
        }

        public static void StdGridColHide(MainAppForm mainapp)
        {
            int i = 0;
            while (i <= 16)
            {
                mainapp.bandedStudentsGridView.Columns[i].VisibleIndex = -1;
                i++;
            }
        }

        public static void StdActionButtonsSwitch(MainAppForm mainapp, string status)
        {
            switch (status)
            {
                case "active":
                    mainapp.PauseStudentButton.Enabled = true;
                    mainapp.EditStudentButton.Enabled = true;
                    mainapp.ResumeStudentButton.Enabled = false;
                    mainapp.FinishStudentButton.Enabled = true;
                    mainapp.StartStudentButton.Enabled = false;
                    mainapp.DeleteStudentButton.Enabled = true;
                    break;
                case "paused":
                    mainapp.PauseStudentButton.Enabled = false;
                    mainapp.EditStudentButton.Enabled = true;
                    mainapp.ResumeStudentButton.Enabled = true;
                    mainapp.FinishStudentButton.Enabled = false;
                    mainapp.StartStudentButton.Enabled = false;
                    mainapp.DeleteStudentButton.Enabled = true;
                    break;
                case "awaiting":
                    mainapp.PauseStudentButton.Enabled = false;
                    mainapp.EditStudentButton.Enabled = true;
                    mainapp.ResumeStudentButton.Enabled = false;
                    mainapp.FinishStudentButton.Enabled = false;
                    mainapp.StartStudentButton.Enabled = true;
                    mainapp.DeleteStudentButton.Enabled = true;
                    break;
                case "finished":
                    mainapp.PauseStudentButton.Enabled = false;
                    mainapp.EditStudentButton.Enabled = true;
                    mainapp.ResumeStudentButton.Enabled = false;
                    mainapp.FinishStudentButton.Enabled = false;
                    mainapp.StartStudentButton.Enabled = false;
                    mainapp.DeleteStudentButton.Enabled = true;
                    break;
            }
        }

        public static void GroupDetails(MainAppForm mainapp)
        {
            MongoInitiate(mainapp);
            //client = new MongoClient(connectionString);
            //server = client.GetServer();
            //gebase = server.GetDatabase("gebase");
            //stddetailcollection = gebase.GetCollection<stdcolls>("stds");
            string number = mainapp.bandedGroupGridView.GetRowCellValue(mainapp.bandedGroupGridView.FocusedRowHandle, "number").ToString();
            mainapp.bandedDetailGroupGridView.GroupPanelText = "group " + number + " details";

            var query = Query.EQ("group", number);

            try
            {
                BindingList<stdcolls> stddetails = new BindingList<stdcolls>(stdcollection.Find(query).ToList());
                mainapp.gridGroupDetail.DataSource = stddetails;

                int i = 0;
                while (i <= 16)
                {
                    mainapp.bandedDetailGroupGridView.Columns[i].VisibleIndex = -1;
                    i++;
                }
                int j = 18;
                while (j <= mainapp.bandedDetailGroupGridView.Columns.Count)
                {
                    mainapp.bandedDetailGroupGridView.Columns[j].VisibleIndex = -1;
                    j++;
                }

                groupcode.GroupGridRefresh(mainapp);
            }
            catch
            {
                mainapp.gridGroup.DataSource = null;
                mainapp.bandedDetailGroupGridView.GroupPanelText = "...";
            }
            //string status = mainapp.bandedGroupGridView.GetRowCellValue(mainapp.bandedGroupGridView.FocusedRowHandle, "status").ToString();
            //ActionButtonSwitch(mainapp, status);

        }
    }
}
