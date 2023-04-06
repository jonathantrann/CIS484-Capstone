using Lab3.Pages.DataClasses;
using System.Data.SqlClient;
using System;

namespace Lab3.Pages.DB
{
    public class DBClass
    {
        // Connection at the Class Level
        public static SqlConnection LabDBConnection = new SqlConnection();

        // Connection String
        private static readonly String? LabDBConnString = "Server=LocalHost;Database=Lab3;Trusted_Connection=True";

        // Connection at the Class Level
        public static SqlConnection AuthDBConnection = new SqlConnection();

        // Connection String
        private static readonly String? AuthDBConnString = "Server=LocalHost;Database=AUTH;Trusted_Connection=True";

        public static SqlDataReader SpecificFaculty(string facultyName)
        {
            LabDBConnection.Close();

            SqlCommand cmdSpecFacultyRead = new SqlCommand();
            cmdSpecFacultyRead.Connection = LabDBConnection;
            cmdSpecFacultyRead.Connection.ConnectionString = LabDBConnString;
            cmdSpecFacultyRead.CommandText = "SELECT Faculty.FacultyFirst, Faculty.FacultyLast, OfficeHours.OfficeHoursID, OfficeHours.OfficeHoursDays, OfficeHours.OHStartTime, OfficeHours.OHEndTime, Faculty.OfficeLocation, OfficeHours.WaitingRoom FROM Faculty INNER JOIN OfficeHours ON Faculty.FacultyID = OfficeHours.FacultyID WHERE Faculty.FacultyFirst LIKE '%' + @FacultyName + '%' OR Faculty.FacultyLast LIKE '%' + @FacultyName + '%' OR Faculty.FacultyFirst + ' ' + Faculty.FacultyLast LIKE '%' + @FacultyName + '%'";

            cmdSpecFacultyRead.Parameters.AddWithValue("@FacultyName", facultyName);

            cmdSpecFacultyRead.Connection.Open();
            SqlDataReader tempReader = cmdSpecFacultyRead.ExecuteReader();

            return tempReader;
        }

        public static SqlDataReader SpecificFacultyClassesReader(int facultyid)
        {
            LabDBConnection.Close();
            SqlCommand cmdSpecFacultyClassRead = new SqlCommand();
            cmdSpecFacultyClassRead.Connection = LabDBConnection;
            cmdSpecFacultyClassRead.Connection.ConnectionString = LabDBConnString;
            cmdSpecFacultyClassRead.CommandText = "SELECT Class.ClassID, Class.ClassName, Class.ClassSection, Faculty.FacultyID, Faculty.FacultyFirst, Faculty.FacultyLast " +
                "FROM Faculty " +
                "JOIN FacultySchedule ON FacultySchedule.FacultyID = Faculty.FacultyID " +
                "JOIN Class ON FacultySchedule.ClassID = Class.ClassID " +
                "WHERE Faculty.FacultyID = @FacultyID;";
            cmdSpecFacultyClassRead.Parameters.AddWithValue("@FacultyID", facultyid);
            cmdSpecFacultyClassRead.Connection.Open();
            SqlDataReader tempReader = cmdSpecFacultyClassRead.ExecuteReader();

            return tempReader;

        }
        public static void DeleteQueueRowsByUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(LabDBConnString))
            using (SqlCommand command = new SqlCommand("DELETE FROM Queue " +
            "WHERE OfficeHoursID IN ( " +
            "SELECT OfficeHoursID FROM OfficeHours " +
            "JOIN Faculty ON OfficeHours.FacultyID = Faculty.FacultyID " +
            "JOIN Student ON Queue.StudentID = Student.StudentID " +
            "WHERE Student.Username = @username);", connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                command.ExecuteNonQuery();
            }

        }

