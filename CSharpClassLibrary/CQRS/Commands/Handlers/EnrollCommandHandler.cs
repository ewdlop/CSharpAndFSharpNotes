using CSharpClassLibrary.CQRS.Utils;
using CSharpClassLibrary.CQRS.Utils.Factory;
using CSharpFunctionalExtensions;
using System;

namespace CSharpClassLibrary.CQRS.Commands
{
    internal sealed class EnrollCommandHandler : ICommandHandler<EnrollCommand>
    {
        private readonly SessionFactory _sessionFactory;
        public Result Handle(EnrollCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);
            //var courseRepository = new CourseRepository(unitOfWork);
            //var studentRepository = new StudentRepository(unitOfWork);
            //Student student = studentRepository.GetById(command.Id);
            //if (student == null)
            //    return Result.Fail($"No student found with Id '{command.Id}'");

            //Course course = courseRepository.GetByName(command.Course);
            //if (course == null)
            //    return Result.Fail($"Course is incorrect: '{command.Course}'");

            //bool success = Enum.TryParse(command.Grade, out Grade grade);
            //if (!success)
            //    return Result.Fail($"Grade is incorrect: '{command.Grade}'");

            //student.Enroll(course, grade);

            unitOfWork.Commit();

            return Result.Ok();
        }
    }
}