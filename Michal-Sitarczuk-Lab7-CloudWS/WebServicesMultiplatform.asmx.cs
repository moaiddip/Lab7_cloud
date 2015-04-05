using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Diagnostics;
using System.Data;
using System.Collections;

namespace Michal_Sitarczuk_Lab7_CloudWS
{
    /// <summary>
    /// QUIZLANG web services by Michal Sitarczuk & Vahidin Ljaic & Sepideh Salehi
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServicesMultiplatform : System.Web.Services.WebService
    {
       [WebMethod]
        public String debugTest()
        {
            return "This is the debug test string";
        }

        private SqlConnection DBConn()
        {
            SqlConnection dbcon = null;
            try
            {
                //FIGURE OUT RELATIVE PATHS
                //dbcon = new SqlConnection("Data Source=(LocalDB)\\v12.0;AttachDbFilename=E:\\other stuff\\Skydrive\\PROJECTS eclipse netbeans etc\\VisualStudio\\Michal-Sitarczuk-Lab7-CloudWS\\Michal-Sitarczuk-Lab7-CloudWS\\App_Data\\Database1.mdf;Integrated Security=True;Connect Timeout=30");
                //dbcon = new SqlConnection("Data Source=(LocalDB)\\v12.0;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;Connect Timeout=30");
                dbcon = new SqlConnection("workstation id=dbLoQui.mssql.somee.com;packet size=4096;user id=db_LoQui;pwd=123.Test.321;data source=dbLoQui.mssql.somee.com;persist security info=False;initial catalog=dbLoQui");

                dbcon.Open();
                Debug.WriteLine("Coonection to DB established!");
                return dbcon;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return dbcon;
            }
        }
        private void closeConnection(SqlConnection con)
        {
            try
            {
                con.Close();
                Debug.WriteLine("Connection closed.");
            }
            catch (Exception ex)
            {
                //Console.Write("Exception == " + ex);
                Debug.WriteLine("Exception: " + ex);
            }
        }

        [WebMethod]
        public ArrayList GetLanguagesList()
        {
            string query = "select * from dbo.Languages";
            SqlConnection con = DBConn();
            ArrayList langs = new ArrayList();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader rd = com.ExecuteReader();
            while (rd.Read())
            {
                langs.Add(rd["Language"].ToString());
                //Debug.WriteLine(rd["Language"].ToString());
            }
            closeConnection(con);
            return langs;
        }

        [WebMethod]
        public Boolean AddWord(String lang, String word)
        {
            try
            {
                string query = "INSERT INTO " + lang + " (word) VALUES ('" + word + "');";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                closeConnection(con);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return false;
            }

        }

        [WebMethod]
        public Boolean AddLanguage(String lang)
        {
            try
            {
                string query = "CREATE TABLE [dbo].[" + lang + "] ([Id] INT IDENTITY (1, 1) NOT NULL, [word] VARCHAR (MAX) NOT NULL, PRIMARY KEY CLUSTERED ([Id] ASC))";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                query = "INSERT INTO Languages (Language) VALUES ('" + lang + "');";
                com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                closeConnection(con);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return false;
            }
        }

        [WebMethod]
        public ArrayList GetAllWords(string lang)
        {
            ArrayList words = new ArrayList();
            //string query = "select * from [dbo].[" + lang + "] (word);";
            string query = "select * from [dbo].[" + lang + "];";
            SqlConnection con = DBConn();
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            SqlDataReader rd = com.ExecuteReader();
            while (rd.Read())
            {
                words.Add(rd["word"].ToString());
                //Debug.WriteLine(rd["Language"].ToString());
            }
            closeConnection(con);
            return words;
        }

        [WebMethod]
        public String RegisterStudent(String name, String surname, String lang_native, String lang_learning)
        {
            String username = "";
            string query = "SELECT * FROM dbo.UserDetails WHERE FirstName = '" + name + "' AND Surname = '" + surname + "';";
            var table = new DataTable();
            bool c = false;
            SqlConnection con;
            SqlCommand com;
            SqlDataReader rd;
            try
            {
                con = DBConn();
                com = new SqlCommand(query, con);
                ArrayList q = new ArrayList();
                int num = 0;
                rd = com.ExecuteReader();
                while (rd.Read())
                {
                    q.Add(rd["FirstName"].ToString());
                    Debug.WriteLine("Person exists in the database: " + rd["FirstNAme"].ToString() + " " + rd["FirstNAme"].ToString());
                }
                rd.Close();
                com.Dispose();
                if (q.Count != 0)
                {
                    c = false;
                }
                else
                {
                    c = true;
                }
                Debug.WriteLine("Boolean c value is: " + c);
                if (c)
                {
                    query = "SELECT * FROM dbo.UserDetails;";
                    com = new SqlCommand(query, con);
                    rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        Debug.WriteLine("User found");
                        num++;
                    }
                    rd.Close();
                    com.Dispose();
                    Debug.WriteLine("Number of all registered users: " + num);
                    String tmpName;
                    String tmpSurname;
                    if (name.Length < 3)
                    {
                        tmpName = name;
                    }
                    else
                    {
                        tmpName = name.Substring(0, 3);
                    }
                    if (surname.Length < 3)
                    {
                        tmpSurname = surname;
                    }
                    else
                    {
                        tmpSurname = surname.Substring(0, 3);
                    }
                    username = tmpName + (num + 1) + tmpSurname;
                    query = "INSERT INTO dbo.UserDetails (Username, FirstName, Surname, NativeLanguage, LearningLanguage) VALUES ('" + username + "', '" + name + "', '" + surname + "', '" + lang_native + "', '" + lang_learning + "');";
                    com = new SqlCommand(query, con);
                    com.ExecuteNonQuery();
                    com.Dispose();
                    closeConnection(con);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                c = false;
            }
            return username;
        }

        [WebMethod]
        public ArrayList GetAllUsers()
        {
            String query = "SELECT * FROM dbo.UserDetails;";
            SqlConnection con = DBConn();
            SqlCommand com = new SqlCommand(query, con);
            ArrayList allUsers = new ArrayList();
            SqlDataReader rd = com.ExecuteReader();
            while (rd.Read())
            {
                allUsers.Add(rd["Surname"].ToString() + ", " + rd["FirstName"].ToString() + ", " + rd["Username"]);
                //Debug.WriteLine(rd["Language"].ToString());
            }
            rd.Close();
            com.Dispose();
            closeConnection(con);
            return allUsers;
        }

        [WebMethod]
        public Boolean LogIn(String username)
        {
            Boolean logged;

            String query = "SELECT username FROM dbo.UserDetails WHERE Username = '" + username + "';";
            SqlConnection con = DBConn();
            SqlCommand com = new SqlCommand(query, con);
            ArrayList usernames = new ArrayList();
            SqlDataReader rd = com.ExecuteReader();
            while (rd.Read())
            {
                usernames.Add(rd["username"].ToString());
            }
            if (usernames.Count > 0)
            {
                logged = true;
            }
            else
            {
                logged = false;
            }
            rd.Close();
            com.Dispose();
            closeConnection(con);
            return logged;
        }

        [WebMethod]
        public String getNativeLanguage(String username)
        {
            String nativeLang = null;
            try
            {
                String query = "SELECT NativeLanguage FROM dbo.UserDetails WHERE Username = '" + username + "';";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    nativeLang = rd["NativeLanguage"].ToString();
                }
                rd.Close();
                com.Dispose();
                closeConnection(con);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
            }
            return nativeLang;
        }

