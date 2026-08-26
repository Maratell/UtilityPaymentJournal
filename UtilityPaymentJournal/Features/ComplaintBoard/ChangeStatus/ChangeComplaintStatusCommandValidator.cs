using FluentValidation;

namespace UtilityPaymentJournal.Features.ComplaintBoard.ChangeStatus
{
    public class ChangeComplaintStatusCommandValidator : AbstractValidator<ChangeComplaintStatusCommand>
    {
        public ChangeComplaintStatusCommandValidator()
        {
            RuleFor(x => x.NewStatus)
                .IsInEnum()
                .WithMessage("Выбран некорректный статус жалобы.");
        }
    }
}
