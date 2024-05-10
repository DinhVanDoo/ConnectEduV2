using ConnectEduV2.ViewModels;
using FluentValidation;

namespace ConnectEdu.Validator
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {

            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Định dạng Email không hợp lệ");
            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("Email không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");

        }
    }
}
