using mPath.Models.Patient;

namespace mPath.Interface;

public interface IPatientService
{
    Task<PatientDetail> GetPatient(int patientID);
    Task<List<PatientDetail>> GetPatients();
    Task<bool> CreatePatient(PatientDetail patientDetail);
    Task<bool> UpdatePatient(PatientDetail patientDetail);
    Task<bool> DeletePatient(int patientID);
}