        public static SqlDataReader SearchedFacultyReader(string facultyName)
        {
            LabDBConnection.Close();

            SqlCommand cmdSpecFacultyRead = new SqlCommand();
            cmdSpecFacultyRead.Connection = LabDBConnection;
            cmdSpecFacultyRead.Connection.ConnectionString = LabDBConnString;
            cmdSpecFacultyRead.CommandText = "SELECT Faculty.FacultyFirst, Faculty.FacultyLast, Faculty.FacultyID " +
                "FROM Faculty WHERE Faculty.FacultyFirst LIKE '%' + @FacultyName + '%' OR Faculty.FacultyLast LIKE '%' + @FacultyName + " +
                "'%' OR Faculty.FacultyFirst + ' ' + Faculty.FacultyLast LIKE '%' + @FacultyName + '%'";

            cmdSpecFacultyRead.Parameters.AddWithValue("@FacultyName", facultyName);

            cmdSpecFacultyRead.Connection.Open();
            SqlDataReader tempReader = cmdSpecFacultyRead.ExecuteReader();

            return tempReader;
        }

        // Reads the data in the faculty table
        public static SqlDataReader FacultyReader()
        {
            LabDBConnection.Close();
            SqlCommand cmdFacultyRead = new SqlCommand();
            cmdFacultyRead.Connection = LabDBConnection;
            cmdFacultyRead.Connection.ConnectionString = LabDBConnString;
            cmdFacultyRead.CommandText = "SELECT * FROM Faculty";

            cmdFacultyRead.Connection.Open();

            SqlDataReader tempReader = cmdFacultyRead.ExecuteReader();

            return tempReader;
        }

        public static SqlDataReader StudentReader()
        {
            LabDBConnection.Close();
            SqlCommand cmdStudentRead = new SqlCommand();
            cmdStudentRead.Connection = LabDBConnection;
            cmdStudentRead.Connection.ConnectionString = LabDBConnString;
            cmdStudentRead.CommandText = "SELECT * FROM Student";

            cmdStudentRead.Connection.Open();

            SqlDataReader tempReader = cmdStudentRead.ExecuteReader();

            return tempReader;
        }



        public static int InsertStudent(Student s)
        {
            string sqlQuery = "INSERT INTO Student (StudentFirst, StudentLast, StudentEmailAddress, StudentPhoneNumber, Username, type) OUTPUT INSERTED.StudentID " +
                              "VALUES (@StudentFirst, @StudentLast, @StudentEmailAddress, @StudentPhoneNumber, @Username, @type);";

            SqlCommand cmdStudentRead = new SqlCommand();
            cmdStudentRead.Connection = LabDBConnection;
            cmdStudentRead.Connection.ConnectionString = LabDBConnString;
            cmdStudentRead.CommandText = sqlQuery;
            cmdStudentRead.Parameters.AddWithValue("@StudentFirst", s.StudentFirst);
            cmdStudentRead.Parameters.AddWithValue("@StudentLast", s.StudentLast);
            cmdStudentRead.Parameters.AddWithValue("@StudentEmailAddress", s.StudentEmailAddress);
            cmdStudentRead.Parameters.AddWithValue("@StudentPhoneNumber", s.StudentPhoneNumber);
            cmdStudentRead.Parameters.AddWithValue("@Username", s.Username);
            cmdStudentRead.Parameters.AddWithValue("@Type", s.type);

            cmdStudentRead.Connection.Open();
            int studentId = (int)cmdStudentRead.ExecuteScalar();
            cmdStudentRead.Connection.Close();

            return studentId;
        }

