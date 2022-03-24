namespace Sneakers.API.Validators;

public class SneakerValidator : AbstractValidator<Sneaker>
{
    public SneakerValidator()
    {
        RuleFor(s => s.Name).NotEmpty().NotNull();
        RuleFor(s => s.Price).GreaterThan(0);
        RuleFor(s => s.Occasions).NotNull();
        RuleFor(s => s.Occasions).Must(s => s.Count >= 1);
        RuleFor(s => s.Brand).NotEmpty().NotNull();
    }
}