        [WebMethod]
        public String getLearningLanguage(String username)
        {
            String learningLang = null;
            try
            {
                String query = "SELECT LearningLanguage FROM dbo.UserDetails WHERE Username = '" + username + "';";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    learningLang = rd["LearningLanguage"].ToString();
                }
                rd.Close();
                com.Dispose();
                closeConnection(con);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
            }
            return learningLang;
        }

        [WebMethod]
        public Boolean createMultipleRandomQuizes(int amount, String nativeLang, String username)
        {
            //ArrayList randomQuizes = new ArrayList();
            try
            {
                String query = "SELECT TOP 1 word FROM " + nativeLang + " ORDER BY NEWID();";
                String query2;
                SqlConnection con = DBConn();
                SqlConnection con2 = DBConn();
                SqlDataReader rd;
                SqlCommand com2;
                SqlCommand com = new SqlCommand(query, con);
                for (int i = 0; i < amount; i++)
                {
                    rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        //randomQuizes.Add(rd["word"].ToString());
                        query2 = "INSERT INTO QuizAnswers (UserID, Q_word) VALUES ('" + username + "', '" + rd["word"].ToString() + "');";
                        //Debug.WriteLine(query2);
                        //Debug.WriteLine(rd["word"].ToString());
                        com2 = new SqlCommand(query2, con2);
                        //Debug.WriteLine("Command txt: "+com2.CommandText+", command connection: "+com2.Connection.State.ToString());
                        com2.ExecuteNonQuery();
                        com2.Dispose();
                    }
                    rd.Close();
                }
                com.Dispose();
                closeConnection(con);
                closeConnection(con2);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return false;
            }
        }

        [WebMethod]
        public Boolean createOneQuiz(String username, String Q_word)
        {
            try
            {
                String query = "INSERT INTO QuizAnswers (UserID, Q_word) VALUES ('" + username + "', '" + Q_word + "');";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                com.Dispose();
                closeConnection(con);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return false;
            }
        }

        [WebMethod]
        public ArrayList getStudentQuizes(String username)
        {
            ArrayList quizesToGo = new ArrayList();
            try
            {
                String query = "SELECT * FROM QuizAnswers WHERE UserID='" + username + "' AND A_word IS NULL;";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    //Debug.WriteLine(rd["ID"].ToString());
                    //Debug.WriteLine(rd["Q_word"].ToString());
                    quizesToGo.Add(rd["ID"].ToString() + ", " + rd["Q_word"].ToString());
                }
                com.Dispose();
                closeConnection(con);
                return quizesToGo;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return quizesToGo;
            }
        }

        [WebMethod]
        public Boolean submitQuizAnswer(int id, String asnwer)
        {
            try
            {
                //String query = "INSERT INTO QuizAnswers (A_word) WHERE ID='" + id + "' VALUES ('"+asnwer+"');";
                String query = "UPDATE QuizAnswers SET A_word = '" + asnwer + "' WHERE ID='" + id + "';";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                com.Dispose();
                closeConnection(con);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return false;
            }
        }

        [WebMethod]
        public Boolean submitGrade(String username, String grade)
        {
            try
            {
                String query = "UPDATE UserDetails SET Grade = '" + grade + "' WHERE Username='" + username + "';";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                com.Dispose();
                closeConnection(con);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return false;
            }
        }

        [WebMethod]
        public String getGrade(String username)
        {
            String grade = "";
            try
            {
                String query = "SELECT Grade FROM UserDetails WHERE Username='" + username + "';";
                SqlConnection con = DBConn();
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Debug.WriteLine(rd["Grade"].ToString());
                    grade = rd["Grade"].ToString();
                }
                rd.Close();
                com.Dispose();
                closeConnection(con);
                return grade;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace.ToString());
                return grade;
            }
        }

        //TESTING CODES ETC
        private DataTable SendQuerytoDB(string inQuery)
        {
            DataTable dt = new DataTable();
            dt = null;

            SqlCommand cmd;
            SqlDataReader dr;
            SqlConnection con = null;
            try
            {
                con = DBConn();
                if (con != null)
                {
                    Debug.WriteLine("Connection not empty. Continuing...");
                    Debug.WriteLine("Query: " + inQuery);
                    cmd = new SqlCommand(inQuery, con);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                }
                else
                {
                    Debug.WriteLine("Connection empty. ERROR!");
                }
            }
            catch (Exception ex)
            {
                //Console.Write("Exception: " + ex);
                Debug.WriteLine("Exception: " + ex);

            }
            finally
            {
                Debug.WriteLine("Closing connection: " + con.ToString());
                closeConnection(con);
            }
            return dt;
        }

        private DataTable SelectData(String inQuery)
        {
            SqlConnection con = DBConn();
            DataTable dt = new DataTable();
            SqlDataReader dr;
            SqlCommand com = new SqlCommand(inQuery, con);
            dr = com.ExecuteReader();
            dt.Load(dr);
            closeConnection(con);
            return dt;
        }


        //[WebMethod]

        //[WebMethod]
        //public void ToArray()
        //{
        //    string inQuery = "SELECT * FROM Languages;";
        //    dt = SendQuerytoDB(inQuery);
        //    var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, dt.Columns.Count) as object[,];
        //    for (var i = 0; i < dt.Rows.Count - 1; i++)
        //    {
        //        for (var j = 0; j < dt.Columns.Count - 1; j++)
        //        {
        //            ret[i, j] = dt.Rows[i][j];
        //            Debug.WriteLine(ret[i, j]);
        //        }
        //    }
        //    //return ret;
        //}
    }
}