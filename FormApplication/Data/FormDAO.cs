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
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FormApp;
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
                FullFormModel form = new FullFormModel();
                form.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                form.Fullname = reader.GetString(reader.GetOrdinal("Fullname"));
                form.Gender = reader.GetString(reader.GetOrdinal("GenderType"));
                form.HouseNumber = reader.GetString(reader.GetOrdinal("HouseUnit"));
                form.Email = reader.GetString(reader.GetOrdinal("Email"));
                form.Mobile = reader.GetString(reader.GetOrdinal("Mobile"));
                form.Profession = reader.GetString(reader.GetOrdinal("Profession"));
                form.Status = reader.GetString(reader.GetOrdinal("StatusType"));

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
                        SpouseModel spouse =new SpouseModel();
                        spouse.FormId = reader2.GetInt32(reader2.GetOrdinal("FormId"));
                        spouse.Names = reader2.GetString(reader2.GetOrdinal("Names"));
                        spouse.Gender = reader2.GetString(reader2.GetOrdinal("GenderType"));
                        spouse.Email = reader2.GetString(reader2.GetOrdinal("EmailAddress"));
                        spouse.Number = reader2.GetString(reader2.GetOrdinal("Number"));

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
                    OnlineModel form = new OnlineModel();

                    form.Fullname = reader.GetFieldValue<string>(reader.GetOrdinal("Fullname"));
                    form.Gender = reader.GetString(reader.GetOrdinal("GenderType"));
                    form.HouseUnit = reader.GetFieldValue<string>(reader.GetOrdinal("HouseUnit"));
                    form.Email = reader.GetFieldValue<string>(reader.GetOrdinal("Email"));
                    form.Mobile = reader.GetFieldValue<string>(reader.GetOrdinal("Mobile"));
                    form.Profession = reader.GetFieldValue<string>(reader.GetOrdinal("Profession"));
                    form.StatusType = reader.GetFieldValue<string>(reader.GetOrdinal("StatusType"));

                    forms.Add(form);
                }
            }
            reader.Close();
            string sqlQuery2 = "SELECT * FROM DBO.SPOUSE";

            //associate @id with Id parameters
            SqlCommand command2 = new SqlCommand(sqlQuery2, this.Connection);

            command2.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

            SqlDataReader reader1 = command.ExecuteReader();

            List<SpouseModel> spouseModels = new List<SpouseModel>();

            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    //create a new object for collection and return it to the list;
                    SpouseModel spouse = new SpouseModel();
                    spouse.FormId = reader1.GetInt32(reader1.GetOrdinal("FormId"));
                    spouse.Names = reader1.GetString(reader1.GetOrdinal("Names"));
                    spouse.Gender = reader1.GetString(reader1.GetOrdinal("GenderType"));
                    spouse.Email = reader1.GetString(reader1.GetOrdinal("EmailAddress"));
                    spouse.Number = reader1.GetString(reader1.GetOrdinal("Number"));

                    spouseModels.Add(spouse);
                }
            }
            foreach (var tenant in forms)
            {
                tenant.SpouseModels = spouseModels.Where(s => s.FormId == tenant.ID).ToList();
            }
            return forms;
        }
        public SpouseModel FetchSpouse(int id)
        {
            string sqlQuery = "SELECT sp.SpouseId, sp.FormId, sp.Names, G.GenderType, sp.EmailAddress, sp.Number from dbo.Spouse as sp join dbo.Gender as G ON sp.Gendertype = G.GenderId WHERE FormId =@id;";
            //associate @id with Id parameters
            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

            command.Parameters.Add("@FormId", System.Data.SqlDbType.Int).Value = id;

            SqlDataReader reader = command.ExecuteReader();

            SpouseModel form = new SpouseModel();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    form.SpouseId = reader.GetFieldValue<int>(reader.GetOrdinal("SpouseId"));
                    form.FormId = reader.GetFieldValue<int>(reader.GetOrdinal("FormId"));
                    form.Names = reader.GetFieldValue<string>(reader.GetOrdinal("Names"));
                    form.Gender = reader.GetString(reader.GetOrdinal("GenderType"));
                    form.Email = reader.GetFieldValue<string>(reader.GetOrdinal("EmailAddress"));
                    form.Number = reader.GetFieldValue<string>(reader.GetOrdinal("Number"));

                }
            }
            reader.Close();
            return form;
        }
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
        internal int Delete(int id)
    {
        string sqlQuery = "DELETE FROM dbo.FullForm WHERE Id = @Id ";
        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

        command.Parameters.Add("@Id", System.Data.SqlDbType.Int, 1000).Value = id;
        //connection.Open();
        int deletedID = command.ExecuteNonQuery();

        return deletedID;
    }
        public int UpdateOccupant(FullFormModel fullFormModel)
        {
            string sqlQuery = "";
            //if fullFormModel.id <=1 1 then create
            if (fullFormModel.ID <= 0)
            {
                sqlQuery = "INSERT INTO dbo.FullForm Values(@Fullname, @Gender, @HouseNumber, @Email, @Mobile, @Profession, @Status)";
            }
            else
            {
                //update
                sqlQuery = "UPDATE dbo.FullForm SET Fullname = @Fullname,  Gender = @Gender, HouseNumber = @HouseNumber, Email = @Email, Mobile = @Mobile, Profession = @Profession, Status = @Status WHERE ID = @ID";
            }
            SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

            command.Parameters.Add("@ID", System.Data.SqlDbType.Int, 1000).Value = fullFormModel.ID;
            command.Parameters.Add("@Fullname", System.Data.SqlDbType.VarChar, 1000).Value = fullFormModel.Fullname;
            command.Parameters.Add("@Gender", System.Data.SqlDbType.Int, 1000).Value = fullFormModel.Gender;
            command.Parameters.Add("@HouseNumber", System.Data.SqlDbType.Int, 1000).Value = fullFormModel.HouseNumber;
            command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 1000).Value = fullFormModel.Email;
            command.Parameters.Add("@Mobile", System.Data.SqlDbType.VarChar, 1000).Value = fullFormModel.Mobile;
            command.Parameters.Add("@Profession", System.Data.SqlDbType.VarChar, 1000).Value = fullFormModel.Profession;
            command.Parameters.Add("@Status", System.Data.SqlDbType.Int, 1000).Value = fullFormModel.Status;


            int newID = command.ExecuteNonQuery();

            return newID;
        }
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
    public int CreateSpouses(FormCollection formCollection)
        {
            int foreignKey = QueryFk();
            string spouseID = formCollection["SpouseId"];
            string names = formCollection["Names"];
            string genders = formCollection["GenderType"];
            string emailAdd = formCollection["EmailAddress"];
            string number = formCollection["Number"];

            var divSpouseId = spouseID.Split(',');
            List<int> spouseIds = new List<int>();
            foreach (string id in divSpouseId)
            {
                int individualSpouseId = int.Parse(id);
                spouseIds.Add(individualSpouseId);
            }

            var divNames = names.Split(',');
            List<string> Naming = new List<string>();
            foreach (string spouseName in divNames)
            {
                Naming.Add(spouseName);
            }

            var divGenderTypes = genders.Split(',');
            List<int> GenderTypes = new List<int>();
            foreach (string spouseGender in divGenderTypes)
            {
                int spouseGenders = int.Parse(spouseGender);
                GenderTypes.Add(spouseGenders);
            }

            var divEmails = emailAdd.Split(',');
            List<string> Emails = new List<string>();
            foreach (string spouseEmail in divEmails)
            {
                Emails.Add(spouseEmail);
            }

            var divNumber = number.Split(',');
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
        FormModel form = new FormModel();

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
                StatutoryModel statuses = new StatutoryModel();
                statuses.StatusId = reader.GetInt32(0);
                statuses.StatusType = reader.GetString(1);

                returnstatuses.Add(statuses);
            }
        }reader.Close();
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
                HousingModel house = new HousingModel();
                house.HouseId = reader.GetInt32(0);
                house.HouseType = reader.GetString(1);
                house.HouseUnit = reader.GetString(2);

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
                GenderModel gender = new GenderModel();
                gender.GenderId = reader.GetInt32(0);
                gender.GenderType = reader.GetString(1);


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