using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace gebase_alpha_0._2._1
{
    
    public class groupcolls
    {
        public ObjectId _id { get; set; }
        public string number { get; set; }
        public int stdcount { get; set; }
        public string teacher { get; set; }
        public string level { get; set; }
        public string days { get; set; }
        public int hours { get; set; }
        public string kind { get; set; }
        public string status { get; set; }
        public string start { get; set; }
        public string time { get; set; }
        public string suntime { get; set; }
        public string montime { get; set; }
        public string tuetime { get; set; }
        public string wedtime { get; set; }
        public string thutime { get; set; }
        public string fritime { get; set; }
        public string sattime { get; set; }
    }

    public class stdcolls
    {
        public ObjectId _id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string address { get; set; }
        public DateTime bdate { get; set; }
        public int age { get; set; }
        public string program { get; set; }
        public string source { get; set; }
        public string daysposs { get; set; }
        public string timeposs { get; set; }
        public int cost { get; set; }
        public DateTime accepted { get; set; }
        /* flags */
        public Boolean isgroup { get; set; }
        public Boolean isindividual { get; set; }
        public Boolean isintensive { get; set; }
        /* ***** */
        public string level { get; set; }
        public string status { get; set; }

        public string fullname { get; set; }
        public string email { get; set; }
        public string mphone { get; set; }
        public string hphone { get; set; }
        public string addphone { get; set; }
        public string group { get; set; }
    }
    public class schedulercoll
    {
        public ObjectId _id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Subject { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Location { get; set; }
        public bool AllDay { get; set; }
        public int EventType { get; set; }
        public string RecurrenceInfo { get; set; }
        public string ReminderInfo { get; set; }
    }
}
