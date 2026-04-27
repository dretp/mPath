using System.Text;
using mPath.Utils.Base;
using mPath.Models.Patient;

namespace mPath.Utils.Patient;

public class PatientUtil : BaseUtils
{
    public PatientUtil()
    {
        
    }


    #region Public Methods

    public async Task<PatientDetail> GetPatientByID(int patientId)
    {
        return await retreivePatientDetailsByID(patientId);
    }

    #endregion



    private async Task<PatientDetail> retreivePatientDetailsByID(int patientId)
    {
        var data = new PatientDetail();
        
        var sql = new StringBuilder();
        sql.AppendLine("SELECT first_name, last_name, gender FROM patient WHERE id = @id;");

        try
        {
            // create a datasource
            await using var dataSource = base.dataSource();

            // create the data command
            await using var cmd = command(dataSource, sql.ToString());
            
            // add parameters values
            cmd.Parameters.AddWithValue("@id", patientId);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                data.FirstName = reader["first_name"].ToString();
                data.LastName = reader["last_name"].ToString();
                data.Gender = reader["gender"].ToString();
            }

            return data;
        }
        catch (Exception e)
        {
            LogError(e, "AccountUtil.RetrieveUserIdForInvite");
            return data;
        }
    }
    
}