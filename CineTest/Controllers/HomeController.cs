using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using CineTest.Models;
using System.Web.UI.WebControls;

namespace CineTest.Controllers
{
    public class HomeController : Controller
    {
        static string ordba = "Data Source=XE;User Id=cinema;Password=cinema;";
        OracleConnection conn = new OracleConnection(ordba);
        OracleCommand cmd;
        OracleDataReader dr;

        
        string[] getSeat= new string[15]; //좌석 선택 정보

        static string pickseat1 = "";
        static string pickseat2 = "";
        static bool isPicked = false;
        bool isFirst = true;

        //회원정보
        String i_memberId; //아이디
        String i_schdul; //선택 스케줄
        String i_movie; //선택 영화
        String i_movieName;
        int i_money; // 결제금액
        int i_seatCount = 0; //좌석 선택수
        String i_seat1; // 좌석1
        String i_seat2; // 좌석2
        String i_year = "2018"; //년도
        String i_month = "06"; //월
        int i_day = 16; //일



        public ActionResult Index()
        {
            /*
            conn.Open();
            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM MOVIE WHERE MOV_NO='2'";
            dr = cmd.ExecuteReader();
            dr.Read();
            //cmd.CommandText = "SELECT * FROM CSTMR WHERE CSTMR_ID = 'BO' AND PW = 'BO'";  테스트용
            //cmd.CommandType = CommandType.Text; ???
            //testLb.Text = dr[2] as String;
            

            ViewBag.Name = dr[2] as String; 
            Console.WriteLine(dr[2]);
            ViewData["isLogin"] = "false";
            ViewData["imageTest"] = "C:\\Users\\jhnote\\Desktop\\DBA\\image\\titanic.jpg";
            // String k=DateTime.Now.ToString("MM"); // 테스트용 
            // ViewData["Testing Month"] = k; //

            conn.Dispose();
            return View();

            */
            ViewData["imageTest"] = "http://img1.daumcdn.net/thumb/C155x225/?fname=http%3A%2F%2Ft1.daumcdn.net%2Fmovie%2Ff7127663f3e94138a5f2d1a50c50127fb4b95d45";
            
            return View();
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            conn.Open();
            cmd = new OracleCommand();
            cmd.Connection = conn;

            //cmd.CommandText = "SELECT * FROM CSTMR WHERE CSTMR_ID = 'usa' AND PW = 'usa'";
            cmd.CommandText = "SELECT * FROM CSTMR";

            //cmd.CommandType = CommandType.Text;

            //테스트용
            dr = cmd.ExecuteReader();
            dr.Read();
            dr.Read();
            ViewBag.Message = dr[2] as String;
            ViewData["depth"] = dr.Depth;
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            try
            {

                String id = collection["Id"];
                String Password = collection["Password"];
                String sql = "SELECT * FROM CSTMR WHERE CSTMR_ID = '" + id + "' AND PW = '" + Password + "'";
                ViewData["sql"] = sql; //테스트용

                //db 연결
                conn.Open();
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                dr = cmd.ExecuteReader();
                dr.Read();
                
                //테스트용
                string result = dr[1] as String;
                ViewBag.Message = result;
                
                //테스트용
                if (result.Equals(id))
                {
                    Console.WriteLine("login success");
                    ViewData["result"] = "success";
                    ViewData["id"] = dr[0] as String;
                    ViewData["isLogin"] = "true";
                    return View("Index");
                }
                else
                {
                    Console.WriteLine("login fail");
                    ViewData["result"] = "fail";
                    return View();
                }
            }
            catch
            {
                ViewBag.Message = "error";
                return View();
            }
        }

        

        /// <summary>
        /// 회원가입
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 회원가입 post
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            try
            {
                String Id = collection["Id"];
                String Pw = collection["Pw"];
                String Name = collection["Name"];
                String Birth = collection["Birth"];
                String PhoneNum = collection["PhoneNum"];
                String RegisterDate = collection["RegisterDate"];

                String sql = "INSERT INTO CSTMR VALUES('" + Id + "', '" + Pw + "', '" + Name + "', TO_DATE('" + Birth + "', 'YYYYMMDD'), '" + PhoneNum + "', TO_DATE('" + RegisterDate + "', 'YYYYMMDD'), 1, 0, NULL)";
                ViewData["TempSQL"] = sql;
                conn.Open();
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                int rowCount = cmd.ExecuteNonQuery();
                ViewData["IsRegister"] = "Register success";

                //로그인 성공시 메인페이지로 이동
                ViewData["id"] = Id;
                ViewData["isLogin"] = "true";
                return View("Index");
            }
            catch
            {

                ViewData["IsRegister"] = "Register fail ";
                return View();
            }
            
        }

        /// <summary>
        /// 예매 일자 선택 페이지
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact(string param)
        {
            ViewBag.Message = "예매 페이지 입니다.";


            //String year=  DateTime.Now.ToString("yyyy");
            //String month = DateTime.Now.ToString("MM");
            //String day = DateTime.Now.ToString("dd");
            //int d = Int32.Parse(day);

            

            String date1 = i_year + "년" + i_month + "월" + i_day.ToString()+"일";
            String date2 = i_year + "년" + i_month + "월" + (i_day + 1).ToString() + "일";
            String date3 = i_year + "년" + i_month + "월" + (i_day + 2).ToString() + "일";
            String date4 = i_year + "년" + i_month + "월" + (i_day + 3).ToString() + "일";
            String date5 = i_year + "년" + i_month + "월" + (i_day + 4).ToString() + "일";
            String date6 = i_year + "년" + i_month + "월" + (i_day + 5).ToString() + "일";
            String date7 = i_year + "년" + i_month + "월" + (i_day + 6).ToString() + "일";
            
            ViewData["date1"] = date1;
            ViewData["date2"] = date2;
            ViewData["date3"] = date3;
            ViewData["date4"] = date4;
            ViewData["date5"] = date5;
            ViewData["date6"] = date6;
            ViewData["date7"] = date7;

            return View();
        }

