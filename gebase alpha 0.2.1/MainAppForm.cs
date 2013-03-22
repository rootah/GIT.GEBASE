using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraScheduler;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using Transitions;


namespace gebase_alpha_0._2._1
{
    public partial class MainAppForm : RibbonForm
    {
        /*  MONGO VARS DECLARE */

        public schedulercoll _schedentity;

        /*  END MONGO VARS DECLARE  */

        public MainAppForm()
        {
            InitializeComponent();
        }

        private void backstageViewButtonItem1_ItemClick(object sender, BackstageViewItemEventArgs e) /* COMPL */
        {
            Application.Exit();
        }

        private void ribbonControl_SelectedPageChanged(object sender, EventArgs e) /* COMPL */
        {
            Properties.Settings.Default.RibbonTabIndex = ribbonControl.SelectedPage.PageIndex;
            Properties.Settings.Default.Save();
            
            RibbonTabRefresh();

            xtraTabControl1.SelectedTabPageIndex = ribbonControl.SelectedPage.PageIndex;
        }

        private void RibbonTabRefresh()
        {
            ribbonControl.SelectedPage = ribbonControl.Pages[Properties.Settings.Default.RibbonTabIndex];

            switch (Properties.Settings.Default.RibbonTabIndex)
            {
                case 0:
                    {
                        groupcode.MongoInitiate();
                        groupcode.GroupTabShow(this);
                        groupcode.GroupDetailView(this);
                        groupcode.GroupGridColHide(this);
                        bandedGroupGridView.FocusedRowHandle = 0;
                        bandedGroupGridView_FocusedRowChanged(null, null);
                    }
                    break;
                case 1:
                    {
                        stdcode.MongoInitiate(this);
                        stdcode.StdTabShow(this);
                        stdcode.StdGridColHide(this);
                        bandedStudentsGridView.FocusedRowHandle = 0;
                        bandedStudentsGridView_FocusedRowChanged(null, null);
                    }
                    break;
                case 2:
                    {
                        SchedGrid.GoToToday();
                    }
                    break;
            }
        }

        private void MainAppForm_Load(object sender, EventArgs e)
        {
            RibbonTabRefresh();
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
        }

        private void ActiveGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {            
            Properties.Settings.Default.GroupFilterFlag = "active";
            Properties.Settings.Default.Save();
            if (ActiveGroupButton.Down)
                ActiveGroupButton.LargeGlyph = Properties.Resources.filterwhite;
            groupcode.GroupGridRefresh(this);
            
            //bandedGroupGridView.FocusedRowHandle = 0;

            bandedGroupGridView_FocusedRowChanged(null, null);
        }

        private void AwaitingGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFilterFlag = "awaiting";
            Properties.Settings.Default.Save();
            ActiveGroupButton.LargeGlyph = Properties.Resources.FilterG32;
            groupcode.GroupGridRefresh(this);
            
            //bandedGroupGridView.FocusedRowHandle = 0;

            bandedGroupGridView_FocusedRowChanged(null, null);
        }

