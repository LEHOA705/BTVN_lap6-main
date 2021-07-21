using System;
using System.Linq;
using System.Web.Http;
using BigShool.Models;
using Microsoft.AspNet.Identity;

namespace BigShool.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course AttendencedTo)
        {
            var UserID = User.Identity.GetUserId();
            BigSchoolContext Context = new BigSchoolContext();

            if (Context.Attendances.Any(Party => Party.Attendee == UserID && Party.CourseId == AttendencedTo.Id))
            {
                Context.Attendances
                    .Remove(Context.Attendances
                        .SingleOrDefault(Party => Party.Attendee == UserID && Party.CourseId == AttendencedTo.Id));

                Context.SaveChanges();
                
                return Ok("Removed");
            }

            var Attendee = new Attendance() { CourseId = AttendencedTo.Id, Attendee = UserID };

            try
            {
                Context.Attendances.Add(Attendee);
                Context.SaveChanges();
            }
            catch (Exception Error)
            {
                return BadRequest(Error.Message.ToString());
            }

            return Ok();
        }
    }
}
