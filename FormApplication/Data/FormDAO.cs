using FormApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
        }
            reader.Close();
            return returnList;
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
                form.ID = reader.GetInt32(0);
                form.Fullname = reader.GetString(1);
                form.Gender = reader.GetString(2);
                form.HouseNumber = reader.GetString(3);
                form.Email = reader.GetString(4);
                form.Mobile = reader.GetString(5);
                form.Profession = reader.GetString(6);
                form.Status = reader.GetString(7);
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
    public int CreateOrUpdate(FullFormModel fullFormModel)
    {
        string sqlQuery = "";
        //if senatemodel.id <=1 1 then create
        if (fullFormModel.ID <= 0)
        {
            sqlQuery = "INSERT INTO dbo.FullForm Values(@Fullname, @Gender, @HouseNumber, @Email, @Mobile, @Profession, @Status )";

        }
        else
        {
            //if senatemodel.id > 1 then update is meant.
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
        //connection.Open();
        int newID = command.ExecuteNonQuery();

        return newID;
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
    }

    public int CreateSpouse(FormModel formModel)
    {
        int foreignKeey = QueryFk();
        string sqlQuery = "";
        // if fullmodel.id <= -1 then create
        if (formModel.SpouseId <= 0)
        {
            sqlQuery = "INSERT INTO dbo.Spouse Values(@FormId, @Names, @GenderType, @EmailAddress, @Number)";
        }
        else
        {
            sqlQuery = "UPDATE dbo.Spouse SET FormId = @FormId, Names = @Names,  GenderType = @GenderType, EmailAddress = @EmailAddress, Number = @Number WHERE SpouseId = @SpouseId";
        }
        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);

        command.Parameters.Add("@SpouseId", System.Data.SqlDbType.Int, 1000).Value = formModel.SpouseId;
        command.Parameters.Add("@FormId", System.Data.SqlDbType.Int, 1000).Value = foreignKeey;
        command.Parameters.Add("@Names", System.Data.SqlDbType.VarChar, 1000).Value = formModel.Names;
        command.Parameters.Add("@GenderType", System.Data.SqlDbType.Int, 1000).Value = formModel.GenderType;
        command.Parameters.Add("@EmailAddress", System.Data.SqlDbType.VarChar, 1000).Value = formModel.EmailAddress;
        command.Parameters.Add("@Number", System.Data.SqlDbType.VarChar, 1000).Value = formModel.Number;

        //connection.Open();
        int newID = command.ExecuteNonQuery();
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
    }
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
    public List<HousingModel> AccessHousesType()
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
                house.HouseType = reader.GetString(1);/*
                        house.HouseUnit = reader.GetString(2);*/


                returnHouseUnit.Add(house);
            }
        }
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

    public List<FormModel> AccessFormView()
    {
        List<FormModel> returnList = new List<FormModel>();

        string sqlQuery = "SELECT F.*,S.Names,S.GenderType,S.EmailAddress,S.Number FROM dbo.FullForm as F JOIN dbo.Spouse as S ON F.ID = S.SpouseId;";
        SqlCommand command = new SqlCommand(sqlQuery, this.Connection);
        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                FormModel formModel = new FormModel();
                formModel.ID = reader.GetInt32(0);
                formModel.Fullname = reader.GetString(1);
                formModel.Gender = reader.GetString(2);
                formModel.HouseNumber = reader.GetString(3);
                formModel.Email = reader.GetString(4);
                formModel.Mobile = reader.GetString(5);
                formModel.Profession = reader.GetString(6);
                formModel.Status = reader.GetString(7);
                //formModel.SpouseId = reader.GetInt32(8);
                //formModel.FormId = reader.GetInt32(9);
                formModel.Names = reader.GetString(10);
                formModel.GenderType = reader.GetString(11);
                formModel.EmailAddress = reader.GetString(12);
                formModel.Number = reader.GetString(13);

                returnList.Add(formModel);
            }
        }
            reader.Close();
            return returnList;
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