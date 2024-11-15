using libarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Linq;
using Microsoft.Data.SqlClient;
using libarySystem.ViewModels;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Data.SqlClient;

namespace libarySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GSSWEBContext gSSWEBContext;
        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger, GSSWEBContext gSSWEBContext, IConfiguration configuration)
        {

            _logger = logger;
            this.gSSWEBContext = gSSWEBContext;
            _connectionString = configuration.GetConnectionString("dbStr");
        }

        public IActionResult Index(string bookName = "", string classID = "", string borrowerID = "", string borrowingStatusID = "")
        {
            //var book = gSSWEBContext.BOOK_DATA.FirstOrDefault();
            //return View(book);

            //查詢標單中的資料
            var bookClassList = gSSWEBContext.BOOK_CLASS.ToList();
            ViewBag.BookClassList = bookClassList;
            var memberList = gSSWEBContext.MEMBER_M.ToList();
            ViewBag.MemberList = memberList;
            var bookCodeList = gSSWEBContext.BOOK_CODE.ToList();
            var filterCodeList = bookCodeList.Where(data => data.CODE_TYPE_DESC == "書籍狀態").Select(s => new { s.CODE_ID, s.CODE_NAME }).ToList();
            ViewBag.BookCodeList = filterCodeList;

            //查詢後提交資料 

            string sqlQuery = @" 
                    SELECT BOOK_DATA.BOOK_ID,BOOK_DATA.BOOK_NAME, BOOK_DATA.BOOK_BOUGHT_DATE, BOOK_CLASS.BOOK_CLASS_NAME, MEMBER_M.USER_CNAME,BOOK_CODE.CODE_NAME AS BOOK_STATUS_NAME
                    FROM BOOK_DATA
                    LEFT JOIN BOOK_CLASS ON BOOK_DATA.BOOK_CLASS_ID = BOOK_CLASS.BOOK_CLASS_ID
                    LEFT JOIN (
		                SELECT * FROM
		                (
			                SELECT A.BOOK_ID, A.KEEPER_ID, A.LEND_DATE, ROW_NUMBER() OVER(PARTITION BY A.BOOK_ID ORDER BY A.LEND_DATE DESC) AS RNK
			                FROM BOOK_LEND_RECORD A
		                ) A
		                WHERE A.RNK = 1
	                ) BOOK_LEND_RECORD ON BOOK_DATA.BOOK_ID = BOOK_LEND_RECORD.BOOK_ID
                    LEFT JOIN MEMBER_M ON BOOK_LEND_RECORD.KEEPER_ID = MEMBER_M.USER_ID
                    LEFT JOIN BOOK_CODE ON BOOK_DATA.BOOK_STATUS = BOOK_CODE.CODE_ID

                    WHERE (@BookName = '' OR BOOK_DATA.BOOK_NAME LIKE '%' + @BookName + '%')
                    AND (@ClassID = '' OR BOOK_CLASS.BOOK_CLASS_ID = @ClassID)
                    AND (@BorrowerID = '' OR MEMBER_M.[USER_ID] = @BorrowerID)
	                AND (@StatusID = '' OR BOOK_DATA.BOOK_STATUS = @StatusID)
	                AND BOOK_CODE.CODE_TYPE='BOOK_STATUS'
                    ORDER BY  BOOK_DATA.BOOK_BOUGHT_DATE DESC ,BOOK_DATA.BOOK_ID,BOOK_CLASS.BOOK_CLASS_NAME; ";

            List<LibaryQuerryData> results = new List<LibaryQuerryData>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 使用參數化查詢來避免 SQL 注入
                    command.Parameters.AddWithValue("@BookName", bookName ?? "");
                    command.Parameters.AddWithValue("@ClassID", classID ?? "");
                    command.Parameters.AddWithValue("@BorrowerID ", borrowerID ?? "");
                    command.Parameters.AddWithValue("@StatusID", borrowingStatusID ?? "");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //int BOOK_ID = (int)reader.GetValue(0);
                            //.GetString(0).ToString());

                            results.Add(new LibaryQuerryData
                            {

                                BOOK_ID = (int)reader.GetValue(0),
                                BOOK_NAME = reader.GetString(1),
                                BOOK_BOUGHT_DATE = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                                BOOK_CLASS_NAME = reader.GetString(3),
                                USER_CNAME = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                BOOK_STATUS_NAME = reader.GetString(5)
                            });
                        }
                    }
                }
            }


            //string bookNames = bookName;
            //var bookList = gSSWEBContext.BOOK_DATA.ToList();
            //var bookNameList = bookList.Where(name=>name.BOOK_NAME == bookNames).Select(s => new { s.BOOK_ID, s.BOOK_NAME }).ToList();
            //ViewBag.BookNameList = bookNameList;
            return View(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //新增書本資料
        [HttpPost]
        public IActionResult CreateData
            (
            string createName = "",
            string createAuthor = "",
            string createPublisher = "",
            string content = "",
            string classID = "",
            string createDate = ""
            )
        {
            string sqlQuery = @"INSERT INTO BOOK_DATA (BOOK_NAME,BOOK_AUTHOR,BOOK_PUBLISHER,BOOK_NOTE, BOOK_BOUGHT_DATE, BOOK_CLASS_ID,BOOK_STATUS)
                                VALUES
                                (@CreateName, @CreateAuthor, @CreatePublisher, @Content,@CreateDate,@ClassID,'A');";
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        //command.CommandText = sqlQuery;
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@CreateName", createName);
                        command.Parameters.AddWithValue("@CreateAuthor", createAuthor);
                        command.Parameters.AddWithValue("@CreatePublisher", createPublisher);
                        command.Parameters.AddWithValue("@Content", content);
                        command.Parameters.AddWithValue("@CreateDate", createDate);
                        command.Parameters.AddWithValue("@ClassID", classID);
                        command.ExecuteNonQuery();

                    }
                    connection.Close();
                    TempData["Message"] = "新增成功！";
                }

                catch (Exception ex)
                {
                    TempData["Message"] = $"新增失敗：{ex.Message}";
                }
                finally { connection.Close(); }

            }

            return RedirectToAction("Index");
        }

        public IActionResult CreateData()
        {
            var bookClassList = gSSWEBContext.BOOK_CLASS.ToList();
            ViewBag.BookClassList = bookClassList;

            return View();
        }


        //刪除書本資料
        public IActionResult DeleteData(int? id)
        {
            string sqlQuery = @"DELETE FROM BOOK_DATA WHERE BOOK_ID = @BookId";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@BookId", id);
                        int num = command.ExecuteNonQuery();

                    }
                    connection.Close();
                    TempData["Message"] = "刪除成功！";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"刪除失敗：{ex.Message}";
                }
            }
            return RedirectToAction("Index");
        }


        //編輯書本資料
        public IActionResult EditData(int? id)
        {
            var editBookClassList = gSSWEBContext.BOOK_CLASS.ToList();
            ViewBag.EditBookClassList = editBookClassList;
            var editMemberList = gSSWEBContext.MEMBER_M.ToList();
            ViewBag.EditMemberList = editMemberList;
            var bookCodeList = gSSWEBContext.BOOK_CODE.ToList();
            var filterCodeList = bookCodeList.Where(data => data.CODE_TYPE_DESC == "書籍狀態").Select(s => new { s.CODE_ID, s.CODE_NAME }).ToList();
            ViewBag.BookCodeList = filterCodeList;

            //gSSWEBContext.BOOK_CLASS

            string sqlQuery = @"SELECT BOOK_NAME,BOOK_AUTHOR,BOOK_PUBLISHER,BOOK_NOTE,BOOK_BOUGHT_DATE,BOOK_CLASS_ID,BOOK_STATUS,BOOK_KEEPER,BOOK_ID 
                                FROM BOOK_DATA 
                                WHERE BOOK_ID = @BookId;";
            var results = new BOOK_DATA();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@BookId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            results = new BOOK_DATA
                            {


                                BOOK_NAME = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                BOOK_AUTHOR = reader.GetString(1),
                                BOOK_PUBLISHER = reader.GetString(2),
                                BOOK_NOTE = reader.GetString(3),
                                BOOK_BOUGHT_DATE = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                BOOK_CLASS_ID = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                BOOK_STATUS = reader.IsDBNull(6) ? "" : reader.GetString(6)
                                , BOOK_KEEPER = reader.IsDBNull(7) ? "" : reader.GetString(7)
                                , BOOK_ID = (int)reader.GetValue(8)

                            };
                        }
                    }

                }
                connection.Close();
            }
            return View(results);
        }

        [HttpPost]
        public IActionResult EditData(BOOK_DATA bOOK_DATA)
        {
            string sqlQuery = @"UPDATE BOOK_DATA
                                SET BOOK_NAME = @BookName, BOOK_AUTHOR = @BookAuthor,BOOK_PUBLISHER=@BookPublisher,BOOK_NOTE=@BookNote,BOOK_BOUGHT_DATE=@BookBoughtDate,BOOK_CLASS_ID=@BookClassId,BOOK_STATUS=@BookStatus,BOOK_KEEPER=@BookKeeper
                                WHERE BOOK_ID = @BookId;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@BookId", bOOK_DATA.BOOK_ID);
                        command.Parameters.AddWithValue("@BookName", bOOK_DATA.BOOK_NAME);
                        command.Parameters.AddWithValue("@BookAuthor", bOOK_DATA.BOOK_AUTHOR);
                        command.Parameters.AddWithValue("@BookPublisher", bOOK_DATA.BOOK_PUBLISHER);
                        command.Parameters.AddWithValue("@BookNote", bOOK_DATA.BOOK_NOTE);
                        command.Parameters.AddWithValue("@BookBoughtDate", bOOK_DATA.BOOK_BOUGHT_DATE);
                        command.Parameters.AddWithValue("@BookClassId", bOOK_DATA.BOOK_CLASS_ID);
                        command.Parameters.AddWithValue("@BookStatus", bOOK_DATA.BOOK_STATUS);
                        command.Parameters.AddWithValue("@BookKeeper", bOOK_DATA.BOOK_KEEPER);
                        int num = command.ExecuteNonQuery();

                    }
                    connection.Close();

                    if (!string.IsNullOrEmpty(bOOK_DATA.BOOK_KEEPER))
                    {
                        BOOK_LEND_RECORD record = new BOOK_LEND_RECORD() {
                            BOOK_ID= bOOK_DATA.BOOK_ID,
                            KEEPER_ID = bOOK_DATA.BOOK_KEEPER,
                            LEND_DATE = DateTime.Now
                        };
                        gSSWEBContext.BOOK_LEND_RECORD.Add(record);
                        gSSWEBContext.SaveChanges();
                    }

                    TempData["Message"] = "修改成功！";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"修改失敗：{ex.Message}";
                }
            }
            return RedirectToAction("Index");

        }

        //查詢借閱資料

        public IActionResult QuerryLendRecord(int? id)
        {
            string sqlQuery = @"SELECT LEND_DATE ,KEEPER_ID,USER_ENAME,USER_CNAME,IDENTITY_FILED
                                FROM BOOK_LEND_RECORD
                                LEFT JOIN MEMBER_M 
                                ON KEEPER_ID = [USER_ID]
                                WHERE BOOK_ID =@BOOK_ID
                                ORDER BY LEND_DATE DESC;";
            List<QuerryLendRecord> results = new List<QuerryLendRecord>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 使用參數化查詢來避免 SQL 注入
                    command.Parameters.AddWithValue("@BOOK_ID", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            results.Add(new QuerryLendRecord
                            {

                                
                                LEND_DATE = (DateTime)(reader.IsDBNull(0) ? (DateTime?)null : reader.GetDateTime(0)),
                                KEEPER_ID = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                USER_ENAME = reader.GetString(2),
                                USER_CNAME = reader.GetString(3),
                                IDENTITY_FILED = (int)reader.GetValue(4)

                            });
                        }
                    }
                }
                return View(results);
            }
        }




 
        

    }
}
