using mPath.Interface;
using mPath.Models.Patient;
using mPath.Utils.Patient;

namespace mPath.Services;

public class PatientService : IPatientService
{
    private readonly PatientUtil _patientUtil;
    
    public PatientService()
    {
        _patientUtil = new PatientUtil();
    }
    

    public async Task<PatientDetail> GetPatient(int patientID)
    {
        return await _patientUtil.GetPatientByID(patientID);
    }

    public async Task<List<PatientDetail>> GetPatients()
    {
        throw new NotImplementedException();
    }

    public async  Task<bool> CreatePatient(PatientDetail patientDetail)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdatePatient(PatientDetail patientDetail)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeletePatient(int patientID)
    {
        throw new NotImplementedException();
    }
}