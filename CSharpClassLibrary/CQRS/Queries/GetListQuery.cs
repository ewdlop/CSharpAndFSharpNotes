using CSharpClassLibrary.CQRS.Dtos;
using System.Collections.Generic;

namespace CSharpClassLibrary.CQRS.Queries
{
    public sealed partial class GetListQuery : IQuery<List<StudentDto>>
    {
        public string EnrolledIn { get; }
        public int? NumberOfCourses { get; }

        public GetListQuery(string enrolledIn, int? numberOfCourses)
        {
            EnrolledIn = enrolledIn;
            NumberOfCourses = numberOfCourses;
        }
    }
}
