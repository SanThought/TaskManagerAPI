using FluentValidation;
using TaskMasterAPI.Dtos;

namespace TaskMasterAPI.Validation;

public sealed class TaskCreateDtoValidator : AbstractValidator<TaskCreateDto>
{
    public TaskCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(120);
    }
}

