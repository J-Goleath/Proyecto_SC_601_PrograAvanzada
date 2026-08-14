using FluentValidation;
using AutoFix.Application.DTOs;

namespace AutoFix.Application.Validators
{
    public class UpdateCitaDTOValidator : AbstractValidator<UpdateCitaDTO>
    {
        public UpdateCitaDTOValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Id de cita inválido");

            RuleFor(c => c.VehiculoId)
                .GreaterThan(0).WithMessage("El vehículo es obligatorio");

            RuleFor(c => c.Fecha)
                .NotEmpty().WithMessage("La fecha es obligatoria");

            RuleFor(c => c.DescripcionFallos)
                .NotEmpty().WithMessage("Debe describir el problema")
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");
        }
    }
}