        private void PausedGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFilterFlag = "paused";
            Properties.Settings.Default.Save();
            ActiveGroupButton.LargeGlyph = Properties.Resources.FilterG32;
            groupcode.GroupGridRefresh(this);
            bandedGroupGridView_FocusedRowChanged(null, null);
        }

        private void FinishedGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFilterFlag = "finished";
            Properties.Settings.Default.Save();
            ActiveGroupButton.LargeGlyph = Properties.Resources.FilterG32;
            groupcode.GroupGridRefresh(this);
            bandedGroupGridView_FocusedRowChanged(null, null);
        }

        private void PauseGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = bandedGroupGridView.FocusedRowHandle;
            string groupstatus = "paused";
            groupcode.GroupAction(this, rowhandle, groupstatus);
        }

        private void ResumeGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = bandedGroupGridView.FocusedRowHandle;
            string groupstatus = "active";

            groupcode.GroupAction(this, rowhandle, groupstatus);
        }

        private void StartGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = bandedGroupGridView.FocusedRowHandle;
            string groupstatus = "active";
            groupcode.GroupAction(this, rowhandle, groupstatus);
        }

        private void FinishGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = bandedGroupGridView.FocusedRowHandle;
            string groupstatus = "finished";
            groupcode.GroupAction(this, rowhandle, groupstatus);
        }

        private void DeleteGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MessageBox.Show("Really delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string _id = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "_id").ToString();
                groupcode.GroupDel(this, _id);
            }
        }

        private void AddGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFormType = "add";
            groupform groupadd = new groupform(this);
            groupadd.StartPosition = FormStartPosition.CenterParent;
            groupadd.simpleButtonOk.Visible = true;
            groupadd.simpleButtonEdit.Visible = false;
            groupadd.ShowDialog();
        }

        private void EditGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFormType = "edit";
            groupform groupadd = new groupform(this);
            groupadd.StartPosition = FormStartPosition.CenterParent;
            groupadd.simpleButtonOk.Visible = true;
            groupadd.simpleButtonEdit.Visible = false;
            groupadd.ShowDialog();
        }

        private void barButtonItem2_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            //Appointment lesson = GETimeTable.Storage.CreateAppointment(AppointmentType.Normal);
            //lesson.Start = Convert.ToDateTime(_groupentity.time);
            //lesson.Duration = TimeSpan.FromMinutes(_groupentity.hours * 45);
            //lesson.Subject = _groupentity.number;
            //lesson.Description = _groupentity.teacher;
            //GETimeTable.Storage.Appointments.Add(lesson);

            //MongoCollection<schedulercoll> schedcollection = gebase.GetCollection<schedulercoll>("sched");
            //var schedresult = schedcollection.FindAll().ToList();
            //SchedGrid.GoToToday();
           
            //GESchedulerStorage.Appointments.DataSource = schedresult;
            //GESchedulerStorage.Appointments.Mappings.AllDay = "AllDay";
            //GESchedulerStorage.Appointments.Mappings.Start = "StartTime";
            //GESchedulerStorage.Appointments.Mappings.End = "EndTime";
            //GESchedulerStorage.Appointments.Mappings.Subject = "Name";
            //GESchedulerStorage.Appointments.Mappings.Label = "Label";
            //GESchedulerStorage.Appointments.Mappings.Location = "Location";
            //GESchedulerStorage.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";
            //GESchedulerStorage.Appointments.Mappings.ReminderInfo = "ReminderInfo";
            //GESchedulerStorage.Appointments.Mappings.Description = "Description";

            //var query = Query.EQ("status", Properties.Settings.Default.GroupFilterFlag);

        }

        /* STUDENTS TAB */

        private void AddStudentsButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.StdFormType = "add";
            studentform stdadd = new studentform(this);
            stdadd.StartPosition = FormStartPosition.CenterParent;
            stdadd.Text = stdadd.Text + " [new]";
            stdadd.ShowDialog();
        }

        private void DeleteStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MessageBox.Show("Really delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string _id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
                stdcode.StdRemove(this, _id);
            }
        }

        private void PauseStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string _id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
            stdcode.StdActionButton(this, _id, "paused");
        }

        private void ResumeStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string _id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
            stdcode.StdActionButton(this, _id, "active");
        }

        private void StartStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string _id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
            stdcode.StdActionButton(this, _id, "active");
        }

        private void FinishStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string _id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
            stdcode.StdActionButton(this, _id, "finished");
        }

        private void ActiveStudentsButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            if (ActiveStudentsButton.Down)
                ActiveStudentsButton.Glyph = Properties.Resources.userwhite;
            else ActiveStudentsButton.Glyph = Properties.Resources.userfilter1;
            Properties.Settings.Default.StdFilterFlag = "active";
            Properties.Settings.Default.Save();

            stdcode.StdGridRefresh(this);
            bandedStudentsGridView_FocusedRowChanged(null, null);
        }

        private void PausedStudentsButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            ActiveStudentsButton.Glyph = Properties.Resources.userfilter1;
            Properties.Settings.Default.StdFilterFlag = "paused";
            Properties.Settings.Default.Save();

            stdcode.StdGridRefresh(this);
            bandedStudentsGridView_FocusedRowChanged(null, null);
        }

        private void AwaitingStudentsButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            ActiveStudentsButton.Glyph = Properties.Resources.userfilter1;
            Properties.Settings.Default.StdFilterFlag = "awaiting";
            Properties.Settings.Default.Save();

            stdcode.StdGridRefresh(this);
            bandedStudentsGridView_FocusedRowChanged(null, null);
        }

        private void FinishedStudentsButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            ActiveStudentsButton.Glyph = Properties.Resources.userfilter1;
            Properties.Settings.Default.StdFilterFlag = "finished";
            Properties.Settings.Default.Save();

            stdcode.StdGridRefresh(this);
            bandedStudentsGridView_FocusedRowChanged(null, null);
        }

        private void EditStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.StdFormType = "edit";
            studentform stdadd = new studentform(this);
            stdadd.StartPosition = FormStartPosition.CenterParent;
            stdadd.Text = stdadd.Text + " [edit]";
            
            stdadd.ShowDialog();
        }

        private void bandedStudentsGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                string status = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "status").ToString();
                stdcode.StdActionButtonsSwitch(this, status);
            }
            catch
            {
                PauseStudentButton.Enabled = false;
                ResumeStudentButton.Enabled = false;
                FinishStudentButton.Enabled = false;
                StartStudentButton.Enabled = false;
                DeleteStudentButton.Enabled = false;
                EditStudentButton.Enabled = false;
            }
        }

        private void bandedGroupGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                string groupstatus = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "status").ToString();
                int focusedrow = bandedGroupGridView.FocusedRowHandle;
                groupcode.ActionButtonSwitch(this, groupstatus, focusedrow);

                //string number = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "number").ToString();
            }
            catch
            {
                PauseGroupButton.Enabled = false;
                ResumeGroupButton.Enabled = false;
                FinishGroupButton.Enabled = false;
                StartGroupButton.Enabled = false;
                DeleteGroupButton.Enabled = false;
                EditGroupButton.Enabled = false;

                //gridGroupDetail.DataSource = null;
                //bandedDetailGroupGridView.GroupPanelText = "group ... detail";
            }
            finally
            {
                //try
                ////if (bandedGroupGridView.RowCount > 0)
                //{
                    //string number = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "number").ToString();
                    //bandedDetailGroupGridView.GroupPanelText = "group " + number + " details";
                    //stdcode.MongoInitiate(this);
                    //stdcode.GroupDetails(this, number);
                //}
                //catch
                //{
                //    //gridGroupDetail.DataSource = null;
                //    //bandedDetailGroupGridView.GroupPanelText = "...";
                //}
            }
        }
        public void DetailGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            if (DetailGroupButton.Down)
            {
                Properties.Settings.Default.GroupDetailsShow = true;
                Properties.Settings.Default.Save();
                splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            }
            else
            {
                Properties.Settings.Default.GroupDetailsShow = false;
                Properties.Settings.Default.Save();
                splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            }
        }
    }
}