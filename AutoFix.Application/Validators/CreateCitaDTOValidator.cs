using FluentValidation;
using AutoFix.Application.DTOs;
using System;

namespace AutoFix.Application.Validators
{
    public class CreateCitaDTOValidator : AbstractValidator<CreateCitaDTO>
    {
        public CreateCitaDTOValidator()
        {
            RuleFor(c => c.VehiculoId)
                .GreaterThan(0).WithMessage("El vehículo es obligatorio");

            RuleFor(c => c.Fecha)
                .NotEmpty().WithMessage("La fecha es obligatoria")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha no puede ser anterior a hoy");

            RuleFor(c => c.DescripcionFallos)
                .NotEmpty().WithMessage("Debe describir el problema")
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");
        }
    }
}
