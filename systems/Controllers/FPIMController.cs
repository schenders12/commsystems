using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using systems.Models;
using systems.Helpers;
using System.Web.Mvc.Ajax;
using System.IO;
using System.Web.Routing;
using Newtonsoft.Json;
using System.Net.Mail;

namespace systems.Controllers
{
    public class FPIMController : Controller
    {
        private PeopleEntities db = new PeopleEntities();
        private CatalogEntities catalogDB = new CatalogEntities();

        // Default page
        public ActionResult Index(string id, string suad, string esfad, string firstname, string lastname)
        {
            if (id == null || id == "")
            {
                // Viewing your own
                id = User.Identity.Name;
            }

            // Get Employee record
            CommEmployee employee = fns.GetEmployee(id, suad, esfad, firstname, lastname);
            if (employee == null) {
                return null;
            }

            FacultyProfile profile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);
            if (profile == null)
            {
                // User does not have a profile, create one
                // If a profile already exists with this last name, create firstname.lastname profile
                FacultyProfile p = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == lastname);
                string profileID = null;
                if (p != null)
                {
                    profileID = firstname + '.' + lastname;
                }
                else
                {
                    profileID = lastname;
                }
                // Create profile
                int result = db.spCreateFacultyProfile(profileID, id, suad, esfad, firstname, lastname);
                profile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);

            }

            if (id != User.Identity.Name)
            {
                ViewBag.WelcomeMsg = "Editing for " + employee.FirstName + " " + employee.LastName;
            }
            else
            {
                ViewBag.WelcomeMsg = "Welcome, " + employee.FirstName + " " + employee.LastName + "!";
            }
            
            ViewBag.fname = employee.FirstName;
            ViewBag.lname = employee.LastName;
            ViewBag.userid = employee.UserId;
            if (employee.MiddleName != "")
                ViewBag.PTitle = "SUNY-ESF: " + employee.FirstName + " " + employee.MiddleName + " " + employee.LastName + " - " + "Profile";
            else
                ViewBag.PTitle = "SUNY-ESF: " + employee.FirstName + " " + employee.LastName + " - " + "Profile";
            ViewBag.PageTitle = "Profile";

            ViewBag.profileId = profile.ProfileId;

            // Get the main page ID
            List<FacultyPage> pages = db.FacultyPages.Where(m => m.ProfileId == profile.ProfileId).ToList();
            FacultyPage page = pages.Find(m => m.PageTitle == "Main");
            ViewBag.MainPageID = page.FacultyPageId;
            return View(profile);
        }

        // *** Manage Modules ***
        [HttpGet]
        public ActionResult ManageModules(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            ViewBag.profileId = profileId;
            return View(myProfile);
        }

        // *** Manage Pages ***
        [HttpGet]
        public ActionResult ManagePages(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            ViewBag.profileId = profileId;
            return View(myProfile);
        }

        // *** Submit a photo ***
        [HttpGet]
        public ActionResult SubmitPhoto(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            CommEmployee employee = db.CommEmployees.Single(m => m.UserId == myProfile.UserId || m.EmailId == myProfile.ESFAD + "@esf.edu%" || m.EmailId == myProfile.ESFAD + "@syr.edu%");

            ProfilePhoto myPhoto = new ProfilePhoto(profileId, employee.FirstName, employee.LastName, employee.EmailId);
            return View(myPhoto);
        }
        [HttpPost]
        public ActionResult SubmitPhoto(ProfilePhoto myPhoto)
        {
            myPhoto.profileId = Request.Form["profileId"];
            myPhoto.profileFirstName = Request.Form["profileFirstName"];
            myPhoto.profileLastName = Request.Form["profileLastName"];
            myPhoto.profileEMail = Request.Form["profileEMail"];

            // Create the Mail message
            MailMessage mail = new MailMessage();
            // Get the attachment
            if (myPhoto.Attachment != null && myPhoto.Attachment.ContentLength > 0)
            {
                var attachment = new Attachment(myPhoto.Attachment.InputStream, myPhoto.Attachment.FileName);
                mail.Attachments.Add(attachment);
            }

           // string strFileName = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            // string attachmentPath = Environment.CurrentDirectory + @"\test.txt";
           // if (strFileName != "")
           // {
           //     Attachment item = new Attachment(FileUpload1.PostedFile.InputStream, strFileName);
           //     mail.Attachments.Add(item);
           // }

            //Set From , To and CC
            var name = myPhoto.profileFirstName + " " + myPhoto.profileLastName;
            mail.From = new MailAddress("annonymous@esf.edu", name);
            mail.To.Add(new MailAddress("schender@esf.edu"));
            mail.CC.Add(new MailAddress(myPhoto.profileEMail));
           // mail.Bcc.Add(new MailAddress("schender@esf.edu"));
            mail.Subject = "New Profile Photo from " + name;
            // mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

            // Fill in body info
           // string Locationstr = Location.Text;
           // string Emailstr = Email.Text;
            //string Groupstr = Group.Text;

            mail.Body = @"Please use this new picture as my profile photo.  " + "\n";

            // Create the email client and send mail
            SmtpClient smtpClient = new SmtpClient("149.119.6.91", 25);
            smtpClient.Send(mail);

            return RedirectToAction("Index", new { profileId = myPhoto.profileId });

         /*   if (Request.Files.Count > 0)
            {
                foreach (string file in Request.Files)
                {
                    string pathFile = string.Empty;
                    if (file != null)
                    {
                        string path = string.Empty;
                        string fileName = string.Empty;
                        string fullPath = string.Empty;
                        path = AppDomain.CurrentDomain.BaseDirectory + "directory where you want to upload file";//here give the directory where you want to save your file
                        if (!System.IO.Directory.Exists(path))//if path do not exit
                        {
                            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "directory_name/");//if given directory dont exist, it creates with give directory name
                        }
                        fileName = Request.Files[file].FileName;

                        fullPath = Path.Combine(path, fileName);
                        if (!System.IO.File.Exists(fullPath))
                        {
                            if (fileName != null && fileName.Trim().Length > 0)
                            {
                                Request.Files[file].SaveAs(fullPath);
                            }
                        }
                    }
                }
            }*/

        }

        // *** Add a page ***
        [Authorize]
        public ActionResult AddFacultyPage(string id)
        {
            FacultyPage myPage = new FacultyPage();
            myPage.ProfileId = id;
            myPage.FacultyPageId = -1;
            myPage.PageTitle = "";

            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~\faculty\" + myPage.ProfileId));
            string fullPath = Server.MapPath("~/faculty/" + myPage.ProfileId + "/");
            ViewBag.LinkURL = fullPath;
            Console.Write("Adding New Page");
            return View(myPage);
        }
        // *** Add an external page ***
        public ActionResult AddFacultyExternalLink(string id)
        {
            FacultyPage myPage = new FacultyPage();
            myPage.ProfileId = id;
            myPage.FacultyPageId = -1;
            myPage.PageTitle = "";
            Console.Write("Adding New Page");
            return View(myPage);
        }

        [HttpPost]
        // *** Add a page ***
       // public ActionResult AddFacultyPage(FacultyPage myPage)
        public ActionResult AddFacultyPage(List<string> items, string title, string profileID)
        {
            FacultyPage myPage = new FacultyPage();
            myPage.PageTitle = title;
            myPage.ProfileId = profileID;
 
            // Set the link URL
            // myPage.LinkURL = myPage.LinkURL + myPage.PageTitle;

            // Loop over all added modules
           // string modules = Request.Form["addMe"];
            myPage.FacultyProfileModules = new EntityCollection<FacultyProfileModule>();
            if (items != null)
            {
              //  var parts = modules.Split(',');
                foreach (var item in items)
                {
                    int selectedModuleId = Convert.ToInt32(item);
                    // Retrieve the data from the existing module and create a new module
                    FacultyProfileModule oldMod = db.FacultyProfileModules.First(m => m.FacultyProfileModuleId == selectedModuleId);
                    FacultyProfileModule newMod = new FacultyProfileModule();

                    newMod.ProfileId = myPage.ProfileId;
                    newMod.FacultyPageId = oldMod.FacultyPageId;
                    newMod.ModuleType = oldMod.ModuleType;
                    newMod.ModuleTitle = oldMod.ModuleTitle;
                    newMod.ModuleData = oldMod.ModuleData;
                    newMod.DisplayOrder = oldMod.DisplayOrder;

                    // Add the module dta to the page
                    myPage.FacultyProfileModules.Add(newMod);

                }
            }
            // Add the Page to the database.  Modules added based on Entity Framework Foreign Key relationship.
            if (ModelState.IsValid)
            {
                db.FacultyPages.AddObject(myPage);
                db.SaveChanges();
            }
           // return RedirectToAction("ManagePages", new { profileId = profileID });
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ManagePages", new { profileId = profileID });
            return Json(new { Url = redirectUrl });
        }

        [HttpPost]
        // *** Add an External page ***
        public ActionResult AddFacultyExternalLink(FacultyPage myPage)
        {
            if (ModelState.IsValid)
            {
                db.FacultyPages.AddObject(myPage);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // *** Edit a Page ***
        [HttpGet]
        public ActionResult EditFacultyPage(string profileId, int pageId)
        {
            FacultyPage page = db.FacultyPages.First(m => m.FacultyPageId == pageId);
            return View(page);
        }

        [HttpPost]
        public ActionResult EditFacultyPage(List<string> items, string title, int id)
        {
            FacultyPage page = new FacultyPage();
            page = db.FacultyPages.First(m => m.FacultyPageId == id);
            page.PageTitle = title;

            // for each profile, save the display order
            List<FacultyProfileModule> modules = db.FacultyProfileModules.Where(m => m.FacultyPageId == id).ToList();
            foreach (FacultyProfileModule module in modules)
            {
                int index = items.IndexOf(module.FacultyProfileModuleId.ToString());
                module.DisplayOrder = index;
            }
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }
            //return RedirectToAction("ManagePages", new { profileId = page.ProfileId });
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ManagePages", new { profileId = page.ProfileId });
            return Json(new { Url = redirectUrl });
        }

        // *** View a Page ***
        [HttpGet]
        public ActionResult ViewFacultyPage(string profileId, int pageId, string fname, string lname)
        {
            // Redirect to viewer 
            FacultyProfile profile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            // Get Employee record
            CommEmployee employee = fns.GetEmployee(profile.UserId, profile.SUAD, profile.ESFAD, fname, lname);
            if (employee == null)
            {
                return null;
            }

            ViewBag.fname = employee.FirstName;
            ViewBag.lname = employee.LastName;
            ViewBag.userid = employee.UserId;
            if (employee.MiddleName != "")
                ViewBag.PTitle = "SUNY-ESF: " + employee.FirstName + " " + employee.MiddleName + " " + employee.LastName + " - " + "Profile";
            else
                ViewBag.PTitle = "SUNY-ESF: " + employee.FirstName + " " + employee.LastName + " - " + "Profile";
            ViewBag.PageTitle = "Main";

            ViewBag.profileId = profile.ProfileId;

            List<FacultyPage> pages = db.FacultyPages.Where(m => m.ProfileId == profileId).ToList();
            FacultyPage page = pages.Find(m => m.FacultyPageId == pageId);
            // Set the department to display the correct banner
            ControllerContext.RouteData.Values["dept"] = profile.Department;
            ViewBag.PageId = pageId;
            ViewBag.dept = profile.Department;
            return View(profile);
        }

        // *** Delete a Page ***
        [HttpGet]
        public ActionResult DeletePagePartial(string pageId)
        {
            int selectedPageId = Convert.ToInt32(pageId);
            FacultyPage myPage = db.FacultyPages.First(m => m.FacultyPageId == selectedPageId);
            return PartialView("DeletePagePartial", myPage);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeletePagePartial(FacultyPage myPage)
        {
            if (ModelState.IsValid)
            {
                // Get all the modules contained in this page and delete
                List<FacultyProfileModule> modules = db.FacultyProfileModules.Where(m => m.FacultyPageId == myPage.FacultyPageId).ToList();
                foreach (FacultyProfileModule module in modules)
                {
                    db.DeleteObject(module);
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                    }
                }
                // Delete the page
                FacultyPage pageUpdate = new FacultyPage();
                pageUpdate = db.FacultyPages.Single(m => m.FacultyPageId == myPage.FacultyPageId);
                db.DeleteObject(pageUpdate);
                db.SaveChanges();
            }
            //return RedirectToAction("ManagePages", new { profileId = myPage.ProfileId });
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ManagePages", new { profileId = myPage.ProfileId });
            return Json(new { Url = redirectUrl });
        }

        // *** Choose a Page Type ***
        [HttpGet]
        public ActionResult ChoosePageTypePartial(string profileID)
        {
            ViewBag.profileID = profileID;
            return PartialView("ChoosePageTypePartial");
        }
        [HttpPost]
        public ActionResult ChoosePageTypePartial()
        {
            string profileID = Request.Form["profileID"];
            string modType = Request.Form["PageType"];
            switch (modType)
            {
                case "Internal":
                    return RedirectToAction("AddFacultyPage", new { id = profileID });
                case "External":
                    return RedirectToAction("AddFacultyExternalLink", new { id = profileID });
                default:
                    return RedirectToAction("FPIM/Index");
            }
        }

        // *** Choose a Module Type ***
        [HttpGet]
        public ActionResult ChooseModuleTypePartial(string profileID)
        {
            ViewBag.profileID = profileID;
            return PartialView("ChooseModuleTypePartial");
        }
        [HttpPost]
        public ActionResult ChooseModuleTypePartial()
        {
            string profileID = Request.Form["profileID"];
            string modType = Request.Form["ModuleType"];
            switch (modType)
            {
                case  "HTML":
                  return RedirectToAction("AddHTMLModule", new { profileID = profileID });
                case "FileInclude":
                  return RedirectToAction("AddFileModule", new { profileID = profileID });
                case "GradModule":
                  return RedirectToAction("AddGradModule", new { profileID = profileID });
                case "Publications":
                  return RedirectToAction("AddPubsModule", new { profileID = profileID });
                case "Catalog":
                  return RedirectToAction("AddCatalogModule", new { profileID = profileID });
               default:
                  return RedirectToAction("ManageModules", new { profileId = profileID });
            }
        }


        // *** Add an HTML Module ***
        [HttpGet]
        public ActionResult AddHTMLModule(string profileID)
        {
            FacultyProfileModule htmlMod = new FacultyProfileModule();
            htmlMod.ProfileId = profileID;
            htmlMod.FacultyPageId = -1;
            htmlMod.ModuleType = "HTML";
            htmlMod.DisplayOrder = -1;
            Console.Write("Adding Faculty Module of type HTML");

            return View(htmlMod);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddHTMLModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                db.FacultyProfileModules.AddObject(module);
                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }

        // *** Add and delete documents ***
        [HttpGet]
        public ActionResult ManageFiles(string profileId)
        {
            Console.Write("Managing files...");
            ViewBag.profileID = profileId;
            JsonResult myFiles = GetCourses(profileId, "Fall 2014");
            ViewBag.myFiles = myFiles;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddFileModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                db.FacultyProfileModules.AddObject(module);
                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }

        // *** Link to a file ***
        [HttpGet]
        public ActionResult LinkToFile(string profileId)
        {
            ViewBag.profileID = profileId;
            //return View();
            return PartialView();
        }

        // *** Add Grad Module ***
        [HttpGet]
        public ActionResult AddGradModule(string profileID)
        {
            FacultyProfileModule fileMod = new FacultyProfileModule();
            fileMod.ProfileId = profileID;
            fileMod.FacultyPageId = -1;
            fileMod.ModuleType = "Grad";
            fileMod.DisplayOrder = -1;
            Console.Write("Adding Grad Faculty Module...");

            return View(fileMod);
            // return null;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddGradModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                db.FacultyProfileModules.AddObject(module);
                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }

        // *** Add Catalog Module ***
        [HttpGet]
        public ActionResult AddCatalogModule(string profileID)
        {
            JsonResult catModules = GetCourses(profileID, "Fall 2014");
            ViewBag.catModules = catModules;

            FacultyProfileModule catMod = new FacultyProfileModule();
            catMod.ProfileId = profileID;
            catMod.FacultyPageId = -1;
            catMod.ModuleType = "Catalog";
            catMod.DisplayOrder = -1;

            return View(catMod);
            // return null;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCatalogModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                db.FacultyProfileModules.AddObject(module);
                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }

        // *** Add Publications Module ***
        [HttpGet]
        public ActionResult AddPubsModule(string profileID)
        {
            FacultyProfileModule pubMod = new FacultyProfileModule();
            pubMod.ProfileId = profileID;
            pubMod.FacultyPageId = -1;
            pubMod.ModuleType = "Grad";
            pubMod.DisplayOrder = -1;
            Console.Write("Adding Publications Faculty Module...");

            return View(pubMod);
            // return null;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPubsModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                db.FacultyProfileModules.AddObject(module);
                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }


        // *** Edit a Module ***
        [HttpGet]
        public ActionResult EditFacultyModule(string profileId, int moduleId)
        {
            //int selectedModuleId = Convert.ToInt32(id);
            FacultyProfileModule module = db.FacultyProfileModules.First(m => m.FacultyProfileModuleId == moduleId);
            return View("EditFacultyModule", module);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditFacultyModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                FacultyProfileModule modUpdate = new FacultyProfileModule();
                modUpdate = db.FacultyProfileModules.Single(m => m.FacultyProfileModuleId == module.FacultyProfileModuleId);

                // Save original title
                var originalTitle = modUpdate.ModuleTitle;

                modUpdate.ModuleData = module.ModuleData;
                modUpdate.ModuleTitle = module.ModuleTitle;
                modUpdate.ModuleType = module.ModuleType;

                // Update all modules with identical titles
                List<FacultyProfileModule> modules = db.FacultyProfileModules.Where(m => m.ModuleTitle == originalTitle).ToList();
                foreach (FacultyProfileModule mod in modules)
                {
                    if (mod.ModuleType == module.ModuleType)
                    {
                        mod.ModuleData = module.ModuleData;
                        mod.ModuleTitle = module.ModuleTitle;
                        mod.ModuleType = module.ModuleType;
                    }
                }

                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }

        // *** Delete a Module ***
        [HttpGet]
        public ActionResult DeleteModulePartial(string id)
        {
            int selectedModuleId = Convert.ToInt32(id);
            FacultyProfileModule module = db.FacultyProfileModules.First(m => m.FacultyProfileModuleId == selectedModuleId);
            return PartialView("DeleteModulePartial", module);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteModulePartial(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
               // Get a list of all the modules with this Title for this user
                List<FacultyProfileModule> modules = db.FacultyProfileModules.Where(m => m.ProfileId == module.ProfileId).Where(m => m.ModuleTitle == module.ModuleTitle).ToList();
                // Delete the module
                foreach (FacultyProfileModule pageModule in modules)
                {
                    db.DeleteObject(pageModule);
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }


        //  *** View a Module ***
        public ActionResult ViewFacultyModule(int id, string PageTitle)
        {
            FacultyProfileModule module = db.FacultyProfileModules.First(m => m.FacultyProfileModuleId == id);
            return View(module);
        }

        //  *** View a Module ***
        public ActionResult ViewModulePartial(int id)
        {
            FacultyProfileModule module = db.FacultyProfileModules.First(m => m.FacultyProfileModuleId == id);
            return PartialView(module);
        }


        // Faculty/Staff Department Associations and Areas of Study
        [HttpGet]
        public ActionResult EditAssoc(string id)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);
          //  List<spFetchFacultyAOSAssocList_Result> depts = db.spFetchFacultyAOSAssocList(myProfile.UserId,myProfile.ESFAD,myProfile.SUAD).ToList();
            //List<spFetchFacultyAOSList_Result> aos = db.spFetchFacultyAOSList(myProfile.UserId, myProfile.ESFAD, myProfile.SUAD).ToList();
           // ViewBag.profileId = myProfile.ProfileId;
           // ViewBag.userID = myProfile.UserId;
            //return View(depts);
            return null;
        }
        [HttpPost]
        public ActionResult EditAssoc(spFetchFacultyAOSAssocList_Result updatedAssoc)
        {
            string assoc = Request.Form["assoc"];
            string id = Request.Form["userID"];
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);
            fns.ProcessAssociation(myProfile.UserId, myProfile.ESFAD, myProfile.SUAD, assoc);

            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Faculty Areas of Study (AOS
        [HttpGet]
        public ActionResult EditAOS(string id)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);
           // List<spFetchFacultyAOSAssocList_Result> depts = db.spFetchFacultyAOSAssocList(myProfile.UserId, myProfile.ESFAD, myProfile.SUAD).ToList();
            ViewBag.profileId = myProfile.ProfileId;
            ViewBag.userID = myProfile.UserId;
          //  return View(depts);
            return null;
        }
        [HttpPost]
        public ActionResult EditAOS(spFetchFacultyAOSAssocList_Result updatedAssoc)
        {
            string assoc = Request.Form["assoc"];
            string id = Request.Form["userID"];
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);
            fns.ProcessAssociation(myProfile.UserId, myProfile.ESFAD, myProfile.SUAD, assoc);
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Faculty/Staff Dept Associations/Areas of Study (AOS)
        [HttpGet]
        public ActionResult ManageDeptAssocAOS(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            //ViewBag.profileId = myProfile.ProfileId;
            //ViewBag.userID = myProfile.UserId;
            //ViewBag.dept = myProfile.Department;
            //ViewBag.reconciledAreas = myProfile.ReconciledAreas;

           // List<FacultyAOSAssoc> myAssocAOS = db.FacultyAOSAssocs.Where(m => m.ProfileId == myProfile.ProfileId).ToList();
            // Get Employee record
            CommEmployee employee = fns.GetEmployee(myProfile.UserId, myProfile.SUAD, myProfile.ESFAD);
            if (employee != null)
            {
                ViewBag.FirstName = employee.FirstName;
                ViewBag.LastName = employee.LastName;
            }
            else
            {
                ViewBag.FirstName = myProfile.UserId;
                ViewBag.LastName = "  ";
            }
            List<FacultyDept> myDepts = db.FacultyDepts.Where(m => m.UserId == myProfile.UserId).ToList();
            List<spFetchFacultyAOSAssocList_Result> myAOSs = db.spFetchFacultyAOSAssocList(myProfile.UserId, myProfile.ESFAD, myProfile.SUAD).ToList();

            ViewBag.myDepts = myDepts;
            ViewBag.myAOSs = myAOSs;

            return View(myProfile);
        }

        // Faculty/Staff Dept Associations/Areas of Study (AOS)
        [HttpGet]
        public ActionResult EditDeptAOS(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            ViewBag.profileId = myProfile.ProfileId;
            ViewBag.userID = myProfile.UserId;
            ViewBag.dept = myProfile.Department;
            ViewBag.reconciledAreas = myProfile.ReconciledAreas;

            List<FacultyAOSAssoc> myAssocAOS = db.FacultyAOSAssocs.Where(m => m.ProfileId == myProfile.ProfileId).ToList();
            return View(myAssocAOS);
        }
        [HttpPost]
        public ActionResult EditDeptAOS()
        {
            string assoc = Request.Form["assoc"];
            string id = Request.Form["userID"];
            string profileId = Request.Form["profileID"];
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);

            // Delete existing Assoc/AOS
            List<FacultyAOSAssoc> delAssocAOS = db.FacultyAOSAssocs.Where(m => m.ProfileId == profileId).ToList();
            foreach (var entry in delAssocAOS)
            {
                db.DeleteObject(entry);
                db.SaveChanges();
            }

            // Loop over all checked boxes
            if (assoc != null)
            {
               var assocId = 0;
               var parts = assoc.Split(',');
               foreach (var part in parts)
               {
                   // Create a new DB entry
                   FacultyAOSAssoc newAssocAOS = new FacultyAOSAssoc();
                   newAssocAOS.ProfileId = profileId;
                   newAssocAOS.AOSCode = part;
                   newAssocAOS.AssocAOSId = assocId;

                   // Get participating areas (if any)
                   string aosParAreas = part + "_PA";
                   string participatingAreas = Request.Form[aosParAreas];
                   newAssocAOS.ParticipatingAreas = participatingAreas;

                   // Add object to DB
                   db.FacultyAOSAssocs.AddObject(newAssocAOS);
                   db.SaveChanges();
                   assocId++;
               }
            }

            return RedirectToAction("Index", new { profileId = profileId });

        }
        [HttpGet]
        public JsonResult GetMainPage(string profileId, string pageTitle)
        {
            // Get all distinct modules based on title
            //var myModules = db.FacultyProfileModules.Select(m => m.ModuleTitle).Distinct().ToList();

            // Return all modules for this profile ID that have no page assignment
            var myMainPage = db.FacultyPages.Select(m => new
            {
                m.FacultyPageId,
                m.ProfileId,
                m.PageTitle
            }).Where(m => m.ProfileId == profileId).Where(m => m.PageTitle == pageTitle).ToList();

            //var results = (from ta in allMyModules
            //                select ta.ModuleTitle).Distinct();
            return Json(myMainPage, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAOSCodes()
        {
            var myAOSCodes = db.CodesAOSs.ToList();
            return Json(myAOSCodes, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetModules(string myId)
        {
            // Get all distinct modules based on title
            //var myModules = db.FacultyProfileModules.Select(m => m.ModuleTitle).Distinct().ToList();

            // Return all modules for this profile ID that have no page assignment
            var allMyModules = db.FacultyProfileModules.Select(m => new
            {
                m.FacultyProfileModuleId,
                m.FacultyPageId,
                m.ProfileId,
                m.ModuleType,
                m.ModuleTitle,
                m.ModuleData
            }).Where (m => m.ProfileId == myId).Where(m => m.FacultyPageId == -1).ToList();

            //var results = (from ta in allMyModules
           //                select ta.ModuleTitle).Distinct();
            return Json(allMyModules, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPageModules(string myId, int pageId)
        {
            // Get all modules for a page
            // Return all modules for this profile ID that have no page assignment
            var allMyModules = db.FacultyProfileModules.Select(m => new
            {
                m.FacultyProfileModuleId,
                m.FacultyPageId,
                m.ProfileId,
                m.ModuleType,
                m.ModuleTitle,
                m.ModuleData
            }).Where(m => m.ProfileId == myId).Where(m => m.FacultyPageId == pageId).ToList();

            return Json(allMyModules, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCourses(string myId, string semester)
        {
            // Return all courses for this profile ID and semester
            var allMyCourses = catalogDB.TimeScheduleWebs.Select(m => new
            {
                m.CourseID,
                m.Semester,
                m.Instructor,
                m.GeneralCourseTitle,
                m.SectionCourseTitle,
                m.Building,
                m.Room,
                m.StartTime,
                m.EndTime
            }).Where(m => m.Semester == semester).Where(m => m.Instructor == myId).ToList();

            return Json(allMyCourses, JsonRequestBehavior.AllowGet);
        }

        // Get file listing
        [HttpGet]
        public JsonResult GetFiles(string myId)
        {
            // Return all files for this user
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~\faculty\" + myId));
            //string myPath = @"http:\\www.esf.edu\faculty\" + myId;
           // DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"http:\\www.esf.edu\faculty\myId"));
           // var allMyFiles = directory.GetFiles().ToList();
           // var allMyFiles = directory.EnumerateFiles().ToList();
            List<string> allMyFiles = new List<string>();
            //DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            foreach (FileInfo fInfo in directory.GetFiles())
            {
                allMyFiles.Add(fInfo.Name);
            }

            return Json(allMyFiles, JsonRequestBehavior.AllowGet);
        }
        // Delete a File
        [HttpPost]
        public ActionResult RemoveFile(string myId, string fileName)
        {
            ViewBag.profileId = myId;
            try
            {
                ViewBag.deleteSuccess = "false";
                var photoName = "";
                photoName = fileName;
                var fullPath = Server.MapPath(@"~\faculty\" + myId + @"\" + fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    ViewBag.deleteSuccess = "true";
                }
            
            }
            catch (Exception)
            {
                ViewBag.deleteSuccess = "false";
                return RedirectToAction("ManageFiles", new { profileID = myId });
            }
            return RedirectToAction("ManageFiles", new { profileID = myId });
        }

        // Display a File
        [HttpGet]
        public ActionResult DisplayFile(string myId, string fileName)
        {
            ViewBag.profileId = myId;
            ViewBag.fileName = fileName;
            FileStream fs = new FileStream(Server.MapPath(@"~\faculty\" + myId), FileMode.Open, FileAccess.Read);
           // return System.IO.File.(fs);
            return PartialView("DisplayFile");
        }
        // Return URL for image (needed for testing locally
        public ActionResult GetImage(string profileId)
        {
            FileInfo photoFile = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(@"~\faculty\profiles\" + profileId + ".jpg"));

            //DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~\faculty\profiles\" + profileId));
            //var root = @"~\faculty\profiles\";
           // var path = Path.Combine(root, profileId);
            //path = Path.GetFullPath(path);
           // if (!path.StartsWith(root))
           // {
                // Ensure that we are serving file only inside the root folder
                // and block requests outside like "../web.config"
            //    throw new HttpException(403, "Forbidden");
            //}
            System.Diagnostics.Debug.WriteLine("Getting file from " + photoFile.FullName);
            ViewBag.Message = ("Getting file from " + photoFile.FullName);
            return File(photoFile.FullName, "image/jpeg");

        }
        // Admin zone
        [HttpGet]
        public ActionResult AdministerContent()
        {
            List<spFacultyList_Result> MyList = db.spFacultyList("").ToList();
            ViewBag.MyList = MyList;
            return View(MyList);

        }
        [HttpPost]
        public ActionResult AdministerContent(FacultyProfile modifiedProfile)
        {
            return null;
        }

        public ActionResult AdminEdit(string userId, string SUAD, string ESFAD)
        {
            return View();

        }

    }
}