        public static bool IsStudent(string Username)
        {
            LabDBConnection.Close();

            string typeQuery = "SELECT type FROM Student WHERE Username = @Username";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = LabDBConnection;
            cmd.Connection.Close();
            cmd.Connection.ConnectionString = LabDBConnString;

            cmd.CommandText = typeQuery;
            cmd.Parameters.AddWithValue("@Username", Username);

            cmd.Connection.Open();
            object result = cmd.ExecuteScalar();

            cmd.Connection.Close();

            if (result != null && result.ToString() == "S")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool IsFaculty(string Username)
        {

            string typeQuery = "SELECT type FROM Faculty WHERE Username = @Username";


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = LabDBConnection;
            cmd.Connection.ConnectionString = LabDBConnString;

            cmd.CommandText = typeQuery;
            cmd.Parameters.AddWithValue("@Username", Username);

            cmd.Connection.Open();
            object result = cmd.ExecuteScalar();

            cmd.Connection.Close();

            if (result != null && result.ToString() == "I")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Adds a new student when signing up for office hours
        public static int InsertFaculty(Faculty f)
        {
            LabDBConnection.Close();
            String sqlQuery = "INSERT INTO Faculty (FacultyFirst, FacultyLast, FacultyEmailAddress, FacultyPhoneNumber, OfficeLocation, Username, type) OUTPUT INSERTED.FacultyID VALUES (@FacultyFirst, @FacultyLast, @FacultyEmailAddress, @FacultyPhoneNumber, @OfficeLocation, @Username, @type)";

            SqlCommand cmdStudentRead = new SqlCommand();
            cmdStudentRead.Connection = LabDBConnection;
            cmdStudentRead.Connection.Close();
            cmdStudentRead.Connection.ConnectionString = LabDBConnString;
            cmdStudentRead.CommandText = sqlQuery;

            cmdStudentRead.Parameters.AddWithValue("@FacultyFirst", f.FacultyFirst);
            cmdStudentRead.Parameters.AddWithValue("@FacultyLast", f.FacultyLast);
            cmdStudentRead.Parameters.AddWithValue("@FacultyEmailAddress", f.FacultyEmailAddress);
            cmdStudentRead.Parameters.AddWithValue("@FacultyPhoneNumber", f.FacultyPhoneNumber);
            cmdStudentRead.Parameters.AddWithValue("@OfficeLocation", f.OfficeLocation);
            cmdStudentRead.Parameters.AddWithValue("@Username", f.Username);
            cmdStudentRead.Parameters.AddWithValue("@Type", f.type);

            cmdStudentRead.Connection.Open();

            int facultyId = (int)cmdStudentRead.ExecuteScalar();

            cmdStudentRead.Connection.Close();

            return facultyId;
        }




        // Genereal Query Reader to run and return any SELECT query
        public static SqlDataReader GeneralReaderQuery(string sqlQuery)
        {

            SqlCommand cmdGeneralRead = new SqlCommand();
            cmdGeneralRead.Connection = LabDBConnection;
            cmdGeneralRead.Connection.ConnectionString = LabDBConnString;
            cmdGeneralRead.CommandText = sqlQuery;
            cmdGeneralRead.Connection.Open();
            SqlDataReader tempReader = cmdGeneralRead.ExecuteReader();

            return tempReader;
        }


        public static bool HashedParameterLogin(string Username, string Password)
        {
            string loginQuery =
                "SELECT Password FROM HashedCredentials WHERE Username = @Username";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = AuthDBConnection;
            cmdLogin.Connection.ConnectionString = AuthDBConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Username", Username);

            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            SqlDataReader hashReader = cmdLogin.ExecuteReader();
            if (hashReader.Read())
            {
                string correctHash = hashReader["Password"].ToString();

                if (PasswordHash.ValidatePassword(Password, correctHash))
                {
                    return true;
                }
            }

            return false;
        }

        public static void CreateHashedUser(string Username, string Password)
        {
            string loginQuery =
                "INSERT INTO HashedCredentials (Username,Password) values (@Username, @Password)";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = AuthDBConnection;
            cmdLogin.Connection.ConnectionString = AuthDBConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Username", Username);
            cmdLogin.Parameters.AddWithValue("@Password", PasswordHash.HashPassword(Password));

            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            cmdLogin.ExecuteNonQuery();

        }

        public static bool StoredProcedureLogin(string Username, string Password)
        {

            SqlCommand cmdProductRead = new SqlCommand();
            cmdProductRead.Connection = AuthDBConnection;
            cmdProductRead.Connection.ConnectionString = AuthDBConnString;
            cmdProductRead.CommandType = System.Data.CommandType.StoredProcedure;
            cmdProductRead.Parameters.AddWithValue("@Username", Username);
            cmdProductRead.Parameters.AddWithValue("@Password", Password);
            cmdProductRead.CommandText = "sp_Lab3Login";
            cmdProductRead.Connection.Open();
            if (((int)cmdProductRead.ExecuteScalar()) > 0)
            {
                return true;
            }

            return false;
        }

        public static SqlDataReader SpecificQueue(String username)
        {
            LabDBConnection.Close();

            SqlCommand cmdSpecFacultyRead = new SqlCommand();
            cmdSpecFacultyRead.Connection = LabDBConnection;
            cmdSpecFacultyRead.Connection.ConnectionString = LabDBConnString;
            cmdSpecFacultyRead.CommandText = "SELECT Queue.ready, Queue.QueuePosition, Faculty.FacultyFirst, Faculty.FacultyLast, OfficeHours.OfficeHoursDays, OfficeHours.OHStartTime, OfficeHours.OHEndTime, OfficeHours.WaitingRoom FROM OfficeHours JOIN Faculty ON OfficeHours.FacultyID = Faculty.FacultyID JOIN Queue ON Queue.OfficeHoursID = OfficeHours.OfficeHoursID JOIN Student ON Queue.StudentID = Student.StudentID WHERE Student.Username = @Username";

            cmdSpecFacultyRead.Parameters.AddWithValue("@Username", username);

            cmdSpecFacultyRead.Connection.Open();
            SqlDataReader tempReader = cmdSpecFacultyRead.ExecuteReader();

            return tempReader;
        }

        public static SqlDataReader SpecificOfficeHours(int facultyid)
        {
            LabDBConnection.Close();

            SqlCommand cmdSpecFacultyRead = new SqlCommand();
            cmdSpecFacultyRead.Connection = LabDBConnection;
            cmdSpecFacultyRead.Connection.ConnectionString = LabDBConnString;
            cmdSpecFacultyRead.CommandText = "SELECT Faculty.FacultyFirst, Faculty.FacultyLast, OfficeHours.OfficeHoursID, OfficeHours.OfficeHoursDays, OfficeHours.OHStartTime, OfficeHours.OHEndTime, Faculty.OfficeLocation, OfficeHours.WaitingRoom FROM Faculty INNER JOIN OfficeHours ON Faculty.FacultyID = OfficeHours.FacultyID WHERE Faculty.FacultyID = @FacultyID";

            cmdSpecFacultyRead.Parameters.AddWithValue("@FacultyID", facultyid);

            cmdSpecFacultyRead.Connection.Open();
            SqlDataReader tempReader = cmdSpecFacultyRead.ExecuteReader();

            return tempReader;
        }


        //public static void InsertQueue(Queue q, int officehoursID)
        //{
        //    LabDBConnection.Close();
        //    using (SqlConnection connection = new SqlConnection(LabDBConnString))
        //    {
        //        connection.Open();

        //        string sqlQuery = "INSERT INTO Queue (QueuePosition, StudentID, OfficeHoursID) VALUES(@QueuePosition, @StudentID, @OfficeHoursID);";

        //        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@QueuePosition", q.QueuePosition);
        //            command.Parameters.AddWithValue("@StudentID", q.StudentID);
        //            command.Parameters.AddWithValue("@OfficeHoursID", officehoursID);


        //            command.ExecuteNonQuery();

        //        }
        //    }
        //}

        public static void InsertQueue(Queue q, int officehoursID)
        {
            if (q == null)
            {
                throw new ArgumentNullException(nameof(q));
            }

            if (officehoursID < 0)
            {
                throw new ArgumentException("officehoursID must be a positive integer.", nameof(officehoursID));
            }

            LabDBConnection.Close();

            using (SqlConnection connection = new SqlConnection(LabDBConnString))
            {
                connection.Open();

                string sqlQuery = "INSERT INTO Queue (QueuePosition, StudentID, OfficeHoursID) VALUES(@QueuePosition, @StudentID, @OfficeHoursID);";

                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@QueuePosition", q.QueuePosition);
                        command.Parameters.AddWithValue("@StudentID", q.StudentID);
                        command.Parameters.AddWithValue("@OfficeHoursID", officehoursID);

                        command.ExecuteNonQuery();
                    }

                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Error inserting into the Queue table: " + ex.Message);
                }
            }
        }


