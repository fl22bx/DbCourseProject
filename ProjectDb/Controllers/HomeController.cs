using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectDb.Models;
using ProjectDb.Models.Context;
using ProjectDb.ViewModel;


namespace ProjectDb.Controllers
{
    public class HomeController : Controller
    {
        private ProjectDb.Models.Context.Context Context { get; }
        public HomeController(ProjectDb.Models.Context.Context context)
        {

            Context = context;
        }

        /// <summary>
        /// Home controller that renders a Home
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            String QueryPartTrails =
                @"SELECT *
                  FROM PartTrail";
            QueryViewModel Result = new QueryViewModel()
            {
                PartTrailsResult = Context.PartTrail(QueryPartTrails)
            };
            return View(Result);
        }

        /// <summary>
        /// Controller method for post
        /// Querys the database for sections based on arguments
        /// </summary>
        /// <param name="Distance"></param>
        /// <param name="Difficulty"></param>
        /// <param name="PartTrail"></param>
        /// <returns>Json with sections form db</returns>
        [HttpPost]
        public JsonResult GetSuggestions(string Distance, string Difficulty, int PartTrail)
        {
            string sqlQuery = $@"
                            SELECT *
                            FROM Sections
                            WHERE partOF = {PartTrail} AND {Distance} AND LevelOfDifficulty = {Difficulty}
                        ";

            var result = Context.SectionSet(sqlQuery);
            var json = JsonConvert.SerializeObject(result);

            return Json(json);
        }

        /// <summary>
        /// Post controller method
        /// Register New user in DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns>Status Code</returns>
        [HttpPost]
        public IActionResult NewUser(string name, string password, string email)
        {
           try
           {

            string sql = $@"
                            INSERT INTO dbo.Users(Name, Password,Email)
                            VALUES ('{name}', '{password.HashCode()}', '{email}');
                           ";

            Context.NoQuerySQL(sql);
                return StatusCode(200);
                
            }
             catch (Exception e)
             {
                return StatusCode(409);
             }

        }

        /// <summary>
        /// Searches db sections after sections destination matching Input
        /// </summary>
        /// <param name="partTrail"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult searchSection(int partTrail, string filterString)
        {
            string sqlQuery = $@"
                            SELECT *
                            FROM Sections
                            WHERE partOF = {partTrail} AND Destination1 LIKE '%{filterString}%'
                        ";

            var result = Context.SectionSet(sqlQuery);
            var json = JsonConvert.SerializeObject(result);

            return Json(json);
        }

        /// <summary>
        /// Register new trekk in db
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult NewTrekk(string name, string password, int section)
        {
            bool Authenticated = Context.AuthenticateUser(name, password);
            try
            {
                if (!Authenticated)
                return StatusCode(500);
                
       
                string sql = $@"
                            INSERT INTO TrekkedSections (trekker, path)
                            VALUES ('{name}', '{section}');
                           ";

                Context.NoQuerySQL(sql); 
                return StatusCode(200);

            }
            catch (Exception e)
            {
                return StatusCode(409);
            }
        }

        /// <summary>
        /// Get users and their total Trekk distance
        /// </summary>
        /// <returns>Json </returns>
        [HttpGet]
        public JsonResult TopList()
        {
            string sql = @"
                SELECT TrekkedSections.trekker, sum(Sections.Length / 1000) as kilometersWalked
                from TrekkedSections
                LEFT JOIN Sections ON TrekkedSections.path = Sections.id
                GROUP BY TrekkedSections.trekker
                ORDER BY kilometersWalked DESC
            ";

            Dictionary<string, int> toplist = Context.TopList(sql);
            var json = JsonConvert.SerializeObject(toplist);
            return Json(json);
        }

        /// <summary>
        /// Get total length of skåneleden from DB
        /// </summary>
        /// <returns>Json sum length</returns>
        [HttpGet]
        public JsonResult TotalLength()
        {
            string sql = @"
                            SELECT sum(length  / 1000) as TotalLength
                            FROM Sections 
                        ";

            var result = new {TotalLength = Context.sum(sql)};
            var json =  JsonConvert.SerializeObject(result);
            return Json(json);
        }

        /// <summary>
        /// get User history from DB
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>JsonResult sections</returns>
        public JsonResult UserData(string Username)
        {
                string sql = $@"
                            SELECT * 
                            from Sections
                            WHERE Sections.ID in 
                            (
                                select Path
                                from TrekkedSections
                                where trekker = '{Username}'
                            )
                        ";

            var result = Context.SectionSet(sql);
            
            var json = JsonConvert.SerializeObject(result);
            return Json(json);
        }

        /// <summary>
        /// Get part trail progression in km for choosen user 
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>Jsonresult </returns>
        public JsonResult ParTrailProgression(string Username)
        {
            string sql = $@"
                        SELECT   PartTrail.ID , PartTrail.NameOfPartTrail, SUM(Length / 1000) as length
                        FROM Sections
                        INNER JOIN PartTrail ON PartTrail.id = Sections.partOf
                        WHERE Sections.ID in 
                        (
                            SELECT path
                            FROM TrekkedSections
                            WHERE trekker = '{Username}'
                        )
                        GROUP BY PartTrail.NameOfPartTrail, PartTrail.ID
                        ";

            var result = Context.progress(sql);

            var json = JsonConvert.SerializeObject(result);
            return Json(json);

        }

        /// <summary>
        /// get sections for parttrail
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Jsonresult sections</returns>
        public JsonResult partTrail(int id)
        {
            string sql = $@"
                            SELECT * 
                            FROM Sections
                            WHERE partOf = '{id}'
                        
                        ";

            var result = Context.SectionSet(sql);
            var json = JsonConvert.SerializeObject(result);

            return Json(json);
        }
    }
}