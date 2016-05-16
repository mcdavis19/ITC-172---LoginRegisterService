using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginRegistrationSrvc" in code, svc and config file together.
public class LoginRegistrationSrvc : ILoginRegistrationSrvc
{
    ShowTrackerEntities db = new ShowTrackerEntities();
    public bool RegisterVenue(Venue v, VenueLogin vl)
    {
        bool registered = true;
        //Stored Prodecdure returns an int. -1 if the venue is already registered.
        int pass = db.usp_RegisterVenue(v.VenueName, v.VenueAddress, v.VenueCity, v.VenueState, v.VenueZipCode,
          v.VenuePhone, v.VenueEmail, v.VenueWebPage,
          v.VenueAgeRestriction, vl.VenueLoginUserName,
          vl.VenueLoginPasswordPlain);
        if (pass == -1)
        {
            registered = false;
        }
        return registered;
    }

    public int LoginVenue(string userName, string password)
    //Returns venueKey
    {
        int reviewerKey = -1;
        //Returns int which represents the successfulness of the login.
        // -1 means it failed.
        int loginSuccess = db.usp_venueLogin(userName, password);
        if (loginSuccess != -1)
        {
            var venueInfo = (from vl in db.VenueLogins
                           where vl.VenueLoginUserName.Equals(userName)
                           select new { vl.VenueKey }).FirstOrDefault();
            reviewerKey = (int)venueInfo.VenueKey;
        }
        return reviewerKey;
    }

    
}
