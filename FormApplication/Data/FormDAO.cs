using FormApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Linq;

namespace FormApplication.Data
{
    internal class FormDAO : IDisposable
    {
        public SqlConnection Connection { get; private set; }
        public FormDAO()
        {
            this.Connection = new SqlConnection(connectionString);
            this.Connection.Open();
        }
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FormApp;
                                            Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                            TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private bool disposedValue;
                   
    public List<FullFormModel> FetchAll()
    {
        List<FullFormModel> returnList = new List<FullFormModel>();
        string sqlQuery = "SELECT F.ID, F.Fullname, G.GenderType, H.HouseUnit, F.Email, F.Mobile, F.Profession, S.StatusType from dbo.FullForm as F join dbo.Gender as G ON F.Gender = G.GenderId join dbo.Housing as H ON F.HouseNumber = H.HouseId join dbo.Statutory as S ON F.Status = S.StatusId";

        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
        //connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                //create a new gadget object. Add it to the list to return.
                FullFormModel form = new FullFormModel() { 
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    Fullname = reader.GetString(reader.GetOrdinal("Fullname")),
                    Gender = reader.GetString(reader.GetOrdinal("GenderType")),
                    HouseNumber = reader.GetString(reader.GetOrdinal("HouseUnit")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Mobile = reader.GetString(reader.GetOrdinal("Mobile")),
                    Profession = reader.GetString(reader.GetOrdinal("Profession")),
                    Status = reader.GetString(reader.GetOrdinal("StatusType"))
                };
                returnList.Add(form);
            }
                reader.Close();
                string sqlQuery2 = "select s.FormId,s.Names, g.GenderType,s.EmailAddress, s.Number from dbo.Spouse as s join dbo.Gender as g on s.GenderType = g.GenderId";

                SqlCommand command2 = new SqlCommand(sqlQuery2, this.Connection);
                //connection.Open();
                SqlDataReader reader2 = command2.ExecuteReader();
                List<SpouseModel> spouseModels = new List<SpouseModel>();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        SpouseModel spouse =new SpouseModel() {
                            FormId = reader2.GetInt32(reader2.GetOrdinal("FormId")),
                            Names = reader2.GetString(reader2.GetOrdinal("Names")),
                            GenderType = reader2.GetString(reader2.GetOrdinal("GenderType")),
                            EmailAddress = reader2.GetString(reader2.GetOrdinal("EmailAddress")),
                            Number = reader2.GetString(reader2.GetOrdinal("Number"))
                        };
                        spouseModels.Add(spouse);
                    }
                }
                foreach (var tenant in returnList)
                {
                    tenant.SpouseModels = spouseModels.Where(s => s.FormId == tenant.ID).ToList();
                }


            }
            //reader.Close();
            return returnList;
    }
    public List<OnlineModel> EditDetails(int id)
        {

            List<OnlineModel> forms = new List<OnlineModel>();
            string sqlQuery = "select f.fullname, g.GenderType, h.HouseUnit, f.email, f.mobile, f.profession, s.StatusType, sp.names,ge.GenderType, sp.emailaddress, sp.number from FullForm AS f join gender as g on f.Gender = g.GenderId join Housing h on f.HouseNumber = h.HouseId join Statutory s on f.Status = s.StatusId join Spouse as sp on f.ID = sp.FormId join gender as ge on ge.GenderId = sp.GenderType WHERE Id = @id";

            //associate @id with Id parameters

            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
            //connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read() && reader.IsDBNull(reader.GetOrdinal("Fullname")))
                {
                    //create a new object, Add it to the list to return.
                    OnlineModel form = new OnlineModel() {
                        Fullname = reader.GetFieldValue<string>(reader.GetOrdinal("Fullname")),
                        Gender = reader.GetString(reader.GetOrdinal("GenderType")),
                        HouseUnit = reader.GetFieldValue<string>(reader.GetOrdinal("HouseUnit")),
                        Email = reader.GetFieldValue<string>(reader.GetOrdinal("Email")),
                        Mobile = reader.GetFieldValue<string>(reader.GetOrdinal("Mobile")),
                        Profession = reader.GetFieldValue<string>(reader.GetOrdinal("Profession")),
                        StatusType = reader.GetFieldValue<string>(reader.GetOrdinal("StatusType"))
                };
                    forms.Add(form);
                }
            }
            reader.Close();
            return forms;
        }
    public List<SpouseModel> FetchAllSpouse(int newId)
        {
            List<SpouseModel> returnList = new List<SpouseModel>();

            string sqlQuery = "select s.SpouseId,s.FormId,s.Names, g.GenderType,s.EmailAddress, s.Number from dbo.Spouse as s join dbo.Gender as g on s.GenderType = g.GenderId WHERE FormId = @FormId";
            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
            ////connection.Open();

            command.Parameters.Add("@FormId", System.Data.SqlDbType.Int).Value = newId;

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    SpouseModel spouse = new SpouseModel() {
                        SpouseId = reader.GetInt32(reader.GetOrdinal("SpouseId")),
                        FormId = reader.GetInt32(reader.GetOrdinal("FormId")),
                        Names = reader.GetString(reader.GetOrdinal("Names")),
                        GenderType = reader.GetString(reader.GetOrdinal("GenderType")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Number = reader.GetString(reader.GetOrdinal("Number"))
                    };
                    returnList.Add(spouse);
                }
                reader.Close();
            }
            
                return returnList;
        }
    public SpouseModel FetchSpouse(int id)
        {
            string sqlQuery = "SELECT sp.SpouseId, sp.FormId, sp.Names, G.GenderType, sp.EmailAddress, sp.Number from dbo.Spouse as sp join dbo.Gender as G ON sp.Gendertype = G.GenderId WHERE SpouseId =@SpouseId;";
            //associate @id with Id parameters
            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

            command.Parameters.Add("@SpouseId", System.Data.SqlDbType.Int).Value = id;

            SqlDataReader reader = command.ExecuteReader();

            SpouseModel form = new SpouseModel();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    form.SpouseId = reader.GetFieldValue<int>(reader.GetOrdinal("SpouseId"));
                    form.FormId = reader.GetFieldValue<int>(reader.GetOrdinal("FormId"));
                    form.Names = reader.GetFieldValue<string>(reader.GetOrdinal("Names"));
                    form.GenderType = reader.GetString(reader.GetOrdinal("GenderType"));
                    form.EmailAddress = reader.GetFieldValue<string>(reader.GetOrdinal("EmailAddress"));
                    form.Number = reader.GetFieldValue<string>(reader.GetOrdinal("Number"));
                }
            }
            reader.Close();
            return form;

        }//not in use
    public FullFormModel FetchOne(int id)
    {
        string sqlQuery = "SELECT F.ID, F.Fullname, G.GenderType, H.HouseUnit, F.Email, F.Mobile, F.Profession, S.StatusType from dbo.FullForm as F join dbo.Gender as G ON F.Gender = G.GenderId join dbo.Housing as H ON F.HouseNumber = H.HouseId join dbo.Statutory as S ON F.Status = S.StatusId WHERE Id =@id";
            //associate @id with Id parameters

            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
            //connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            FullFormModel form = new FullFormModel();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //create a new gadget object. Add it to the list to return.
                    form.ID = reader.GetFieldValue<int>(reader.GetOrdinal("ID"));
                    form.Fullname = reader.GetFieldValue<string>(reader.GetOrdinal("Fullname"));
                    form.Gender = reader.GetString(reader.GetOrdinal("GenderType"));
                    form.HouseNumber = reader.GetFieldValue<string>(reader.GetOrdinal("HouseUnit"));
                    form.Email = reader.GetFieldValue<string>(reader.GetOrdinal("Email"));
                    form.Mobile = reader.GetFieldValue<string>(reader.GetOrdinal("Mobile"));
                    form.Profession = reader.GetFieldValue<string>(reader.GetOrdinal("Profession"));
                    form.Status = reader.GetFieldValue<string>(reader.GetOrdinal("StatusType"));
                }
            }
            reader.Close();
            return form;
    }
    public FullFormModel FetchIndividualData(int id)
        {

            List<FullFormModel> returnList = new List<FullFormModel>();
            string sqlQuery = "SELECT F.ID, F.Fullname, G.GenderType, H.HouseUnit, F.Email, F.Mobile, F.Profession, S.StatusType from dbo.FullForm as F join dbo.Gender as G ON F.Gender = G.GenderId join dbo.Housing as H ON F.HouseNumber = H.HouseId join dbo.Statutory as S ON F.Status = S.StatusId WHERE Id =@id";

            //associate @id with Id Parameters

            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = id;

            SqlDataReader reader = command.ExecuteReader();

            FullFormModel form = new FullFormModel();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //create a new gadget object. Add it to the list to return.
                    form.ID = reader.GetFieldValue<int>(reader.GetOrdinal("ID"));
                    form.Fullname = reader.GetFieldValue<string>(reader.GetOrdinal("Fullname"));
                    form.Gender = reader.GetString(reader.GetOrdinal("GenderType"));
                    form.HouseNumber = reader.GetFieldValue<string>(reader.GetOrdinal("HouseUnit"));
                    form.Email = reader.GetFieldValue<string>(reader.GetOrdinal("Email"));
                    form.Mobile = reader.GetFieldValue<string>(reader.GetOrdinal("Mobile"));
                    form.Profession = reader.GetFieldValue<string>(reader.GetOrdinal("Profession"));
                    form.Status = reader.GetFieldValue<string>(reader.GetOrdinal("StatusType"));
                }
            }
            reader.Close();

            string sqlQuery2 = "select s.SpouseId, s.FormId,s.Names, g.GenderType,s.EmailAddress, s.Number from dbo.Spouse as s join dbo.Gender as g on s.GenderType = g.GenderId where FormId =@FormId";

            SqlCommand command2 = new SqlCommand(sqlQuery2, this.Connection);
            //connection.Open();

            command2.Parameters.Add("FormId", System.Data.SqlDbType.Int).Value = id;

            SqlDataReader reader2 = command2.ExecuteReader();
            List<SpouseModel> spouseModels = new List<SpouseModel>();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    SpouseModel spouseModel = new SpouseModel()
                    {
                        SpouseId = reader2.GetInt32(reader2.GetOrdinal("SpouseId")),
                        FormId = reader2.GetInt32(reader2.GetOrdinal("FormId")),
                        Names = reader2.GetString(reader2.GetOrdinal("Names")),
                        GenderType = reader2.GetString(reader2.GetOrdinal("GenderType")),
                        EmailAddress = reader2.GetString(reader2.GetOrdinal("EmailAddress")),
                        Number = reader2.GetString(reader2.GetOrdinal("Number"))
                    };
                    spouseModels.Add(spouseModel);
                }
            }
            form.SpouseModels = spouseModels.Where(s => s.FormId == form.ID).ToList();
            return form;
        }
    public List<FullFormModel> SearchForName(string searchPhrase)
        {
            List<FullFormModel> returnList = new List<FullFormModel>();
            string sqlQuery = "SELECT F.ID, F.Fullname, G.GenderType, H.HouseUnit, F.Email, F.Mobile, F.Profession, S.StatusType from dbo.FullForm " +
                "as F join dbo.Gender as G ON F.Gender = G.GenderId join dbo.Housing as H ON F.HouseNumber = H.HouseId " +
                "join dbo.Statutory as S ON F.Status = S.StatusId WHERE F.fullname LIKE @searchPhrase";
            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
            command.Parameters.Add("@searchPhrase", System.Data.SqlDbType.NVarChar).Value = "%" + searchPhrase + "%";
            //connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //create a new gadget object. Add it to the list to return.
                    FullFormModel form = new FullFormModel();
                    form.ID = reader.GetInt32(0);
                    form.Fullname = reader.GetString(1);
                    form.Gender = reader.GetString(2);
                    form.HouseNumber = reader.GetString(3);
                    form.Email = reader.GetString(4);
                    form.Mobile = reader.GetString(5);
                    form.Profession = reader.GetString(6);
                    form.Status = reader.GetString(7);

                    returnList.Add(form);
                }
                reader.Close();
                string sqlQuery2 = "select s.FormId,s.Names, g.GenderType,s.EmailAddress, s.Number from dbo.Spouse as s join dbo.Gender as g on s.GenderType = g.GenderId";

                SqlCommand command2 = new SqlCommand(sqlQuery2, this.Connection);
                //connection.Open();
                SqlDataReader reader2 = command2.ExecuteReader();
                List<SpouseModel> spouseModels = new List<SpouseModel>();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        SpouseModel spouse = new SpouseModel();
                        spouse.FormId = reader2.GetInt32(0);
                        spouse.Names = reader2.GetString(1);
                        spouse.GenderType = reader2.GetString(2);
                        spouse.EmailAddress = reader2.GetString(3);
                        spouse.Number = reader2.GetString(4);

                        spouseModels.Add(spouse);
                    }
                }
                foreach (var tenant in returnList)
                {
                    tenant.SpouseModels = spouseModels.Where(s => s.FormId == tenant.ID).ToList();
                }


            }
            //reader.Close();
            return returnList;
        }
    public List<FullFormModel> SearchHouseNumber(string searchHouseNo)
        {
            List<FullFormModel> returnList = new List<FullFormModel>();
            string sqlQuery = "SELECT F.ID, F.Fullname, G.GenderType, H.HouseUnit, F.Email, F.Mobile, F.Profession, S.StatusType from dbo.FullForm " +
                "as F join dbo.Gender as G ON F.Gender = G.GenderId join dbo.Housing as H ON F.HouseNumber = H.HouseId " +
                "join dbo.Statutory as S ON F.Status = S.StatusId WHERE  H.HouseUnit LIKE @searchHouseNo";
            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
            command.Parameters.Add("@searchHouseNo", System.Data.SqlDbType.NVarChar).Value = "%" + searchHouseNo + "%";
            //connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //create a new gadget object. Add it to the list to return.
                    FullFormModel form = new FullFormModel();
                    form.ID = reader.GetInt32(0);
                    form.Fullname = reader.GetString(1);
                    form.Gender = reader.GetString(2);
                    form.HouseNumber = reader.GetString(3);
                    form.Email = reader.GetString(4);
                    form.Mobile = reader.GetString(5);
                    form.Profession = reader.GetString(6);
                    form.Status = reader.GetString(7);

                    returnList.Add(form);
                }
                reader.Close();
                string sqlQuery2 = "select s.FormId,s.Names, g.GenderType,s.EmailAddress, s.Number from dbo.Spouse as s join dbo.Gender as g on s.GenderType = g.GenderId";

                SqlCommand command2 = new SqlCommand(sqlQuery2, this.Connection);
                //connection.Open();
                SqlDataReader reader2 = command2.ExecuteReader();
                List<SpouseModel> spouseModels = new List<SpouseModel>();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        SpouseModel spouse = new SpouseModel();
                        spouse.FormId = reader2.GetInt32(0);
                        spouse.Names = reader2.GetString(1);
                        spouse.GenderType = reader2.GetString(2);
                        spouse.EmailAddress = reader2.GetString(3);
                        spouse.Number = reader2.GetString(4);

                        spouseModels.Add(spouse);
                    }
                }
                foreach (var tenant in returnList)
                {
                    tenant.SpouseModels = spouseModels.Where(s => s.FormId == tenant.ID).ToList();
                }


            }
            //reader.Close();
            return returnList;
        }

    internal int Delete(int id)
    {
        string sqlQuery = "DELETE FROM dbo.FullForm WHERE Id = @Id ";
        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

        command.Parameters.Add("@Id", System.Data.SqlDbType.Int, 1000).Value = id;
        //connection.Open();
        int deletedID = command.ExecuteNonQuery();

        return deletedID;
    }
    public int CreateOccupant(FormModel formModel)
    {
        string sqlQuery = "";
        // if fullmodel.id <= -1 then create
        if (formModel.ID <= 0)
        {
            sqlQuery = "INSERT INTO dbo.FullForm Values(@Fullname, @Gender, @HouseNumber, @Email, @Mobile, @Profession, @Status)";
        }
        else
        {
            sqlQuery = "UPDATE dbo.FullForm SET Fullname = @Fullname,  Gender = @Gender, HouseNumber = @HouseNumber, Email = @Email, Mobile = @Mobile, Profession = @Profession, Status = @Status WHERE ID = @ID";
        }
        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

        command.Parameters.Add("@ID", System.Data.SqlDbType.Int, 1000).Value = formModel.ID;
        command.Parameters.Add("@Fullname", System.Data.SqlDbType.VarChar, 1000).Value = formModel.Fullname;
        command.Parameters.Add("@Gender", System.Data.SqlDbType.Int, 1000).Value = formModel.Gender;
        command.Parameters.Add("@HouseNumber", System.Data.SqlDbType.Int, 1000).Value = formModel.HouseNumber;
        command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 1000).Value = formModel.Email;
        command.Parameters.Add("@Mobile", System.Data.SqlDbType.VarChar, 1000).Value = formModel.Mobile;
        command.Parameters.Add("@Profession", System.Data.SqlDbType.VarChar, 1000).Value = formModel.Profession;
        command.Parameters.Add("@Status", System.Data.SqlDbType.Int, 1000).Value = formModel.Status;

        //connection.Open();
        int newID = command.ExecuteNonQuery();
        return newID;
    }//not in use
        //public bool CheckForSpouse(int id)
        //{
        //    if (id > 0)
        //        return true;
        //    else
        //        return false;
        //}
    public int CreateOccupants(FormCollection formCollection)
        {
            string id = "-1";
            string name = formCollection["Fullname"];
            string gender = formCollection["Gender"];
            string houseNumber = formCollection["HouseNumber"];
            string email = formCollection["Email"];
            string mobile = formCollection["Mobile"];
            string profession = formCollection["Profession"];
            string status = formCollection["Status"];

            string sqlQuery = "";
            // if fullmodel.id <= -1 then create
            int mainId = int.Parse(id);
            
            if (mainId <= 0)
            {
                sqlQuery = "INSERT INTO dbo.FullForm Values(@Fullname, @Gender, @HouseNumber, @Email, @Mobile, @Profession, @Status)";
            }
            else
            {
                sqlQuery = "UPDATE dbo.FullForm SET Fullname = @Fullname,  Gender = @Gender, HouseNumber = @HouseNumber, Email = @Email, Mobile = @Mobile, Profession = @Profession, Status = @Status WHERE ID = @ID";
            }
            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

            command.Parameters.Add("@ID", System.Data.SqlDbType.Int, 1000).Value = mainId;
            command.Parameters.Add("@Fullname", System.Data.SqlDbType.VarChar, 1000).Value = name;
            command.Parameters.Add("@Gender", System.Data.SqlDbType.Int, 1000).Value = gender;
            command.Parameters.Add("@HouseNumber", System.Data.SqlDbType.Int, 1000).Value = houseNumber;
            command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 1000).Value = email;
            command.Parameters.Add("@Mobile", System.Data.SqlDbType.VarChar, 1000).Value = mobile;
            command.Parameters.Add("@Profession", System.Data.SqlDbType.VarChar, 1000).Value = profession;
            command.Parameters.Add("@Status", System.Data.SqlDbType.Int, 1000).Value = status;

            //connection.Open();
            int newID = command.ExecuteNonQuery();
            return newID;
        }
    public int SaveEditedOccupant(FormCollection formCollection)
        {
            string id = formCollection["ID"];
            string name = formCollection["Fullname"];
            string gender = formCollection["Gender"];
            string houseNumber = formCollection["HouseNumber"];
            string email = formCollection["Email"];
            string mobile = formCollection["Mobile"];
            string profession = formCollection["Profession"];
            string status = formCollection["Status"];

            string sqlQuery = "";
            // if fullmodel.id <= -1 then create
            int mainId = int.Parse(id);

            if (mainId <= 0)
            {
                sqlQuery = "INSERT INTO dbo.FullForm Values(@Fullname, @Gender, @HouseNumber, @Email, @Mobile, @Profession, @Status)";
            }
            else
            {
                sqlQuery = "UPDATE dbo.FullForm SET Fullname = @Fullname,  Gender = @Gender, HouseNumber = @HouseNumber, Email = @Email, Mobile = @Mobile, Profession = @Profession, Status = @Status WHERE ID = @ID";
            }
            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

            command.Parameters.Add("@ID", System.Data.SqlDbType.Int, 1000).Value = mainId;
            command.Parameters.Add("@Fullname", System.Data.SqlDbType.VarChar, 1000).Value = name;
            command.Parameters.Add("@Gender", System.Data.SqlDbType.Int, 1000).Value = gender;
            command.Parameters.Add("@HouseNumber", System.Data.SqlDbType.Int, 1000).Value = houseNumber;
            command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 1000).Value = email;
            command.Parameters.Add("@Mobile", System.Data.SqlDbType.VarChar, 1000).Value = mobile;
            command.Parameters.Add("@Profession", System.Data.SqlDbType.VarChar, 1000).Value = profession;
            command.Parameters.Add("@Status", System.Data.SqlDbType.Int, 1000).Value = status;

            //connection.Open();
            int newID = command.ExecuteNonQuery();

            return newID;
        }
    public int SaveEditedSpouses(FormCollection formCollection)
        {
           
            string spouseID = formCollection["SpouseId"];

            if (spouseID == null)
            {
                return 0;
            }
            else
            {
                string formID = formCollection["FormId"];
                string names = formCollection["Names"];
                string genders = formCollection["GenderType"];
                string emailAdd = formCollection["EmailAddress"];
                string number = formCollection["Number"];

                string[] divSpouseId = spouseID.Split(',');
                List<int> spouseIds = new List<int>();
                foreach (string id in divSpouseId)
                {
                    int individualSpouseId = int.Parse(id);
                    spouseIds.Add(individualSpouseId);
                }

                string[] divFormId = formID.Split(',');
                List<int> formIds = new List<int>();
                foreach (string id in divFormId)
                {
                    int individualFormId = int.Parse(id);
                    formIds.Add(individualFormId);
                }

                string[] divNames = names.Split(',');
                List<string> Naming = new List<string>();
                foreach (string spouseName in divNames)
                {
                    Naming.Add(spouseName);
                }

                string[] divGenderTypes = genders.Split(',');
                List<int> GenderTypes = new List<int>();
                foreach (string spouseGender in divGenderTypes)
                {
                    int spouseGenders = int.Parse(spouseGender);
                    GenderTypes.Add(spouseGenders);
                }

                string[] divEmails = emailAdd.Split(',');
                List<string> Emails = new List<string>();
                foreach (string spouseEmail in divEmails)
                {
                    Emails.Add(spouseEmail);
                }

                string[] divNumber = number.Split(',');
                List<string> Numbers = new List<string>();
                foreach (string spouseNumbers in divNumber)
                {
                    Numbers.Add(spouseNumbers);
                }

                string sqlQuery = "";
                //if the spouseid <= -1 then create
                int newID = 0;
                for (int i = 0; i < spouseIds.Count; i++)
                {
                    if (spouseIds[i] <= 0)
                    {
                        sqlQuery = "INSERT INTO dbo.Spouse Values(@FormId, @Names, @GenderType, @EmailAddress, @Number)";
                    }
                    else
                    {
                        sqlQuery = "UPDATE dbo.Spouse SET FormId = @FormId, Names = @Names,  GenderType = @GenderType, EmailAddress = @EmailAddress, Number = @Number WHERE SpouseId = @SpouseId";
                    }
                    SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
                    command.Parameters.Add("@SpouseId", System.Data.SqlDbType.Int, 1000).Value = spouseIds[i];
                    command.Parameters.Add("@FormId", System.Data.SqlDbType.Int, 1000).Value = formIds[i];
                    command.Parameters.Add("@Names", System.Data.SqlDbType.VarChar, 1000).Value = Naming[i];
                    command.Parameters.Add("@GenderType", System.Data.SqlDbType.Int, 1000).Value = GenderTypes[i];
                    command.Parameters.Add("@EmailAddress", System.Data.SqlDbType.VarChar, 1000).Value = Emails[i];
                    command.Parameters.Add("@Number", System.Data.SqlDbType.VarChar, 1000).Value = Numbers[i];

                    newID = command.ExecuteNonQuery();

                }
                return newID;
            }
            
        }
    public int CheckForNewSpouse(FormCollection formCollection)
        {
            if (formCollection["SpouseName"] != null)
            {
                string foreignKey = formCollection["ID"];
                string spousesId = formCollection["NewSpouseId"];
                string spouseName = formCollection["SpouseName"];
                string spouseGender = formCollection["SpouseGender"];
                string spouseEmail = formCollection["SpouseEmail"];
                string spouseNumber = formCollection["SpouseNumber"];

                string[] spousesFormId = foreignKey.Split(',');
                List<int> spouseFormIds = new List<int>();
                foreach (string ids in spousesFormId)
                {
                    int individualFormId = int.Parse(ids);
                    spouseFormIds.Add(individualFormId);
                }

                string[] divNewSpouse = spousesId.Split(',');
                List<int> newSpouseId = new List<int>();
                foreach(string id in divNewSpouse)
                {
                    int individualNewSpouseId = int.Parse(id);
                    newSpouseId.Add(individualNewSpouseId);
                }

                string[] divNames = spouseName.Split(',');
                List<string> SpouseNames = new List<string>();
                foreach (string newSpouseName in divNames)
                {
                    SpouseNames.Add(newSpouseName);
                }

                string[] divGenderTypes = spouseGender.Split(',');
                List<int> spouseGenderTypes = new List<int>();
                foreach (string NewSpouseGender in divGenderTypes)
                {
                    int spousesGender = int.Parse(NewSpouseGender);
                    spouseGenderTypes.Add(spousesGender);
                }

                string[] divEmailAddress = spouseEmail.Split(',');
                List<string> spouseEmailAddress = new List<string>();
                foreach (string spousesEmail in divEmailAddress)
                {
                    spouseEmailAddress.Add(spousesEmail);
                }

                string[] divNumbers = spouseNumber.Split(',');
                List<string> spouseNumbers = new List<string>();
                foreach (string spousesNumbers in divNumbers)
                {
                    spouseNumbers.Add(spousesNumbers);
                }

                string sqlQuery = "";
                //if the spouseid <= -1 then create
                int newID = 0;
                for (int i = 0; i < newSpouseId.Count; i++)
                {
                    if (newSpouseId[i] <= 0)
                    {
                        sqlQuery = "INSERT INTO dbo.Spouse Values(@FormId, @Names, @GenderType, @EmailAddress, @Number)";
                    }
                    else
                    {
                        sqlQuery = "UPDATE dbo.Spouse SET FormId = @FormId, Names = @Names,  GenderType = @GenderType, EmailAddress = @EmailAddress, Number = @Number WHERE SpouseId = @SpouseId";
                    }
                    SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
                    command.Parameters.Add("@SpouseId", System.Data.SqlDbType.Int, 1000).Value = newSpouseId[i];
                    command.Parameters.Add("@FormId", System.Data.SqlDbType.Int, 1000).Value = spouseFormIds[i];
                    command.Parameters.Add("@Names", System.Data.SqlDbType.VarChar, 1000).Value = SpouseNames[i];
                    command.Parameters.Add("@GenderType", System.Data.SqlDbType.Int, 1000).Value = spouseGenderTypes[i];
                    command.Parameters.Add("@EmailAddress", System.Data.SqlDbType.VarChar, 1000).Value = spouseEmailAddress[i];
                    command.Parameters.Add("@Number", System.Data.SqlDbType.VarChar, 1000).Value = spouseNumbers[i];

                    newID = command.ExecuteNonQuery();

                }
                return newID;
            }
            else
            {
                return 0;
            }

        }
    public int CreateSpouses(FormCollection formCollection)
        {
            int foreignKey = QueryFk();
            string spouseID = formCollection["SpouseId"];
            string names = formCollection["Names"];
            string genders = formCollection["GenderType"];
            string emailAdd = formCollection["EmailAddress"];
            string number = formCollection["Number"];

            string[] divSpouseId = spouseID.Split(',');
            List<int> spouseIds = new List<int>();
            foreach (string id in divSpouseId)
            {
                int individualSpouseId = int.Parse(id);
                spouseIds.Add(individualSpouseId);
            }

            string[] divNames = names.Split(',');
            List<string> Naming = new List<string>();
            foreach (string spouseName in divNames)
            {
                Naming.Add(spouseName);
            }

            string[] divGenderTypes = genders.Split(',');
            List<int> GenderTypes = new List<int>();
            foreach (string spouseGender in divGenderTypes)
            {
                int spouseGenders = int.Parse(spouseGender);
                GenderTypes.Add(spouseGenders);
            }

            string[] divEmails = emailAdd.Split(',');
            List<string> Emails = new List<string>();
            foreach (string spouseEmail in divEmails)
            {
                Emails.Add(spouseEmail);
            }

            string[] divNumber = number.Split(',');
            List<string> Numbers = new List<string>();
            foreach (string spouseNumbers in divNumber)
            {
                Numbers.Add(spouseNumbers);
            }

            string sqlQuery = "";
            //if the spouseid <= -1 then create
            int newID = 0;
            for (int i =0; i < spouseIds.Count; i++)
            {
                if(spouseIds[i] <= 0)
                {
                    sqlQuery = "INSERT INTO dbo.Spouse Values(@FormId, @Names, @GenderType, @EmailAddress, @Number)";
                }
                else
                {
                    sqlQuery = "UPDATE dbo.Spouse SET FormId = @FormId, Names = @Names,  GenderType = @GenderType, EmailAddress = @EmailAddress, Number = @Number WHERE SpouseId = @SpouseId";
                }
                SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
                command.Parameters.Add("@SpouseId", System.Data.SqlDbType.Int, 1000).Value = spouseIds[i];
                command.Parameters.Add("@FormId", System.Data.SqlDbType.Int, 1000).Value = foreignKey;
                command.Parameters.Add("@Names", System.Data.SqlDbType.VarChar, 1000).Value = Naming[i];
                command.Parameters.Add("@GenderType", System.Data.SqlDbType.Int, 1000).Value = GenderTypes[i];
                command.Parameters.Add("@EmailAddress", System.Data.SqlDbType.VarChar, 1000).Value = Emails[i];
                command.Parameters.Add("@Number", System.Data.SqlDbType.VarChar, 1000).Value = Numbers[i];

                newID = command.ExecuteNonQuery();
                
            }
            return newID;
        }

    public int QueryFk()
    {
        int newKey = 0;
        string sqlQuery = "SELECT TOP 1 ID FROM FullForm ORDER BY ID DESC";

        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
        SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                newKey = reader.GetInt32(0);
            }
        }
        reader.Close();
        return newKey;
    }//get the auto generated id of the occupants and send the id to be used in the spouse table
    public List<StatutoryModel> AccessStatusList()
    {
        List<StatutoryModel> returnstatuses = new List<StatutoryModel>();

        string sqlQuery = "SELECT * from dbo.Statutory";
        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                //collect the types and add it to the list to return
                StatutoryModel statuses = new StatutoryModel() {

                    StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                    StatusType = reader.GetString(reader.GetOrdinal("StatusType"))
                };
                returnstatuses.Add(statuses);
            }
        }
            reader.Close();
            return returnstatuses;
    }
    public List<HousingModel> AccessHouseUnit()
    {
        List<HousingModel> returnHouseUnit = new List<HousingModel>();

        string sqlQuery = "SELECT * from dbo.Housing";

        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                //collect the address and add it to the list to return
                HousingModel house = new HousingModel() {
                    HouseId = reader.GetInt32(reader.GetOrdinal("HouseId")),
                    HouseType = reader.GetString(reader.GetOrdinal("HouseType")),
                    HouseUnit = reader.GetString(reader.GetOrdinal("HouseUnit"))
                };
                returnHouseUnit.Add(house);
            }
        }
            reader.Close();
            return returnHouseUnit;
    }
    public List<GenderModel> AccessGender()
    {
        List<GenderModel> returnGender = new List<GenderModel>();

        string sqlQuery = "SELECT * from dbo.Gender";

        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                    //collect the address and add it to the list to return
                    GenderModel gender = new GenderModel()
                    {
                        GenderId = reader.GetInt32(reader.GetOrdinal("GenderId")),
                        GenderType = reader.GetString(reader.GetOrdinal("GenderType"))
                    };
                returnGender.Add(gender);
            }
        }
            reader.Close();
            return returnGender;
    }
    protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    try
                    {
                        this.Connection.Dispose();
                    }
                    catch { }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FormDAO()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

    public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}