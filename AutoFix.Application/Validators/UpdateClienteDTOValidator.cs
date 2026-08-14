using FluentValidation;
using AutoFix.Application.DTOs;

namespace AutoFix.Application.Validators
{
    public class UpdateClienteDTOValidator : AbstractValidator<UpdateClienteDTO>
    {
        public UpdateClienteDTOValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Id de cliente inválido");

            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

            RuleFor(c => c.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo no tiene un formato válido");

            // La contraseña es opcional al editar (solo se cambia si viene con valor)
            RuleFor(c => c.Contraseña)
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
                .When(c => !string.IsNullOrEmpty(c.Contraseña));

            RuleFor(c => c.Rol)
                .NotEmpty().WithMessage("El rol es obligatorio");
        }
    }
}
