
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using TestMVC001.Core.Models;
using TestMVC001.Core.Services;
using TestMVC001.Models;

namespace TestMVC001.Controllers
{
    [Authorize(Roles = "SysAdmin,Admin,Staff")]
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            var groupsModel = new GroupsModel();
            return View(groupsModel);
        }

        [Route("gettree")]
        public JsonResult GetTree()
        {

            var entityItemsList = NotificationService.GetSMSTree("97005");  //TODO pass ORgID to db
            var groups = new List<Group>();
            //Convert to tree
            //Step 1: Find all parents
            foreach (var entityItem in entityItemsList)
            {
                if (entityItem.Parent == null || String.IsNullOrEmpty(entityItem.Parent))
                {
                    var group = new Group
                    {
                        Id = entityItem.Id,
                        GroupName = entityItem.Entity,
                        ChildGroups = new List<Group>()
                    };
                    groups.Add(group);
                }
            }
            entityItemsList.RemoveAll(item => item.Parent == null || String.IsNullOrEmpty(item.Parent));
            FindParent(entityItemsList, ref groups);
            return new JsonResult { Data = groups, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Route("sendnotification")]
        public ContentResult SendNotification(NotificationModel model)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            const string notificationSender = "School Admin";
            const string notificationCategory = "Test";
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO [dbo].[tblNotification] ([PhoneNumber],[Sender],[Message],[Category]) VALUES('" + model.ToPhoneNumber + "', '" + notificationSender + "',  '" + model.Message + "',   '" + notificationCategory + "') ";
                var cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            var smsResponseModel = SendSmsService.SendNotification(model.ToPhoneNumber, model.Message);
            AttendanceService.InsertSmsResponse(smsResponseModel);
            //return new JsonResult { Data = smsResponseModel, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            if (smsResponseModel.Status == "Success")
            {
                return Content("Message Sent!");
            }
            else
            {
                return Content(smsResponseModel.Status);
            }
        }

        [Route("notification/getphonenumbers"), HttpGet]
        public JsonResult GetPhoneNumbers(List<string> groupIdsList)
        {
            var phoneNumbers = NotificationService.GetPhoneNumbers(groupIdsList, "97005");  //TODO pass ORgID to db
            return new JsonResult { Data = phoneNumbers, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
        public void FindParent(List<EntityItem> entityItemsList, ref List<Group> groups)
        {
            //Step 2: Find all children for each parent
            while (entityItemsList.Count > 0)
            {
                var itemsToRemove = new List<EntityItem>();
                foreach (var entityItem in entityItemsList)
                {
                    var group = new Group
                    {
                        Id = entityItem.Id,
                        GroupName = entityItem.Entity,
                        ChildGroups = new List<Group>()
                    };
                    foreach (var parent in groups)
                    {
                        if (entityItem.Parent == parent.Id)
                        {
                            parent.ChildGroups.Add(group);
                            itemsToRemove.Add(entityItem);
                        }
                        else
                        {
                            if (parent.ChildGroups.Count > 0)
                            {
                                foreach (var child in parent.ChildGroups)
                                {
                                    if (child.Id == entityItem.Parent)
                                    {
                                        child.ChildGroups.Add(group);
                                        itemsToRemove.Add(entityItem);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                entityItemsList.RemoveAll(itemsToRemove.Contains);
            }
        }
    }

    public class Group
    {
        public string Id;
        public string GroupName;
        public List<Group> ChildGroups;
    }
}
