using FluentValidation;
using AutoFix.Application.DTOs;

namespace AutoFix.Application.Validators
{
    public class CreateClienteDTOValidator : AbstractValidator<CreateClienteDTO>
    {
        public CreateClienteDTOValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

            RuleFor(c => c.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo no tiene un formato válido");

            RuleFor(c => c.Contraseña)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");

            RuleFor(c => c.Rol)
                .NotEmpty().WithMessage("El rol es obligatorio");
        }
    }
}
