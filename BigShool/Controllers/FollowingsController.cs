using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BigShool.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace BigShool.Controllers
{
    public class FollowingsController : ApiController
    {
        public IHttpActionResult Flow(Following Follow)
        {
            var UserID = User.Identity.GetUserId();

            if (UserID == null)
                return BadRequest("Please login first!");
            if (UserID == Follow.FolloweeId)
                return BadRequest("Can not follow myself!");

            BigSchoolContext Context = new BigSchoolContext();

            Following Flow = Context.Followings.FirstOrDefault(Party => Party.FollowerId == UserID && Party.FolloweeId == Follow.FolloweeId);

            if (Flow != null)
            {
                Context.Followings
                    .Remove(Context.Followings
                        .SingleOrDefault(Party => Party.FollowerId == UserID && Party.FolloweeId == Follow.FolloweeId));
                
                Context.SaveChanges();

                return Ok("Removed");
            }

            Follow.FollowerId = UserID;
            Context.Followings.Add(Follow);
            Context.SaveChanges();

            return Ok();
        }
    }
}
