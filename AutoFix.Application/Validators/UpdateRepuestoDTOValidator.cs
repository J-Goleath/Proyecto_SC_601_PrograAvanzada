using FluentValidation;
using AutoFix.Application.DTOs;

namespace AutoFix.Application.Validators
{
    public class UpdateRepuestoDTOValidator : AbstractValidator<UpdateRepuestoDTO>
    {
        public UpdateRepuestoDTOValidator()
        {
            RuleFor(r => r.Id)
                .GreaterThan(0).WithMessage("Id de repuesto inválido");

            RuleFor(r => r.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

            RuleFor(r => r.Codigo)
                .NotEmpty().WithMessage("El código es obligatorio")
                .MaximumLength(50).WithMessage("El código no puede exceder los 50 caracteres");

            RuleFor(r => r.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");

            RuleFor(r => r.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock debe ser un número positivo");

            RuleFor(r => r.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

            RuleFor(r => r.Categoria)
                .MaximumLength(50).WithMessage("La categoría no puede exceder los 50 caracteres");

            RuleFor(r => r.Ubicacion)
                .MaximumLength(100).WithMessage("La ubicación no puede exceder los 100 caracteres");
        }
    }
}
