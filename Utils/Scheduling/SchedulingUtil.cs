using System.Text;
using mPath.Location.Models;
using mPath.Utils.Base;

namespace mPath.Utils.Scheduling;

public class SchedulingUtil : BaseUtils
{
    public SchedulingUtil()
    {
    }

    public async Task<List<ShiftModel>> GetEmployeeSchedule(int employeeId, DateTime? dateUtc)
    {
        return await retrieveEmployeeSchedule(employeeId, dateUtc);
    }

    public async Task<List<ShiftModel>> GetLocationSchedule(string storeId, DateTime? dateUtc)
    {
        return await retrieveLocationSchedule(storeId, dateUtc);
    }

    public async Task<List<ShiftModel>> GetWeeklySchedule(string storeId, DateTime weekStartUtc)
    {
        return await retrieveWeeklySchedule(storeId, weekStartUtc);
    }

    public async Task<ShiftModel?> GetShiftById(int shiftId)
    {
        return await retrieveShiftById(shiftId);
    }

    public async Task<int> CreateShift(ShiftModel shift)
    {
        return await createShift(shift);
    }

    public async Task<bool> UpdateShiftNotes(int shiftId, string notes)
    {
        return await updateShiftNotes(shiftId, notes);
    }

    public async Task<bool> DeleteShift(int shiftId)
    {
        return await deleteShift(shiftId);
    }

    public async Task<List<AvailabilitySlotModel>> GetEmployeeAvailability(int employeeId)
    {
        return await retrieveEmployeeAvailability(employeeId);
    }

    private async Task<List<ShiftModel>> retrieveEmployeeSchedule(int employeeId, DateTime? dateUtc)
    {
        var shifts = new List<ShiftModel>();

        var sql = new StringBuilder();
        sql.AppendLine("SELECT s.id, s.employee_id, s.location_id, s.role_id, s.start_time, s.end_time, s.notes, s.created_at,");
        sql.AppendLine("       l.store_id, l.name AS location_name, r.name AS role_name, CONCAT_WS(' ', e.first_name, e.last_name) AS employee_name");
        sql.AppendLine("FROM shifts s");
        sql.AppendLine("JOIN locations l ON l.id = s.location_id");
        sql.AppendLine("JOIN roles r ON r.id = s.role_id");
        sql.AppendLine("JOIN employees e ON e.id = s.employee_id");
        sql.AppendLine("WHERE s.employee_id = @employee_id");
        if (dateUtc.HasValue)
        {
            sql.AppendLine("AND DATE(s.start_time) = @shift_date");
        }
        sql.AppendLine("ORDER BY s.start_time;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            if (dateUtc.HasValue)
            {
                cmd.Parameters.AddWithValue("@shift_date", dateUtc.Value.Date);
            }

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                shifts.Add(new ShiftModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                    EmployeeName = reader["employee_name"].ToString() ?? string.Empty,
                    LocationId = reader.GetInt32(reader.GetOrdinal("location_id")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("role_id")),
                    StoreId = reader["store_id"].ToString() ?? string.Empty,
                    LocationName = reader["location_name"].ToString() ?? string.Empty,
                    RoleName = reader["role_name"].ToString() ?? string.Empty,
                    StartTime = reader.GetDateTime(reader.GetOrdinal("start_time")),
                    EndTime = reader.GetDateTime(reader.GetOrdinal("end_time")),
                    Notes = reader["notes"] == DBNull.Value ? string.Empty : reader["notes"].ToString() ?? string.Empty,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                });
            }

