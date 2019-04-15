//using DotNetAuth.Profiles;
using FriendJam2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FriendJam2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Profile/
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var _webApiFactory = new WebAPIFactory(
               "http://localhost",
               2826,
               "33c0ac01baa6477da389527c1c6cef90",
               Scope.UserReadPrivate | Scope.PlaylistReadPrivate,
               TimeSpan.FromSeconds(20)
           );

            try
            {
                //This will open the user's browser and returns once
                //the user is authorized.
                var _spotify = _webApiFactory.GetWebApi().Result;
            }
            finally
            {

            }

            using (var client = new HttpClient())
            {
                var headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Bearer {0}", Request.QueryString["access_token"].Value));
                HttpResponseMessage message = await client.GetAsync("https://api.spotify.com/v1/me/playlists");

                using (var responseStreamAllPlaylists = await message.Content.ReadAsStreamAsync())
                using (var readerAllPlaylists = new StreamReader(responseStreamAllPlaylists))
                {
                    string allplaylist;

                    string allPlaylistsString = readerAllPlaylists.ReadToEnd();

                    var allPlaylistsJson = JObject.Parse(allPlaylistsString);
                }
            }



            FullTrack track = null; // _spotify.GetTrack("3Hvu1pq89D4R0lyPBoujSv");
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var spotifyUsername = user == null ? "" : user.Logins.First().ProviderKey;
            return View(new HomeViewModel { SpotifyUsername = spotifyUsername, Track = null /*track.Name*/});
        }
        //public RedirectResult Login()
        //{
        //    var userProcessUri = Url.Action("Callback", "Profile", null, protocol: Request.Url.Scheme);
        //    var provider = LoginProvider.Get(LoginProviderRegistry.Twitter.Fullname);
        //    var authorizationUrl = DotNetAuth.Profiles.Login.GetAuthenticationUri(provider, new Uri(userProcessUri), new DefaultLoginStateManager(Session), requiredProperties);
        //    authorizationUrl.Wait();
        //    return Redirect(authorizationUrl.Result.AbsoluteUri);
        //}
        //// GET: /Process
        //[HttpGet]
        //public ActionResult Callback(string providerName)
        //{
        //    var userProcessUri = Url.Action("Callback", "Profile", null, protocol: Request.Url.Scheme);
        //    var provider = LoginProvider.Get(LoginProviderRegistry.Twitter.Fullname);
        //    var profile = DotNetAuth.Profiles.Login.GetProfile(provider, Request.Url, userProcessUri, new DefaultLoginStateManager(Session), requiredProperties);
        //    profile.Wait();
        //    return Content(profile.Result.ToString());
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}