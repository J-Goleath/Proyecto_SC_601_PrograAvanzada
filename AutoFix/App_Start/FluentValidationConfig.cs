using System.Web.Mvc;
using FluentValidation.Mvc;

namespace AutoFix
{
    public static class FluentValidationConfig
    {
        public static void RegisterValidation()
        {
            // Conecta FluentValidation con el pipeline de validación de MVC,
            // así los Validators se ejecutan automáticamente en cada
            // ModelState.IsValid, sin tocar los Controllers.
            FluentValidationModelValidatorProvider.Configure();
        }
    }
}