        public static SqlDataReader GetStudentID(String username)
        {
            LabDBConnection.Close();
            SqlCommand cmdIDRead = new SqlCommand();
            cmdIDRead.Connection = LabDBConnection;
            cmdIDRead.Connection.ConnectionString = LabDBConnString;
            cmdIDRead.CommandText = "SELECT StudentID FROM Student WHERE Username = @Username;";
            cmdIDRead.Parameters.AddWithValue("@Username", username);
            cmdIDRead.Connection.Open();
            SqlDataReader tempReader = cmdIDRead.ExecuteReader();

            return tempReader;
        }


        public static SqlDataReader GetStudentInfo(String username)
        {
            LabDBConnection.Close();
            SqlCommand cmdInfoRead = new SqlCommand();
            cmdInfoRead.Connection = LabDBConnection;
            cmdInfoRead.Connection.Close();
            cmdInfoRead.Connection.ConnectionString = LabDBConnString;
            cmdInfoRead.CommandText = "SELECT Student.StudentFirst, Student.StudentLast, Student.StudentEmailAddress, Student.StudentPhoneNumber, Student.Username FROM Student WHERE Student.Username = @Username;";
            cmdInfoRead.Parameters.AddWithValue("@Username", username);
            cmdInfoRead.Connection.Open();
            SqlDataReader tempReader = cmdInfoRead.ExecuteReader();

            return tempReader;
        }