        /// <summary>
        /// 예매 영화 일정 선택 페이지
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult Ticket(string param)
        {
            // 데이터 조정
            i_day=i_day+Int32.Parse(param)-1;




            // 영화1 초기화
            String tempDay1 = i_day.ToString();
            String tempDay2 = (i_day+1).ToString();
            conn.Open();
            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT SCHDUL_NO,TO_CHAR(SCRN_STRT,'HH24MI') FROM SCRN_SCHDUL WHERE SCRN_STRT >= TO_DATE('201806"+tempDay1+ "', 'YYYYMMDD') AND SCRN_STRT < TO_DATE('201806"+tempDay2+"','YYYYMMDD') AND MOV_NO = '1' ";
            dr = cmd.ExecuteReader();

            
            //입력
            dr.Read();
            ViewData["get1S1"] = dr[1];
            ViewData["Ticket_get1S1"] = dr[0];
            dr.Read();
            ViewData["get1S2"] = dr[1];
            ViewData["Ticket_get1S2"] = dr[0];
            dr.Read();
            ViewData["get1S3"] = dr[1];
            ViewData["Ticket_get1S3"] = dr[0];

            //영화 2 초기화

            cmd.CommandText = "SELECT SCHDUL_NO,TO_CHAR(SCRN_STRT,'HH24MI') FROM SCRN_SCHDUL WHERE SCRN_STRT >= TO_DATE('201806" + tempDay1 + "', 'YYYYMMDD') AND SCRN_STRT < TO_DATE('201806" + tempDay2 + "','YYYYMMDD') AND MOV_NO = '2' ";
            dr = cmd.ExecuteReader();
            dr.Read();
            ViewData["get2S1"] = dr[1];
            



            // Seat 초기화
            for (int i = 0; i < 15; i++)
            {
                getSeat[i] = "0";
            }
            // 다음페이지용 초기화
            ViewData["isPicked"] = "false"; 
            isFirst = true;
            return View();
        }

        /// <summary>
        /// 예매 좌석 페이지
        /// </summary>
        /// <returns></returns>
        public ActionResult Seat(string param)
        {
            if (isFirst) //전 페이지에서 넘어왔을때
            {

                i_movie = param;
                isFirst = false;
            }else
            {
                if (!isPicked)
                {
                    pickseat1 = param;
                    i_seat1 = param;
                    pickseat1 = "clickSeat" + pickseat1;
                    ViewData[pickseat1] = ";background-color:aqua";

                    isPicked = true;
                    ViewData["testingSeat2"] = "first";
                    i_seatCount = 1;
                }
                else
                {
                    ViewData["testingSeat2"] = "second";
                    pickseat2 = param;
                    i_seat2 = param;
                    ViewData[pickseat1] = ";background-color:aqua";
                    pickseat2 = "clickSeat" + pickseat2;
                    ViewData[pickseat2] = ";background-color:aqua";
                    i_seatCount = 2;
                    isPicked = false;
                }
            }
            
            
            
           
            //ViewData["clickSeat1"] = ";background-color:aqua";

            conn.Open();
            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT RSRV_NO FROM SEAT_RSRV WHERE SCHDUL_NO = '1' ";
            dr = cmd.ExecuteReader();

            
            for(int i = 0; i < 15; i++)
            {
                dr.Read();
                if (dr[0] == DBNull.Value)
                {
                    getSeat[i] = "null";
                }
                else
                {
                    //getSeat[i] = "not null";
                    getSeat[i] = dr[0] as String;
                }
                
            }
            ViewData["testingSeat"] = getSeat[0];
            
            //dr.Read();
            //ViewData["testingSeat"] = dr[0] as string;
            return View();
        }


        /// <summary>
        /// 결제 페이지
        /// </summary>
        /// <returns></returns>
        public ActionResult Buy()
        {
            ViewData["Buy_seat1"] = pickseat1;
            ViewData["Buy_seat2"] = pickseat2;

            ViewData["Buy_movie"] = i_movie;
            ViewData["Buy_date"] = i_year + "년 " + i_month + "월 " + i_day.ToString() + "일";
            ViewData["Buy_countSeat"] = i_seatCount.ToString();
            if (i_seatCount == 1)
            {
                ViewData["Buy_seat"] = i_seat1;
                ViewData["Buy_price"] = "9000";
            }
            else
            {
                ViewData["Buy_seat"] = i_seat1+", "+i_seat2;
                ViewData["Buy_price"] = "18000";
            }
            return View();
        }




        /// <summary>
        /// 확인용
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            conn = new OracleConnection(ordba);
            conn.Open();
            cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT * FROM MOVIE WHERE MOV_NO='1'";
            //cmd.CommandType = CommandType.Text;
            //testLb.Text = dr[2] as String;



            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            ViewBag.Name = dr[2] as String;
            Console.WriteLine(dr[2]);
            conn.Dispose();

            ViewBag.Message = "Your application description page.";

            return View();
        }
        
    }
}