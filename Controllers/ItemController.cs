/*
' Copyright (c) 2020 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Linq;
using System.Web.Mvc;
using Modules.OrganizationOrganization.Components;
using Modules.OrganizationOrganization.Models;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using Newtonsoft.Json;
using System.IO;
using System.Web;

namespace Modules.OrganizationOrganization.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {

        public ActionResult Delete(int itemId)
        {
            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }

        public ActionResult Edit(int itemId = -1)
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);

            var userlist = UserController.GetUsers(PortalSettings.PortalId);
            var users = from user in userlist.Cast<UserInfo>().ToList()
                        select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };

            ViewBag.Users = users;

            var item = (itemId == -1)
                 ? new Item { ModuleId = ModuleContext.ModuleId }
                 : ItemManager.Instance.GetItem(itemId, ModuleContext.ModuleId);

            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (item.ItemId == -1)
            {
                item.CreatedByUserId = User.UserID;
                item.CreatedOnDate = DateTime.UtcNow;
                item.LastModifiedByUserId = User.UserID;
                item.LastModifiedOnDate = DateTime.UtcNow;

                ItemManager.Instance.CreateItem(item);
            }
            else
            {
                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.ItemName = item.ItemName;
                existingItem.ItemDescription = item.ItemDescription;
                existingItem.AssignedUserId = item.AssignedUserId;

                ItemManager.Instance.UpdateItem(existingItem);
            }

            return RedirectToDefaultRoute();
        }

        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            var items = ItemManager.Instance.GetItems(ModuleContext.ModuleId);
            ViewBag.organization = ItemManager.Instance.GetOrganization();
            return View(items);
        }

        [HttpPost]
        public ActionResult SaveOrganization(Organization organization)
        {
           
            var file = Request.Files["HelpSectionImages"];
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fieldname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var paths = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            /*    try
                {
                    if (!Directory.Exists(paths))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(paths);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }*/

                file.SaveAs(paths);
                paths =paths.ToString().Replace("\\", "/");
                organization.ImagePath = paths;
            }
            ItemManager.Instance.SaveOrganization(organization);
            Organization orgs = new Organization();
            orgs.Name = organization.Name;
            orgs.Code = organization.Code;
            orgs.ImagePath = organization.ImagePath;
            return Json(new { data = JsonConvert.SerializeObject(orgs, Formatting.Indented) }, JsonRequestBehavior.AllowGet);
        }

        

        //[HttpPost]
        //public JsonResult UploadFiles()
        //{
        //    // Checking no of files injected in Request object  
        //    if (Request.Files.Count > 0)
        //    {
        //        try
        //        {
        //            //  Get all files from Request object  
        //            HttpFileCollectionBase files = Request.Files;
        //            for (int i = 0; i < files.Count; i++)
        //            {
        //                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
        //                //string filename = Path.GetFileName(Request.Files[i].FileName);  

        //                HttpPostedFileBase file = files[i];
        //                string fname;

        //                // Checking for Internet Explorer  
        //                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //                {
        //                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //                    fname = testfiles[testfiles.Length - 1];
        //                }
        //                else
        //                {
        //                    fname = file.FileName;
        //                }

        //                // Get the complete folder path and store the file inside it.  
        //                fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
        //                file.SaveAs(fname);
        //            }
        //            // Returns message that successfully uploaded  
        //            return Json("File Uploaded Successfully!");
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json("Error occurred. Error details: " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        return Json("No files selected.");
        //    }
        //}

        [HttpGet]
        public JsonResult GetOrganization(string organizationId)
        {

            try
            {
                var organizations = ItemManager.Instance.GetOrganization(int.Parse(organizationId));
                return Json(new { data = JsonConvert.SerializeObject(organizations, Formatting.Indented) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                throw e;
            }

        }


        [HttpGet]
        public JsonResult DeleteOrganization(string organizationId)
        {

            try
            {
                var delEm = ItemManager.Instance.DeleteOrganizatio(int.Parse(organizationId));
                return Json(new { data = JsonConvert.SerializeObject(delEm, Formatting.Indented) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        //public JsonResult InsertProduct(Organization prdObj)
        //{
        //    var profile = Request.Files;
        //    string imgname = string.Empty;
        //    string ImageName = string.Empty;
        //    //Following code will check that image is there
        //    //If it will Upload or else it will use Default Image
        //    if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
        //    {
        //        var logo = System.Web.HttpContext.Current.Request.Files["file"];
        //        if (logo.ContentLength > 0)
        //        {
        //            var profileName = Path.GetFileName(logo.FileName);
        //            var ext = Path.GetExtension(logo.FileName);
        //            ImageName = profileName;
        //            var comPath = Server.MapPath("~/Images/") + ImageName;
        //            logo.SaveAs(Path.Combine(Server.MapPath("~/Images/"), ImageName));
        //            prdObj.ImagePath = comPath;
        //            // prdObj.Add()
        //            return Json(db.InsertProduct(prdObj.Name, prdObj.ImagePath, prdObj.Code, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //        prdObj.ImagePath = Server.MapPath("~/Images/") + "profile.jpg";
        //    int response = 1;
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

    }
}
