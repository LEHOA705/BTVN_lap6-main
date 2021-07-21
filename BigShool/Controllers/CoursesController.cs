using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using BigShool.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace BigShool.Controllers
{
    public class CoursesController : Controller
    {
        private BigSchoolContext Context = new BigSchoolContext();
        public ActionResult Index()
        {
            var ListCourse = Context.Courses
                .Where(Course => Course.DateTime > DateTime.Now)
                .OrderBy(Course => Course.DateTime).ToList();

            var Loop = 0;
            var LoggedUserID = User.Identity.GetUserId();

            while (Loop < ListCourse.Count())
            {
                if (LoggedUserID != null)
                {
                    ListCourse[Loop].IsLogin = true;
                    var CurrentCourseID = ListCourse[Loop].Id;
                    var CurrentLecturerID = ListCourse[Loop].LecturerId;

                    Attendance CheckIfGoing = Context.Attendances
                        .FirstOrDefault(Party => Party.CourseId == CurrentCourseID && Party.Attendee == LoggedUserID);

                    Following CheckIfFlow = Context.Followings
                        .FirstOrDefault(Party => Party.FolloweeId == CurrentLecturerID && Party.FollowerId == LoggedUserID);

                    if (CheckIfGoing != null) ListCourse[Loop].IsGoing = true;
                    if (CheckIfFlow != null) ListCourse[Loop].IsFollowing = true;
                }

                var UserID = ListCourse[Loop].LecturerId.ToString();
                var UserInfo = Context.AspNetUsers.First(UserData => UserData.Id == UserID);

                var CategoryID = ListCourse[Loop].CategoryId;
                var CategoryName = Context.Categories.Single(CategoryData => CategoryData.Id == CategoryID);

                ListCourse[Loop].Name = UserInfo.Name;
                ListCourse[Loop].CategoryName = CategoryName.Name.ToString();

                Loop++;
            }

            return View(ListCourse);
        }

        [Authorize]
        public ActionResult Create()
        {
            Course CouseList = new Course();

            CouseList.ListCategory = Context.Categories.ToList();

            return View(CouseList);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course CreateCourse)
        {
            ModelState.Remove("LecturerId");

            if (!ModelState.IsValid)
            {
                CreateCourse.ListCategory = Context.Categories.ToList();
                return View("Create", CreateCourse);
            }

            CreateCourse.LecturerId = User.Identity.GetUserId();

            Context.Courses.Add(CreateCourse);
            Context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Modify(string ID)
        {
            var CourseInfo = Context.Courses.Where(Party => Party.Id.ToString() == ID).First();
            CourseInfo.ListCategory = Context.Categories.ToList();

            return View(CourseInfo);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Modify(string ID, FormCollection Collection)
        {
            var ModifyCourse = Context.Courses.First(Party => Party.Id.ToString() == ID);
            if (ModifyCourse != null)
            {
                ModifyCourse.Id = Convert.ToInt32(Collection["Id"]);

                UpdateModel(ModifyCourse);

                Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return this.Modify(ID);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var UserID = User.Identity.GetUserId();

            var ListAttend = Context.Attendances.Where(Party => Party.Attendee == UserID).ToList();

            var Courses = new List<Course>();

            foreach (Attendance Attend in ListAttend)
            {
                var GetCourse = Context.Courses.Single(Party => Party.Id == Attend.CourseId);
                var GetUserName = Context.AspNetUsers.Single(Party => Party.Id == GetCourse.LecturerId);
                var GetCategoryName = Context.Categories.Single(Party => Party.Id == GetCourse.CategoryId);

                GetCourse.CategoryName = GetCategoryName.Name;
                GetCourse.Name = GetUserName.Name;

                Courses.Add(GetCourse);
            }

            return View(Courses);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var UserID = User.Identity.GetUserId();

            var ListCourse = Context.Courses.Where(Party => Party.LecturerId == UserID && Party.DateTime >= DateTime.Now).ToList();

            var Loop = 0;
            var UserInfo = Context.AspNetUsers.
                Single(UserData => UserData.Id == UserID);

            while (Loop < ListCourse.Count())
            {
                var CategoryID = ListCourse[Loop].CategoryId;
                var CategoryName = Context.Categories.Single(CategoryData => CategoryData.Id == CategoryID);

                ListCourse[Loop].Name = UserInfo.Name;
                ListCourse[Loop].CategoryName = CategoryName.Name.ToString();

                Loop++;
            }

            return View(ListCourse);
        }

        [Authorize]
        public ActionResult Remove(int ID)
        {
            var RemoveCourse = Context.Courses.Where(Party => Party.Id == ID).First();

            if (RemoveCourse != null)
            {
                var AttendList = Context.Attendances.Where(Party => Party.CourseId == RemoveCourse.Id);
                
                foreach(var Attend in AttendList)
                {
                    Context.Attendances.Remove(Attend);
                }

                Context.Courses.Remove(RemoveCourse);
                Context.SaveChanges();
            }

            return RedirectToAction("Mine");
        }

        [Authorize]
        public ActionResult Following()
        {
            var Courses = new List<Course>();
            var UserID = User.Identity.GetUserId();
            var FlowList = Context.Followings.Where(Party => Party.FollowerId == UserID).ToList();

            foreach (var Flow in FlowList)
            {
                var LecturerID = Flow.FolloweeId;

                if (Flow.FolloweeId == LecturerID)
                {
                    var CourseListByUID = Context.Courses.Where(Party => Party.LecturerId == LecturerID).ToList();

                    foreach(var CourseByUID in CourseListByUID)
                    {
                        var CourseUserID = CourseByUID.LecturerId.ToString();
                        var UserInfo = Context.AspNetUsers.First(UserData => UserData.Id == CourseUserID);

                        var CategoryID = CourseByUID.CategoryId;
                        var CategoryName = Context.Categories.Single(CategoryData => CategoryData.Id == CategoryID);

                        CourseByUID.Name = UserInfo.Name;
                        CourseByUID.CategoryName = CategoryName.Name.ToString();

                        CourseByUID.IsLogin = true;
                        var CurrentCourseID = CourseByUID.Id;
                        var CurrentLecturerID = CourseByUID.LecturerId;

                        Attendance CheckIfGoing = Context.Attendances
                            .FirstOrDefault(Party => Party.CourseId == CurrentCourseID && Party.Attendee == UserID);

                        Following CheckIfFlow = Context.Followings
                            .FirstOrDefault(Party => Party.FolloweeId == CurrentLecturerID && Party.FollowerId == UserID);

                        if (CheckIfGoing != null) CourseByUID.IsGoing = true;
                        if (CheckIfFlow != null) CourseByUID.IsFollowing = true;

                        Courses.Add(CourseByUID);
                    }
                }
            }

            return View(Courses);
        }
    }
}