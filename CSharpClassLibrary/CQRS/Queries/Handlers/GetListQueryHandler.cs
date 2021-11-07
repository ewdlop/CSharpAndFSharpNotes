using CSharpClassLibrary.CQRS.Dtos;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CSharpClassLibrary.CQRS.Queries
{
    public sealed partial class GetListQuery
    {
        internal sealed class GetListQueryHandler : IQueryHandler<GetListQuery, List<StudentDto>>
        {
            private readonly QueriesConnectionString _connectionString;

            public GetListQueryHandler(QueriesConnectionString connectionString)
            {
                _connectionString = connectionString;
            }

            public List<StudentDto> Handle(GetListQuery query)
            {
                string sql = @"
                    SELECT s.StudentID Id, s.Name, s.Email,
	                    s.FirstCourseName Course1, s.FirstCourseCredits Course1Credits, s.FirstCourseGrade Course1Grade,
	                    s.SecondCourseName Course2, s.SecondCourseCredits Course2Credits, s.SecondCourseGrade Course2Grade
                    FROM dbo.Student s
                    WHERE (s.FirstCourseName = @Course
		                    OR s.SecondCourseName = @Course
		                    OR @Course IS NULL)
                        AND (s.NumberOfEnrollments = @Number
                            OR @Number IS NULL)
                    ORDER BY s.StudentID ASC";

                using (SqlConnection connection = new SqlConnection(_connectionString.Value))
                {
                    return connection
                        .Query<StudentDto>(sql, new
                        {
                            Course = query.EnrolledIn,
                            Number = query.NumberOfCourses
                        })
                        .ToList();
                }
            }
        }
    }
}
