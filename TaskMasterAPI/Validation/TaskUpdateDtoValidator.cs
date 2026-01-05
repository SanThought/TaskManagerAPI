using FluentValidation;
using TaskMasterAPI.Dtos;

namespace TaskMasterAPI.Validation;

public sealed class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
{
    public TaskUpdateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(120);

        // IsCompleted: no special rule needed
    }
}

