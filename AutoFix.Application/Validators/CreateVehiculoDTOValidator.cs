using FluentValidation;
using AutoFix.Application.DTOs;

namespace AutoFix.Application.Validators
{
    public class CreateVehiculoDTOValidator : AbstractValidator<CreateVehiculoDTO>
    {
        public CreateVehiculoDTOValidator()
        {
            RuleFor(v => v.Placa)
                .NotEmpty().WithMessage("La placa es obligatoria")
                .Length(4, 20).WithMessage("La placa debe tener entre 4 y 20 caracteres");

            RuleFor(v => v.Marca)
                .NotEmpty().WithMessage("La marca es obligatoria")
                .MaximumLength(50).WithMessage("La marca no puede exceder los 50 caracteres");

            RuleFor(v => v.Modelo)
                .NotEmpty().WithMessage("El modelo es obligatorio")
                .MaximumLength(50).WithMessage("El modelo no puede exceder los 50 caracteres");

            RuleFor(v => v.Anio)
                .InclusiveBetween(1900, 2100).WithMessage("El año debe estar entre 1900 y 2100");

            RuleFor(v => v.Color)
                .MaximumLength(20).WithMessage("El color no puede exceder los 20 caracteres");

            RuleFor(v => v.ClienteId)
                .GreaterThan(0).WithMessage("El cliente es obligatorio");
        }
    }
}
