using System.Diagnostics;
using Domain.Entities;
using Domain.Dto;
using Dapper;
using Infrastructure.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StudentService
{
    private string _connectionString = "Server=127.0.0.1;Port=5432;Database=students;User Id=postgres;Password=11042004;";
     private readonly DataContext _context;

    public StudentService(DataContext context)
    {
        _context = context;
    }


    public async Task <List<StudentDto>> GetStudents() 
    {
        var sw =    new Stopwatch();
        sw.Start();
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            string sql = "SELECT * FROM \"Students\"";
            var result = connection.Query<StudentDto>(sql);

            sw.Stop();
            System.Console.WriteLine($"Elapsed Times with dapper /  {sw.ElapsedMilliseconds}");
            return result.ToList();
        }

    }
    public  async Task<List<StudentDto>> GetStudentsWithoutDapper()
    {
        string sql = "SELECT * FROM \"Students\"";
        var students = new List<StudentDto>();
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var sw = new Stopwatch();
            sw.Start();
             await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                using (var reader =  await command.ExecuteReaderAsync())
                {
                    while ( await reader.ReadAsync())
                    {
                        var student = new StudentDto()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            name = reader.GetString(reader.GetOrdinal("name")),
                            surname = reader.GetString(reader.GetOrdinal("surname")),
                            Address = reader.GetString(reader.GetOrdinal("address")),
                            Phone = reader.GetString(reader.GetOrdinal("phone"))
                        };
                        students.Add(student);
                    }
                }
            }
            sw.Stop();
            System.Console.WriteLine($"Elapsed Times without dapper /  {sw.ElapsedMilliseconds}");
            connection.Close();
        }

        return students;
    }
  public  async Task<List<StudentDto>> GetStudentWithEnt()
    {
          var sw = new Stopwatch();
        sw.Start();
        var list =  await _context.Students.Select(s => new StudentDto()
        {
           Id=s.Id,
           name=s.name,
           surname=s.surname,
           Phone = s.Phone,
           Address=s.Address


        }).ToListAsync();
 
             sw.Stop();
            System.Console.WriteLine($"Elapsed Times with Ef  {sw.ElapsedMilliseconds}");
        return new List<StudentDto>(list);
}
}