        public static SqlDataReader AdminQueue()
        {
            LabDBConnection.Close();
            SqlCommand cmdAllQueueReader = new SqlCommand();
            cmdAllQueueReader.Connection = LabDBConnection;
            cmdAllQueueReader.Connection.ConnectionString = LabDBConnString;
            cmdAllQueueReader.CommandText = "SELECT Student.StudentFirst, Student.StudentLast, OfficeHours.OfficeHoursDays, OfficeHours.OHStartTime, OfficeHours.OHEndTime, Faculty.FacultyFirst, Faculty.FacultyLast, OfficeHours.WaitingRoom, Queue.MeetingPurpose, Queue.QueuePosition FROM Queue JOIN Student ON Student.StudentID = Queue.StudentID JOIN OfficeHours ON Queue.OfficeHoursID = OfficeHours.OfficeHoursID JOIN Faculty ON OfficeHours.FacultyID = Faculty.FacultyID;";
            cmdAllQueueReader.Connection.Open();
            SqlDataReader tempReader = cmdAllQueueReader.ExecuteReader();

            return tempReader;

        }



        public static bool StudentQueueExists(int studentID, int officeHoursID)
        {
            LabDBConnection.Close();

            using (LabDBConnection)
            {
                LabDBConnection.Open();

                string query = "SELECT COUNT(*) FROM Queue WHERE StudentID = @StudentID AND OfficeHoursID = @OfficeHoursID";
                using (SqlCommand command = new SqlCommand(query, LabDBConnection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentID);
                    command.Parameters.AddWithValue("@OfficeHoursID", officeHoursID);

                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        return true;
                    }

                    return false;

                }
            }
        }

        public static SqlDataReader OfficeHoursReader(int facultyid, int officehoursid)
        {
            LabDBConnection.Close();
            SqlCommand cmdSpecOfficeHoursRead = new SqlCommand();
            cmdSpecOfficeHoursRead.Connection = LabDBConnection;
            cmdSpecOfficeHoursRead.Connection.ConnectionString = LabDBConnString;
            cmdSpecOfficeHoursRead.CommandText = "SELECT Faculty.FacultyFirst, Faculty.FacultyLast, Faculty.FacultyID, OfficeHours.OfficeHoursID, OfficeHours.OfficeHoursDays, " +
                "OfficeHours.OHStartTime, OfficeHours.OHEndTime, OfficeHours.WaitingRoom, Faculty.OfficeLocation FROM Faculty INNER JOIN OfficeHours ON Faculty.FacultyID = OfficeHours.FacultyID " +
                "WHERE Faculty.FacultyID = @FacultyID " +
                "AND OfficeHours.OfficeHoursID = @OfficeHoursID;";
            cmdSpecOfficeHoursRead.Parameters.AddWithValue("@FacultyID", facultyid);
            cmdSpecOfficeHoursRead.Parameters.AddWithValue("@OfficeHoursID", officehoursid);
            cmdSpecOfficeHoursRead.Connection.Open();
            SqlDataReader tempReader = cmdSpecOfficeHoursRead.ExecuteReader();
            return tempReader;
        }

        public static SqlDataReader GetFacultyID(String username)
        {
            LabDBConnection.Close();
            SqlCommand cmdIDRead = new SqlCommand();
            cmdIDRead.Connection = LabDBConnection;
            cmdIDRead.Connection.ConnectionString = LabDBConnString;
            cmdIDRead.CommandText = "SELECT FacultyID FROM Faculty WHERE Username = @Username;";
            cmdIDRead.Parameters.AddWithValue("@Username", username);
            cmdIDRead.Connection.Open();
            SqlDataReader tempReader = cmdIDRead.ExecuteReader();

            return tempReader;
        }


        public static SqlDataReader GetFaultyInfo(String username)
        {
            LabDBConnection.Close();
            SqlCommand cmdInfoRead = new SqlCommand();
            cmdInfoRead.Connection = LabDBConnection;
            cmdInfoRead.Connection.ConnectionString = LabDBConnString;
            cmdInfoRead.CommandText = "SELECT Faculty.FacultyFirst, Faculty.FacultyLast, Faculty.FacultyEmailAddress, Faculty.FacultyPhoneNumber, Faculty.Username FROM Faculty WHERE Faculty.Username = @Username;";
            cmdInfoRead.Parameters.AddWithValue("@Username", username);
            cmdInfoRead.Connection.Open();
            SqlDataReader tempReader = cmdInfoRead.ExecuteReader();

            return tempReader;
        }

        public static void InsertOfficeHours(OfficeHours o, int facultyid)
        {
            LabDBConnection.Close();
            using (SqlConnection connection = new SqlConnection(LabDBConnString))
            {
                connection.Open();

                string sqlQuery = "INSERT INTO OfficeHours (OfficeHoursDays, OHStartTime, OHEndTime, WaitingRoom, FacultyID) VALUES(@OfficeHoursDays, @OHStartTime, @OHEndTime, @WaitingRoom, @FacultyID);";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    command.Parameters.AddWithValue("@OfficeHoursDays", o.OfficeHoursDays);
                    command.Parameters.AddWithValue("@OHStartTime", o.OHStartTime);
                    command.Parameters.AddWithValue("@OHEndTime", o.OHEndTime);
                    command.Parameters.AddWithValue("@WaitingRoom", o.WaitingRoom);
                    command.Parameters.AddWithValue("@FacultyID", facultyid);

                    command.ExecuteNonQuery();

                }
            }
        }

        public static SqlDataReader StudentsInSpecificQueue(int facultyid, int officehoursid)
        {
            LabDBConnection.Close();
            SqlCommand cmdStudentsInQueueRead = new SqlCommand();
            cmdStudentsInQueueRead.Connection = LabDBConnection;
            cmdStudentsInQueueRead.Connection.ConnectionString = LabDBConnString;
            cmdStudentsInQueueRead.CommandText = "SELECT Faculty.FacultyFirst, Faculty.FacultyLast, OfficeHours.OfficeHoursID, OfficeHours.OfficeHoursDays, " +
                "OfficeHours.OHStartTime, OfficeHours.OHEndTime, OfficeHours.WaitingRoom, Faculty.OfficeLocation, Student.StudentID, Student.StudentFirst, Student.StudentLast, Queue.QueueID " +
                "FROM Student " +
                "JOIN Queue ON Student.StudentID = Queue.StudentID " +
                "JOIN OfficeHours ON OfficeHours.OfficeHoursID = Queue.OfficeHoursID " +
                "JOIN Faculty ON OfficeHours.FacultyID = Faculty.FacultyID " +
                "WHERE Faculty.FacultyID = @FacultyID " +
                "AND OfficeHours.OfficeHoursID = @OfficeHoursID;";
            cmdStudentsInQueueRead.Parameters.AddWithValue("@FacultyID", facultyid);
            cmdStudentsInQueueRead.Parameters.AddWithValue("@OfficeHoursID", officehoursid);
            cmdStudentsInQueueRead.Connection.Open();
            SqlDataReader tempReader = cmdStudentsInQueueRead.ExecuteReader();
            return tempReader;
        }
        public static void NotifyStudent(int queueId)
        {
            LabDBConnection.Close();
            using (SqlConnection connection = new SqlConnection(LabDBConnString))
            {
                connection.Open();

                string sqlQuery = "UPDATE Queue SET ready = 1 WHERE queueID = @queueID;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    command.Parameters.AddWithValue("@queueID", queueId);

                    command.ExecuteNonQuery();

                }
            }
        }
    }
}
