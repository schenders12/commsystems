using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using systems.Models;
using System.Text.RegularExpressions;
using System.IO;
using systems.Helpers;

namespace systems.Helpers.catalog
{
    public class catalog_fns
    {


        // Catalog region
        #region fpimCatalog

        // Catalog module
        public static string CatalogModule(string prof, string type, string year)
        {
            var courses = GetCourses("Leopold", "Fall 2014");

            if (prof == null || prof == "")
            {
                return "";
            }
            else
            {
                var headerlines = "";
                var closinglines = "";
                var set = "";
                var entries = "";
                // Open database context
                PeopleEntities db = new PeopleEntities();
                // Execute SP to get list of advisees for professor
                var advisees = db.spAdviseesPublic(prof);
                foreach (var item in advisees)
                {
                    var filepath = "E:\\Servers\\WWW\\forms\\pim\\gs\\" + item.UserId + ".jpg";
                    FileInfo PhotoFile = new FileInfo(filepath);
                    var studentphoto = "";
                    if (!PhotoFile.Exists)
                    {
                        studentphoto = "<h4 style='margin-top: 10px'><img class='right' src='http://www.esf.edu/images/clear.gif' alt='" + item.FirstNm + " " + item.LastNm + "' />" + item.FirstNm + " " + item.LastNm;
                    }
                    else
                    {
                        studentphoto = "<h4 style='margin-top: 10px'><img class='right' src='http://www.esf.edu/forms/pim/gs/" + item.UserId + ".jpg' alt='" + item.FirstNm + " " + item.LastNm + "' />" + item.FirstNm + " " + item.LastNm;
                    }
                    if (item.SEmailID)
                    {
                        studentphoto = studentphoto + "<br /><span class='ten'><a href='mailto:" + item.EmailId.Trim() + "'>" + item.EmailId.Trim() + "</a></span></h4>";
                    }
                    else
                    {
                        studentphoto = studentphoto + "</h4>";
                    }
                    entries = entries + studentphoto;
                    var degree = "";
                    // Degree information, if they allow it
                    if (item.SDegProg)
                    {
                        degree = "<li><strong>Degree Sought</strong>: " + item.DegProg;
                        if (item.AreaOfStudy == "" && item.CorrectAOS == "")
                        {
                            if (item.ProgStudy != "")
                            {
                                degree = degree + " in " + item.ProgStudy;
                            }
                            else
                            {
                                degree = degree + "</li>";
                            }
                        }
                    }
                    // Major Professor information
                    if (item.SMajorProf)
                    {
                        // CLEAN UP presentation layer - currently all caps ProfName
                        // ADD links
                        var mp = fns.FixMPName(item.MajorProf.Trim());
                        degree = degree + "<li><strong>Graduate Advisor(s):</strong> " + mp.Substring(0, 1) + mp.Substring(1).ToLower();

                        if (item.CoMajorProf != "" && item.CoMajorProf != null)
                        {
                            var cmp = fns.FixMPName(item.CoMajorProf.Trim());
                            degree = degree + " and " + cmp.Substring(0, 1) + cmp.Substring(1).ToLower() + "</li>";
                        }
                        else
                        {
                            degree = degree + "</li>";
                        }
                    }
                    // Area of study information
                    if (item.SAreaOfStudy)
                    {
                        // If Corrected AOS filled in, use it.
                        if (item.CorrectAOS != null && item.CorrectAOS != "" && item.CorrectAOS != item.AreaOfStudy)
                        {
                            degree = degree + "<li><strong>Area of Study</strong>: " + fns.FixProgramOfStudy(item.CorrectAOS) + "</li>";
                        }
                        else
                        {
                            var fix = item.AreaOfStudy.Substring(4); // Trim prefix 
                            degree = degree + "<li><strong>Area of Study</strong>: " + fns.FixProgramOfStudy(fix) + "</li>";
                        }
                    }
                    // Undergraduate information
                    if (item.SBaccDegCollNm)
                    {
                        degree = degree + "<li><strong>Undergraduate Institute</strong>: " + fns.FixInstitution(item.BaccDegCollNm);
                        if ((bool)item.SBaccDegCurr)
                        {
                            degree = degree + " (" + fns.FixMajor(item.BaccDegCurr) + ")</li>";
                        }
                        else
                        {
                            degree = degree + "</li>";
                        }
                    }
                    // Previous Graduate Study
                    if (item.SGradDegCollNm)
                    {
                        if (item.CorrectGradInst != null || item.GradDegCollNm != "")
                        {
                            degree = degree + "<li><strong>Previous Graduate Study</strong>: ";
                            if (item.CorrectGradInst != null && item.CorrectGradInst != "")
                            {
                                degree = degree + fns.FixInstitution(item.CorrectGradInst);
                            }
                            else
                            {
                                degree = degree + fns.FixInstitution(item.GradDegCollNm);
                            }
                            if ((bool)item.SGradDegCurr)
                            {
                                if (item.CorrectGradMajor != null && item.CorrectGradMajor != "")
                                {
                                    degree = degree + " (" + fns.FixMajor(item.CorrectGradMajor) + ")</li>";
                                }
                                else
                                {
                                    degree = degree + " (" + fns.FixMajor(item.GradDegCurr) + ")</li>";
                                }
                            }
                        }
                    }
                    entries = entries + "<ul>" + degree + "</ul>";
                    var theirentries = db.spCurrViewFields(item.UserId);
                    foreach (var entry in theirentries)
                    {
                        if (entry.FieldDisplay)
                        {
                            if ((entry.FieldContents.Substring(0, 3) != "<a ") && (entry.FieldContents.Substring(0, 7) == "http://"))
                            {
                                entries = entries + "<p><strong>" + entry.FieldName + "</strong><br /><a href='" + entry.FieldContents + "'>Web Link</a></p>";
                            }
                            else if (entry.FieldContents.Substring(0, 3) == "www")
                            {
                                entries = entries + "<p><strong>" + entry.FieldName + "</strong><br /><a href='http://" + entry.FieldContents + "'>Web Link</a></p>";
                            }
                            else
                            {
                                entries = entries + "<p><strong>" + entry.FieldName + "</strong><br />" + entry.FieldContents + "</p>";
                            }
                        }
                    }
                }

                switch (type)
                {
                    case "full":
                        headerlines = "<hr class='clearer' /><div id='borderbox' class='bggray'><h3 style='background-image: url(http://www.esf.edu/includes/gpim/bgfull.jpg); background-position: right; background-repeat: no-repeat'><a href='http://www.esf.edu/graduate/' style='border: none'><img src='http://www.esf.edu/images/clear.gif' style='margin: 0; padding: 0; width: 280px; height: 24px; float: right' /></a>Current Graduate Advisees</h3><div style='clear: both'>";
                        closinglines = "</div></div>";
                        set = headerlines + entries + closinglines;
                        return set;
                    case "rightnarrow":
                        headerlines = "<div id='sideright' style='width: 420px; clear: right;' class='bggray' ><h3 style='background-image: url(http://www.esf.edu/includes/gpim/bgnarrow.jpg); background-position: right; background-repeat: no-repeat'>Graduate Advisees</h3><div style='clear: both;'>";
                        closinglines = "</div></div>";
                        set = headerlines + entries + closinglines;
                        return set;
                    case "rightwide":
                        headerlines = "<div id='sideright' style='width: 550px; clear: right;' class='bggray' ><h3 style='background-image: url(http://www.esf.edu/includes/gpim/bg.jpg); background-position: right; background-repeat: no-repeat'><a href='http://www.esf.edu/graduate/' style='border: none'><img src='http://www.esf.edu/images/clear.gif' style='margin: 0; padding: 0; width: 280px; height: 24px; float: right' /></a>Graduate Advisees</h3><div style='clear: both;'>";
                        closinglines = "</div></div>";
                        set = headerlines + entries + closinglines;
                        return set;
                    case "left":
                        headerlines = "<div id='sideleft' style='width: 550px; clear: left;' class='bggray' ><h3 style='background-image: url(http://www.esf.edu/includes/gpim/bg.jpg); background-position: right; background-repeat: no-repeat'><a href='http://www.esf.edu/graduate/' style='border: none'><img src='http://www.esf.edu/images/clear.gif' style='margin: 0; padding: 0; width: 280px; height: 24px; float: right' /></a>Graduate Advisees</h3><div style='clear: both;'>";
                        closinglines = "</div></div>";
                        set = headerlines + entries + closinglines;
                        return set;
                    default:
                        return ""; // Not properly formatted.
                }
                //return prof;
            }
        }

       // [HttpGet]
       // public static JsonResult GetCourses(string myId, string semester)
        public static string GetCourses(string myId, string semester)
        {
            CatalogEntities catalogDB = new CatalogEntities();

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

            //return Json(allMyCourses, JsonRequestBehavior.AllowGet);
            return "";
        }
        #endregion
    }
}