            return shifts;
        }
        catch (Exception e)
        {
            LogError(e, "SchedulingUtil.retrieveEmployeeSchedule");
            return shifts;
        }
    }

    private async Task<List<ShiftModel>> retrieveLocationSchedule(string storeId, DateTime? dateUtc)
    {
        var shifts = new List<ShiftModel>();

        var sql = new StringBuilder();
        sql.AppendLine("SELECT s.id, s.employee_id, s.location_id, s.role_id, s.start_time, s.end_time, s.notes, s.created_at,");
        sql.AppendLine("       l.store_id, l.name AS location_name, r.name AS role_name, CONCAT_WS(' ', e.first_name, e.last_name) AS employee_name");
        sql.AppendLine("FROM shifts s");
        sql.AppendLine("JOIN locations l ON l.id = s.location_id");
        sql.AppendLine("JOIN roles r ON r.id = s.role_id");
        sql.AppendLine("JOIN employees e ON e.id = s.employee_id");
        sql.AppendLine("WHERE l.store_id = @store_id");
        if (dateUtc.HasValue)
        {
            sql.AppendLine("AND DATE(s.start_time) = @shift_date");
        }
        sql.AppendLine("ORDER BY s.start_time;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@store_id", storeId);
            if (dateUtc.HasValue)
            {
                cmd.Parameters.AddWithValue("@shift_date", dateUtc.Value.Date);
            }

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                shifts.Add(new ShiftModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                    EmployeeName = reader["employee_name"].ToString() ?? string.Empty,
                    LocationId = reader.GetInt32(reader.GetOrdinal("location_id")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("role_id")),
                    StoreId = reader["store_id"].ToString() ?? string.Empty,
                    LocationName = reader["location_name"].ToString() ?? string.Empty,
                    RoleName = reader["role_name"].ToString() ?? string.Empty,
                    StartTime = reader.GetDateTime(reader.GetOrdinal("start_time")),
                    EndTime = reader.GetDateTime(reader.GetOrdinal("end_time")),
                    Notes = reader["notes"] == DBNull.Value ? string.Empty : reader["notes"].ToString() ?? string.Empty,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                });
            }

            return shifts;
        }
        catch (Exception e)
        {
            LogError(e, "SchedulingUtil.retrieveLocationSchedule");
            return shifts;
        }
    }

    private async Task<ShiftModel?> retrieveShiftById(int shiftId)
    {
        var sql = new StringBuilder();
        sql.AppendLine("SELECT s.id, s.employee_id, s.location_id, s.role_id, s.start_time, s.end_time, s.notes, s.created_at,");
        sql.AppendLine("       l.store_id, l.name AS location_name, r.name AS role_name, CONCAT_WS(' ', e.first_name, e.last_name) AS employee_name");
        sql.AppendLine("FROM shifts s");
        sql.AppendLine("JOIN locations l ON l.id = s.location_id");
        sql.AppendLine("JOIN roles r ON r.id = s.role_id");
        sql.AppendLine("JOIN employees e ON e.id = s.employee_id");
        sql.AppendLine("WHERE s.id = @shift_id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@shift_id", shiftId);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ShiftModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                    EmployeeName = reader["employee_name"].ToString() ?? string.Empty,
                    LocationId = reader.GetInt32(reader.GetOrdinal("location_id")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("role_id")),
                    StoreId = reader["store_id"].ToString() ?? string.Empty,
                    LocationName = reader["location_name"].ToString() ?? string.Empty,
                    RoleName = reader["role_name"].ToString() ?? string.Empty,
                    StartTime = reader.GetDateTime(reader.GetOrdinal("start_time")),
                    EndTime = reader.GetDateTime(reader.GetOrdinal("end_time")),
                    Notes = reader["notes"] == DBNull.Value ? string.Empty : reader["notes"].ToString() ?? string.Empty,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                };
            }

            return null;
        }
        catch (Exception e)
        {
            LogError(e, "SchedulingUtil.retrieveShiftById");
            return null;
        }
    }

    private async Task<int> createShift(ShiftModel shift)
    {
        var sql = new StringBuilder();
        sql.AppendLine("INSERT INTO shifts (employee_id, location_id, role_id, start_time, end_time, notes)");
        sql.AppendLine("VALUES (@employee_id, @location_id, @role_id, @start_time, @end_time, @notes)");
        sql.AppendLine("RETURNING id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@employee_id", shift.EmployeeId);
            cmd.Parameters.AddWithValue("@location_id", shift.LocationId);
            cmd.Parameters.AddWithValue("@role_id", shift.RoleId);
            cmd.Parameters.AddWithValue("@start_time", shift.StartTime);
            cmd.Parameters.AddWithValue("@end_time", shift.EndTime);
            cmd.Parameters.AddWithValue("@notes", string.IsNullOrWhiteSpace(shift.Notes) ? (object)DBNull.Value : shift.Notes);

            var result = await cmd.ExecuteScalarAsync();
            if (result == null)
            {
                return 0;
            }

            return Convert.ToInt32(result);
        }
        catch (Exception e)
        {
            LogError(e, "SchedulingUtil.createShift");
            return 0;
        }
    }

    private async Task<bool> updateShiftNotes(int shiftId, string notes)
    {
        var sql = new StringBuilder();
        sql.AppendLine("UPDATE shifts SET notes = @notes WHERE id = @shift_id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@shift_id", shiftId);
            cmd.Parameters.AddWithValue("@notes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
        catch (Exception e)
        {
            LogError(e, "SchedulingUtil.updateShiftNotes");
            return false;
        }
    }

    private async Task<bool> deleteShift(int shiftId)
    {
        var sql = new StringBuilder();
        sql.AppendLine("DELETE FROM shifts WHERE id = @shift_id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@shift_id", shiftId);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
        catch (Exception e)
        {
            LogError(e, "SchedulingUtil.deleteShift");
            return false;
        }
    }

    private async Task<List<AvailabilitySlotModel>> retrieveEmployeeAvailability(int employeeId)
    {
        var slots = new List<AvailabilitySlotModel>();

        var sql = new StringBuilder();
        sql.AppendLine("SELECT id, employee_id, day_of_week, start_time, end_time");
        sql.AppendLine("FROM availability");
        sql.AppendLine("WHERE employee_id = @employee_id");
        sql.AppendLine("ORDER BY day_of_week, start_time;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                slots.Add(new AvailabilitySlotModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                    DayOfWeek = reader.GetInt32(reader.GetOrdinal("day_of_week")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("start_time")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("end_time"))
                });
            }

            return slots;
        }
        catch (Exception e)
        {
            LogError(e, "SchedulingUtil.retrieveEmployeeAvailability");
            return slots;
        }
    }

    private async Task<List<ShiftModel>> retrieveWeeklySchedule(string storeId, DateTime weekStartUtc)
    {
        var shifts = new List<ShiftModel>();

        // Normalise to Monday of the given week
        var monday = weekStartUtc.Date.AddDays(-(((int)weekStartUtc.DayOfWeek + 6) % 7));
        var sunday = monday.AddDays(7);

        var sql = new StringBuilder();
        sql.AppendLine("SELECT s.id, s.employee_id, s.location_id, s.role_id, s.start_time, s.end_time, s.notes, s.created_at,");
        sql.AppendLine("       l.store_id, l.name AS location_name, r.name AS role_name, CONCAT_WS(' ', e.first_name, e.last_name) AS employee_name");
        sql.AppendLine("FROM shifts s");
        sql.AppendLine("JOIN locations l ON l.id = s.location_id");
        sql.AppendLine("JOIN roles r ON r.id = s.role_id");
        sql.AppendLine("JOIN employees e ON e.id = s.employee_id");
        if (!string.IsNullOrWhiteSpace(storeId))
        {
            sql.AppendLine("WHERE l.store_id = @store_id");
            sql.AppendLine("AND s.start_time >= @week_start AND s.start_time < @week_end");
        }
        else
        {
            sql.AppendLine("WHERE s.start_time >= @week_start AND s.start_time < @week_end");
        }
        sql.AppendLine("ORDER BY s.start_time, s.employee_id;");

        try
        {
            await using var source = dataSource();
            await using var cmd = command(source, sql.ToString());
            if (!string.IsNullOrWhiteSpace(storeId))
            {
                cmd.Parameters.AddWithValue("@store_id", storeId);
            }
            cmd.Parameters.AddWithValue("@week_start", monday);
            cmd.Parameters.AddWithValue("@week_end", sunday);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                shifts.Add(new ShiftModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                    EmployeeName = reader["employee_name"].ToString() ?? string.Empty,
                    LocationId = reader.GetInt32(reader.GetOrdinal("location_id")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("role_id")),
                    StoreId = reader["store_id"].ToString() ?? string.Empty,
                    LocationName = reader["location_name"].ToString() ?? string.Empty,
                    RoleName = reader["role_name"].ToString() ?? string.Empty,
                    StartTime = reader.GetDateTime(reader.GetOrdinal("start_time")),
                    EndTime = reader.GetDateTime(reader.GetOrdinal("end_time")),
                    Notes = reader["notes"] == DBNull.Value ? string.Empty : reader["notes"].ToString() ?? string.Empty,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                });
            }

            return shifts;
        }
        catch (Exception e)
        {
            LogError(e, "SchedulingUtil.retrieveWeeklySchedule");
            return shifts;
        }
    }

}
